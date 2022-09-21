using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using weather_report.Models;

namespace weather_report.Services
{
    public class CitiesApiConsumer
    {
        private IEnumerable<City> cities;
        //private readonly IWeatherApiService _service;

        public CitiesApiConsumer()
        {
            cities = new List<City>();
        }
        public async Task GetCities()
        {
            string content = await RequestHandler.SendGetRequestAsync(Globals.TUI_CITIES_URL);
            if (!String.IsNullOrEmpty(content))
            {
                CityDataParser <City> parser = new CityDataParser<City>();
                cities = parser.GetDataListFromString(content);
            }
            else
            {
                throw new ArgumentNullException("Error: null data from serer");
            }
        }

        public IEnumerable<City> EnumCities()
        {
            return cities;
           /* foreach (var city in cities)
            {
                yield return city;
            }*/
        }
    }
}
