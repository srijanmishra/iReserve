using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using iReserve.Models;
using iReserve.DAL;
using System.Diagnostics;

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
    }
}
