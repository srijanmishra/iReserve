using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using iReserve.DAL;
using iReserve.Models;

namespace iReserve.Controllers
{
    public class UserAccountController : Controller
    {
        //
        // GET: /UserAccount/Login
        public ActionResult Login()
        {
            Session["UserID"] = null;
            Session["UserRole"] = null;

            FormsAuthentication.SignOut();
            return View();
        }

        //
        // POST: /UserAccount/Login

        [HttpPost]
        public ActionResult Login(UserLoginModel login)
        {
            UserAccountDAL agent = new UserAccountDAL();
            login.Password = PasswordGenerator.EncryptPassword(login.Password);
            
            try
            {
                bool res = agent.IDCheck(login.UserName, login.Password);
                if (res)
                {
                    res = agent.RoleCheck(login.UserName, login.Password, login.Role);
                    if (res)
                    {
                        FormsAuthentication.SetAuthCookie(login.UserName, false);
                        Session["UserID"] = login.UserName;
                        Session["UserRole"] = login.Role;

                        if (login.Role.Equals("D"))
                        {
                            return RedirectToAction("DeliveryIndex", "Home");
                        }

                        else if (login.Role.Equals("F"))
                        {
                            return RedirectToAction("AddMenu", "FoodCourtAdmin");
                        }

                        
                        else if (login.Role.Equals("M"))
                        {
                            return RedirectToAction("AddMovie", "MovieAdmin");
                        }
                       
                        else if (login.Role.Equals("P"))
                        {
                            return RedirectToAction("AddVenue", "PartyAdmin");
                        }

                        else if (login.Role.Equals("U"))
                        {
                            return RedirectToAction("Index", "Home");
                        }

                        else
                        {
                            ModelState.AddModelError("", "You do not have admin priveleges. Role unverified.");
                            return View(login);
                        }

                    }

                    else
                    {
                        ModelState.AddModelError("", "You do not have admin priveleges. Role unverified.");
                        return View(login);
                    }
                }

                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    return View(login);
                }
            }

            catch
            {

                return View();
            }
        }

        //
        // GET: /UserAccount/Register
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /UserAccount/Login

        [HttpPost]
        public string Register(string UserId, string UserName, string JoiningDate, string Password, string EmailId, string PhoneNumber)
        {
            UserAccountDAL agent = new UserAccountDAL();
            UserRegisterModel regUser = new UserRegisterModel();

            regUser.EmployeeID = UserId;
            regUser.Name = UserName;
            regUser.DateOfJoining = Convert.ToDateTime(JoiningDate);
            regUser.PhoneNumber = PhoneNumber;
            regUser.EmailId = EmailId;
            regUser.Password = PasswordGenerator.EncryptPassword(Password);
            
            try
            {
                bool res = agent.NewUserRegister(regUser);
                if (res)
                {
                    FormsAuthentication.SetAuthCookie(regUser.EmployeeID, false);
                    Session["UserID"] = regUser.EmployeeID;
                    Session["UserRole"] = "U";
                    return "DONE";
                }

                else
                {
                    //ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    //return View(regUser);
                    return "ERROR: Registration incorrect";
                }

            }
            catch
            {
                return "ERROR: Registration incorrect";
            }
        }

        public JsonResult CheckName(string userName)
        {

            UserAccountDAL agent = new UserAccountDAL();
            bool res = agent.NameCheck(userName, "");

            if (res)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // GET: /UserAccount/ChangePassword

        public ActionResult ChangePassword()
        {
            //Authentication
            string type = (string)Session["UserRole"];
            if (type == null)
            {
                return RedirectToAction("Register", "UserAccount");
            }
            
            return View();
        }

        //
        // POST: /UserAccount/ChangePassword

        [HttpPost]
        public ActionResult ChangePassword(PasswordChangeModel pswdChange)
        {
            UserAccountDAL agent = new UserAccountDAL();
            pswdChange.NewPassword = PasswordGenerator.EncryptPassword(pswdChange.NewPassword);

            bool res = agent.PasswordChanger(Session["UserID"].ToString(), pswdChange.OldPassword, pswdChange.NewPassword);

            if (res)
            {
                return RedirectToAction("Index", "Home");
            }

            else
            {
                return View(pswdChange);
            }
        }

        //
        // POST: /UserAccount/LogOff

        [HttpPost]
        public ActionResult LogOff()
        {
            Session["UserID"] = null;
            Session["UserRole"] = null;

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "UserAccount");
        }

        public string GetPassword()
        {
            return PasswordGenerator.Generate(6);
        }
    }
}
