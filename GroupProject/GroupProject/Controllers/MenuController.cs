using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GroupProject.Models;
using GroupProject.Authentication;
using GroupProject.Classes;
using GroupProject.Database.ModelsGenerated;
using GroupProject.Extensions;

namespace GroupProject.Controllers
{
    public class MenuController : BaseController
    {
        [AllowAccess(new [] {AuthenticationTypeEnum.Anonymous, AuthenticationTypeEnum.Resident, AuthenticationTypeEnum.Employee })]
        [HttpGet]
        public ActionResult About()
        {
            ViewData["currentUser"] = CurrentUser;
            ViewData["currentLink"] = "About";
            return View();
        }

        
        [AllowAccess(new [] { AuthenticationTypeEnum.Anonymous, AuthenticationTypeEnum.Resident, AuthenticationTypeEnum.Employee })]
        [HttpGet]
        public ActionResult InformationBlocks()
        {
            ViewData["currentUser"] = CurrentUser;
            ViewData["currentLink"] = "InformationBlocks";
            return View();
        }


        [AllowAccess(new [] {AuthenticationTypeEnum.Anonymous, AuthenticationTypeEnum.Resident, AuthenticationTypeEnum.Employee })]
        [HttpGet]
        public ActionResult PersonalCabinet()
        {
            if (Authentication.CurrentUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "PersonalCabinet");
            }
            return RedirectToAction("Login", "Menu");
        }


        [AllowAccess(new[] {AuthenticationTypeEnum.Resident, AuthenticationTypeEnum.Employee })]
        [HttpGet]
        public ActionResult FinancialStatements()
        {
            ViewData["currentUser"] = CurrentUser;
            ViewData["currentLink"] = "FinancialStatements";
            return View();
        }


        [AllowAccess(new[] {AuthenticationTypeEnum.Resident, AuthenticationTypeEnum.Employee })]
        [HttpGet]
        public ActionResult Deptors()
        {
            ViewData["currentUser"] = CurrentUser;
            ViewData["currentLink"] = "Deptors";
            return View();
        }


        [AllowAccess(new [] {AuthenticationTypeEnum.Anonymous, AuthenticationTypeEnum.Resident, AuthenticationTypeEnum.Employee })]
        [HttpGet]
        public ActionResult Contacts()
        {
            ViewData["currentUser"] = CurrentUser;
            ViewData["currentLink"] = "Contacts";
            return View();
        }


        [AllowAccess(new [] {AuthenticationTypeEnum.Employee })]
        [HttpGet]
        public ActionResult Tags()
        {
            ViewData["currentUser"] = CurrentUser;
            ViewData["currentLink"] = "Tags";
            return View();
        }


        [AllowAccess(new [] {AuthenticationTypeEnum.Anonymous })]
        [HttpGet]
        public ActionResult Register()
        {
            List<string> addresses = DatabaseContext.Houses.Select(h => h.Address).ToList();
            ViewData["addresses"] = addresses;
            ViewData["currentUser"] = CurrentUser;
            ViewData["currentLink"] = "PersonalCabinet";
            return View(new RegistrationModel());
        }


        [AllowAccess(new [] {AuthenticationTypeEnum.Anonymous })]
        [HttpPost]
        public ActionResult Register(RegistrationModel registrationModel)
        {
            try
            {
                Setting setting = DatabaseContext.Settings.FirstOrDefault(s =>
                    s.House.Address == registrationModel.Address && s.EntranceNumber == registrationModel.Entrance &&
                    s.SettingNumber == registrationModel.SettingNumber);
                Resident resident = new Resident()
                {
                    FullName = registrationModel.Surname + " " + registrationModel.Name + " " +
                               registrationModel.Patronymic,
                    PhoneNumber = registrationModel.PhoneNumber,
                    Password = registrationModel.Password,
                    Requests = new List<Request>(),
                    ResidentTags = new List<ResidentTag>()
                };
                resident.Setting = setting;
                DatabaseContext.Residents.Add(resident);
                DatabaseContext.SaveChanges();
                return RedirectToAction("Index", "PersonalCabinet");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return RedirectToAction("Register");
            }
        }
        

        [AllowAccess(new [] { AuthenticationTypeEnum.Anonymous, AuthenticationTypeEnum.Resident, AuthenticationTypeEnum.Employee })]
        [HttpGet]
        public ActionResult LoginInformation()
        {
            return PartialView("_LoginInformation", CurrentUser);
        }


