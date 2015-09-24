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

        [Authorize]
        public ActionResult ViewVenues()
        {
            PartyDAL agent = new PartyDAL();
            var venueList = agent.ViewVenues();
            return View(venueList);
        }

        [Authorize]
        public ActionResult MakeBooking(int venueId, DateTime eventDate, double cost)
        {
            PartyDAL agent = new PartyDAL();

            try
            {
                PartyBooking booking =  new PartyBooking();
                booking.EmployeeID = Convert.ToInt32(Session["UserID"].ToString());
                booking.VenueID = venueId;
                booking.EventDate = eventDate;
                booking.Cost = cost;
                bool res = agent.MakeBooking(booking);
                if (res)
                {
                    TempData["message"] = "The booking is successful";
                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    ModelState.AddModelError("", "The booking failed");
                    return View(booking);
                }

            }
            catch
            {
                return View();
            }         
        }
    }
}
