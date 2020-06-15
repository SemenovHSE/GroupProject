using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GroupProject.Database.ModelsGenerated;
using GroupProject.Models;
using Newtonsoft.Json;
using System.IO;
using System.Web.Optimization;
using System.Web.UI.WebControls.WebParts;
using GroupProject.Classes;
using GroupProject.Enums;
using GroupProject.Extensions;

namespace GroupProject.Controllers
{
    public class PersonalCabinetController : BaseController
    {
        // GET: PersonalCabinet
        public ActionResult Index()
        {
            ViewData["currentUser"] = CurrentUser;
            return View();
        }


        [HttpGet]
        public ActionResult RenderGeneralInformation()
        {
            try
            {
                UserModel userModel = new UserModel()
                {
                    FullName = CurrentUser.FullName,
                    PhoneNumber = CurrentUser.PhoneNumber
                };
                if (CurrentUser is Resident)
                {
                    ResidentGeneralInformationModel model = new ResidentGeneralInformationModel();
                    model.User = userModel;
                    Resident resident = CurrentUser as Resident;
                    Setting setting = resident.Setting;
                    House house = resident.Setting.House;
                    SettingModel settingModel = new SettingModel()
                    {
                        Address = house.Address,
                        SettingNumber = setting.SettingNumber,
                        Entrance = setting.EntranceNumber,
                        Size = setting.Size,
                        RoomsNumber = setting.RoomsNumber
                    };
                    model.Setting = settingModel;
                    return PartialView("_ResidentGeneralInformation", model);
                }
                if (CurrentUser is Employee)
                {
                    Employee employee = CurrentUser as Employee;
                    EmployeeGeneralInformationModel model = new EmployeeGeneralInformationModel()
                    {
                        User = userModel
                    };
                    return PartialView("_EmployeeGeneralInformation", model);
                }
                return new EmptyResult();
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return new HttpNotFoundResult();
            }
        }


        [HttpPost]
        public JsonResult UpdateGeneralInformation(UserModel residentModel)
        {
            try
            {
                Resident resident = CurrentUser as Resident;
                resident = DatabaseContext.Residents.FirstOrDefault(r => r.Id == resident.Id);
                resident.FullName = residentModel.FullName;
                resident.PhoneNumber = residentModel.PhoneNumber;
                DatabaseContext.SaveChanges();
                return Json(new {result = true});
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return Json(new {result = false});
            }
        }


        [HttpPost]
        public JsonResult UpdateSettingInformation(SettingModel settingModel)
        {
            try
            {
                Resident resident = CurrentUser as Resident;
                Setting setting = DatabaseContext.Settings.FirstOrDefault(s => s.Id == resident.SettingId);
                setting.House.Address = settingModel.Address;
                setting.SettingNumber = settingModel.SettingNumber;
                setting.EntranceNumber = settingModel.Entrance;
                setting.Size = settingModel.Size;
                setting.RoomsNumber = settingModel.RoomsNumber;
                DatabaseContext.SaveChanges();
                return Json(new {result = true});
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return Json(new {result = false});
            }
        }


        [HttpGet]
        public ActionResult RenderRequestsGrid(int page = 1)
        {
            try
            {
                int pageSize = 5;
                IQueryable<Request> requests = DatabaseContext.Requests.OrderByDescending(r => r.Date);
                IEnumerable<RequestModel> requestsModels = requests.Skip((page - 1) * pageSize).Take(pageSize).ToList()
                    .Select(r =>
                        new RequestModel()
                        {
                            RequestId = r.Id,
                            Theme = r.Theme,
                            Status = new StatusModel() { Id = r.Status.Id, Name = r.Status.Name },
                            Date = r.Date.ToString("g")
                        });
                PageInfo pageInfo = new PageInfo()
                {
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = requests.Count()
                };
                RequestsPageModel requestsModel = new RequestsPageModel()
                {
                    Requests = requestsModels,
                    PageInfo = pageInfo
                };
                ViewData["currentUser"] = CurrentUser;
                return PartialView("_RequestsGrid", requestsModel);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return new HttpNotFoundResult();
            }
        }


        [HttpGet]
        public ActionResult RenderSendRequest()
        {
            try
            {
                return PartialView("_SendRequest");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return new HttpNotFoundResult();
            }
        }


