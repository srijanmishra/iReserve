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

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult ViewMovieBookings()
        {
            MovieDAL agent = new MovieDAL();
            var userid = Convert.ToInt32(Session["UserID"].ToString());
            List<ViewMovieBookings> bookingList = agent.Bookings(userid);

            return View(bookingList);
        }

        [Authorize]
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
