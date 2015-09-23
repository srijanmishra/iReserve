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
        //
        // GET: /Movie/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewMovieBookings()
        {
            MovieDAL agent = new MovieDAL();
            var temp = Session["UserID"];
            var userId = Convert.ToInt32(temp.ToString());
            List<ViewMovieBookings> bookingList = agent.Bookings(userId);

            return View(bookingList);
        }

        public ActionResult ViewCurrentMovies()
        {
            MovieDAL agent = new MovieDAL();
            List<ViewCurrentMovies> movieList = agent.CurrentMovies();
            //Console.WriteLine(movieList);
            Debug.WriteLine(movieList);
            
            return View(movieList);
        }
    }
}
