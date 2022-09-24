Project name: weather_report
Author: Diego Polidori <d.poldo@libero.it>

STEP 1) Desktop application for weather forecast
===================================================
It is required to develop a desktop application to retrieve weather data related to a set of cities for the next two days (current day and following day) 
and print the result to an output channel (a console, or maybe to a file for future implementations).
The task is accomplished in two consecutive steps: first, get the list of the cities of interest, by sending a GET request to a TUI Musement web API 
(endpoint: https://api.musement.com/api/v3/cities/).
Second, for each city, send another GET request to a web API, whose endpoint is https://api.weatherapi.com/v1/forecast.json?key={0}&q={1}&days=2&aqi=no&alerts=no 
and is parameterized according the API key released by weatherapi.com after registration and {1} is the second parameter which may be the city coordinates in decimal degrees, 
or even the city name, acording to the API documentation (https://www.weatherapi.com/api-explorer.aspx#forecast and https://www.weatherapi.com/docs/).
The first problem is to design the model classes based on the format of Json response returned by the web APIs
By inspecting the official documentation (https://api.musement.com/swagger_3.5.0.json) and with the help of a third party tool (Postman), 
the response of api/v3/cities is an array of Json abjects from which we can notice all the relevant properties of each city: here is an extract
****************************************
{
	"id": 72,
	"uuid": "416f62f0-3384-11ea-a8ce-06c1426e0cac",
	"top": false,
	"name": "Porto",
	"code": "porto",
	"content": "<some text>",
	"meta_description": "<some text>",
	"meta_title": "Things to do in Porto: Attractions, tours, and museums",
	"headline": "Things to do in Porto",
	"weight": 100,
	"latitude": 41.162,
	"longitude": -8.623,
	"country": {
		"id": 139,
		"name": "Portugal",
		"iso_code": "PT"
	},
	"cover_image_url": "https://images.musement.com/cover/0002/69/porto-jpg_header-168797.jpeg",
	"url": "https://www.musement.com/us/porto/",
	"activities_count": 92,
	"time_zone": "Europe/Lisbon",
	"list_count": 0,
	"venue_count": 7,
	"show_in_popular": false
},
{
<other city>
},....
****************************************
It was natural to create two classes mapping the listed property: the City class and Country class, that is also a navigation property in the City.cs file.
The data filling operation is achieved in the following way: the RequestHandler class calls the cities API, return a string (if successful)
and with the help of the Newtonsoft.Json package, deserializes it to the collection of cities and countries properties.
///////////////////////////////////////////////
The data excraction from weatherapi API has been a little bit more complicated. First, I had to carefully detect the portion of the response containing the useful part. 
In the end, this is contained in the forecast->forecastday section, which is an array of weather data (temperature, wind speed, humidity and a condition object containing 
in its turn the daily forecast entence).
Even if the STEP 1 question refers only to the condition->text property, I decided to design a class with all the properties listed in the forecast->forecastday->day object. 
This may seem redundant, but thinking of possible future evolutions, it seemed to me the most convenient choice.
The only difference is that I decided to expand the inner *condition* object and include its properties directly in the WeatherReport class.
Furthermore, this choice allows me to face the second step, the design the APIs for writing / reading forecasts for a given city.
The data filling operation is similar to that performed in the previous section: the RequestHandler class calls the weatherapi API, the string returned  is deserialized
to a dynamic object and, using Reflection, the WeatherReport object is built. It is to be noticed that WeatherApiConsumer class finally builds a list of WeatherReport objects,
whose length is equal to number of days passed to the weatherapi API (2, as stated before).
Finally, the logging process occurs: I realized a ConsoleLogger class that implements an ILogger interface. The ConsoleLogger prints on the console the daily forecasts. 
Of course, we can figure out to log forecasts to a text file; in this case we simply have to build a class to achieve this goal by implementing the same interface.

++++++++++++++++++
CLASS VIEW SUMMARY
++++++++++++++++++
INTERFACES
°° ILogger -> exposes methods to log forecasts to any support
°° IStringDataParser<T> -> generic interface, where T is a reference type and has a default constructor
------------------
MODELS
Simple POCOs:
Country.cs: three properties: Id (integer), Name and Iso_code (string)
City.cs: many properties and a Country reference as a navigation property
WeatherReport.cs: meteo data. Designed according to forecast->forecastday->day section of weatherapi response
------------------
SERVICES
+ RequestHandler.cs: exposes a single method SendGetRequestAsync, which receives a string uri as input parameter and calls the related API. 
Used by CitiesApiConsumer and WeatherApiConsumer
+ CityDataParser.cs: implements IStringDataParser<T> interface where T is City type
+ DynamicDataParser.cs: implements IStringDataParser<T> interface where T is WeatherReport type
+ CitiesApiConsumer.cs: exposes method GetCities to build the list of City objects based on response from GET API from /api/v3/cities
+ WeatherApiConsumer.cs: exposes method GetForecastByCityCoordinates to build the list of WeatherReport objects returned (and parsed) by weatherapi.com/v1/forecast.json
+ ConsoleLogger.cs: implements ILogger interface to print data to standard console
Both CitiesApiConsumer and WeatherApiConsumer are injected an instance of IStringDataParser which parses the string response received by related web API.
------------------
* Globals.cs: exposes some constants, such as the uris to cities and forecast apis. Contains also an extension method to capitalize an input string. 
Used in DynamicDataParser class to set a value by an object property name. It also contains the key needed to call weatherapi.com apis.
* Program.cs: entry point. Gets input from user to set the weatherapi key and performs other preliminary checks (number of input args, name and value).

===================================================
STEP 2) APIs design for write / read forecast data
===================================================
The proposed solution for this step starts from the definition of the WeatherReport class. 
Let's examine first the problem of writing data to a storage and suppose we are using a Relational Database. 
Therefore, somewhere there exists a data table, named City, in which we can suppose an Id column inside.
We could design a table mapping the WeatherReport class, with an external key, CityId, referencing the City table.
We should also include a column for storing the date (yyyy-MM-dd) of the forecast to write (and subsequently read).
Thus, the structure of the Sql WeatherReport table will be as following:
[Id]|Maxtemp_c|Maxtemp_f|Mintemp_c|Mintemp_f|Avgtemp_c|Avgtemp_f|Maxwind_mph|Maxwind_kph|[..other columns as in WeatherReport.cs..]|Text|Icon|Code|CityId|Date
Again, we are assuming a redundant structure as compared to the initial specifications of the tasks: we want to provide a system able to answer to questions such 'How is the 
weather on date X in city Y?', but also 'What is the temperature (humidity, precipitation percentage etc) on date X in city Y?'.
With this structure in mind, the API should manage a POST request, and the input data, in Json encoding, should be in the following format:
{
	"Maxtemp_c":<some value>,
	"Maxtemp_f":<some value>,
	"Mintemp_c":<some value>,
	"Mintemp_f":<some value>,
	"Avgtemp_c":<some value>,
	"Avgtemp_f":<some value>,
	"Maxwind_mph":<some value>,
	"Maxwind_kph":<some value>,
	"Totalprecip_mm":<some value>,
	"Totalprecip_in":<some value>,
	"Avgvis_km":<some value>,
	"Avgvis_miles":<some value>,
	"Avghumidity":<some value>,
	"Daily_will_it_rain":<some value>,
	"Daily_chance_of_rain":<some value>,
	"Daily_will_it_snow":<some value>,
	"Daily_chance_of_snow":<some value>,
	"Uv":<some value>,
	"Text":"<some value>",
	"Icon":"<some value>",
	"Code":<some value>,
	"CityId":<some value>,
	"Date":"<some date>"
}

The backend POST api has the following signature:

[HttpPost("/forecasts")]
public IActionResult Post([FromBody]WeatherReport wrRequest)
{
    ....
}
We could also allow the method to accept an array of WeatherReport objects: in fact, from STEP 1 a forecast contains at least 2 days of data
A better solution is therefore:

[HttpPost("/forecasts")]
public IActionResult Post([FromBody]List<WeatherReport> wrRequests)
{
    ....
}
The possible responses from POST method can be:
* "Data succesfully imported" status code 201
* "Error in input data" status code 400
* "An error occurred, could not save data", maybe an internal server error, or db connection error: status code 500

To read a forecast, we could proceed in this way: the client performs a GET request sending at least the numeric Id of a city.
If no further parameter is sent, we assume that the request is related to the current date.
To get the result of any other day, the client must send a second parameter, date, as a plain string (we can assume that in the backend a conversion is 
performed from string to Sql type smalldatetime).
Therefore, the GET method will be as the following, including a (pseudo)implementation of the response for different cases

[HttpGet("/forecast/cityId/{date}")]
public IActionResult Get(int? id, string date)
{
	if(id == null)
	{
		return NotFound("Missing id of city")
	}
	else{
		City city = GetCityById(id);
		if(city == null)
		{
			return NotFound("City not found");
		}
		
		if(String.IsNullOrEmpty(date))
		{
			//Get forecast of current day: 
			//if !exist, return 404
			//else return Ok(forecast.text)
		}
		else
		{
			//Get forecast of input day: 
			//if !exist, return 404
			//else return Ok(forecast.text)
		}
	}

}

To summarize, the possible responses for GET method are:
* "Weather in [city] on [date] is [text [temperature | pressure | rain percentage]]", status code 200
* "Missing id of city" status code 404
* "City not found" status code 404
* "Forecast on [Date] not found for [city]" status code 404
* "An error occurred, please try later", maybe an internal server error, status code 500