using BookSheling.BAL;
using BookSheling.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BookSheling.Controllers
{
    [CheckAccess]
    public class AuthorController : Controller
    {

        Uri Baseurl = new Uri("https://localhost:44395/api");
        private readonly HttpClient _client;
        public AuthorController()
        {
            _client = new HttpClient();
            _client.BaseAddress = Baseurl;
        }
        [HttpGet]
        public IActionResult GetAllAuthors()
        {
            List<Author_Model> author = new List<Author_Model>();
            HttpResponseMessage response = _client.GetAsync($"{Baseurl}/Author/Getall").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                IEnumerable<Author_Model> json = JsonConvert.DeserializeObject<List<Author_Model>>(data);
                //var dataobj = json.data;
                //var extractDatajosn = JsonConvert.SerializeObject(dataobj, Formatting.Indented);
                //author = JsonConvert.DeserializeObject<List<User_Models>>(extractDatajosn);
                return View("GetAllAuthors", json);
            }
            return View("GetAllAuthors");
        }
        public IActionResult DeleteAuthor(int AuthorID)
        {

            HttpResponseMessage response = _client.DeleteAsync($"{Baseurl}/Author/Delete?AuthorID=" + AuthorID).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Masssege"] = "Author Delete Successfully";
            }
            return RedirectToAction("GetAllAuthors");
        }


        [HttpGet]
        public IActionResult Edit(int AuthorID)
        {
            Author_Model author_Model = new Author_Model();
            HttpResponseMessage response = _client.GetAsync($"{Baseurl}/Author/AuthorGetbyId/" + AuthorID).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Author_Model json = JsonConvert.DeserializeObject<Author_Model>(data) as Author_Model;
                return View("AddAuhtor", json);
            }
            return View("GetAllAuthors");

        }

        [HttpPost]
        public async Task<IActionResult> Post(Author_Model author_Model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(author_Model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                if (author_Model.AuthorID != null || author_Model.AuthorID > 0)
                {
                    ViewBag.Action = "Edit";
                }
                ViewBag.Action = "Add";
                if (author_Model.AuthorID == 0)
                {
                    HttpResponseMessage response = _client.PostAsync($"{Baseurl}/Author/Post/", content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Masssege"] = "Author insert Successfully";
                        return RedirectToAction("GetAllAuthors");
                    }
                    TempData.Clear();

                }
                else
                {
                    HttpResponseMessage response = await _client.PutAsync($"{Baseurl}/Author/Put/", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Masssege"] = "Auhtor Updated Successfully";
                        return RedirectToAction("GetAllAuthors");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error Ocuures" + ex.Message;
            }
            return RedirectToAction("GetAllAuthors");
        }
        public IActionResult AddAuhtor()
        {
            return View();
        }
    }
}
