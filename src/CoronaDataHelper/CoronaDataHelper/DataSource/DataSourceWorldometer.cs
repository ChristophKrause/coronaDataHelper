using CoronaDataHelper.Interface;
using CoronaDataHelper.JSON;
using CoronaDataHelper.Scraper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CoronaDataHelper.DataSource {

	internal class DataSourceWorldometer : IDataSource {
		private const string URLJSONDATASOURCE = "https://www.worldometers.info/coronavirus/country/";
		private const string FILENAMEJSON = "coronavirus-source-data_worldometer.json";

		private JSONCoronaVirusData m_oJSONCoronaVirusData = new JSONCoronaVirusData();

		private List<JSONCountry> m_listCountries = new List<JSONCountry>();

		internal DataSourceWorldometer() {
		}

		public object process() {
			prepare();
			return getJSONData(URLJSONDATASOURCE, FILENAMEJSON);
		}

		private void prepare() {
			JSONCountry oJSONCountry = new JSONCountry();
			oJSONCountry.location = "Italy";
			m_oJSONCoronaVirusData.ITA = oJSONCountry;
			m_listCountries.Add(oJSONCountry);

			oJSONCountry = new JSONCountry();
			oJSONCountry.location = "Spain";
			m_oJSONCoronaVirusData.ESP = oJSONCountry;
			m_listCountries.Add(oJSONCountry);

			oJSONCountry = new JSONCountry();
			oJSONCountry.location = "Us";
			m_oJSONCoronaVirusData.USA = oJSONCountry;
			m_listCountries.Add(oJSONCountry);

			oJSONCountry = new JSONCountry();
			oJSONCountry.location = "Germany";
			m_oJSONCoronaVirusData.DEU = oJSONCountry;
			m_listCountries.Add(oJSONCountry);

			oJSONCountry = new JSONCountry();
			oJSONCountry.location = "France";
			m_oJSONCoronaVirusData.FRA = oJSONCountry;
			m_listCountries.Add(oJSONCountry);

			oJSONCountry = new JSONCountry();
			oJSONCountry.location = "Iran";
			m_oJSONCoronaVirusData.IRN = oJSONCountry;
			m_listCountries.Add(oJSONCountry);

			oJSONCountry = new JSONCountry();
			oJSONCountry.location = "Uk";
			m_oJSONCoronaVirusData.GBR = oJSONCountry;
			m_listCountries.Add(oJSONCountry);

			oJSONCountry = new JSONCountry();
			oJSONCountry.location = "Netherlands";
			m_oJSONCoronaVirusData.NLD = oJSONCountry;
			m_listCountries.Add(oJSONCountry);

			oJSONCountry = new JSONCountry();
			oJSONCountry.location = "Belgium";
			m_oJSONCoronaVirusData.BEL = oJSONCountry;
			m_listCountries.Add(oJSONCountry);

			oJSONCountry = new JSONCountry();
			oJSONCountry.location = "Sweden";
			m_oJSONCoronaVirusData.SWE = oJSONCountry;
			m_listCountries.Add(oJSONCountry);

			oJSONCountry = new JSONCountry();
			oJSONCountry.location = "Brazil";
			m_oJSONCoronaVirusData.BRA = oJSONCountry;
			m_listCountries.Add(oJSONCountry);

			oJSONCountry = new JSONCountry();
			oJSONCountry.location = "Ireland";
			m_oJSONCoronaVirusData.IRL = oJSONCountry;
			m_listCountries.Add(oJSONCountry);

			oJSONCountry = new JSONCountry();
			oJSONCountry.location = "Canada";
			m_oJSONCoronaVirusData.CAN = oJSONCountry;
			m_listCountries.Add(oJSONCountry);

			oJSONCountry = new JSONCountry();
			oJSONCountry.location = "Israel";
			m_oJSONCoronaVirusData.ISR = oJSONCountry;
			m_listCountries.Add(oJSONCountry);

			oJSONCountry = new JSONCountry();
			oJSONCountry.location = "Austria";
			m_oJSONCoronaVirusData.AUT = oJSONCountry;
			m_listCountries.Add(oJSONCountry);

			oJSONCountry = new JSONCountry();
			oJSONCountry.location = "India";
			m_oJSONCoronaVirusData.IND = oJSONCountry;
			m_listCountries.Add(oJSONCountry);
		}

		private JSONCoronaVirusData getJSONData(string strjSONURL, string strFileNameJSON) {
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

			if (File.Exists(strFileNameJSON)) {
				string strJsonText = File.ReadAllText(strFileNameJSON);
				return JsonConvert.DeserializeObject<JSONCoronaVirusData>(strJsonText);
			}
			foreach (var item in m_listCountries) {
				var data = getJSON(item);
				item.data = data;
			}

			string strJson = JsonConvert.SerializeObject(m_oJSONCoronaVirusData, Formatting.Indented, new JsonSerializerSettings {
				NullValueHandling = NullValueHandling.Ignore
			});

			File.WriteAllText(strFileNameJSON, strJson);
			return m_oJSONCoronaVirusData;
		}

		private List<JSONDailyData> getJSON(JSONCountry oJSONCountry) {
			ScraperWorldometer oScraperWorldometer = new ScraperWorldometer();
			string strUri = URLJSONDATASOURCE + oJSONCountry.location;
			Console.WriteLine("Getting Daily Cases for " + oJSONCountry.location + " " + strUri);
			string strNeedle = "name: 'Daily Cases',";
			Dictionary<DateTime, int> dictDateToInfected = oScraperWorldometer.processUrl(strUri, strNeedle);
			Console.WriteLine("Getting Death for " + oJSONCountry.location + " " + strUri);
			strNeedle = "name: 'Daily Deaths',";
			Dictionary<DateTime, int> dictDateToDeath = oScraperWorldometer.processUrl(strUri, strNeedle);

			List<JSONDailyData> listJSONDailyData = new List<JSONDailyData>();

			foreach (var item in dictDateToInfected) {
				JSONDailyData oJSONDailyData = new JSONDailyData();
				oJSONDailyData.date = item.Key.ToString("yyyy-MM-dd");
				oJSONDailyData.new_deaths = dictDateToDeath[item.Key];
				oJSONDailyData.new_cases = item.Value;
				listJSONDailyData.Add(oJSONDailyData);
			}

			return listJSONDailyData;
		}
	}
}