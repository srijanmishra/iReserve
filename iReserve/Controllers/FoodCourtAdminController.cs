using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using iReserve.Models;
using iReserve.DAL;
using iReserve.ViewModels;

namespace iReserve.Controllers
{
    [Authorize]
    public class FoodCourtAdminController : Controller
    {
        //
        // GET: /FoodCourtAdmin/AddMenu

        [Authorize]
        public ActionResult AddMenu()
        {
            FoodCourtAdminDAL agent = new FoodCourtAdminDAL();
            ViewBag.list1 = agent.FoodCourts();
            ViewBag.list2 = agent.Caterers();
            ViewBag.list3 = agent.Dishes();
            
            return View();
        }

        //
        // POST: /FoodCourtAdmin/InsertMenuDetails
        [Authorize]
        public string InsertMenuDetails(object sender, EventArgs evtArgs)
        {
            FoodCourtAdminDAL agent = new FoodCourtAdminDAL();

            DateTime temp1 = Convert.ToDateTime(Request.Form["SD"]);
            int temp2 = Convert.ToInt32(Request.Form["NP"]);
            
            bool res = agent.AddMenuDetails(Request.Form["FCN"], Request.Form["CN"], temp1, temp2, Request.Form["DN"]);

            if (res)
            {
                return ("DONE");
            }

            else
            {
                return ("ERROR");
            }
        }

        //
        // GET: /FoodCourtAdmin/RemoveMenu

        [Authorize]
        public ActionResult RemoveMenu()
        {
            FoodCourtAdminDAL agent = new FoodCourtAdminDAL();
            MenuViewModel list = agent.GetMenus();
            return View(list);
        }

        //
        // POST: /FoodCourtAdmin/RemoveMenuItem_Confirmed

        [OutputCache(Duration = 0)]
        [Authorize]
        public void RemoveMenuItem_Confirmed(int menuID)
        {
            FoodCourtAdminDAL agent = new FoodCourtAdminDAL();

            bool res = agent.RemoveMenuItemFromTable(menuID);

            if (res)
            {
                Response.Redirect("/FoodCourtAdmin/RemoveMenu");
            }

            else
            {
                Debug.WriteLine("ERROR");
            }
        }
    }
}
