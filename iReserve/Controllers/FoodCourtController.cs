using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iReserve.Models;
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
    }
}
