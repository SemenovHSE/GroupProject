using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GroupProject.Models;
using GroupProject.Authentication;
using GroupProject.Database.ModelsGenerated;

namespace GroupProject.Controllers
{
    public class MenuController : BaseController
    {
        // GET: Menu
        public ActionResult Index()
        {
            return RedirectToAction("About");
        }


        [HttpGet]
        public ActionResult About()
        {
            return View();
        }


        [HttpGet]
        public ActionResult PersonalCabinet()
        {
            if (Authentication.CurrentUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "PersonalCabinet");
            }
            return RedirectToAction("Login", "Menu");
        }


        [HttpGet]
        public ActionResult Register()
        {
            List<string> addresses = DatabaseContext.Houses.Select(h => h.Address).ToList();
            ViewData["Addresses"] = addresses;
            return View(new RegistrationModel());
        }


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


        [HttpGet]
        public ActionResult Login()
        {
            return View(new LoginModel());
        }


        [HttpGet]
        public ActionResult LoginInformation()
        {
            return PartialView("_LoginInformation", CurrentUser);
        }


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
            return View(login);
        }


        public ActionResult Logout()
        {
            Authentication.Logout();
            return RedirectToAction("About", "Menu");
        }

        [AllowAccess(new[] { AuthenticationTypeEnum.Employee })]
        public ContentResult Test()
        {
            return Content("ПРИВЕТ!");
        }
    }
}