        [HttpGet]
        public ActionResult RenderTagsGrid(int page = 1)
        {
            try
            {
                int pageSize = 5;
                IQueryable<Tag> tags = DatabaseContext.Tags.OrderBy(t => t.Id);
                IEnumerable<PersonalCabinetTagModel> tagsModels = tags.Skip((page - 1) * pageSize).Take(pageSize).ToList()
                    .Select(t =>
                    {
                        bool residentIsSubscribedToTag =
                            DatabaseContext.ResidentTags.FirstOrDefault(r => r.ResidentId == CurrentUser.Id && r.TagId == t.Id) != null;
                        return new PersonalCabinetTagModel()
                        {
                            TagId = t.Id,
                            Name = t.Name,
                            ResidentIsSubscribed = residentIsSubscribedToTag
                        };
                    });
                PageInfo pageInfo = new PageInfo()
                {
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = tags.Count()
                };
                PersonalCabinetTagsPageModel tagsModel = new PersonalCabinetTagsPageModel()
                {
                    Tags = tagsModels,
                    PageInfo = pageInfo
                };
                return PartialView("_TagsGrid", tagsModel);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return new HttpNotFoundResult();
            }
        }

        [HttpPost]
        public JsonResult SendRequest(SendRequestModel model)
        {
            try
            {
                Request request = new Request()
                {
                    Theme = model.Theme,
                    Body = model.Body,
                    Date = DateTime.UtcNow,
                    Status = DatabaseContext.Statuses.FirstOrDefault(s => s.Id == 1),
                    Resident = CurrentUser as Resident
                };               
                DatabaseContext.Requests.Add(request);
                DatabaseContext.SaveChanges();
                if (model.Files != null)
                {
                    string rootDirectory = Server.MapPath("~\\Files");
                    string currentRequestDirectory = "\\Request_" + request.Id;
                    string requestDirectoryName = rootDirectory + currentRequestDirectory;
                    DirectoryInfo directory = new DirectoryInfo(requestDirectoryName);
                    if (!directory.Exists)
                    {
                        directory.Create();
                        request.File = currentRequestDirectory;
                    }
                    foreach (var file in model.Files)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        file.SaveAs(directory.FullName + "\\" + fileName);
                    }
                }
                return Json(new { result = true });
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return Json(new { result = false });
            }
        }


        [HttpGet]
        public ActionResult RenderSendReply(int requestId)
        {
            try
            {
                Request request = DatabaseContext.Requests.FirstOrDefault(r => r.Id == requestId);
                if (request == null)
                {
                    throw  new ArgumentException("Выбранный запрос не найден");
                }
                SendReplyModel model = new SendReplyModel()
                {
                    RequestId = requestId
                };
                return PartialView("_SendReply", model);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return new HttpNotFoundResult();
            }
        }


