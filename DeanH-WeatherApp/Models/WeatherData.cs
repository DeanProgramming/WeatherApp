namespace DeanH_WeatherApp.Models
{
    public class WeatherData
    {
        public Coord Coord { get; set; }
        public MainData Main { get; set; }
        public Sys Sys { get; set; }
        public Weather[] Weather { get; set; }
    }

    public class MainData
    {
        public double Temp { get; set; } 
        public double Humidity { get; set; }
    }

    public class Coord
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
    }

    public class Weather
    {
        public string Description { get; set; }
    }

    public class Sys
    {
        public string Sunrise { get; set; }
        public string Sunset { get; set; }
    }
}
