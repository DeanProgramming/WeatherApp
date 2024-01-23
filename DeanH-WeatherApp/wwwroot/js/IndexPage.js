function refreshMap(latitude, longitude) {
    var iframe = document.getElementById('mapIframe');
    var src = `https://www.openstreetmap.org/export/embed.html?bbox=${longitude - 0.008}%2C${latitude - 0.005}%2C${longitude + 0.008}%2C${latitude + 0.005}&amp;layer=mapnik`;
    iframe.src = src;
}

function fetchCurrentWeather(url) {
    fetch(url)
        .then(response => {
            if (response.ok) {
                return response.json();
            } else {
                document.getElementById('errorMessage').textContent = 'Please enter a valid location.';
                return null;  // Resolve with null to avoid console error
            }
        })
        .then(data => {
            if (data.main == null) {
                document.getElementById('errorMessage').textContent = 'Please enter a valid location.';
                return;
            }

            document.getElementById('temperature').textContent = data.main.temp;
            document.getElementById('humidity').textContent = data.main.humidity;
            document.getElementById('weatherDescription').textContent = data.weather[0].description;

            const sunriseDate = new Date(data.sys.sunrise * 1000);
            const sunsetDate = new Date(data.sys.sunset * 1000);

            const formattedSunriseTime = sunriseDate.toLocaleTimeString().slice(0, 5);
            const formattedSunsetTime = sunsetDate.toLocaleTimeString().slice(0, 5);

            document.getElementById('sunRise').textContent = formattedSunriseTime;
            document.getElementById('sunSet').textContent = formattedSunsetTime;

            refreshMap(data.coord.lat, data.coord.lon);

            // Show the weather information
            document.getElementById('weatherInfo').classList.remove('hidden');
            document.getElementById('errorMessage').textContent = '';
        })
        .catch(error => {
            document.getElementById('errorMessage').textContent = 'Please enter a valid location.';
        });
}

if (navigator.geolocation) {
    navigator.geolocation.getCurrentPosition(
        (position) => {
            // Handle the user's current position
            var url = '/Home/GetWeatherByLatLong?lat=' + position.coords.latitude + "&lon=" + position.coords.longitude;
            fetchCurrentWeather(url);
            var url = '/Home/Get5DayForecastByLatLong?lat=' + position.coords.latitude + "&lon=" + position.coords.longitude;
            fetchNextFiveDayWeather(url);
        },
        (error) => {
            console.error(error);
        }
    );
} else {
    // Geolocation is not supported
    console.log("Not Supported");
}

function updateWeather() {
    var location = document.getElementById('locationInput').value;
    var url = '/Home/GetWeatherByLocation?location=' + encodeURIComponent(location);

    // Check if the location is not empty
    if (location !== '') {
        var url = '/Home/GetWeatherByLocation?location=' + encodeURIComponent(location);
        fetchCurrentWeather(url);
        var url = '/Home/Get5DayForecastByLocation?location=' + encodeURIComponent(location);
        fetchNextFiveDayWeather(url);
    } else {
        // Display an error message or take appropriate action for empty location
        document.getElementById('errorMessage').textContent = 'Please enter a valid location.';
    }
}


function isSameDay(date1, date2) {
    return date1.substr(0, 10) === date2.substr(0, 10);
}

class NextDaysData {
    constructor() {
        this.date = "";
        this.displayDate = "";
        this.averageTemperature = 0;
        this.highTemperature = 0;
        this.lowTemperature = 0;
    }
}

function fetchNextFiveDayWeather(url) {
    fetch(url)
        .then(response => {
            if (response.ok) {
                return response.json();
            } else {
                document.getElementById('errorMessage').textContent = 'Please enter a valid location.';
                return null;
            }
        })
        .then(data => {
            if (data == null) {
                document.getElementById('errorMessage').textContent = 'Please enter a valid location.';
                return;
            }

            const forecastContainer = document.getElementById('forecastContainer');
            const forecastTitle = document.getElementById('forecastTitle');
            // Clear previous content
            forecastContainer.innerHTML = '';
            forecastTitle.innerHTML = '';

            var currentTime = new Date().toISOString();
            currentTime = currentTime.substr(0, 10);

            const nextCoupleDaysForecast = [];

            let averageTemperature = 0;
            let lowestTemperature = 100;
            let highestTemperature = -100;
            let dateSets = 0;

            data.forEach(day => {
                if (isSameDay(day.dt_txt, currentTime)) {
                    averageTemperature += day.main.temp;
                    dateSets++;
                    lowestTemperature = Math.min(lowestTemperature, day.main.temp);
                    highestTemperature = Math.max(highestTemperature, day.main.temp);
                } else {
                    const nextDay = new NextDaysData();

                    let month = currentTime.slice(5, 7);
                    let dayOfMonth = currentTime.slice(8, 10);

                    nextDay.date = currentTime;
                    nextDay.displayDate = dayOfMonth + "-" + month;
                    nextDay.averageTemperature = (averageTemperature / dateSets);
                    nextDay.lowTemperature = lowestTemperature;
                    nextDay.highTemperature = highestTemperature;
                    nextCoupleDaysForecast.push(nextDay);

                    averageTemperature = day.main.temp;
                    dateSets = 1;
                    currentTime = day.dt_txt;
                    lowestTemperature = 100;
                    highestTemperature = -100;
                }
            });

            let startIndex = 0;

            if (nextCoupleDaysForecast[0].date == new Date().toISOString().substr(0, 10)) {
                startIndex++; //Skip if its the current day
            }

            const dayElement = document.createElement('div');
            dayElement.className = 'day';

            dayElement.innerHTML = `
                    <h2>Next ${5 - startIndex} Day Weather Forecast</h2>
                `;
            forecastTitle.appendChild(dayElement);

            for (let i = startIndex; i < nextCoupleDaysForecast.length; i++) {
                const dayElement = document.createElement('div');
                dayElement.className = 'forecastDay';

                dayElement.innerHTML = `
                                <p class="date">Date: ${nextCoupleDaysForecast[i].displayDate}</p>
                            <p class="temperature">Average Temperature:</p>
                            <p class="temperature"> ${nextCoupleDaysForecast[i].averageTemperature.toFixed(2)} °C</p>

                            <p class="temperature">Highest Temperature:</p>
                            <p class="temperature"> ${nextCoupleDaysForecast[i].highTemperature.toFixed(2)} °C</p>

                            <p class="temperature">Lowest Temperature:</p>
                            <p class="temperature"> ${nextCoupleDaysForecast[i].lowTemperature.toFixed(2)} °C</p>
                    `;

                forecastContainer.appendChild(dayElement);
            }
        })
        .catch(error => {
            document.getElementById('errorMessage').textContent = 'Please enter a valid location.';
        });
}
