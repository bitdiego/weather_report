using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weather_report.Interfaces;
using weather_report.Models;

namespace weather_report.Services
{
    public class ConsoleLogger : ILogger
    {
        public void LogCity(City city)
        {
            Console.WriteLine(city);
        }

        public void LogCities(IEnumerable<City> cities)
        {
            foreach (var city in cities)
            {
                Console.WriteLine(city.Name +" "+ city.Latitude +" " +city.Longitude + " "+city.Country.Name);
            }
        }
        public void LogForecast()
        {
            throw new NotImplementedException();
        }
    }
}
