
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
        [Required]
        [Display(Name = "Food Court Name")]
        [MaxLength(20, ErrorMessage = "Food Court Name can have only 20 letters")]
        public string FoodCourtName { get; set; }

        [Required]
        [Display(Name = "Caterer Name")]
        [MaxLength(10, ErrorMessage = "Food Court Name can have only 20 letters")]
        public string CatererName { get; set; }

        [Required]
        [Display(Name = "Serving Date")]
        [DataType(DataType.Date)]
        public DateTime ServingDate { get; set; }

        [Required]
        [Display(Name = "Number of Plates")]
        [RegularExpression(@"([1-9][0-9]*)", ErrorMessage = "Number of plates must contain only digits.")]
        [Range(1,100, ErrorMessage="Range is between 1 and 100")]
        public int NumberOfPlates { get; set; }

        [Required]
        [Display(Name = "Dish Name")]
        [MaxLength(20, ErrorMessage = "Dish Name can have only 20 letters")]
        public string DishName { get; set; }
    }

    public class MealSearchModel
    {
        public int MenuId { get; set; }
        public string DishName { get; set; }
        public char DishType { get; set; }
        public string FoodCourtName { get; set; }
        public string CatererName { get; set; }
        public string CatererSpecialty { get; set; }
        public double PricePerPlate { get; set; }
        public int NoOfPlatesAvailable { get; set; }
    }
}