using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iReserve.Models
{
    public class ViewPartyBookings
    {
        public int BookingId { get; set; }
        public string VenueName { get; set; }
        public string BookingDate { get; set; }
        public string EventDate { get; set; }
        public double Amount { get; set; }
        public string ApprovalStatus { get; set; }
    }

    public class AddVenue
    {
        [Required]
        public string VenueName { get; set; }
        [Required]
        public string VenueAddress { get; set; }
        [Required]
        [Range(1, 200)]
        public int VenueCapacity { get; set; }
    }

    public class PartyBooking
    {
        public int EmployeeID { get; set; }
        public int VenueID { get; set; }
        public DateTime EventDate { get; set; }
        public double Cost { get; set; }
    }

    public class VenueDetails
    {
        public AddVenue VenueDetails { get; set; }
        public int VenueID { get; set; }
    }
}