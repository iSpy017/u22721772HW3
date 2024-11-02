using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u22721772_HW3.Models
{
    public class HomeViewModel
    {


        public IPagedList<student> Students { get; set; }
        public IPagedList<book> Books { get; set; }

    }
}