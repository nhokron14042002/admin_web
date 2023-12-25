using SV20T1080033.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV20T1080033.Web.Models
{
    public class OrderSearchOutput : PaginationSearchOutput
    {
        public List<Order> Data { get; set; }
    }
}