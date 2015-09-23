using System;
using System.Collections.Generic;
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
        // GET: /PartyAdmin/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /PartyAdmin/Create

        public ActionResult AddVenue()
        {
            return View();
        }

        //
        // POST: /PartyAdmin/Create

        [HttpPost]
        public string AddVenue(string VN, string VA, string VC)
        {
            AddVenue objVenue = new AddVenue();

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

        //
        // GET: /PartyAdmin/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /PartyAdmin/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /PartyAdmin/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /PartyAdmin/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
