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
        public IEnumerable<City> cities;
        //private readonly IWeatherApiService _service;

        public CitiesApiConsumer()
        {
            cities = new List<City>();
        }
        public async Task GetCities()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(Globals.TUI_CITIES_URL);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (String.IsNullOrEmpty(content))
                    {
                        throw new InvalidOperationException("Error: empty response from server");
                    }
                    else
                    {
                        cities = JsonConvert.DeserializeObject<List<City>>(content);
                    }
                }
                else
                {
                    throw new HttpRequestException("Server error response");
                    //Console.WriteLine("Internal server Error");
                }
            }
        }
    }
}
