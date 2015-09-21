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
        public List<int> EmployeeIDCollection { get; set; }
        public List<bool> IsApproved { get; set; }
    }
}