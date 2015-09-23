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
    public class MovieAdminController : Controller
    {
        //
        // GET: /MovieAdmin/

        public ActionResult Index()
        {
            return RedirectToAction("AddMovie");
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
        public string AddMovie(string MovieName, string ShowDate, string MatineeTime, string EveningTime, string NightTime, string MovieLang, string MovieCost)
        {
            MovieAdminDAL agent = new MovieAdminDAL();

            AddMovie obj = new Models.AddMovie();
            bool result = false;

            obj.MovieName = MovieName;
            obj.ShowDate = Convert.ToDateTime(ShowDate);
            obj.Language = MovieLang;
            obj.Cost = Convert.ToDecimal(MovieCost);

            bool timing1 = Convert.ToBoolean(MatineeTime);
            bool timing2 = Convert.ToBoolean(EveningTime);
            bool timing3 = Convert.ToBoolean(NightTime);
            
            try
            {
                if (timing1)
                {
                    obj.Show = "Matinee";
                    result = agent.AddMovie(obj);
                    if (!result)
                    {
                        return "ERROR";
                    }
                }

                if (timing2)
                {
                    obj.Show = "Evening";
                    result = agent.AddMovie(obj);
                    if (!result)
                    {
                        return "ERROR";
                    }
                }

                if (timing3)
                {
                    obj.Show = "Night";
                    result = agent.AddMovie(obj);
                    if (!result)
                    {
                        return "ERROR";
                    }
                }

                return "DONE";                
            }
            catch (Exception)
            {
                return "ERROR"; ;
            }
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

        [Authorize]
        public ActionResult RemoveMovie()
        {
            MovieAdminDAL agent = new MovieAdminDAL();
            Dictionary<int, AddMovie> list = agent.MovieShows();
            return View(list);
        }

        //
        // POST: /FoodCourtAdmin/RemoveMenuItem_Confirmed

        [OutputCache(Duration = 0)]
        [Authorize]
        public void RemoveMovieItemConfirmed(int movieId)
        {
            MovieAdminDAL agent = new MovieAdminDAL();

            bool res = agent.RemoveShow(movieId);

            if (res)
            {
                Response.Redirect("/MovieAdmin/RemoveMovieShow");
            }

            else
            {
                Debug.WriteLine("ERROR");
            }
        }
    }
}
