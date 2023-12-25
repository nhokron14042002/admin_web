using Microsoft.AspNetCore.Mvc;
using SV20T1080033.BusinessLayer;
using SV20T1080033.DataLayers;
using SV20T1080033.DomainModels;
using SV20T1080033.Web.Models;
using System.Drawing.Printing;

namespace SV20T1080033.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShipperController : Controller
    {
        const int PAGE_SIZE = 10;
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            var data = CommonDataService.ListOfShippers(out int rowCount, page, PAGE_SIZE, searchValue ?? "");
            var model = new PaginationSearchShipper()
            {
                Page = page,
                PageSize = PAGE_SIZE,
                SearchValue = searchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            string errorMessage = Convert.ToString(TempData["ErrorMessage"]);
            ViewBag.ErrorMessage = errorMessage;
            return View(model);
        }


        public IActionResult Create()
        {
            var model = new Shipper()
            {
                ShipperID = 0,
            };
            ViewBag.Title = "Bổ sung người giao hàng";
            return View(model); ;
        }
        public IActionResult Edit(int id = 0)
        {
            var model = CommonDataService.GetShipper(id);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Cập nhật người giao hàng";
            return View("Create", model);
        }
        public IActionResult Delete(int id = 0)
        {
            var model = CommonDataService.GetShipper(id);
            if (model == null)
                return RedirectToAction("Index");

            if (Request.Method == "POST")
            {
                bool success = CommonDataService.DeleteShipper(id);
                if (!success)
                    TempData["ErrorMessage"] = "Không thể xóa khách hàng " + model.ShipperName;
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult Save(Shipper data)
        {
            ViewBag.Title = data.ShipperID == 0 ? "Bổ sung người giao hàng" : "Cập nhật người giao hàng";
            if (data.ShipperID == 0)
            {
                int ShipperID = CommonDataService.AddShipper(data);
                if (ShipperID > 0)
                    return RedirectToAction("Index");

                ViewBag.ErrorMessage = "Không bổ sung được dữ liệu";
                return View("Create", data);
            }
            else
            {
                bool success = CommonDataService.UpdateShipper(data);
                if (success)
                    return RedirectToAction("Index");

                ViewBag.ErrorMessage = "Không cập nhật được dữ liệu";
                return View("Create", data);
            }
        }
    }
}
