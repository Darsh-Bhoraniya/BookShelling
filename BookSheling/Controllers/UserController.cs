using BookSheling.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Data;
using static System.Net.WebRequestMethods;
using System.Security.AccessControl;
using System.Text;
using BookSheling.BAL;

namespace BookSheling.Controllers
{
    public class UserController : Controller
    {
        Uri Baseurl = new Uri("https://localhost:44395/api");
        private readonly HttpClient _client;
        public UserController()
        {

            _client = new HttpClient();
            _client.BaseAddress = Baseurl;
        }


        [HttpGet]
        [CheckAccess]
        public IActionResult GetAllUsers()
        {

            List<User_Models> users = new List<User_Models>();
            HttpResponseMessage response = _client.GetAsync($"{Baseurl}/User/Getall").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic json = JsonConvert.DeserializeObject(data);
                var dataobj = json.data;
                var extractDatajosn = JsonConvert.SerializeObject(dataobj, Formatting.Indented);
                users = JsonConvert.DeserializeObject<List<User_Models>>(extractDatajosn);
            }
            return View("GetAllUsers", users);
        }
        [HttpGet]

        public IActionResult DeleteUsers(int UserID)
        {
            //Geting Code From The Postman

            /*var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Delete, "https://localhost:44395/api/User/Delete?UserID=7");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            {
                TempData["Masssege"] = "User Deleted Successfully";
            }
            Console.WriteLine(await response.Content.ReadAsStringAsync());*/

            _client.DefaultRequestHeaders.Accept.Clear();

            // Manual  add a {?UserId= UseId} They get Data From the UerId = Xyz;

            HttpResponseMessage response = _client.DeleteAsync($"{Baseurl}/User/Delete?UserID=" + UserID).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Masssege"] = "User Deleted Successfully";
            }
            return RedirectToAction("GetAllUsers");
        }


        [HttpGet]
        public IActionResult Edit(int UserID)
        {
            User_Models user_Models = new User_Models();
            HttpResponseMessage response = _client.GetAsync($"{Baseurl}/User/GetById/" + UserID).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic json = JsonConvert.DeserializeObject(data);
                var dataobj = json.data;
                var extractDatajosn = JsonConvert.SerializeObject(dataobj, Formatting.Indented);
                user_Models = JsonConvert.DeserializeObject<User_Models>(extractDatajosn);
            }
            return View("RegisterUsers", user_Models);
        }

        [HttpPost]
        public async Task<IActionResult> Post(User_Models user_Models)
        {
            try
            {

                string data = JsonConvert.SerializeObject(user_Models);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                if (user_Models.UserID == 0)
                {
                    HttpResponseMessage response = _client.PostAsync($"{Baseurl}/User/Post/", content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Masssege"] = "User insert Successfully";
                        return RedirectToAction("GetAllUsers");
                    }
                }
                else
                {
                    HttpResponseMessage response = await _client.PutAsync($"{Baseurl}/User/Update/", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Masssege"] = "User Updated Successfully";
                        return RedirectToAction("GetAllUsers");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error Ocuures" + ex.Message;
            }
            return RedirectToAction("GetAllUsers");
        }
        public IActionResult RegisterUsers()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> checkLoginDetails(Login_Model user)
        {
            if (!TryValidateModel(user)) return View("Login", user);
            int id = await Login(user);
            if (!(user != null && id > 0))
            {
                TempData["error"] = "Username or Password is incorrect";
                return View("Login", user);
            }
            _SetSession(id,user);
            int? a = UserSessonCV.UserId();
            TempData.Clear();
            if (HttpContext.Session.GetString("UserName") != null && HttpContext.Session.GetString("Password") != null)
                return RedirectToAction("Index", "Home");
            return View("Login");
        }
        private void _SetSession(int id,Login_Model user)
        {
            HttpContext.Session.SetString("UserName", user.UserName);
            HttpContext.Session.SetString("Password", user!.Password);
            HttpContext.Session.SetInt32("UserId", id);

          }
            public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Login");
        }
        [HttpPost]
        public async Task<int> Login(Login_Model login_Model)
        {   
            try
            {

                string data = JsonConvert.SerializeObject(login_Model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync($"{Baseurl}/User/Login", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Masssege"] = "User login Successfully";
                    string userid = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<int>(userid);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error Ocuures" + ex.Message;
            }
            return -1;
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
