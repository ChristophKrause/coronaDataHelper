using CoronaDataHelper.Interface;
using CoronaDataHelper.JSON;
using Newtonsoft.Json;
using System;
using System.IO;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CoronaDataHelper.DataSource {

	internal class DataSourceMarlonLueckert : IDataSource {
		//https://v2.rki.marlon-lueckert.de/
		private const string URLJSONDATASOURCEDEATHS = "https://v2.rki.marlon-lueckert.de/states/history/deaths";
		private const string URLJSONDATASOURCECASES = "https://v2.rki.marlon-lueckert.de/states/history/cases";
		private const string FILENAMEJSONCASES = "marlon-lueckert_case_data.json";
		private const string FILENAMEJSONDEATHS = "marlon-lueckert_death_data.json";
		private const string FILENAMEJSON = "marlon-lueckert_data.json";

		public object process() {


			JSONStateDataMarlonLueckert oJSONStateDataMarlonLueckertCases = getJSONData(URLJSONDATASOURCECASES, FILENAMEJSONCASES);
			JSONStateDataMarlonLueckert oJSONStateDataMarlonLueckertDeaths = getJSONData(URLJSONDATASOURCEDEATHS, FILENAMEJSONDEATHS);

		


			JSONCoronaVirusDataGermany oJSONCoronaVirusDataGermany = new JSONCoronaVirusDataGermany();

			oJSONCoronaVirusDataGermany.addData(oJSONStateDataMarlonLueckertCases, oJSONStateDataMarlonLueckertDeaths);
			string strJson = JsonConvert.SerializeObject(oJSONCoronaVirusDataGermany, Formatting.Indented, new JsonSerializerSettings {
				NullValueHandling = NullValueHandling.Ignore
			});

			File.WriteAllText(FILENAMEJSON, strJson);

			return oJSONCoronaVirusDataGermany;
		}

		
		private static JSONStateDataMarlonLueckert getJSONData(string strjSONURL, string strFileNameJSON) {
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
			return JsonConvert.DeserializeObject<JSONStateDataMarlonLueckert>(strJSON);
		}
	}
}