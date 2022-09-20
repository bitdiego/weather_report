using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using weather_report.Models;
using weather_report.Services;
using Xunit;

namespace WeatherReport_Test
{
    public class WPUnitTest : IDisposable
    {
        CitiesApiConsumer testConsumer;
        public WPUnitTest()
        {
            //cities = new List<City>();
            testConsumer = new CitiesApiConsumer();
            Console.WriteLine("Inside SetUp Constructor");
        }

        public void Dispose()
        {
            Console.WriteLine("Inside CleanUp or Dispose method");
        }
        [Fact]
        public async Task GetCitiesApi_Test()
        {
            await testConsumer.GetCities();
            var cityList = testConsumer.cities as List<City>;
            Assert.True(cityList != null && cityList.Count > 0);

        }
    }
}