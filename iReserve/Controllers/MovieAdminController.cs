using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iReserve.DAL;
using iReserve.Models;

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
        // GET: /MovieAdmin/AddMovie

        public ActionResult AddMovie()
        {
            return View();
        }

        //
        // POST: /MovieAdmin/AddMovie

        [HttpPost]
        public ActionResult AddMovie(AddMovie movie)
        {
            MovieAdminDAL agent = new MovieAdminDAL();

            try
            {
                bool result = agent.AddMovie(movie);

                if (result)
                {
                    TempData["message"] = "Movie Created";
                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    ModelState.AddModelError("", "The movie cannot be added");
                    return View(movie);
                }                
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
