using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
                bool res = agent.LoginCheck(login);
                if (res)
                {
                    return RedirectToAction("Index", "Home");
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
                //regUser.Password = "TESTER";
                bool res = agent.NewUserRegister(regUser);
                if (res)
                {
                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    //regUser.Password = "";
                    return View(regUser);
                }

            }
            catch
            {
                //regUser.Password = "";
                return View();
            }
        }
    }
}
