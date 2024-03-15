using BookSheling.Models;
using BookSheling.BAL;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BookSheling.Controllers
{
    [CheckAccess]
    public class BooksTypesController : Controller
    { 
            Uri Baseurl = new Uri("https://localhost:44395/api");
            private readonly HttpClient _client;
            public BooksTypesController()
            {
                _client = new HttpClient();
                _client.BaseAddress = Baseurl;
            }
            [HttpGet]
            public IActionResult GetAllBooksTypes()
            {
                List<Books_TypesModel> author = new List<Books_TypesModel>();
                HttpResponseMessage response = _client.GetAsync($"{Baseurl}/BookType/Getall").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    IEnumerable<Books_TypesModel> json = JsonConvert.DeserializeObject<List<Books_TypesModel>>(data);
                    //var dataobj = json.data;
                    //var extractDatajosn = JsonConvert.SerializeObject(dataobj, Formatting.Indented);
                    //author = JsonConvert.DeserializeObject<List<User_Models>>(extractDatajosn);
                    return View("GetAllBooksTypes", json);
                }
                return View("GetAllBooksTypes");
            }
            public IActionResult DeleteBooksTypes(int TypeID)
            {

                HttpResponseMessage response = _client.DeleteAsync($"{Baseurl}/BookType/Delete?TypeID=" + TypeID).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["Masssege"] = "BookType Delete Successfully";
                }
                return RedirectToAction("GetAllBooksTypes");
            }


            [HttpGet]
            public IActionResult Edit(int TypeID)
            {
                Books_TypesModel book_Typesmodel = new Books_TypesModel();
                HttpResponseMessage response = _client.GetAsync($"{Baseurl}/BookType/BooksTypesGetbyId/" + TypeID).Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    Books_TypesModel json = JsonConvert.DeserializeObject<Books_TypesModel>(data) as Books_TypesModel;
                    return View("AddBookTypes", json);
                }
                return View("GetAllBooksTypes");

            }

            [HttpPost]
            public async Task<IActionResult> Post(Books_TypesModel book_Typesmodel)
            {
                try
                {
                    string data = JsonConvert.SerializeObject(book_Typesmodel);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                    if (book_Typesmodel.TypeID != null || book_Typesmodel.TypeID> 0)
                    {
                        ViewBag.Action = "Edit";
                    }
                    ViewBag.Action = "Add";
                    if (book_Typesmodel.TypeID == 0)
                    {
                        HttpResponseMessage response = _client.PostAsync($"{Baseurl}/BookType/Post/", content).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["Masssege"] = "BookType insert Successfully";
                            return RedirectToAction("GetAllBooksTypes");
                        }
                        TempData.Clear();

                    }
                    else
                    {
                        HttpResponseMessage response = await _client.PutAsync($"{Baseurl}/BookType/Put/", content);
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["Masssege"] = "BookType Updated Successfully";
                            return RedirectToAction("GetAllBooksTypes");
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error Ocuures" + ex.Message;
                }
                return RedirectToAction("GetAllBooksTypes");
            }
            public IActionResult AddBookTypes()
            {
                return View();
            }
        }
}
