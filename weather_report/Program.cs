// See https://aka.ms/new-console-template for more information
using System;
using System.Globalization;
using weather_report;
using weather_report.Interfaces;
using weather_report.Models;
using weather_report.Services;

/*
if (args.Count() != 3)
{
    Console.WriteLine("Error: wrong numbers of input parameters");
    return;
}   
if(String.Compare(args[1], "--key", true) != 0)
{
    Console.WriteLine("Error: wrong parameter name: expected --key, entered {0}", args[1]);
    return;
}*/

//Globals.WEATHER_API_KEY = args[2];

Console.WriteLine("Please enter a valid weatherapi key");
string? input = Console.ReadLine();
while(String.IsNullOrEmpty(input))
{
    Console.WriteLine("Error: please enter an API key");
    input = Console.ReadLine();
}
Globals.WEATHER_API_KEY = input;

CitiesApiConsumer consumer = new CitiesApiConsumer(new CityDataParser<City>());
try
{
    await consumer.GetCities();
    ILogger logger = new ConsoleLogger();

    foreach(var city in consumer.EnumCities())
    {
        WeatherApiConsumer weather=new WeatherApiConsumer(new DynamicDataParser<WeatherReport>());
        await weather.GetForecastByCityCoordinates(city.Latitude, city.Longitude);
        logger.LogForecast(city.Name, weather.EnumForecast());
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

