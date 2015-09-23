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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewPartyBookings()
        {
            PartyDAL agent = new PartyDAL();
            var temp = Session["UserID"];
            var userId = Convert.ToInt32(temp.ToString());
            List<ViewPartyBookings> bookingList = agent.Bookings(userId, "");

            return View(bookingList);
        }
    }
}
