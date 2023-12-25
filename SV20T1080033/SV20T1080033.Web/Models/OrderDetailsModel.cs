using SV20T1080033.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV20T1080033.Web.Models
{
    public class OrderDetailsModel
    {
        /// <summary>
        /// lấy tt đơn đặt hàng
        /// </summary>
        public Order Order { get; set; }
        /// <summary>
        /// lấy thông tin chi tết đơn đặt hàng
        /// </summary>
        public List<OrderDetail> OrderDetails { get; set; }
    }
}