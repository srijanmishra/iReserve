using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iReserve.Controllers
{
    public class MovieAdminController : Controller
    {
        //
        // GET: /MovieAdmin/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /MovieAdmin/Add

        public ActionResult AddMovie()
        {
            return View();
        }

        //
        // POST: /MovieAdmin/Add

        [HttpPost]
        public ActionResult Add(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /MovieAdmin/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /MovieAdmin/Delete/5

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
