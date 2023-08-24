using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Client.Controllers
{
    public class StatisticController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string APIUrl = "";
        public StatisticController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            APIUrl = "https://localhost:7104/api/Statistic";
        }
        List<Statistic> statistics { get; set; }
        public IActionResult Index()
        {
            var statisticsJson = TempData["Statistics"] as string;
            var statisticDetailJson = TempData["StatisticDetails"] as string;
            List<Statistic> statistics = null;
            
            if (!string.IsNullOrEmpty(statisticsJson))
            {
                statistics = JsonConvert.DeserializeObject<List<Statistic>>(statisticsJson);
               
            }
            if (!string.IsNullOrEmpty(statisticDetailJson))
            {
                ViewBag.statisticDetails = JsonConvert.DeserializeObject<List<StatisticDetails>>(statisticDetailJson);
            }

            return View(statistics);
        }
        [HttpPost]
        public async Task<IActionResult> GetAllData(string month,string year)
        {

            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("AccessDiened!");
            }
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync($"{APIUrl}/{month}/{year}");
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();

                // Chuyển đổi chuỗi JSON thành danh sách thành viên
                statistics = JsonConvert.DeserializeObject<List<Statistic>>(strData);
                TempData["Statistics"] = JsonConvert.SerializeObject(statistics);

                // Truyền danh sách thành viên cho view thông qua model
                return RedirectToAction("Index");
            }
            else
            {
                // Xử lý lỗi (ví dụ: hiển thị mã trạng thái lỗi)
                var statusCode = response.StatusCode;
                return View("Error");
            }

        }
    }
}