        [AllowAccess(new [] { AuthenticationTypeEnum.Anonymous, AuthenticationTypeEnum.Resident, AuthenticationTypeEnum.Employee })]
        [HttpGet]
        public ActionResult HouseInformation()
        {
            HouseModel model = null;
            if (CurrentUser is Employee)
            {
                Employee employee = CurrentUser as Employee;
                model = new HouseModel()
                {
                    Address = employee.House.Address
                };
            }

            return PartialView("_HouseInformation", model);
        }


        [AllowAccess(new [] { AuthenticationTypeEnum.Anonymous })]
        [HttpGet]
        public ActionResult Login()
        {
            ViewData["currentUser"] = CurrentUser;
            ViewData["currentLink"] = "PersonalCabinet";
            return View(new LoginModel());
        }


        [AllowAccess(new [] { AuthenticationTypeEnum.Anonymous })]
        [HttpPost]
        public ActionResult Login(LoginModel login)
        {
            if (ModelState.IsValid)
            {
                var user = Authentication.Login(login.PhoneNumber, login.Password, false);
                if (user != null)
                {
                    return RedirectToAction("Index", "PersonalCabinet");
                }
                ModelState["Password"].Errors.Add("Пароли не совпадают");
            }

            ViewData["currentLink"] = "PersonalCabinet";
            return View(login);
        }


        [AllowAccess(new [] { AuthenticationTypeEnum.Resident, AuthenticationTypeEnum.Employee })]
        [HttpGet]
        public ActionResult Logout()
        {
            Authentication.Logout();
            return RedirectToAction("About", "Menu");
        }


