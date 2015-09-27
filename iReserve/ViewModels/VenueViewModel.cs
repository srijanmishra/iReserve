using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iReserve.Models;

namespace iReserve.ViewModels
{
    public class VenueViewModel
    {
        public List<int> VenueIdList { get; set; }
        public List<AddVenue> VenueList { get; set; }
        public List<bool> isSelected { get; set; }
    }
}