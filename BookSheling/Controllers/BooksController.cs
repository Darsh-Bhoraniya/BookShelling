using BookSheling.BAL;
using BookSheling.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

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
            ViewBag.authorname = AuthorComboBox();
            ViewBag.Books_Type = Type_Combobox();
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
        [HttpGet]
        public IActionResult Delete(int BookID)
        {
            HttpResponseMessage response = _client.DeleteAsync($"{Baseurl}/Books/Delete?BookID=" + BookID).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Masssege"] = "Book Delete Successfully";
            }
            return RedirectToAction("GetAllBooks");
        }
        public IActionResult AddBooks()
        {
            ViewBag.authorname = AuthorComboBox();
            ViewBag.Books_Type = Type_Combobox();
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int BookID)
        {
            ViewBag.authorname = AuthorComboBox();
            ViewBag.Books_Type = Type_Combobox();
            Book_Model book_Model = new Book_Model();
            HttpResponseMessage response = _client.GetAsync($"{Baseurl}/Books/GetById/" + BookID).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Book_Model json = JsonConvert.DeserializeObject<Book_Model>(data) as Book_Model;
                return View("AddBooks", json);
            }
            return View("GetAllBooks");

        }

        [HttpPost]
        public async Task<IActionResult> Post(Book_Model book_Model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(book_Model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                /* if (book_Model.BookID != null || book_Model.BookID > 0)
                 {
                     ViewBag.Action = "Edit";
                 }
                 ViewBag.Action = "Add";*/
                if (book_Model.BookID == 0)
                {
                    HttpResponseMessage response = _client.PostAsync($"{Baseurl}/Books/Post/", content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Masssege"] = "New Book Insrted Sucessfully";
                        return RedirectToAction("GetAllBooks");
                    }
                    TempData.Clear();
                }
                else
                {
                    HttpResponseMessage response = await _client.PutAsync($"{Baseurl}/Books/Update/", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Masssege"] = "Book Updated Successfully";
                        return RedirectToAction("GetAllBooks");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error Ocuures" + ex.Message;
            }
            return RedirectToAction("GetAllBooks");
        }
        public List<AuthorComboBox> AuthorComboBox()
        {
            List<AuthorComboBox> authorCombos = new List<AuthorComboBox>();
            HttpResponseMessage r = _client.GetAsync($"{Baseurl}/Author/AuthorComboBox/").Result;
            if (r.IsSuccessStatusCode)
            {
                string data = r.Content.ReadAsStringAsync().Result;
                authorCombos = JsonConvert.DeserializeObject<List<AuthorComboBox>>(data);
            }
            List<AuthorComboBox> authorComboBoxes = new List<AuthorComboBox>();
            foreach (var item in authorCombos)
            {

                AuthorComboBox authorlist = new AuthorComboBox();
                authorlist.AuthorID = Convert.ToInt32(item.AuthorID);
                authorlist.AuthorName = Convert.ToString(item.AuthorName);
                authorComboBoxes.Add(authorlist);
            }
            return authorComboBoxes;
        }

        public List<Book_Type> Type_Combobox()
        {
            List<Book_Type> Book_types = new List<Book_Type>();
            HttpResponseMessage r = _client.GetAsync($"{Baseurl}/BookType/Getall/").Result;
            if (r.IsSuccessStatusCode)
            {
                string data = r.Content.ReadAsStringAsync().Result;
                Book_types = JsonConvert.DeserializeObject<List<Book_Type>>(data);
            }
            List<Book_Type> Type_DropDowns = new List<Book_Type>();
            foreach (var item in Book_types)
            {

                Book_Type book_TypeList = new Book_Type();
                book_TypeList.TypeID = Convert.ToInt32(item.TypeID);
                book_TypeList.TypeName = Convert.ToString(item.TypeName);
                Type_DropDowns.Add(book_TypeList);
            }
            return Type_DropDowns;
        }

    }
}
