using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weather_report.Models;

namespace weather_report.Interfaces
{
    public interface ILogger
    {
        void LogCity(City city);
        void LogCities(IEnumerable<City> cities);
        void LogForecast(string cityName, IEnumerable<WeatherReport> forecasts);
    }
}
