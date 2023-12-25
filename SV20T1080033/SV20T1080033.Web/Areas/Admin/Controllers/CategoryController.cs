using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1080033.BusinessLayer;
using SV20T1080033.DataLayers;
using SV20T1080033.DomainModels;
using SV20T1080033.Web.Models;

namespace SV20T1080033.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize(Roles = $"{WebUserRoles.Administrator}")]
    [Area("Admin")]
    public class CategoryController : Controller
    {
        const int PAGE_SIZE = 10;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            var data = CommonDataService.ListOfCategories(out int rowCount, page, PAGE_SIZE, searchValue ?? "");
            var model = new PaginationSearchCategory()
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            var model = new Category()
            {
                CategoryId = 0,
            };
            ViewBag.Title = "Bổ sung loại hàng";
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Edit(int id = 0)
        {
            var model = CommonDataService.GetCategory(id);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Cập nhật loại hàng";
            return View("Create", model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Delete(int id = 0)
        {
            var model = CommonDataService.GetCategory(id);
            if (model == null)
                return RedirectToAction("Index");

            if (Request.Method == "POST")
            {
                bool success = CommonDataService.DeleteCategory(id);
                if (!success)
                    TempData["ErrorMessage"] = "Không thể xóa loại hàng " + model.CategoryName;
                return RedirectToAction("Index");
            }

            return View(model);
        }
        public IActionResult Save(Category data)
        {
            ViewBag.Title = data.CategoryId == 0 ? "Bổ sung loại hàng" : "Cập nhật loại hàng";
            /* kiểm tra dữ liệu đầu vào */

            if (string.IsNullOrWhiteSpace(data.CategoryName))
                ModelState.AddModelError(nameof(data.CategoryName), "Tên loại hàng không được rỗng");
            /* kiểm tra dữ liệu đầu vào */

            if (string.IsNullOrWhiteSpace(data.Description))
                ModelState.AddModelError(nameof(data.Description), "Mô tả không được rỗng");
            /* kiểm tra dữ liệu đầu vào */

           

            
            /* trạng thái không hợp lệ */
            if (!ModelState.IsValid)
            {
                return View("Create", data);
            }
            if (data.CategoryId == 0)
            {
                int categoryId = CommonDataService.AddCategory(data);
                if (categoryId > 0)
                    return RedirectToAction("Index");

                ViewBag.ErrorMessage = "Không bổ sung được dữ liệu";
                return View("Create", data);
            }
            else
            {
                bool success = CommonDataService.UpdateCategory(data);
                if (success)
                    return RedirectToAction("Index");

                ViewBag.ErrorMessage = "Không cập nhật được dữ liệu";
                return View("Create", data);
            }
        }
    }
}
