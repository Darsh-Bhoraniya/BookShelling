using BookSheling.BAL;
using Microsoft.AspNetCore.Mvc;

namespace BookSheling.Controllers
{
    [CheckAccess]
    public class BooksController : Controller
    {
        public IActionResult GetAllBooks()
        {
            return View();
        }
    }
}
