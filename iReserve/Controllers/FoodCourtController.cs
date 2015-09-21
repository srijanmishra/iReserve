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

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult ViewFoodBookings()
        {
            FoodDAL agent = new FoodDAL();
            var userid = Convert.ToInt32(Session["UserID"].ToString());
            List<ViewMealBookings> bookingList = agent.MealBookings(10001);

            return View(bookingList);
        }
    }
}
