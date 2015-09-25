using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iReserve.DAL;
using iReserve.Models;

namespace iReserve.Controllers
{
    public class PartyAdminController : Controller
    {
        //
        // GET: /PartyAdmin/Create

        public ActionResult AddVenue()
        {
            //Authentication
            string type = (string)Session["UserRole"];
            if (type == null)
            {
                return RedirectToAction("Register", "UserAccount");
            }
            else if (type.CompareTo("P") != 0)
            {
                Session["UserID"] = null;
                Session["UserRole"] = null;
                return RedirectToAction("Login", "UserAccount");
            }

            return View();
        }

        //
        // POST: /PartyAdmin/Create

        [HttpPost]
        public string AddVenue(string VN, string VA, string VC)
        {
            AddVenue objVenue = new Models.AddVenue();

            objVenue.VenueName = VN;
            objVenue.VenueAddress = VA;
            objVenue.VenueCapacity = Convert.ToInt32(VC);

            PartyAdminDAL agent = new PartyAdminDAL();

            try
            {
                bool result = agent.AddVenue(objVenue);

                if (result)
                {
                    return "DONE";
                }

                else
                {
                    return "ERROR";
                }
            }

            catch (Exception)
            {
                return "ERROR";
            }
        }

        public ActionResult RemoveVenue()
        {
            //Authentication
            string type = (string)Session["UserRole"];
            if (type == null)
            {
                return RedirectToAction("Register", "UserAccount");
            }
            else if (type.CompareTo("P") != 0)
            {
                Session["UserID"] = null;
                Session["UserRole"] = null;
                return RedirectToAction("Login", "UserAccount");
            }

            PartyAdminDAL agent = new PartyAdminDAL();
            Dictionary<int, AddVenue> list = agent.VenueList();
            return View(list);
        }

        //
        // POST: /FoodCourtAdmin/RemoveMenuItem_Confirmed

        [OutputCache(Duration = 0)]
        public string RemoveVenueConfirmed(int venueId)
        {
            PartyAdminDAL agent = new PartyAdminDAL();

            bool res = agent.RemoveVenue(venueId);

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
