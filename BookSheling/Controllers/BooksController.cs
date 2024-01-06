using Microsoft.AspNetCore.Mvc;

namespace BookSheling.Controllers
{
    public class BooksController : Controller
    {
        public IActionResult GetAllBooks()
        {
            return View();
        }
    }
}
