using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u22721772_HW3.Models
{
    public class MaintainViewModel
    {
        public IPagedList<borrow> borrows { get; set; }
        public IPagedList<author> authors { get; set; }
        public IPagedList<type> types { get; set; }
    }
}