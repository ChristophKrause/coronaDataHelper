using CoronaDataHelper.Interface;
using CoronaDataHelper.JSON;
using Newtonsoft.Json;
using System;
using System.IO;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CoronaDataHelper.DataSource {

	internal class DataSourceMarlonLueckertGermany : IDataSource {
		//https://v2.rki.marlon-lueckert.de/
		private const string URLJSONDATASOURCEDEATHS = "https://api.corona-zahlen.org/germany/history/deaths";
		private const string URLJSONDATASOURCECASES = "https://api.corona-zahlen.org/germany/history/cases";
		private const string FILENAMEJSONCASES = "marlon-lueckert_germany_cases.json";
		private const string FILENAMEJSONDEATHS = "marlon-lueckert_germany_deaths.json";
		private const string FILENAMEJSON = "marlon-lueckert_germany.json";

		public object process() {
			JSONCoronaVirusData oJSONCoronaVirusDataRetval = new JSONCoronaVirusData();

			JSONGermanyDataMarlonLueckert oJSONStateDataMarlonLueckertCases = getJSONData(URLJSONDATASOURCECASES, FILENAMEJSONCASES);
			JSONGermanyDataMarlonLueckert oJSONStateDataMarlonLueckertDeaths = getJSONData(URLJSONDATASOURCEDEATHS, FILENAMEJSONDEATHS);
			oJSONStateDataMarlonLueckertCases.combine(oJSONStateDataMarlonLueckertDeaths);			

			JSONCountry oJSONCountry = JSONGermanyDataMarlonLueckert.convert(oJSONStateDataMarlonLueckertCases);
			oJSONCoronaVirusDataRetval.DEU = oJSONCountry;

			string strJson = JsonConvert.SerializeObject(oJSONCoronaVirusDataRetval, Formatting.Indented, new JsonSerializerSettings {
				NullValueHandling = NullValueHandling.Ignore
			});
			File.WriteAllText(FILENAMEJSON, strJson);

			return oJSONCoronaVirusDataRetval;
		}

		
		//TODO: same method, but differen T => this can be solved better!
		private static JSONGermanyDataMarlonLueckert getJSONData(string strjSONURL, string strFileNameJSON) {
			string strJSON;

			if (!string.IsNullOrWhiteSpace(strFileNameJSON) && File.Exists(strFileNameJSON)) {
				FileInfo oFileInfo = new FileInfo(strFileNameJSON);
				TimeSpan ts = DateTime.Now - oFileInfo.CreationTime;
				Console.WriteLine("ts.TotalHours:" + ts.TotalHours);
				if (ts.TotalHours > 11) {
					try {
						Console.WriteLine("Deleting old file");
						File.Delete(strFileNameJSON);
					} catch { }
				}
			}

			if (string.IsNullOrWhiteSpace(strFileNameJSON)) {
				Console.WriteLine("Downloading Data");
				strJSON = Util.downloadPageSource(strjSONURL);
			} else if (!File.Exists(strFileNameJSON)) {
				Console.WriteLine("Downloading Data");
				strJSON = Util.downloadPageSource(strjSONURL);
				File.WriteAllText(strFileNameJSON, strJSON);
			} else {
				Console.WriteLine("Read file Data");
				strJSON = File.ReadAllText(strFileNameJSON);
			}

			Console.WriteLine("deserialize Data");
			return JsonConvert.DeserializeObject<JSONGermanyDataMarlonLueckert>(strJSON);
		}
	}
}