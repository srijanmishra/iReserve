using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iReserve.DAL;
using iReserve.ViewModels;

namespace iReserve.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ViewAll()
        {
            var userid = Convert.ToInt32(Session["UserID"].ToString());
            MovieDAL agent1 = new MovieDAL();
            FoodDAL agent2 = new FoodDAL();
            PartyDAL agent3 = new PartyDAL();
            
            AllBookings bookingCollection = new AllBookings();

            bookingCollection.MovieBookings = agent1.Bookings(10001);
            bookingCollection.FoodBookings = agent2.MealBookings(10001);
            bookingCollection.PartyBookings = agent3.Bookings(10001);

            return View(bookingCollection);
        }

        public ActionResult DeliveryIndex()
        {
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            DeliveryDAL agent = new DeliveryDAL();
            DeliveryModel bookings = agent.Bookings();
            return View(bookings);
        }

    }
}
