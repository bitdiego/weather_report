// See https://aka.ms/new-console-template for more information
using weather_report;
using weather_report.Interfaces;
using weather_report.Models;
using weather_report.Services;

CitiesApiConsumer consumer = new CitiesApiConsumer();
try
{
    await consumer.GetCities();
    ILogger logger = new ConsoleLogger();
    //logger.LogCities(consumer.EnumCities());
    foreach(var city in consumer.EnumCities())
    {
        //logger.LogCity(city);
        string uri = String.Format(Globals.WEATHER_FORECAST_API_URL, Globals.WEATHER_API_KEY, city.Latitude.ToString() + "," + city.Longitude.ToString());
        string weatherResponse = await RequestHandler.SendGetRequestAsync(uri);
        IStringDataParser<WeatherReport> wrParser = new DynamicDataParser<WeatherReport>();
        var plop = wrParser.GetDataListFromString(weatherResponse);
        //foreach (var forecast in plop)
        {
            Console.WriteLine("Processed city {0} | {1} - {2}", city.Name, plop[0].Text, plop[1].Text);
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

