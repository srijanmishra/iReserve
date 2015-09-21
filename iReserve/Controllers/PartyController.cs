using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iReserve.DAL;
using iReserve.Models;

namespace iReserve.Controllers
{
    public class PartyController : Controller
    {
        //
        // GET: /Party/

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult ViewPartyBookings()
        {
            PartyDAL agent = new PartyDAL();
            var userid = Convert.ToInt32(Session["UserID"].ToString());
            List<ViewPartyBookings> bookingList = agent.Bookings(10001, "");

            return View(bookingList);
        }
    }
}
