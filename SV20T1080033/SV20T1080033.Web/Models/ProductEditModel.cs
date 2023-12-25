using SV20T1080033.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV20T1080033.Web.Models
{
    public class ProductEditModel
    {
            /// <summary>
            /// Lấy ra thông tin một mặt hàng
            /// </summary>
            public Product Product { get; set; }
            /// <summary>
            /// Lấy ra danh sách thuộc tính của mặt hàng
            /// </summary>
            public List<ProductAttribute> ProductAttributes { get; set; }
            /// <summary>
            /// Lấy ra danh sách ảnh của mặt hàng
            /// </summary>
            public List<ProductPhoto> ProductPhotos { get; set; }
        }
    }
