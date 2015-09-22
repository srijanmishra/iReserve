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

    public class MenuDetails
    {
        [DataType(DataType.Text)]
        [Display(Name = "Food Court Name")]
        [Required]
        [MaxLength(20, ErrorMessage = "Food Court Name can have only 20 letters")]
        public string FoodCourtName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Caterer Name")]
        [Required]
        [MaxLength(10, ErrorMessage = "Food Court Name can have only 20 letters")]
        public string CatererName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Serving Date")]
        [Required]
        public DateTime ServingDate { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Number of Plates")]
        [Required]
        public int NumberOfPlates { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Dish Name")]
        [Required]
        [MaxLength(20, ErrorMessage = "Dish Name can have only 20 letters")]
        public string DishName { get; set; }
    }
}