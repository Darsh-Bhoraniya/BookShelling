using Microsoft.AspNetCore.Mvc;

namespace BookSheling.Controllers
{
    public class UserController : Controller
    {
        public IActionResult GetAllUsers()
        {
            return View();
        }
    }
}
