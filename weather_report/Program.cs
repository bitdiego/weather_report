// See https://aka.ms/new-console-template for more information
using weather_report.Interfaces;
using weather_report.Services;

CitiesApiConsumer consumer = new CitiesApiConsumer();
try
{
    await consumer.GetCities();
    ILogger logger = new ConsoleLogger();
    logger.LogCities(consumer.cities);
    /*foreach(var city in consumer.cities)
    {
        logger.LogCity(city);
    }*/
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

