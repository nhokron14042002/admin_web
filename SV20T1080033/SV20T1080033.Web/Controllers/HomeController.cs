using Microsoft.AspNetCore.Mvc;
using SV20T1080033.Web.Models;
using System.Diagnostics;

namespace SV20T1080033.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var data = new HomeIndexModel()
            {
               TitleMessage = " LIST OF PERSONS AND STUDENTS",
               ListOfPersons = new PersonDAL().List(),
               ListOfStudents = new StudentDAL().List(),
       
        };
            

            //var dal = new PersonDAL();
            //var data = dal.List();
            //ViewBag.TitleMessage= "List of Person";
          //  ViewBag.ListOfPersions = data;
            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult InputEmployee()
        {
            return View();
        }
        public IActionResult GetEmployee(InputEmployee data)
        {
        //    String s = "";
        //    foreach( String n in name) {
        //        s+= n;
        //    }
            return Content($"name: {data.name},age:{data.age}");
        }

       

    }
}