using BookSheling.BAL;
using BookSheling.DAL;
using BookSheling.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;

namespace BookSheling.Controllers
{
    [CheckAccess]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Count_DAL dal = new Count_DAL();
            DataTable dataTable = dal.PR_DASHBOARD_COUNTS();

            // Convert DataTable to Dictionary for simplicity
            Dictionary<string, int> dataDictionary = new Dictionary<string, int>
            {
                { "BookCount", Convert.ToInt32(dataTable.Rows[0]["BookCount"]) },
                { "UserCount", Convert.ToInt32(dataTable.Rows[0]["UserCount"]) },
                { "AuthorCount", Convert.ToInt32(dataTable.Rows[0]["AuthorCount"]) },

            };

            // Pass data to view using ViewBag or ViewData
            ViewBag.DashboardData = dataDictionary;


            return View();
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
    }
}
