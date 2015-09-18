using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int Amount { get; set; }
        public bool Status { get; set; }
    }
}