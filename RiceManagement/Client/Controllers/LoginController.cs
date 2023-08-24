using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using Client.Models;


namespace Client.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string APIUrl = "";
        public LoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            APIUrl = "https://localhost:7104/api/Account";
        }
        public IActionResult Index()
        {
            return View();
        }
            
        [HttpPost]
        public async Task<IActionResult> LoginAsync(User user)
        {
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"{APIUrl}", content);
            Console.WriteLine(response.Content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<JObject>(responseContent);

                if (jsonResponse != null)
                {
                    var token = jsonResponse.Value<string>("data");

                    // Lưu token vào Session hoặc Cookie
                    HttpContext.Session.SetString("Token", token);
                    // Hoặc HttpContext.Response.Cookies.Append("Token", token);

                    return RedirectToAction("Index", "Statistic");
                }
            }


            return RedirectToAction("Index");
        }
    }
}