        [AllowAccess(new [] { AuthenticationTypeEnum.Employee })]
        [HttpGet]
        public ActionResult RenderTagsGrid(int page = 1)
        {
            try
            {
                int pageSize = 5;
                IQueryable<Tag> tags = DatabaseContext.Tags.OrderBy(t => t.Id);
                IEnumerable<MenuTagModel> tagsModels = tags.Skip((page - 1) * pageSize).Take(pageSize).ToList()
                    .Select(t =>
                    {
                        return new MenuTagModel()
                        {
                            TagId = t.Id,
                            Name = t.Name
                        };
                    });
                PageInfo pageInfo = new PageInfo()
                {
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = tags.Count()
                };
                MenuTagsPageModel tagsModel = new MenuTagsPageModel()
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


        [AllowAccess(new [] { AuthenticationTypeEnum.Employee })]
        [HttpPost]
        public JsonResult UpdateTag(MenuTagModel tagModel)
        {
            try
            {
                Tag tag = DatabaseContext.Tags.FirstOrDefault(t => t.Id == tagModel.TagId);
                if (tag == null)
                {
                    throw new ArgumentException("Выбранного тега не найдено");
                }
                tag.Name = tagModel.Name;
                DatabaseContext.SaveChanges();
                return Json(new {result = true});
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return Json(new {result = false});
            }
        }

        
        [AllowAccess(new [] { AuthenticationTypeEnum.Employee })]
        [HttpGet]
        public ActionResult RenderCreateTag()
        {
            try
            {
                return PartialView("_CreateTag");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return new HttpNotFoundResult();
            }
        }


        [AllowAccess(new [] { AuthenticationTypeEnum.Employee })]
        [HttpPost]
        public JsonResult CreateTag(CreateTagModel tagModel)
        {
            try
            {
                Tag tag = new Tag()
                {
                    Name = tagModel.Name
                };
                DatabaseContext.Tags.Add(tag);
                DatabaseContext.SaveChanges();
                return Json(new {result = true});
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return Json(new {result = false});
            }
        }


        [AllowAccess(new[] { AuthenticationTypeEnum.Employee })]
        [HttpGet]
        public ActionResult RenderInformationBlocksGrid(int page = 1)
        {
            try
            {
                int pageSize = 5;
                IQueryable<InformationBlock> informationBlocks =
                    DatabaseContext.InformationBlocks.OrderByDescending(b => b.Date);
                IEnumerable<InformationBlockGridModel> models = informationBlocks.Skip((page - 1) * pageSize)
                    .Take(pageSize).ToList()
                    .Select(b =>
                    {
                        return new InformationBlockGridModel()
                        {
                            Id = b.Id,
                            Title = b.Title,
                            Date = b.Date.ToString("g")
                        };
                    });
                PageInfo pageInfo = new PageInfo()
                {
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = informationBlocks.Count()
                };
                InformationBlockGridPageModel model = new InformationBlockGridPageModel()
                {
                    InformationBlocks = models,
                    PageInfo = pageInfo
                };
                return PartialView("_InformationBlocksGrid", model);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return new HttpNotFoundResult();
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


        [AllowAccess(new [] { AuthenticationTypeEnum.Resident, AuthenticationTypeEnum.Employee })]
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


        [AllowAccess(new [] { AuthenticationTypeEnum.Employee })]
        [HttpGet]
        public ActionResult RenderInformationBlock(int? informationBlockId = null)
        {
            try
            {
                InformationBlockModel model = new InformationBlockModel();
                if (informationBlockId != null)
                {
                    InformationBlock informationBlock =
                        DatabaseContext.InformationBlocks.FirstOrDefault(b => b.Id == informationBlockId);
                    if (informationBlock == null)
                    {
                        throw new ArgumentException("Выбранной новости не найдено");
                    }
                    string informationBlockPath = "\\InformationBlock_" + informationBlockId;
                    IEnumerable<string> informationBlockFiles = GetFilesNames(informationBlockPath);
                    model.Id = informationBlock.Id;
                    model.Title = informationBlock.Title;
                    model.Body = informationBlock.Body;
                    model.Files = informationBlockFiles;
                    model.Date = informationBlock.Date.ToString("g");
                    IEnumerable<Tag> tags = DatabaseContext.Tags.ToList();
                    model.Tags = tags.Select(t =>
                    {
                        return new SelectListItem()
                        {
                            Text = t.Name,
                            Value = t.Id.ToString(),
                            Selected = informationBlock.TagInformationBlocks.FirstOrDefault(b => b.TagId == t.Id) != null
                        };
                    });
                    model.TagsIds = tags.Where(t =>
                            informationBlock.TagInformationBlocks.FirstOrDefault(b => b.TagId == t.Id) != null)
                        .Select(t => t.Id.ToString());
                }
                else
                {
                    IEnumerable<Tag> tags = DatabaseContext.Tags.ToList();
                    model.Tags = tags.Select(t =>
                    {
                        return new SelectListItem()
                        {
                            Text = t.Name,
                            Value = t.Id.ToString(),
                            Selected = false
                        };
                    });
                }
                return PartialView("_CreateEditInformationBlock", model);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return new HttpNotFoundResult();
            }
        }


        [AllowAccess(new [] { AuthenticationTypeEnum.Employee })]
        [HttpPost]
        public JsonResult CreateInformationBlock(CreateEditInformationBlockModel model)
        {
            try
            {
                InformationBlock informationBlock = new InformationBlock()
                {
                    Title = model.Title,
                    Body = model.Body,
                    Date = DateTime.UtcNow,
                    Employee = (CurrentUser as Employee)
                };
                DatabaseContext.InformationBlocks.Add(informationBlock);
                DatabaseContext.SaveChanges();
                IEnumerable<int> ids = model.TagsIds.Select(t => Convert.ToInt32(t));
                IEnumerable<Tag> tags = ids.Select(tagId =>
                    DatabaseContext.Tags.FirstOrDefault(t => t.Id == tagId));
                List<TagInformationBlock> tagInformationBlocks = tags.Select(t =>
                {
                    return new TagInformationBlock()
                    {
                        Tag = t,
                        InformationBlock = informationBlock
                    };
                }).ToList();
                informationBlock.TagInformationBlocks = tagInformationBlocks;
                DatabaseContext.SaveChanges();
                if (model.Files != null)
                {
                    string rootDirectory = Server.MapPath("~\\Files");
                    string currentInformationBlockDirectory = "\\InformationBlock_" + informationBlock.Id;
                    string informationBlockDirectoryName = rootDirectory + currentInformationBlockDirectory;
                    DirectoryInfo directory = new DirectoryInfo(informationBlockDirectoryName);
                    if (!directory.Exists)
                    {
                        directory.Create();
                        informationBlock.File = currentInformationBlockDirectory;
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


        [AllowAccess(new [] { AuthenticationTypeEnum.Employee })]
        [HttpPost]
        public JsonResult EditInformationBlock(CreateEditInformationBlockModel model)
        {
            try
            {
                InformationBlock informationBlock =
                    DatabaseContext.InformationBlocks.FirstOrDefault(b => b.Id == model.Id.Value);
                if (informationBlock == null)
                {
                    throw new ArgumentException("Выбранной новости не найдено");
                }
                informationBlock.Title = model.Title;
                informationBlock.Body = model.Body;
                IEnumerable<int> ids = model.TagsIds.Select(t => Convert.ToInt32(t));
                IEnumerable<Tag> tags = ids.Select(tagId =>
                    DatabaseContext.Tags.FirstOrDefault(t => t.Id == tagId));
                List<TagInformationBlock> tagInformationBlocks = tags.Select(t =>
                {
                    return new TagInformationBlock()
                    {
                        Tag = t,
                        InformationBlock = informationBlock
                    };
                }).ToList();
                informationBlock.TagInformationBlocks = tagInformationBlocks;
                DatabaseContext.SaveChanges();
                if (model.Files != null)
                {
                    string rootDirectory = Server.MapPath("~\\Files");
                    string currentInformationBlockDirectory = "\\InformationBlock_" + informationBlock.Id;
                    string informationBlockDirectoryName = rootDirectory + currentInformationBlockDirectory;
                    DirectoryInfo directory = new DirectoryInfo(informationBlockDirectoryName);
                    if (!directory.Exists)
                    {
                        directory.Create();
                        informationBlock.File = currentInformationBlockDirectory;
                        DatabaseContext.SaveChanges();
                    }
                    else
                    {
                        foreach (var file in directory.GetFiles())
                        {
                            file.Delete();
                        }
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


        [AllowAccess(new[] { AuthenticationTypeEnum.Employee })]
        [HttpPost]
        public JsonResult RemoveInformationBlock(int informationBlockId)
        {
            try
            {
                InformationBlock informationBlock =
                    DatabaseContext.InformationBlocks.FirstOrDefault(b => b.Id == informationBlockId);
                if (informationBlock == null)
                {
                    throw new ArgumentException("Выбранной новости не найдено");
                }
                DatabaseContext.InformationBlocks.Remove(informationBlock);
                DatabaseContext.SaveChanges();
                return Json(new {result = true});
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return Json(new {result = false});
            }
        }


        [AllowAccess(new [] { AuthenticationTypeEnum.Anonymous, AuthenticationTypeEnum.Resident })]
        [HttpGet]
        public ActionResult RenderInformationBlocks()
        {
            try
            {
                ViewData["currentLink"] = "InformationBlocks";
                return PartialView("_ViewInformationBlocks");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return new HttpNotFoundResult();
            }
        }


        [AllowAccess(new [] { AuthenticationTypeEnum.Anonymous, AuthenticationTypeEnum.Resident })]
        [HttpGet]
        public ActionResult RenderInformationBlocksPage(InformationBlockPageFilterModel filterModel)
        {
            try
            {
                int pageSize = 5;
                IEnumerable<InformationBlock> informationBlocks =
                    DatabaseContext.InformationBlocks.OrderByDescending(b => b.Date).ToList();

                if (CurrentUser is Resident)
                {
                    Resident resident = CurrentUser as Resident;

                    informationBlocks = informationBlocks.Where(b => b.Employee.HouseId == resident.Setting.HouseId);

                    if (filterModel.UserTags)
                    {
                        informationBlocks = informationBlocks.Where(b =>
                        {
                            var informationBlockTags = b.TagInformationBlocks.Where(t => t.InformationBlockId == b.Id)
                                .Select(t => t.TagId);
                            var residentTags = resident.ResidentTags.Where(r => r.ResidentId == resident.Id)
                                .Select(t => t.TagId);
                            var showInformationBlock = informationBlockTags.Any(t => residentTags.Contains(t));
                            return showInformationBlock;
                        });
                    }
                }

                IEnumerable<ViewInformationBlockModel> informationBlocksModels = informationBlocks
                    .Skip((filterModel.Page - 1) * pageSize).Take(pageSize).ToList().Select(
                    b =>
                    {
                        string informationBlockPath = "\\InformationBlock_" + b.Id;
                        IEnumerable<string> files = GetFilesNames(informationBlockPath);
                        return new ViewInformationBlockModel()
                        {
                            Id = b.Id,
                            Title = b.Title,
                            Body = b.Body,
                            Date = b.Date.ToString("g"),
                            Files = files,
                            Tags = b.TagInformationBlocks.Select(t => t.Tag.Name)
                        };
                    });
                PageInfo pageInfo = new PageInfo()
                {
                    PageNumber = filterModel.Page,
                    PageSize = pageSize,
                    TotalItems = informationBlocks.Count()
                };
                ViewInformationBlockPageModel informationBlocksModel = new ViewInformationBlockPageModel()
                {
                    InformationBlocks = informationBlocksModels,
                    PageInfo = pageInfo
                };
                return PartialView("_ViewInformationBlocksPage", informationBlocksModel);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return new HttpNotFoundResult();
            }
        }
    }
}