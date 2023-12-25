using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV20T1080033.Web.Models
{
    public class OrderSearchInput : PaginationSearchInput
    {
        public int Status { get; set; }
    }
}