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
            return View();
        }

        //
        // POST: /UserAccount/Login

        [HttpPost]
        public ActionResult Login(UserLoginModel login)
        {
            UserAccountDAL agent = new UserAccountDAL();
            try
            {
                bool res = agent.IDCheck(login.UserName, login.Password);
                if (res)
                {
                    res = agent.RoleCheck(login.UserName, login.Password, login.Role);
                    if (res)
                    {
                        Session["UserID"] = login.UserName;
                        if (login.Role.Equals("D"))
                        {
                            return RedirectToAction("DeliveryIndex", "Home");
                        }

                        /*
                        else if (login.Role.Equals("F"))
                        {
                            return RedirectToAction("FoodCourtIndex", "Home");
                        }

                        else if (login.Role.Equals("M"))
                        {
                            return RedirectToAction("MovieIndex", "Home");
                        }

                        else if (login.Role.Equals("P"))
                        {
                            return RedirectToAction("PartyIndex", "Home");
                        }
                        */

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
        public ActionResult Register(UserRegisterModel regUser)
        {
            UserAccountDAL agent = new UserAccountDAL();
            
            try
            {
                bool res = agent.NewUserRegister(regUser);
                if (res)
                {
                    Session["UserID"] = regUser.EmployeeID;
                    TempData["message"] = "Password generated";
                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    return View(regUser);
                }

            }
            catch
            {
                return View();
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
            return View();
        }

        //
        // POST: /UserAccount/ChangePassword

        [HttpPost]
        public ActionResult ChangePassword(PasswordChangeModel pswdChange)
        {
            UserAccountDAL agent = new UserAccountDAL();

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

            return RedirectToAction("Login", "UserAccount");
        }
    }
}
