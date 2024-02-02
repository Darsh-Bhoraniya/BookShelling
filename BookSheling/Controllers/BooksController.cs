using BookSheling.BAL;
using BookSheling.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookSheling.Controllers
{
    [CheckAccess]
    public class BooksController : Controller
    {
        Uri Baseurl = new Uri("https://localhost:44395/api");
        private readonly HttpClient _client;
        public BooksController()
        {
            _client = new HttpClient();
            _client.BaseAddress = Baseurl;
        }
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            List<Book_Model> Book = new List<Book_Model>();
            HttpResponseMessage response = _client.GetAsync($"{Baseurl}/Books/Getall").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var json = JsonConvert.DeserializeObject<List<Book_Model>>(data);
                //var dataobj = json.data;
                //var extractDatajosn = JsonConvert.SerializeObject(dataobj, Formatting.Indented);
                //author = JsonConvert.DeserializeObject<List<User_Models>>(extractDatajosn);
                return View("GetAllBooks", json);
            }
            return View("GetAllBooks");
        }
    }
}
