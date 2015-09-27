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
        
        [Required]
        public DateTime EventDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
        [RegularExpression(@"(?=.)^\$?(([1-9][0-9]{0,2})|[0-9]+)?(\.[0-9]{1,2})?$", ErrorMessage = "Cost can contain only numbers and a decimal point.")]
        public double Cost { get; set; }
    }

    public class VenueNameId
    {
        public string VenueName { get; set; }
        public int VenueID { get; set; }
    }

    public class VenueDetails
    {
        public AddVenue VenueItem { get; set; }
        public int VenueID { get; set; }
    }
}