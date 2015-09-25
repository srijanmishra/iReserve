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

        public ActionResult ViewAll()
        {
            //Authentication
            string type = (string)Session["UserRole"];
            if (type == null)
            {
                return RedirectToAction("Register", "UserAccount");
            }
            else if (type.CompareTo("F") != 0)
            {
                Session["UserID"] = null;
                Session["UserRole"] = null;
                return RedirectToAction("Login", "UserAccount");
            }

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

            //Authentication
            string type = (string)Session["UserRole"];
            if (type == null)
            {
                return RedirectToAction("Register", "UserAccount");
            }
            else if (type.CompareTo("D") != 0)
            {
                Session["UserID"] = null;
                Session["UserRole"] = null;
                return RedirectToAction("Login", "UserAccount");
            }

            DeliveryDAL agent = new DeliveryDAL();
            DeliveryModel bookings = agent.Bookings();
            return View(bookings);
        }

        [OutputCache(Duration = 0)]
        public string UpdateStatus(int bookingID, string status)
        {
            DeliveryDAL agent = new DeliveryDAL();
            bool res = agent.UpdateBookingStatus(bookingID, status);

            if (res)
            {
                return "DONE";
            }

            else
            {
                return "ERROR";
            }
        }
    }
}
