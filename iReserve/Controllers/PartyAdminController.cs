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
        public ActionResult AddVenue(AddVenue venue)
        {
            PartyAdminDAL agent = new PartyAdminDAL();

            try
            {
                bool result = agent.AddVenue(venue);

                if (result)
                {
                    TempData["message"] = "Venue Created";
                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    ModelState.AddModelError("", "The venue cannot be added");
                    return View(venue);
                }
            }
            catch
            {
                return View();
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
