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
        public ActionResult ViewPartyBookings()
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

            PartyDAL agent = new PartyDAL();
            var userid = Convert.ToInt32(Session["UserID"].ToString());
            List<ViewPartyBookings> bookingList = agent.Bookings(10001, "");

            return View(bookingList);
        }

        public ActionResult ViewVenues()
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

            PartyDAL agent = new PartyDAL();
            var venueList = agent.ViewVenues();
            return View(venueList);
        }

        public ActionResult BookParty()
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

            PartyDAL agent = new PartyDAL();
            ViewBag.list1 = agent.GetVenues();

            return View();
        }
        
        public string MakeBooking(string VenueName, string EventDate, string Cost)
        {
            PartyDAL agent = new PartyDAL();
            var venueid = agent.FindVenueId(VenueName);

            PartyBooking booking =  new PartyBooking();
            booking.EmployeeID = Convert.ToInt32(Session["UserID"].ToString());
            booking.VenueID = Convert.ToInt32(venueid);
            booking.EventDate = Convert.ToDateTime(EventDate);
            booking.Cost = Convert.ToDouble(Cost);
            bool res = agent.MakeBooking(booking);
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
