using Microsoft.AspNetCore.Mvc;
using BookSheling.Controllers;
namespace BookSheling.Controllers
{
    public class Registration : Controller
    {
        UserController u = new UserController();
        public IActionResult NewUser()
        {
            ViewBag.RoleList = u.dropDowns();
            return View();
        }
    }
}
