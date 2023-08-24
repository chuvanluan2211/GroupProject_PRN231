using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System;
using Client.Models;

namespace Client.Controllers
{
    public class StatisticDetailsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string APIUrl = "";
        public StatisticDetailsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            APIUrl = "https://localhost:7104/api/Statistic";
        }

        public async Task<IActionResult> Index(string iID,string eID)
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("AccessDiened!");
            }
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync($"{APIUrl}/Details/{iID}/{eID}");
            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();

                // Chuyển đổi chuỗi JSON thành danh sách thành viên
                List<StatisticDetails> statisticsDetails = JsonConvert.DeserializeObject<List<StatisticDetails>>(strData);
                TempData["StatisticDetails"] = JsonConvert.SerializeObject(statisticsDetails);

                // Truyền danh sách thành viên cho view thông qua model
                return RedirectToAction("Index", "Statistic");
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
