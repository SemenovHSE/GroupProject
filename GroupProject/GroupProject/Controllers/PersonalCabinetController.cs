using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GroupProject.Database.ModelsGenerated;
using GroupProject.Models;
using Newtonsoft.Json;

namespace GroupProject.Controllers
{
    public class PersonalCabinetController : BaseController
    {
        // GET: PersonalCabinet
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult RenderResidentGeneralInformation()
        {
            try
            {
                Resident resident = CurrentUser as Resident;
                Setting setting = resident.Setting;
                House house = resident.Setting.House;
                GeneralInformationModel model = new GeneralInformationModel()
                {
                    FullName = resident.FullName,
                    PhoneNumber = resident.PhoneNumber,
                    Address = house.Address,
                    SettingNumber = setting.SettingNumber,
                    Entrance = setting.EntranceNumber,
                    Size = setting.Size,
                    RoomsNumber = setting.RoomsNumber
                };
                return PartialView("_GeneralInformation", model);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return new HttpNotFoundResult();
            }
        }


        [HttpGet]
        public ActionResult RenderResidentRequests()
        {
            try
            {
                return PartialView("_ResidentRequests");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return new HttpNotFoundResult();
            }
        }


        [HttpGet]
        public string GetRequests()
        {
            try
            {
                List<RequestModel> requestsModels = new List<RequestModel>();
                List<Request> requests = DatabaseContext.Requests.ToList();
                requests.ForEach(r =>
                {
                    RequestModel model = new RequestModel()
                    {
                        Id = requestsModels.Count,
                        RequestId = r.Id,
                        Theme = r.Theme,
                        Status = r.Status.Name,
                        Date = r.Date.ToString("g"),
                        ViewAction = "Посмотреть"
                    };
                    requestsModels.Add(model);
                });
                var dataSource = new { total = 1, page = 1, records = 3, rows = requestsModels };
                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return JsonConvert.SerializeObject(string.Empty);
            }
        }
    }
}