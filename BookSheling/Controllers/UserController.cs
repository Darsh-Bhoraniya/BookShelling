using BookSheling.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Data;
using static System.Net.WebRequestMethods;
using System.Security.AccessControl;

namespace BookSheling.Controllers
{
    public class UserController : Controller
    {
        Uri Baseurl = new Uri("https://localhost:44395/api");
        private readonly HttpClient _client;
        public UserController() { 
        
            _client = new HttpClient();
            _client.BaseAddress = Baseurl;
        }
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            List<User_Models> users = new List<User_Models>();
            HttpResponseMessage response = _client.GetAsync($"{Baseurl}/User/Getall").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic json = JsonConvert.DeserializeObject(data);
                var dataobj = json.data;
                var extractDatajosn = JsonConvert.SerializeObject(dataobj,Formatting.Indented);
                users = JsonConvert.DeserializeObject<List<User_Models>>(extractDatajosn);
            }
            return View("GetAllUsers",users);
        }
    }
}
