using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iReserve.Models;
using iReserve.ViewModels;
using iReserve.DAL;

namespace iReserve.Controllers
{
    public class FoodCourtController : Controller
    {
        //
        // GET: /FoodCourt/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewFoodBookings()
        {
            FoodDAL agent = new FoodDAL();
            var temp = Session["UserID"];
            var userId = Convert.ToInt32(temp.ToString());
            List<ViewMealBookings> bookingList = agent.MealBookings(userId);

            return View(bookingList);
        }

        public ActionResult SearchMeal()
        {
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
    }
}
