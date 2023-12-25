using Microsoft.AspNetCore.Mvc;
using SV20T1080033.BusinessLayer;
using SV20T1080033.DataLayers;
using SV20T1080033.DomainModels;
using SV20T1080033.Web.Models;
using SV20T1080033.Web.AppCodes;
using Microsoft.AspNetCore.Authorization;

namespace SV20T1080033.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator}")]

    [Area("Admin")]
    public class EmployeesController : Controller
    {
		const int PAGE_SIZE = 10;
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IActionResult Index(int page = 1, string searchValue = "")
		{
			var data = CommonDataService.ListOfEmployees(out int rowCount, page, PAGE_SIZE, searchValue ?? "");
			var model = new PaginationSearchEmployee()
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
			var model = new Employee()
			{
				EmployeeID = 0,
			};
			ViewBag.Title = "Bổ sung nhân viên";
			return View(model);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IActionResult Edit(int id = 0)
		{
			var model = CommonDataService.GetEmployee(id);
			if (model == null)
				return RedirectToAction("Index");

			ViewBag.Title = "Cập nhật nhân viên";
			return View("Create", model);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IActionResult Delete(int id = 0)
		{
			var model = CommonDataService.GetEmployee(id);
			if (model == null)
				return RedirectToAction("Index");

			if (Request.Method == "POST")
			{
				bool success = CommonDataService.DeleteEmployee(id);
				if (!success)
					TempData["ErrorMessage"] = "Không thể xóa nhân viên " + model.FullName;
				return RedirectToAction("Index");
			}

			return View(model);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/*public IActionResult Edit(int id = 0)
		{
			Employee model = new Employee()
			{
				EmployeeID = 123,
				FullName = "Trần Nguyên Phong",
				Address = "77 Nguyễn Huệ",
				BirthDate = new DateTime(1976, 12, 20),
				Email = "tnphong@gmail.com",
				Phone = "0935254782",
				IsWorking = true,
				Photo = "photo.png"
			};

			ViewBag.Title = "Cập nhật nhân viên";
			return View("Create", model);
		}*/

		public IActionResult Save(Employee data, IFormFile? uploadPhoto)
		{

			ViewBag.Title = data.EmployeeID == 0 ? "Bổ sung nhân viên" : "Cập nhật nhân viên";

			if (string.IsNullOrWhiteSpace(data.FullName))
				ModelState.AddModelError(nameof(data.FullName), "Tên nhân viên không được rỗng");
			else if (CheckString.ContainsNumber(data.FullName) == true || CheckString.ContainsSpecial(data.FullName) == true)
				ModelState.AddModelError(nameof(data.FullName), "Tên nhân viên không hợp lệ");

			if (Calculator.AgeCalculate(data.BirthDate) < 18)
				ModelState.AddModelError(nameof(data.BirthDate), "Ngày sinh nhân viên không hợp lệ");

			if (string.IsNullOrWhiteSpace(data.Address))
				ModelState.AddModelError(nameof(data.Address), "Địa chỉ không được rỗng");
			else if (CheckString.ContainsSpecial(data.Address) == true)
				ModelState.AddModelError(nameof(data.Address), "Địa chỉ không hợp lệ");

			if (string.IsNullOrWhiteSpace(data.Phone))
				ModelState.AddModelError(nameof(data.Phone), "Số điện thoại không được rỗng");
			else if (CheckString.IsPhone(data.Phone) == false)
				ModelState.AddModelError(nameof(data.Phone), "Số điện thoại không hợp lệ");

			if (string.IsNullOrWhiteSpace(data.Email))
				ModelState.AddModelError(nameof(data.Email), "Email không được rỗng");
			else if (CheckString.IsEmail(data.Email) == false)
				ModelState.AddModelError(nameof(data.Email), "Email không hợp lệ");

			//Xử lý với ảnh
			//Upload ảnh lên (nếu có), sau khi upload xong thì mới lấy tên file ảnh vừa upload để gán cho trường Photo của Employee
			if (uploadPhoto != null)
			{
				string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
				string filePath = System.IO.Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images\employees", fileName);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					uploadPhoto.CopyTo(stream);
				}
				data.Photo = fileName;
			}

			if (!ModelState.IsValid)
			{
				return View("Create", data);
			}

			//return Json(data);

			if (data.EmployeeID == 0)
			{
				int employeeId = CommonDataService.AddEmployee(data);
				if (employeeId > 0)
				{
					return RedirectToAction("Index");
				}
				ViewBag.ErrorMessage = "Không bổ sung được dữ liệu";
				return View("Create", data);
			}
			else
			{
				bool success = CommonDataService.UpdateEmployee(data);
				if (success)
				{
					return RedirectToAction("Index");
				}
				ViewBag.ErrorMessage = "Không cập nhật được dữ liệu";
				return View("Create", data);
			}

		}

	}
}
