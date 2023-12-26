using Microsoft.AspNetCore.Mvc.Rendering;
using SV20T1080033.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using SV20T1080033.BusinessLayers;
using SV20T1080033.DomainModels;
using Newtonsoft.Json.Linq;


namespace SV20T1080033.Web
{
    public class SelectListHelper
    {
        public static List<SelectListItem> Provinces() {
         List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "--Chọn tỉnh/thành--"

            });
            foreach (var item in CommonDataService.ListOfProvinces())
                list.Add(new SelectListItem()
                {
                    Value = item.ProvinceName,
                    Text = item.ProvinceName

                });

            return list;
        
        }

        /// <summary>
        /// Danh sách loại hàng
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> categories()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn loại hàng --"
            });
            foreach (var item in CommonDataService.ListOfCategoriess())
            {
                list.Add(new SelectListItem()
                {
                    Value = item.CategoryId.ToString(),
                    Text = item.CategoryName
                });
            }
            return list;
        }
       

        public static List<SelectListItem> supplierss()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn nhà cung cấp --"
            });
            foreach (var item in CommonDataService.ListOfSupplierss())
            {
                list.Add(new SelectListItem()
                {
                    Value = item.SupplierID.ToString(),
                    Text = item.SupplierName
                });
            }
            return list;
        }

        /// <summary>
        /// Lấy danh sách người giao hàng
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Shippers()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "0",
                Text = "--Chọn người giao hàng--"
            });
            foreach (var item in CommonDataService.ListOfShipperss(""))
            {
                list.Add(new SelectListItem()
                {
                    Value = item.ShipperID.ToString(),
                    Text = item.ShipperName
                });
            }
            return list;
        }

        public static List<SelectListItem> employees()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "--chọn nhân viên --"
            });
            foreach (var item in CommonDataService.ListOfEmployeess())
            {
                list.Add(new SelectListItem()
                {
                    Value = item.EmployeeID.ToString(),
                    Text = item.FullName
                });
            }
            return list;
        }
    }
}