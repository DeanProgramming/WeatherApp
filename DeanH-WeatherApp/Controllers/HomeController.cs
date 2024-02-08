using DeanH_WeatherApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace DeanH_WeatherApp.Controllers
{
    public class HomeController : Controller
    {
        public string apiKey = "78d568fc875b58a3430f950f42fcc553"; //Inapproapiate to have my API key exposed like this. I would take additional steps to protect it in build

        private readonly HttpClient _httpClient = new HttpClient();


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
            if (string.IsNullOrWhiteSpace(location))
            { 
                return null; 
            }

            string apiUrl = $"weather?q={location}&appid={apiKey}&units=metric";
            return await GetWeather(apiUrl);
        }

        public async Task<IActionResult?> GetWeatherByLatLong(string lat, string lon)
        {
            if (string.IsNullOrWhiteSpace(lat) || string.IsNullOrWhiteSpace(lon))
            {
                return View();
            }

            string apiUrl = $"weather?lat={lat}&lon={lon}&appid={apiKey}&units=metric";
            return await GetWeather(apiUrl);
        }

        private async Task<IActionResult?> GetWeather(string apiUrl)
        {
            try
            {
                    
                _httpClient.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                WeatherData weatherData = new WeatherData(); 

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync(); 
                    weatherData = JsonConvert.DeserializeObject<WeatherData>(responseBody);
                }
                else
                {                
                    return Json(null);
                }   

                return Json(weatherData); 
            }
            catch (Exception ex)
            {   
                Console.WriteLine($"Exception caught: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        public async Task<IActionResult?> Get5DayForecastByLocation(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            { 
                return null; 
            }

            string apiUrl = $"forecast?q={location}&appid={apiKey}&units=metric";
            return await Get5DayForecastWeather(apiUrl);
        }

        public async Task<IActionResult?> Get5DayForecastByLatLong(string lat, string lon)
        {
            if (string.IsNullOrWhiteSpace(lat) || string.IsNullOrWhiteSpace(lon))
            {
                return View();
            }

            string apiUrl = $"forecast?lat={lat}&lon={lon}&appid={apiKey}&units=metric";
            return await Get5DayForecastWeather(apiUrl);
        }

        private async Task<IActionResult?> Get5DayForecastWeather(string apiUrl)
        { 
            try
            {
                _httpClient.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                List<NextFiveDayForecastData.List> nextFiveDaysList = new List <NextFiveDayForecastData.List>();

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync(); 
                    NextFiveDayForecastData.Root forecastData = JsonConvert.DeserializeObject<NextFiveDayForecastData.Root>(responseBody);

                    // Extract the list of the next 5 days
                    nextFiveDaysList = forecastData.list.Take(80).ToList();

                    return Json(nextFiveDaysList);
                }
                else
                {        
                    return Json(null); 
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}