using Microsoft.AspNetCore.Mvc;

namespace BookSheling.Controllers
{
    public class AuthorController : Controller
    {
        public IActionResult GetAllAuthors()
        {
            return View();
        }
    }
}
