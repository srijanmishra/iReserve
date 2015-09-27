﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iReserve.DAL;
using iReserve.Models;
using iReserve.ViewModels;

namespace iReserve.Controllers
{
    public class MovieAdminController : Controller
    {
        //
        // GET: /MovieAdmin/AddMovie

        public ActionResult AddMovie()
        {
            //Authentication
            string type = (string)Session["UserRole"];
            if (type == null)
            {
                return RedirectToAction("Register", "UserAccount");
            }
            else if (type.CompareTo("M") != 0)
            {
                Session["UserID"] = null;
                Session["UserRole"] = null;
                return RedirectToAction("Login", "UserAccount");
            }

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

        public ActionResult RemoveMovie()
        {
            //Authentication
            string type = (string)Session["UserRole"];
            if (type == null)
            {
                return RedirectToAction("Register", "UserAccount");
            }
            else if (type.CompareTo("M") != 0)
            {
                Session["UserID"] = null;
                Session["UserRole"] = null;
                return RedirectToAction("Login", "UserAccount");
            }

            MovieAdminDAL agent = new MovieAdminDAL();
            MovieViewModel list = agent.MovieShows();
            return View(list);
        }

        //
        // POST: /FoodCourtAdmin/RemoveMenuItem_Confirmed

        [OutputCache(Duration = 0)]
        public string RemoveMovieItemConfirmed(int movieId, bool matinee, bool evening, bool night)
        {
            MovieAdminDAL agent = new MovieAdminDAL();
            bool res1 = false, res2 = false, res3 = false;
            if (matinee)
                res1 = agent.RemoveShow(movieId, "Matinee");
            if (evening)
                res2 = agent.RemoveShow(movieId, "Evening");
            if (night)
                res3 = agent.RemoveShow(movieId, "Night");

            if (res1 && res2 && res3)
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
