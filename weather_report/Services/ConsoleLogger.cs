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
        private readonly string dummyString = "Processed city ";
        public void LogCity(City city)
        {
            Console.WriteLine(city.Name + " " + city.Latitude + " " + city.Longitude + " " + city.Country.Name);
        }

        public void LogCities(IEnumerable<City> cities)
        {
            foreach (var city in cities)
            {
                Console.WriteLine(city.Name +" "+ city.Latitude +" " +city.Longitude + " "+city.Country.Name);
            }
        }
        public void LogForecast(string cityName, IEnumerable<WeatherReport> forecasts)
        {
            int nItems = forecasts.Count();
            int counter = 0;
            Console.Write(dummyString + cityName);
            Console.Write(" | ");
            foreach (WeatherReport weather in forecasts)
            {
                Console.Write(weather.Text);
                if (counter < nItems - 1)
                {
                    Console.Write(" - ");
                }
                ++counter;
            }


            /*var xfList = forecasts.ToList();
            
            for(int i=0; i< xfList.Count(); i++)
            {
                Console.Write(xfList[i].Text);
                
                if (i < xfList.Count() - 1) 
                {
                    Console.Write(" - ");
                }
            }*/
            Console.WriteLine();
        }
    }
}
