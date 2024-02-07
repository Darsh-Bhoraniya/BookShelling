using BookSheling.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Data;
using static System.Net.WebRequestMethods;
using System.Security.AccessControl;
using System.Text;
using BookSheling.BAL;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office.CustomUI;

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
            ViewBag.RoleList = dropDowns();

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
            ViewBag.RoleList = dropDowns();
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
            ViewBag.RoleList = dropDowns();
            return View();
        }
        public List<Role_DropDown> dropDowns()
        {
            List<Role_DropDown> users = new List<Role_DropDown>();
            HttpResponseMessage r = _client.GetAsync($"{Baseurl}/Role/Getall").Result;
            if (r.IsSuccessStatusCode)
            {
                string data = r.Content.ReadAsStringAsync().Result;
                users = JsonConvert.DeserializeObject<List<Role_DropDown>>(data);
            }
            List<Role_DropDown> role_DropDowns = new List<Role_DropDown>();

            foreach (var item in users)
            {

                Role_DropDown list = new Role_DropDown();
                list.RoleID = Convert.ToInt32(item.RoleID);
                list.RoleName = Convert.ToString(item.RoleName);
                role_DropDowns.Add(list);
            }
            return role_DropDowns;
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
        [HttpGet]
        public IActionResult ExportData()
        {
            List<User_Models> users = new List<User_Models>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/User/Getall").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;

                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataofObject = jsonObject.data;
                var extractedDataJson = JsonConvert.SerializeObject(dataofObject, Formatting.Indented);
                users = JsonConvert.DeserializeObject<List<User_Models>>(extractedDataJson);
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Users");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Name";
                worksheet.Cell(currentRow, 2).Value = "UserName";
                worksheet.Cell(currentRow, 3).Value = "Email";
                worksheet.Cell(currentRow, 4).Value = "PhoneNumber";
                worksheet.Cell(currentRow, 5).Value = "Password";
                worksheet.Cell(currentRow, 6).Value = "RoleID";
                worksheet.Cell(currentRow, 7).Value = "Created";
                worksheet.Cell(currentRow, 8).Value = "Modified";

                foreach (var user in users)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = user.UserID;
                    worksheet.Cell(currentRow, 2).Value = user.UserName;
                    worksheet.Cell(currentRow, 3).Value = user.Email;
                    worksheet.Cell(currentRow, 4).Value = user.PhoneNumber;
                    worksheet.Cell(currentRow, 5).Value = user.Password;
                    worksheet.Cell(currentRow, 6).Value = user.RoleID;
                    worksheet.Cell(currentRow, 7).Value = Convert.ToDateTime(user.Created);
                    worksheet.Cell(currentRow, 8).Value = user.Modified;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Users.xlsx");
                }
            }
        }
    }
}