        [HttpPost]
        public JsonResult SendReply(SendReplyModel model)
        {
            try
            {
                Request request = DatabaseContext.Requests.FirstOrDefault(r => r.Id == model.RequestId);
                if (request == null)
                {
                    throw new ArgumentException("Выбранной завявки не найдено");
                }
                if (request.Status.Id == (int)RequestStatus.Waiting)
                {
                    Status toStatus =
                        DatabaseContext.Statuses.FirstOrDefault(s => s.Id == (int) RequestStatus.UnderConsideration);
                    if (toStatus == null)
                    {
                        throw new ArgumentException("Статуса для изменения не найдено");
                    }
                    request.Status = toStatus;
                    DatabaseContext.SaveChanges();
                }
                Reply reply = new Reply()
                {
                    Body = model.Body,
                    Date = DateTime.UtcNow,
                    Employee = CurrentUser as Employee,
                    Request = request
                };
                DatabaseContext.Replies.Add(reply);
                DatabaseContext.SaveChanges();
                if (model.Files != null)
                {
                    string rootDirectory = Server.MapPath("~\\Files");
                    string currentReplyDirectory = "\\Reply_" + reply.Id;
                    string replyDirectoryName = rootDirectory + currentReplyDirectory;
                    DirectoryInfo directory = new DirectoryInfo(replyDirectoryName);
                    if (!directory.Exists)
                    {
                        directory.Create();
                        reply.File = currentReplyDirectory;
                    }

                    foreach (var file in model.Files)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        file.SaveAs(directory.FullName + "\\" + fileName);
                    }
                }
                return Json(new {result = true});
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return Json(new {result = false});
            }
        }


        [HttpPost]
        public JsonResult CloseRequest(int requestId)
        {
            try
            {
                Request request = DatabaseContext.Requests.FirstOrDefault(r => r.Id == requestId);
                if (request == null)
                {
                    throw new ArgumentException("Выбранной заявки не найдено");
                }
                Status requestClosedStatus =
                    DatabaseContext.Statuses.FirstOrDefault(s => s.Id == (int) RequestStatus.Closed);
                if (requestClosedStatus == null)
                {
                    throw new ArgumentException("Выбранного статуса для изменения не найдено");
                }
                request.Status = requestClosedStatus;
                DatabaseContext.SaveChanges();
                return Json(new {result = true});
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return Json(new {result = false});
            }
        }


        private IEnumerable<string> GetFilesNames(string path)
        {
            IEnumerable<string> files = new List<string>();
            DirectoryInfo directory = new DirectoryInfo(Server.MapPath("~\\Files\\" + path));
            if (directory.Exists)
            {
                files = directory.GetFiles().Select(file => file.Name);
            }
            return files;
        }


        [HttpGet]
        public ActionResult GetRequestProgress(int requestId)
        {
            try
            {
                Request request = DatabaseContext.Requests.FirstOrDefault(r => r.Id == requestId);
                if (request == null)
                {
                    throw new ArgumentException("Выбранного запроса не найдено");
                }
                string requestPath = "\\Request_" + requestId;
                IEnumerable<string> requestFiles = GetFilesNames(requestPath);
                RequestModel requestModel = new RequestModel()
                {
                    RequestId = requestId,
                    ResidentFullName = request.Resident.FullName,
                    Theme = request.Theme,
                    Body = request.Body,
                    Files = requestFiles,
                    Status = new StatusModel() { Id = request.Status.Id, Name = request.Status.Name },
                    Date = request.Date.ToString("g")
                };
                IEnumerable<Reply> replies =
                    DatabaseContext.Replies.Where(r => r.RequestId == requestId).OrderBy(r => r.Date).ToList();
                IEnumerable<ReplyModel> repliesModels = replies.Select(r =>
                {
                    string replyPath = "\\Reply_" + r.Id;
                    IEnumerable<string> files = GetFilesNames(replyPath);
                    return new ReplyModel()
                    {
                        ReplyId = r.Id,
                        Body = r.Body,
                        Files = files,
                        Date = r.Date.ToString("g"),
                        EmployeeFullName = r.Employee.FullName
                    };
                }).ToList();
                RequestProgressModel model = new RequestProgressModel()
                {
                    Request = requestModel,
                    Replies = repliesModels
                };
                ViewData["currentUser"] = CurrentUser;
                return PartialView("_RequestProgress", model);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return new HttpNotFoundResult();
            }
        }


        [HttpGet]
        public ImageResult GetImage(string filePath)
        {
            try
            {
                string path = Server.MapPath("\\Files\\" + filePath);
                return this.Image(path, "image/gif");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return null;
            }
        }


        [HttpPost]
        public JsonResult SubscribeToTag(int tagId)
        {
            try
            {
                Tag tag = DatabaseContext.Tags.FirstOrDefault(t => t.Id == tagId);
                if (tag == null)
                {
                    throw new ArgumentException("Выбранного тега не найдено");
                }
                Resident resident = CurrentUser as Resident;
                ResidentTag residentTag = new ResidentTag()
                {
                    Resident = resident,
                    Tag = tag
                };
                DatabaseContext.ResidentTags.Add(residentTag);
                DatabaseContext.SaveChanges();
                return Json(new {result = true});
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return Json(new {result = false});
            }
        }


        [HttpPost]
        public JsonResult UnsubscribeFromTag(int tagId)
        {
            try
            {
                Tag tag = DatabaseContext.Tags.FirstOrDefault(t => t.Id == tagId);
                if (tag == null)
                {
                    throw new ArgumentException("Выбранного тега не найдено");
                }
                Resident resident = CurrentUser as Resident;
                ResidentTag residentTag =
                    DatabaseContext.ResidentTags.FirstOrDefault(r => r.ResidentId == resident.Id && r.TagId == tag.Id);
                if (residentTag == null)
                {
                    throw new ArgumentException("Пользователь не подписан на тег");
                }
                DatabaseContext.ResidentTags.Remove(residentTag);
                DatabaseContext.SaveChanges();
                return Json(new {result = true});
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return Json(new {result = false});
            }
        }
    }
}