# Live Weather and Map

## Overview

**Live Weather and Map** is a dynamic web application developed using **.NET 6.0** and **ASP.NET MVC**. The project seamlessly integrates the **OpenWeather API** and **Open Map API** to offer users a comprehensive experience by displaying real-time weather information alongside an interactive map.

## Features

- **Real-time Weather Information:** The application fetches and displays live weather data using the **OpenWeather API**, keeping users up-to-date on current weather conditions.

- **Interactive Map:** Leveraging the **Open Map API**, the application provides an localized map.

- **Next Few Days Forecast Analysis:** The system retrieves the next few days' forecast and analyzes the data to provide users with insightful information:

  - **Average Daily Temperature:** The application calculates and presents the average temperature for each day in the forecast.

  - **Peak Temperature:** Users can easily identify the peak temperature for each day, providing a quick overview of temperature highs.

  - **Lowest Temperature:** The system also highlights the lowest temperature recorded each day, helping users prepare for cooler periods.

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

## Project URL
Visit the [Weather App](https://weatherapp-deanholland.azurewebsites.net/) to explore the code and see the project in action.

## Unit Testing

In addition to the main project, this repository includes a companion project containing unit tests for the **.NET** components. The test project comprises 10 different unit tests that validate the functionality, performance, and reliability of various aspects of the application.
