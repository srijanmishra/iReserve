using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iReserve.Models;

namespace iReserve.ViewModels
{
    public class MovieViewModel
    {
        public List<int> MovieIdList { get; set; }
        public List<AddMovie> MovieItemList { get; set; }
        public List<bool> isSelected { get; set; }
    }
}