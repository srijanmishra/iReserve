using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iReserve.Models;

namespace iReserve.ViewModels
{
    public class DeliveryModel
    {
        public List<ViewPartyBookings> bookingCollection { get; set; }
        public List<string> EmployeeIDCollection { get; set; }
    }
}