using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1080033.BusinessLayer;
using SV20T1080033.DataLayers;
using SV20T1080033.DomainModels;
using SV20T1080033.Web.Models;

namespace SV20T1080033.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator}")]
    [Area("Admin")]
    public class SupplierController : Controller
    {
		const int PAGE_SIZE = 10;
		public IActionResult Index(int page = 1, string searchValue = "")
        {
			var data = CommonDataService.ListOfSuppliers(out int rowCount, page, PAGE_SIZE, searchValue ?? "");
			var model = new PaginationSearchSupplier()
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
			var model = new Supplier()
			{
				SupplierID = 0,
			};
			ViewBag.Title = "Bổ sung nhà cung cấp";
			return View(model);
		}
        public IActionResult Edit(int id=0)
        {
            var model = CommonDataService.GetSupplier(id);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Cập nhật nhà cung cấp";
            return View("Create", model);
        }
        public IActionResult Delete(int id = 0)
        {
            var model = CommonDataService.GetSupplier(id);
            if (model == null)
                return RedirectToAction("Index");

            if (Request.Method == "POST")
            {
                bool success = CommonDataService.DeleteSupplier(id);
                if (!success)
                    TempData["ErrorMessage"] = "Không thể xóa nhà cung cấp " + model.SupplierName;
                return RedirectToAction("Index");
            }

            return View(model);
        }
		public IActionResult Save(Supplier data)
		{
			ViewBag.Title = data.SupplierID == 0 ? "Bổ sung nhà cung cấp" : "Cập nhật nhà cung cấp";
			if (data.SupplierID == 0)
			{
				int SupplierID = CommonDataService.AddSupplier(data);
				if (SupplierID > 0)
					return RedirectToAction("Index");

				ViewBag.ErrorMessage = "Không bổ sung được dữ liệu";
				return View("Create", data);
			}
			else
			{
				bool success = CommonDataService.UpdateSupplier(data);
				if (success)
					return RedirectToAction("Index");

				ViewBag.ErrorMessage = "Không cập nhật được dữ liệu";
				return View("Create", data);
			}
		}
	}
}
