using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iReserve.Models;

namespace iReserve.ViewModels
{
    public class MenuViewModel
    {
        public List<int> MenuIdList { get; set; }
        public List<MenuDetails> MenuItemList { get; set; }
        public List<bool> isSelected { get; set; }
    }
}