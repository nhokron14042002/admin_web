using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SV20T1080033.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{WebUserRoles.Administrator}")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
