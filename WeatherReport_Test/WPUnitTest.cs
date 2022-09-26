using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using weather_report;
using weather_report.Models;
using weather_report.Services;
using Xunit;

namespace WeatherReport_Test
{
    public class WPUnitTest : IDisposable
    {
        private readonly string goodApiKey = "7ad1890196274ff89ea154035221209";
        private readonly string badApiKey = "7ad1890196274ff89ea154035221209";
        CitiesApiConsumer testConsumer;

        public WPUnitTest()
        {
            testConsumer = new CitiesApiConsumer(new CityDataParser<City>());
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
            var cityList = testConsumer.EnumCities();
            var cityType = cityList.GetType();
            Assert.True(cityList != null && (cityList as List<City>).Count>0);
        }
        
        [Theory]
        [InlineData(41.162, -8.623, 2)] //Porto
        [InlineData(41.898, 12.483, 2)] //Rome
        [InlineData(40.760886, -111.890922, 2)] //Salt Lake City
        public async Task SingleWeatherResponse_Test(double lat, double lon, int expected)
        {
            Globals.WEATHER_API_KEY = goodApiKey;
            WeatherApiConsumer weather = new WeatherApiConsumer(new DynamicDataParser<WeatherReport>());
            await weather.GetForecastByCityCoordinates(lat, lon);
            var foreList = weather.EnumForecast();
            Assert.True(foreList!= null && (foreList as List<WeatherReport>).Count==expected);
        }

        [Fact]
        public void BadApiKeySubmitted_Test()
        {
            Globals.WEATHER_API_KEY = badApiKey;
            WeatherApiConsumer weather = new WeatherApiConsumer(new DynamicDataParser<WeatherReport>());

            Assert.ThrowsAsync<ArgumentNullException>(() => weather.GetForecastByCityCoordinates(0.0, 0.0));
        }
    }
}