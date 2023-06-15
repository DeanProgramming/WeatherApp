using DeanH_WeatherApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace DeanH_WeatherApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
        } 
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult?> GetWeather(string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                return View();
            }

            string apiKey = "78d568fc875b58a3430f950f42fcc553";
            string apiUrl = $"weather?q={location}&appid={apiKey}&units=metric"; 
             
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync(); Console.WriteLine(responseBody);
                WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(responseBody);

                return Json(weatherData);
            }

            return View();
        }
    }
}