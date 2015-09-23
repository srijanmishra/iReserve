﻿using System;
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

        [Authorize]
        public ActionResult UpdateMenu()
        {
            FoodCourtAdminDAL agent = new FoodCourtAdminDAL();
            MenuViewModel list = agent.GetMenus();
            
            return View(list);
        }

        [Authorize]
        public ActionResult GetMenuDetailsForUpdate(string menuID, string FCName, string CName, string DName, string SDate, string NoP)
        {
            FoodCourtAdminDAL agent = new FoodCourtAdminDAL();
            UpdateMenuDetails obj = new UpdateMenuDetails();
            obj.MenuItem = new MenuDetails();

            obj.MenuID = Convert.ToInt32(menuID);
            obj.MenuItem.FoodCourtName = FCName;
            obj.MenuItem.CatererName = CName;
            obj.MenuItem.DishName = DName;
            obj.MenuItem.ServingDate = Convert.ToDateTime(SDate);
            obj.MenuItem.NumberOfPlates =Convert.ToInt32(NoP);

            List<string> tempList = new List<string>();

            tempList = agent.FoodCourts();
            tempList.Remove(FCName);
            ViewBag.list1 = tempList;

            tempList = agent.Caterers();
            tempList.Remove(CName);
            ViewBag.list2 = tempList;

            tempList = agent.Dishes();
            tempList.Remove(CName);
            ViewBag.list3 = tempList;

            return PartialView("_AddMenuPartial", obj);
        }

        [Authorize]
        public string UpdateMenuDetails(string menuID, string FCName, string CName, string DName, string SDate, string NoP)
        {
            FoodCourtAdminDAL agent = new FoodCourtAdminDAL();
            UpdateMenuDetails obj = new UpdateMenuDetails();
            obj.MenuItem = new MenuDetails();

            obj.MenuID = Convert.ToInt32(menuID);
            obj.MenuItem.FoodCourtName = FCName;
            obj.MenuItem.CatererName = CName;
            obj.MenuItem.DishName = DName;
            obj.MenuItem.ServingDate = Convert.ToDateTime(SDate);
            obj.MenuItem.NumberOfPlates = Convert.ToInt32(NoP);

            bool res = agent.UpdateMenuItem(obj);

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
