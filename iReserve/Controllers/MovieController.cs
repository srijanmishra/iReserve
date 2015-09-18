using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iReserve.Models;
using iReserve.DAL;

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
            List<ViewMovieBookings> bookingList = agent.Bookings(10001);

            return View();
        }

    }
}
