using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iReserve.Models;
using iReserve.ViewModels;
using iReserve.DAL;
using System.Diagnostics;

namespace iReserve.Controllers
{
    public class FoodCourtController : Controller
    {
        public ActionResult ViewFoodBookings()
        {
            //Authentication
            string type = (string)Session["UserRole"];
            if (type == null)
            {
                return RedirectToAction("Register", "UserAccount");
            }
            else if (type.CompareTo("U") != 0)
            {
                Session["UserID"] = null;
                Session["UserRole"] = null;
                return RedirectToAction("Login", "UserAccount");
            }

            FoodDAL agent = new FoodDAL();
            var temp = Session["UserID"];
            var userId = Convert.ToInt32(temp.ToString());
            List<ViewMealBookings> bookingList = agent.MealBookings(userId);

            return View(bookingList);
        }

        public ActionResult SearchMeal()
        {
            //Authentication
            string type = (string)Session["UserRole"];
            if (type == null)
            {
                return RedirectToAction("Register", "UserAccount");
            }
            else if (type.CompareTo("U") != 0)
            {
                Session["UserID"] = null;
                Session["UserRole"] = null;
                return RedirectToAction("Login", "UserAccount");
            }

            return View();
        }

        public ActionResult GetLists(string option)
        {
            int searchType = Convert.ToInt32(option);

            List<string> collectedList = new List<string>();

            FoodCourtAdminDAL agent = new FoodCourtAdminDAL();

            switch (searchType)
            {
                case 1:
                    collectedList = agent.FoodCourts();
                    break;
                case 2:
                    collectedList = agent.Caterers();
                    break;
                case 3:
                    collectedList = agent.GetSpecialties();
                    break;
            }

            ViewBag.list = collectedList;
            ViewBag.option = searchType;

            return PartialView("_SearchMealPartial");
        }

        public ActionResult SearchMenus(string optionNumber, string optionValue)
        {
            int optNo = Convert.ToInt32(optionNumber);
            List<MealSearchModel> searchMenu = new List<MealSearchModel>();
            FoodDAL agent = new FoodDAL();

            switch (optNo)
            {
                case 1:
                    searchMenu = agent.SearchByFoodCourt(optionValue);
                    break;
                case 2:
                    searchMenu = agent.SearchByCaterer(optionValue);
                    break;
                case 3:
                    searchMenu = agent.SearchBySpecialty(optionValue);
                    break;
                case 4:
                    bool option = Convert.ToBoolean(optionValue);
                    searchMenu = agent.SearchByDishType(option);
                    break;
            }

            ViewBag.option = optNo;

            return PartialView("_SearchResultsPartial", searchMenu);
        }

        public void MakeMealBooking(string menuId, string dishName, string pricePerPlate)
        {
            MakeBookingDetails obj = new MakeBookingDetails();

            obj.EmployeeId = Convert.ToInt32(Session["UserID"]);
            obj.MenuId = Convert.ToInt32(menuId);
            obj.PricePerPlate = Convert.ToDouble(pricePerPlate);
            obj.DishName = dishName;

            TempData["model"] = obj;

            //return RedirectToAction("BookMeal");
        }
               
        public ActionResult BookMeal()
        {
            //Authentication
            string type = (string)Session["UserRole"];
            if (type == null)
            {
                return RedirectToAction("Register", "UserAccount");
            }
            else if (type.CompareTo("U") != 0)
            {
                Session["UserID"] = null;
                Session["UserRole"] = null;
                return RedirectToAction("Login", "UserAccount");
            }

            MakeBookingDetails obj = (MakeBookingDetails)TempData["model"];
            return View(obj);
        }

        public string BookMealIntoDatabase(string EmpId, string MenuId, string NoOfPlates, string Cost)
        {
            FoodDAL agent = new FoodDAL();

            MakeBookingDetails obj = new MakeBookingDetails();

            obj.EmployeeId = Convert.ToInt32(EmpId);
            obj.MenuId = Convert.ToInt32(MenuId);
            obj.NumberOfPlates = Convert.ToInt32(NoOfPlates);
            obj.DateOfBooking = DateTime.Today.ToShortDateString();
            obj.TotalAmount = Convert.ToDouble(Cost);

            bool res = agent.InsertBookingIntoDatabase(obj);

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
