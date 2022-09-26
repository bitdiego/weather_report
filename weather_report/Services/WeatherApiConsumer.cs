using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weather_report.Interfaces;
using weather_report.Models;

namespace weather_report.Services
{
    public class WeatherApiConsumer
    {
        private IEnumerable<WeatherReport> weatherReportList;
        private readonly IStringDataParser<WeatherReport> _parser;

        public WeatherApiConsumer(IStringDataParser<WeatherReport> parser)
        {
            _parser = parser;
            weatherReportList = new List<WeatherReport>();
        }

        public async Task GetForecastByCityCoordinates(double lat, double lon)
        {
            string uri = String.Format(Globals.WEATHER_FORECAST_API_URL, Globals.WEATHER_API_KEY, lat.ToString() + "," + lon.ToString());
            string weatherResponse = await RequestHandler.SendGetRequestAsync(uri);
            if (!String.IsNullOrEmpty(weatherResponse))
            {
                //IStringDataParser<WeatherReport> wrParser = new DynamicDataParser<WeatherReport>();
                weatherReportList = _parser.GetDataListFromString(weatherResponse);
            }
            else
            {
                throw new ArgumentNullException("Error: null data from serer");
            }
        }

        public IEnumerable<WeatherReport> EnumForecast()
        {
            return weatherReportList;
           /* foreach (var wr in weatherReportList)
            {
                yield return wr;
            }*/
        }
    }
}
