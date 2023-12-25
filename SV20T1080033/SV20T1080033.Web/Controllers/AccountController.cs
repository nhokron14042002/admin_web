using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1080033.BusinessLayers;
using static SV20T1080033.BusinessLayers.UserAccountService;

namespace SV20T1080033.Web.Controllers
{
	public class AccountController : Controller
	{
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(string userName = "", string password = "")
		{
			ViewBag.UserName = userName;
			ViewBag.PassWord = password;

			var userAccount = UserAccountService.Authorize(userName, password, TypeOfAccounts.Employee);

			if (userAccount != null)
			{
				//Dang nhap thanh cong
				//1. Tao doi tuong luu cac thong tin cua phien dang nhap
				WebUserData userData = new WebUserData() {
					UserId = userAccount.UserId,
					UserName = userAccount.UserName,
					DisplayName = userAccount.FullName,
					Email = userAccount.Email,
					Photo = userAccount.Photo,
					ClientIP = HttpContext.Connection.RemoteIpAddress?.ToString(),
					SessionId = HttpContext.Session.Id,
					AdditionalData = "",
					Roles = new List<string>() { WebUserRoles.Administrator}
				};
				//2.Thiet lap (ghi nhan) phien dang nhap
				await HttpContext.SignInAsync(userData.CreatePrincipal()); //Authentication
				//3. Quay lai trang chu cua Admin
				return RedirectToAction("Index", "Dashboard", new { area = "Admin"});
			}
			else
			{
				//Dang nhap ko thanh cong tra lai giao dien de dang nhap
				ModelState.AddModelError("Error", "Đăng nhập thất bại");
				return View();
			}
		}

		public async Task<IActionResult> Logout()
		{
			HttpContext.Session.Clear();
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login");
		}

		[Authorize]
		[HttpGet]
		public IActionResult ChangePassword()
		{
			return View();
		}

        [Authorize]
        [HttpPost]
        public IActionResult ChangePassword(string oldPassword, string newPassword, string confirmPassword)
		{
			string userName = User?.GetUserData()?.UserName ?? "";

			//Kiem tra mat khau cu
			var userAccount = UserAccountService.Authorize(userName, oldPassword, TypeOfAccounts.Employee);
			if (userAccount == null)
				ModelState.AddModelError("Error", "Mật khẩu cũ không đúng");
			//Kiem tra xac nhan mat khau hop le
			if (confirmPassword != newPassword)
				ModelState.AddModelError("Error", "Xác nhận mật khẩu không hợp lệ");

			if (!ModelState.IsValid)
				return View();

			//Doi mat khau
			bool result = UserAccountService.ChangePassword(userName, newPassword, TypeOfAccounts.Employee);
			return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
		}
	}
}
