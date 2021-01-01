using System.Collections.Generic;
using System.Linq.Expressions;

namespace CoronaDataHelper.JSON {

	public class JSONCoronaVirusDataGermany {
		public JSONCountry BB { get; set; }
		public JSONCountry BE { get; set; }
		public JSONCountry BW { get; set; }
		public JSONCountry BY { get; set; }
		public JSONCountry HB { get; set; }
		public JSONCountry HE { get; set; }
		public JSONCountry HH { get; set; }
		public JSONCountry MV { get; set; }
		public JSONCountry NI { get; set; }
		public JSONCountry NW { get; set; }
		public JSONCountry RP { get; set; }
		public JSONCountry SH { get; set; }
		public JSONCountry SL { get; set; }
		public JSONCountry SN { get; set; }
		public JSONCountry ST { get; set; }
		public JSONCountry TH { get; set; }


		internal JSONCoronaVirusDataGermany() {
			BB = new JSONCountry() {location = "Brandenburg", data = new List<JSONDailyData>()};
			BE = new JSONCountry() {location = "Berlin", data = new List<JSONDailyData>()};
			BW = new JSONCountry() {location = "Baden-Wurttemberg", data = new List<JSONDailyData>()};
			BY = new JSONCountry() {location = "Bayern", data = new List<JSONDailyData>()};
			HB = new JSONCountry() {location = "Bremen", data = new List<JSONDailyData>()};
			HE = new JSONCountry() {location = "Hessen", data = new List<JSONDailyData>()};
			HH = new JSONCountry() {location = "Hamburg", data = new List<JSONDailyData>()};
			MV = new JSONCountry() {location = "Mecklenburg-Vorpommern", data = new List<JSONDailyData>()};
			NI = new JSONCountry() {location = "Niedersachsen", data = new List<JSONDailyData>()};
			NW = new JSONCountry() {location = "Nordrhein-Westfalen", data = new List<JSONDailyData>()};
			RP = new JSONCountry() {location = "Rheinland-Pfalz", data = new List<JSONDailyData>()};
			SH = new JSONCountry() {location = "Schleswig-Holstein", data = new List<JSONDailyData>()};
			SL = new JSONCountry() {location = "Saarland", data = new List<JSONDailyData>()};
			SN = new JSONCountry() {location = "Sachsen", data = new List<JSONDailyData>()};
			ST = new JSONCountry() {location = "Sachsen-Anhalt", data = new List<JSONDailyData>()};
			TH = new JSONCountry() {location = "Thuringen", data = new List<JSONDailyData>()};

		}

		internal void addData(JSONDailyReport oJSONDailyReport) {
			JSONCountry oJSONCountry = null;
			switch (oJSONDailyReport.Province_State) {
				case "Brandenburg":
					oJSONCountry = BB;
					break;
				case "Berlin":
					oJSONCountry = BE;
					break;
				case "Baden-Wurttemberg":
					oJSONCountry = BW;
					break;
				case "Bayern":
					oJSONCountry = BY;
					break;
				case "Bremen":
					oJSONCountry = HB;
					break;
				case "Hessen":
					oJSONCountry = HE;
					break;
				case "Hamburg":
					oJSONCountry = HH;
					break;
				case "Mecklenburg-Vorpommern":
					oJSONCountry = MV;
					break;
				case "Niedersachsen":
					oJSONCountry = NI;
					break;
				case "Nordrhein-Westfalen":
					oJSONCountry = NW;
					break;
				case "Rheinland-Pfalz":
					oJSONCountry = RP;
					break;
				case "Schleswig-Holstein":
					oJSONCountry = SH;
					break;
				case "Saarland":
					oJSONCountry = SL;
					break;
				case "Sachsen":
					oJSONCountry = SN;
					break;
				case "Sachsen-Anhalt":
					oJSONCountry = ST;
					break;
				case "Thuringen":
					oJSONCountry = TH;
					break;
				default:
					break;
			}

			oJSONCountry.data.Add(oJSONDailyReport.convert());

		}

		internal void addData(JSONStateDataMarlonLueckert oJSONStateDataMarlonLueckertCases,
			JSONStateDataMarlonLueckert oJSONStateDataMarlonLueckertDeaths) {
			oJSONStateDataMarlonLueckertCases.combine(oJSONStateDataMarlonLueckertDeaths);

			foreach (var item in oJSONStateDataMarlonLueckertCases.data.BB.history) {
				BB.data.Add(item.convert());
			}

			foreach (var item in oJSONStateDataMarlonLueckertCases.data.SH.history) {
				SH.data.Add(item.convert());
			}

			foreach (var item in oJSONStateDataMarlonLueckertCases.data.HH.history) {
				HH.data.Add(item.convert());
			}

			foreach (var item in oJSONStateDataMarlonLueckertCases.data.NI.history) {
				NI.data.Add(item.convert());
			}

			foreach (var item in oJSONStateDataMarlonLueckertCases.data.HB.history) {
				HB.data.Add(item.convert());
			}

			foreach (var item in oJSONStateDataMarlonLueckertCases.data.NW.history) {
				NW.data.Add(item.convert());
			}

			foreach (var item in oJSONStateDataMarlonLueckertCases.data.HE.history) {
				HE.data.Add(item.convert());
			}

			foreach (var item in oJSONStateDataMarlonLueckertCases.data.RP.history) {
				RP.data.Add(item.convert());
			}

			foreach (var item in oJSONStateDataMarlonLueckertCases.data.BW.history) {
				BW.data.Add(item.convert());
			}

			foreach (var item in oJSONStateDataMarlonLueckertCases.data.BY.history) {
				BY.data.Add(item.convert());
			}

			foreach (var item in oJSONStateDataMarlonLueckertCases.data.SL.history) {
				SL.data.Add(item.convert());
			}

			foreach (var item in oJSONStateDataMarlonLueckertCases.data.BE.history) {
				BE.data.Add(item.convert());
			}

			foreach (var item in oJSONStateDataMarlonLueckertCases.data.MV.history) {
				MV.data.Add(item.convert());
			}

			foreach (var item in oJSONStateDataMarlonLueckertCases.data.SN.history) {
				SN.data.Add(item.convert());
			}

			foreach (var item in oJSONStateDataMarlonLueckertCases.data.ST.history) {
				ST.data.Add(item.convert());
			}

			foreach (var item in oJSONStateDataMarlonLueckertCases.data.TH.history) {
				TH.data.Add(item.convert());
			}

		}
	}
}