using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var temp = Session["UserID"];
            var userId = Convert.ToInt32(temp.ToString());
            MovieDAL agent1 = new MovieDAL();
            FoodDAL agent2 = new FoodDAL();
            PartyDAL agent3 = new PartyDAL();
            
            AllBookings bookingCollection = new AllBookings();

            bookingCollection.MovieBookings = agent1.Bookings(userId);
            bookingCollection.FoodBookings = agent2.MealBookings(userId);
            bookingCollection.PartyBookings = agent3.Bookings(userId, "");

            return View(bookingCollection);
        }

        [OutputCache(Duration = 0)]
        public ActionResult DeliveryIndex()
        {
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            DeliveryDAL agent = new DeliveryDAL();
            DeliveryModel bookings = agent.Bookings();
            return View(bookings);
        }

        [OutputCache(Duration = 0)]
        public void UpdateStatus(int bookingID, string status)
        {
            DeliveryDAL agent = new DeliveryDAL();
            bool res = agent.UpdateBookingStatus(bookingID, status);

            if (res)
            {
                Response.Redirect("/Home/DeliveryIndex");
            }

            else
            {
                Debug.WriteLine("ERROR");
            }
        }
    }
}
