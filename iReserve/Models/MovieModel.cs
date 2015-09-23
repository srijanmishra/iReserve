using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iReserve.Models
{
    public class ViewMovieBookings
    {
        public int BookingId { get; set; }
        public int EmployeeNumber { get; set; }
        public string MovieName { get; set; }
        public string ShowDate { get; set; }
        public string Show { get; set; }
        public string BookingDate { get; set; }
        public int NumberOfSeats { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public double Amount { get; set; }
        public bool Status { get; set; }
    }

    public class ViewCurrentMovies
    {
        public int MovieID { get; set; }
        public string MovieName { get; set; }
        public string Language { get; set; }
        public string ShowDate { get; set; }
    }

    public class AddMovie
    {
        [Required]
        [Display(Name="Movie Name")]
        public string MovieName { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name="Show Date")]
        public DateTime ShowDate { get; set; }
        
        [Display(Name="Show Timings")]
        public string Show { get; set; }
        
        [Required]
        [Display(Name="Language")]
        public string Language { get; set; }
        
        [Required]
        [Display(Name="Movie Cost")]
        [RegularExpression(@"([1-9][0-9]*)", ErrorMessage = "Cost must contain only digits.")]
        public decimal Cost { get; set; }
        
        
        public int BookedTickets { get; set; }
    }
}