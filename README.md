# Live Weather and Map

## Overview

**Live Weather and Map** is a dynamic web application developed using **.NET 6.0** and **ASP.NET MVC**. The project seamlessly integrates the **OpenWeather API** and **Open Map API** to offer users a comprehensive experience by displaying real-time weather information alongside an interactive map. The application's architecture follows a three-tier design, consisting of the presentation layer, the application layer, and integration with external services.

## Project URL
Visit the [Weather App](https://weatherapp-deanholland.azurewebsites.net/) to explore the code and see the project in action.

## Features

- **Real-time Weather Information:** The application fetches and displays live weather data using the **OpenWeather API**, keeping users up-to-date on current weather conditions.

- **Interactive Map:** Leveraging the **Open Map API**, the application provides an localized map.

- **Next Few Days Forecast Analysis:** The system retrieves the next few days' forecast and analyzes the data to provide users with insightful information:

  - **Average Daily Temperature:** The application calculates and presents the average temperature for each day in the forecast.

  - **Peak Temperature:** Users can easily identify the peak temperature for each day, providing a quick overview of temperature highs.

  - **Lowest Temperature:** The system also highlights the lowest temperature recorded each day, helping users prepare for cooler periods.
    
- **Seamless Integration**: By integrating functionalities of both the OpenWeatherMap and OpenStreetMap APIs, the application delivers an immersive experience, allowing users to select a location of interest, view its current weather conditions, and explore the surrounding areas through the intuitive map interface.
  
- **Scalability and Performance**: Designed with scalability and performance in mind, the application supports horizontal scaling through load balancing and auto-scaling mechanisms. Performance optimizations include minimizing API call latency and employing asynchronous processing and parallelization techniques.

## Design Documentation

[Design Document](https://deanprogramming.github.io/CV/Live%20Weather%20and%20Map%20Application%20Design%20Doc.pdf) 

## System Architecture
The Live Weather and Map Application is built on a three-tier architecture:
1. **Presentation Layer**: Consists of user interface components responsible for rendering weather information and maps. It includes web pages, views, and client-side scripts designed to be intuitive and user-friendly.
2. **Application Layer**: Serves as the intermediary between the presentation layer and external services. It handles user requests, fetches data from APIs, and orchestrates information flow within the application.
3. **Integration with External Services**: The application interacts with the OpenWeatherMap and OpenStreetMap APIs using RESTful principles, exchanging JSON-formatted data over HTTP protocols to retrieve real-time weather updates, forecasts, and geographical information.

## Components
- **Model**: Defines the structure and data manipulation logic for the application. The `NextFiveDayForecastData` model includes classes representing forecast data for the next five days, while the `WeatherData` model represents current weather data.
  
- **View**: Implemented using Razor Pages along with HTML, CSS, and JavaScript, it provides the user interface for interacting with weather data. Key files include:
  - `Index.cshtml`: Displays the home page, allowing users to input their location and view current weather information, the five-day weather forecast, and an interactive map.
  - `IndexPage.js`: Handles fetching and displaying current weather and forecast data, as well as user input validation and UI updates.

- **Controller**: Manages application logic, including routing, data retrieval, and view rendering. The `HomeController.cs` handles HTTP requests related to weather data, communicates with the OpenWeatherMap API, and returns JSON responses.

## Deployment

The application is deployed on **Azure**, ensuring a reliable and scalable hosting environment.

## Getting Started

1. **Prerequisites**
   - Install [.NET Core](https://dotnet.microsoft.com/download) 

2. **Installation**
   - Clone the repository: `git clone https://github.com/DeanProgramming/WeatherApp.git`
   - Navigate to the project directory: `cd WeatherApp`

3. **API Setup**
   - Find API key called "API_KEY" and import a free api key from [Open Weather](https://openweathermap.org/api)

4. **Run the Application**
   - Run the application using the following command: `dotnet run`

5. **Access the Application**
   - Open your web browser and go to `http://localhost:5000` to access the Weather App.

## Unit Testing

In addition to the main project, this repository includes a companion project containing unit tests for the **.NET** components. The test project comprises 10 different unit tests that validate the functionality, performance, and reliability of various aspects of the application.
