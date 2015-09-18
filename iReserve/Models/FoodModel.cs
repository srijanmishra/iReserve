using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iReserve.Models
{
    public class ViewMealBookings
    {
        public int BookingId { get; set; }
        public string FoodCourtName { get; set; }
        public string CatererName { get; set; }
        public string DishName { get; set; }
        public double PricePerPlate { get; set; }
        public int NumberOfPlates { get; set; }
        public double TotalAmount { get; set; }
        public string DateOfBooking { get; set; }
    }
}