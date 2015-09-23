using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iReserve.Models;

namespace iReserve.ViewModels
{
    public class UpdateMenuDetails
    {
        public int MenuID { get; set; }
        

        public MenuDetails MenuItem { get; set; }
    }
}