using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iReserve.Models;

namespace iReserve.ViewModels
{
    public class AllBookings
    {
        public List<ViewMovieBookings> MovieBookings { get; set; }
        public List<ViewMealBookings> FoodBookings { get; set; }
        public List<ViewPartyBookings> PartyBookings { get; set; }
    }
}