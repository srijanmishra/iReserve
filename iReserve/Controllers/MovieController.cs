using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iReserve.Models;
using iReserve.DAL;
using System.Diagnostics;

namespace iReserve.Controllers
{
    public class MovieController : Controller
    {
        public ActionResult ViewMovieBookings()
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

            MovieDAL agent = new MovieDAL();
            var temp = Session["UserID"];
            var userId = Convert.ToInt32(temp.ToString());
            List<ViewMovieBookings> bookingList = agent.Bookings(userId);

            return View(bookingList);
        }

        public ActionResult ViewCurrentMovies()
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

            MovieDAL agent = new MovieDAL();
            List<ViewCurrentMovies> movieList = agent.CurrentMovies();
            
            return View(movieList);
        }
    }
}
