using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DeanH_WeatherApp.Controllers;
using DeanH_WeatherApp.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using Newtonsoft.Json;


[TestFixture]
public class NetTestCases
{
    private Mock<IHttpClientFactory> httpClientFactoryMock;
    private Mock<HttpMessageHandler> httpMessageHandlerMock;
    private HttpClient httpClient;

    [SetUp]
    public void Setup()
    {
        httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        httpClient = new HttpClient(httpMessageHandlerMock.Object);

         
        httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
    }


    [Test]
    public async Task GetWeatherByLocation_ReturnsCorrectResult()
    {
        // Arrange
        var location = "New York";
        var expectedWeatherData = new WeatherData
        { 
            Coord = new Coord { Lat = 1.0, Lon = 2.0 },
            Main = new MainData { Temp = 20.0, Humidity = 50.0 },
            Sys = new Sys { Sunrise = "6:00 AM", Sunset = "6:00 PM" },
            Weather = new[] { new Weather { Description = "Clear sky" } }
        };

        var controller = new HomeController(httpClientFactoryMock.Object);
 
        var expectedResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonConvert.SerializeObject(expectedWeatherData))
        };

        httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await controller.GetWeatherByLocation(location) as JsonResult;

        // Assert
        Assert.IsNotNull(result); 

        var actualWeatherData = result.Value as WeatherData;
        Assert.IsNotNull(actualWeatherData);
 
        Assert.AreEqual(expectedWeatherData.Coord.Lat, actualWeatherData.Coord.Lat);
        Assert.AreEqual(expectedWeatherData.Coord.Lon, actualWeatherData.Coord.Lon); 
    }
    
    [Test]
    public async Task GetWeatherByLocation_HandlesApiError()
    {
        var controller = new HomeController(httpClientFactoryMock.Object);
 
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.InternalServerError
        };

        httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponseMessage);

        // Act
        var result = await controller.GetWeatherByLocation("New York") as JsonResult;

        // Assert
        Assert.IsNotNull(result, "Result should not be null.");
        Assert.IsNull(result.Value, "Weather data should be null for error response.");
    }

    [Test]
    public async Task GetWeatherByLocation_EmptyInputString()
    { 
        var controller = new HomeController(httpClientFactoryMock.Object);
        
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.InternalServerError
        };

        httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponseMessage);
 
        httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Act
        var result = await controller.GetWeatherByLocation(" ") as JsonResult;
        
        Assert.IsNull(result, "WeatherData should be null.");
    }

    [Test]
    public async Task GetWeatherByLocation_InvalidLocation()
    { 
    var controller = new HomeController(httpClientFactoryMock.Object);
    
    
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.InternalServerError
        };

        httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponseMessage);
 
        httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
 
        // Act
        var result = await controller.GetWeatherByLocation(" ") as JsonResult;
        
        Assert.IsNull(result, "WeatherData should be null.");
 
        httpMessageHandlerMock.Reset();
        httpClientFactoryMock.Reset();
    }

    [Test]
    public async Task GetWeatherByLatAndLong_ReturnsCorrectResult()
    { 
        var controller = new HomeController(httpClientFactoryMock.Object);
    
        var expectedWeatherData = new WeatherData
        {
            Coord = new Coord { Lat = 40.7128, Lon = -74.0060 },
            Main = new MainData { Temp = 20.5, Humidity = 70.0 },
            Sys = new Sys { Sunrise = "6:00 AM", Sunset = "6:00 PM" },
            Weather = new[] { new Weather { Description = "Clear sky" } }
        };


        var httpResponseMessage = new HttpResponseMessage
        { 
            Content = new StringContent(JsonConvert.SerializeObject(expectedWeatherData))
        };

        httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponseMessage);
 

        httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Act
        var result = await controller.GetWeatherByLatLong("40.7128", "-74.0060") as JsonResult;

        // Assert
        Assert.IsNotNull(result, "Result should not be null.");
        
        var weatherData = result.Value as WeatherData;
        Assert.IsNotNull(weatherData, "Weather data should not be null.");
        Assert.AreEqual(expectedWeatherData.Coord.Lat, weatherData.Coord.Lat, "Latitude should match.");
        Assert.AreEqual(expectedWeatherData.Coord.Lon, weatherData.Coord.Lon, "Longitude should match.");


        httpMessageHandlerMock.Protected()
            .Verify<Task<HttpResponseMessage>>("SendAsync", 
                Times.Once(), 
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString().Contains("weather")),
                ItExpr.IsAny<CancellationToken>());
    
        httpMessageHandlerMock.Reset();
        httpClientFactoryMock.Reset();
    }


    [Test]
    public async Task GetWeatherByLatAndLong_HandlesApiError()
    {
        var controller = new HomeController(httpClientFactoryMock.Object);
 
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.InternalServerError
        };

        httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponseMessage);

        // Act
        var result = await controller.GetWeatherByLatLong("40.7128", "-74.0060") as JsonResult;

        // Assert
        Assert.IsNotNull(result, "Result should not be null.");
        Assert.IsNull(result.Value, "Weather data should be null for error response.");
    }

    [Test]
    public async Task GetForecastByLocation_ReturnsCorrectResult()
    { 
        var controller = new HomeController(httpClientFactoryMock.Object);
    
        var expectedForecastData = new NextFiveDayForecastData.Root
        {
            city = new NextFiveDayForecastData.City
            {
                id = 1,
                name = "New York",
                coord = new NextFiveDayForecastData.Coord { lat = 40.7128, lon = -74.0060 },
                country = "TestCountry",
                population = 100000,
                timezone = 3600,
                sunrise = 1609459200,
                sunset = 1609502400
            },
            cod = "200",
            message = 0,
            cnt = 5,
            list = new List<NextFiveDayForecastData.List>
            {
                new NextFiveDayForecastData.List
                {
                    dt = 1609459200,
                    main = new NextFiveDayForecastData.Main
                    {
                        temp = 20.5,
                        feels_like = 19.0,
                        temp_min = 18.0,
                        temp_max = 22.0,
                        pressure = 1012,
                        humidity = 70
                    },
                    weather = new List<NextFiveDayForecastData.Weather>
                    {
                        new NextFiveDayForecastData.Weather
                        {
                            id = 800,
                            main = "Clear",
                            description = "Clear sky",
                            icon = "01d"
                        }
                    },
                    clouds = new NextFiveDayForecastData.Clouds { all = 0 },
                    wind = new NextFiveDayForecastData.Wind
                    {
                        speed = 3.0,
                        deg = 150,
                        gust = 5.0
                    },
                    visibility = 10000,
                    pop = 0.1,
                    sys = new NextFiveDayForecastData.Sys { pod = "d" },
                    dt_txt = "2022-01-01 12:00:00"
                } 
            }
        };
 
        var httpResponseMessage = new HttpResponseMessage
        { 
            Content = new StringContent(JsonConvert.SerializeObject(expectedForecastData))
        };

        httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponseMessage);
 
        httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Act
        var result = await controller.Get5DayForecastByLocation("New York") as JsonResult;
        
        // Assert
        Assert.IsNotNull(result, "Result should not be null.");
        
        var forecastData = result.Value as List<NextFiveDayForecastData.List>;
        Assert.IsNotNull(forecastData, "Forecast data should not be null.");  

        httpMessageHandlerMock.Protected()
            .Verify<Task<HttpResponseMessage>>("SendAsync", 
                Times.Once(), 
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString().Contains("forecast")),
                ItExpr.IsAny<CancellationToken>());
    
        httpMessageHandlerMock.Reset();
        httpClientFactoryMock.Reset();
    }
    
    [Test]
    public async Task GetForecastByLocation_HandlesApiError()
    {
        var controller = new HomeController(httpClientFactoryMock.Object);
 
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.InternalServerError
        };

        httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponseMessage);

        // Act
        var result = await controller.Get5DayForecastByLocation("New York") as JsonResult;

        // Assert
        Assert.IsNotNull(result, "Result should not be null.");
        Assert.IsNull(result.Value, "Forecast data should be null for error response.");
    }

    [Test]
    public async Task GetForecastByLatAndLong_ReturnsCorrectResult()
    { 
        var controller = new HomeController(httpClientFactoryMock.Object);
    
        var expectedForecastData = new NextFiveDayForecastData.Root
        {
            city = new NextFiveDayForecastData.City
            {
                id = 1,
                name = "New York",
                coord = new NextFiveDayForecastData.Coord { lat = 40.7128, lon = -74.0060 },
                country = "TestCountry",
                population = 100000,
                timezone = 3600,
                sunrise = 1609459200,
                sunset = 1609502400
            },
            cod = "200",
            message = 0,
            cnt = 5,
            list = new List<NextFiveDayForecastData.List>
            {
                new NextFiveDayForecastData.List
                {
                    dt = 1609459200,
                    main = new NextFiveDayForecastData.Main
                    {
                        temp = 20.5,
                        feels_like = 19.0,
                        temp_min = 18.0,
                        temp_max = 22.0,
                        pressure = 1012,
                        humidity = 70
                    },
                    weather = new List<NextFiveDayForecastData.Weather>
                    {
                        new NextFiveDayForecastData.Weather
                        {
                            id = 800,
                            main = "Clear",
                            description = "Clear sky",
                            icon = "01d"
                        }
                    },
                    clouds = new NextFiveDayForecastData.Clouds { all = 0 },
                    wind = new NextFiveDayForecastData.Wind
                    {
                        speed = 3.0,
                        deg = 150,
                        gust = 5.0
                    },
                    visibility = 10000,
                    pop = 0.1,
                    sys = new NextFiveDayForecastData.Sys { pod = "d" },
                    dt_txt = "2022-01-01 12:00:00"
                } 
            }
        };
 
        var httpResponseMessage = new HttpResponseMessage
        { 
            Content = new StringContent(JsonConvert.SerializeObject(expectedForecastData))
        };

        httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponseMessage);
 
        httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Act
        var result = await controller.Get5DayForecastByLatLong("40.7128", "-74.0060") as JsonResult;
        
        // Assert
        Assert.IsNotNull(result, "Result should not be null.");
        
        var forecastData = result.Value as List<NextFiveDayForecastData.List>;
        Assert.IsNotNull(forecastData, "Forecast data should not be null.");  

        httpMessageHandlerMock.Protected()
            .Verify<Task<HttpResponseMessage>>("SendAsync", 
                Times.Once(), 
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString().Contains("forecast")),
                ItExpr.IsAny<CancellationToken>());
    
        httpMessageHandlerMock.Reset();
        httpClientFactoryMock.Reset();
    }
    
    [Test]
    public async Task GetForecastByLatAndLong_HandlesApiError()
    {
        var controller = new HomeController(httpClientFactoryMock.Object);
 
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.InternalServerError
        };

        httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponseMessage);

        // Act
        var result = await controller.Get5DayForecastByLatLong("40.7128", "-74.0060") as JsonResult;

        // Assert
        Assert.IsNotNull(result, "Result should not be null.");
        Assert.IsNull(result.Value, "Forecast data should be null for error response.");
    } 
}
