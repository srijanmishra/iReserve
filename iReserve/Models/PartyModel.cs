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
}