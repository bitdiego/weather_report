// See https://aka.ms/new-console-template for more information
using weather_report.Services;

CitiesApiConsumer consumer = new CitiesApiConsumer();
try
{
    await consumer.GetCities();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

