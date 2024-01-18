using DeanH_WeatherApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace DeanH_WeatherApp.Controllers
{
    public class HomeController : Controller
    {
        public string apiKey = "78d568fc875b58a3430f950f42fcc553"; //Inapproapiate to have my API key exposed like this. I would take additional steps to protect it in build

        private readonly HttpClient _httpClient;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        } 

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult?> GetWeatherByLocation(string location)
        {
            string apiUrl = $"weather?q={location}&appid={apiKey}&units=metric";
            return await GetWeather(apiUrl);
        }

        public async Task<IActionResult?> GetWeatherByLatLong(string lat, string lon)
        {
            if (string.IsNullOrEmpty(lat) || string.IsNullOrEmpty(lon))
            {
                return View();
            }

            string apiUrl = $"weather?lat={lat}&lon={lon}&appid={apiKey}&units=metric";
            return await GetWeather(apiUrl);
        }

        private async Task<IActionResult?> GetWeather(string apiUrl)
        {
            _httpClient.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
                WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(responseBody);

                return Json(weatherData);
            }

            return View();
        }
    }
}