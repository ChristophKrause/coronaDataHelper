using System;
using System.Collections.Generic;
using System.Data;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;

namespace CoronaDataHelper.JSON {

	public class JSONGermanyDataMarlonLueckert {
		public List<DailyData> data { get; set; }
		public Meta meta { get; set; }

		internal void combine(JSONGermanyDataMarlonLueckert oJSONGermanyDataMarlonLueckert) {
			if (oJSONGermanyDataMarlonLueckert == null || oJSONGermanyDataMarlonLueckert.data.Count == 0) {
				return;
			}
			if (data == null || data.Count == 0) {
				return;
			}

			foreach (var item in data) {
				DailyData oDailyDataForeign = findItem(oJSONGermanyDataMarlonLueckert.data, item.date);
				if (item.deaths == null) {
					item.deaths = oDailyDataForeign.deaths;
				}
				if (item.cases == null) {
					item.cases = oDailyDataForeign.cases;
				}
			}
		}
		private DailyData findItem(List<DailyData> listDailyData, DateTime dtNeedle) {
			foreach (var item in listDailyData) {
				if (item.date == dtNeedle) {
					return item;
				}
			}
			throw new Exception("Can not find item for date " + dtNeedle);

		}

		internal static JSONCountry convert(JSONGermanyDataMarlonLueckert oJSONStateDataMarlonLueckertCases) {
			JSONCountry oJSONCountry = new JSONCountry();
			oJSONCountry.location = "Germany";
			oJSONCountry.data = new List<JSONDailyData>();
			foreach (var item in oJSONStateDataMarlonLueckertCases.data) {
				oJSONCountry.data.Add(item.convert());
			}
			return oJSONCountry;
		}
	}	
}