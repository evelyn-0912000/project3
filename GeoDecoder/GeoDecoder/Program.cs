using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Net;

namespace GeoDecoder {
	enum Weather {
		raining,
		clear
	};
	class Parser {
		public static Tuple<double, double> Parse(string inp, string filter) {
			if (inp == "cls") {
				Console.Clear();
				return Tuple.Create<double, double>(0.0, 0.0);
			}
			else {
				using (var webClient = new System.Net.WebClient()) {

					var json = webClient.DownloadString("http://maps.googleapis.com/maps/api/geocode/json?address=" + inp);
					RootObject data = new JavaScriptSerializer().Deserialize<RootObject>(json);
					Console.WriteLine("Filter: '" + filter + "'\n");
					if (data.status == "OK") {
						data = Parser.Filter(filter, data);
						int i = 0;
						foreach (var item in data.results) {
							i += 1;
						}
						if (i == 1) {
							foreach (var item in data.results) {
								int componentcount = 0;
								foreach (var component in item.address_components) {
									componentcount += 1;
								}
								Console.WriteLine(i.ToString() + ". " + "(" + item.geometry.location.lat + "," + item.geometry.location.lng + ") - " + item.formatted_address);
								var coordinates = Tuple.Create<double, double>(item.geometry.location.lat, item.geometry.location.lng);
								return coordinates;
							}
						}
						else {
							Console.WriteLine("Multiple results found, please refine search");
							return Tuple.Create<double, double>(0.0, 0.0);
						}
					}
					else {
						Console.WriteLine("No results found");
						return Tuple.Create<double, double>(0.0, 0.0);
					}
				}

			}
			return Tuple.Create<double, double>(0.0, 0.0);
		}
		public static RootObject Filter(string tofilter, RootObject filtrate) {
			RootObject toReturn = new RootObject();
			if (tofilter != null) {
				List<Result> results = new List<Result>();
				toReturn.status = "OK";
				foreach (var item in filtrate.results) {
					bool containsFilter = false;
					foreach (var component in item.address_components) {
						if (component.long_name == tofilter) {
							containsFilter = true;
						}
					}
					if (containsFilter == true) {
						results.Add(item);
					}
				}
				toReturn.results = results;
			}
			else {
				toReturn = filtrate;
			}
			return toReturn;
		}

		public static bool CheckInternet() {
			try {
				using (var client = new System.Net.WebClient()) {
					using (var page = client.OpenRead("http://www.google.com/")) {
						return true;
					}
				}
			}
			catch {
				return false;
			}
		}

		//WEATHER FETCHER STARTS HERE

		public static Tuple<string,Weather,bool,string, string> FetchWeather(string lat, string lon) {
			//Rain heavyness, Raining bool, will rain bool, time will rain, time rain heavyness
			using (WebClient webCl = new WebClient()) {
				byte[] data = webCl.DownloadData("http://gps.buienradar.nl/getrr.php?lat=" + lat + "&lon=" + lon);
				string result = System.Text.Encoding.UTF8.GetString(data).Replace(System.Environment.NewLine, " ");
				int defaultOffset = 1; //1 + (1 for every 5 minutes after the current time) -> 2 is current time
				string[] dataArray = result.Split(" "[0]);
				//Checks if its raining at the current time
				string weatherNow = dataArray[defaultOffset];

				//Checks here if it will start raining somewhere troughout the day
				bool goingToRain = false;
				string time = "";
				string secondheavyness = "";
				if (weatherNow.Substring(0, 3) == "000") {
					foreach (string line in dataArray) {
						if (line.Substring(0, 3) != "000") {
							time = line.Substring(4, 5);
							goingToRain = true;
							secondheavyness = line.Substring(0, 3);
							break;
						}
					}
				}
				if (weatherNow.Substring(0,3) == "000") {
					return new Tuple<string, Weather, bool, string, string>(weatherNow.Substring(0,3), Weather.clear, goingToRain, time, secondheavyness);
				}
				else {
					return new Tuple<string, Weather, bool, string, string>(weatherNow.Substring(0, 3), Weather.raining, goingToRain, time, secondheavyness);
				}
			}
		}

		//WEATHER PARSER ENDS HERE

	}
	//JSON DECLARATIONS
	public class AddressComponent {
		public string long_name { get; set; }
		public string short_name { get; set; }
		public List<string> types { get; set; }
	}

	public class Location {
		public double lat { get; set; }
		public double lng { get; set; }
	}

	public class Northeast {
		public double lat { get; set; }
		public double lng { get; set; }
	}

	public class Southwest {
		public double lat { get; set; }
		public double lng { get; set; }
	}

	public class Viewport {
		public Northeast northeast { get; set; }
		public Southwest southwest { get; set; }
	}

	public class Geometry {
		public Location location { get; set; }
		public string location_type { get; set; }
		public Viewport viewport { get; set; }
	}

	public class Result {
		public List<AddressComponent> address_components { get; set; }
		public string formatted_address { get; set; }
		public Geometry geometry { get; set; }
		public string place_id { get; set; }
		public List<string> types { get; set; }
	}

	public class RootObject {
		public List<Result> results { get; set; }
		public string status { get; set; }
	}
	//END JSON DECLARATIONS

	class Program {
		static void Main(string[] args) {
			Console.WriteLine("GeoLocation Project 3 - Command Prompt (by Wessel)");
			while (Parser.CheckInternet()) {
				string filter = "Netherlands";//null;
				Tuple<double,double> coordinates = Parser.Parse(Console.ReadLine(), filter);
				//if (filter == "Netherlands") {
					var weatherResult = Parser.FetchWeather(coordinates.Item1.ToString(), coordinates.Item2.ToString());
					if (weatherResult.Item2 == Weather.raining) {
						Console.WriteLine("It's raining over there (" + weatherResult.Item1 + ")");
					}
					else {
						Console.Write("It's clear over there");
						if (weatherResult.Item3 == true) {
							Console.WriteLine(" but it will start raining at " + weatherResult.Item4 + " -> (" + weatherResult.Item5 + ")");
					}
					else {
						Console.WriteLine("");
					}
					}
				//}
				
			}
			Console.WriteLine("No Internet found, please try again");
			Console.Read();
		}
	}
}
