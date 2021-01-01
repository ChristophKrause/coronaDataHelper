using System;
using System.Collections.Generic;
using System.Data;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;

namespace CoronaDataHelper.JSON {

	public class JSONStateDataMarlonLueckert {
		public Data data { get; set; }
		public Meta meta { get; set; }

		internal void combine(JSONStateDataMarlonLueckert oJSONStateDataMarlonLueckert) {
			data.combine(oJSONStateDataMarlonLueckert.data);
		}
	}

	public class Data {
		public StateData SH { get; set; }
		public StateData HH { get; set; }
		public StateData NI { get; set; }
		public StateData HB { get; set; }
		public StateData NW { get; set; }
		public StateData HE { get; set; }
		public StateData RP { get; set; }
		public StateData BW { get; set; }
		public StateData BY { get; set; }
		public StateData SL { get; set; }
		public StateData BE { get; set; }
		public StateData BB { get; set; }
		public StateData MV { get; set; }
		public StateData SN { get; set; }
		public StateData ST { get; set; }
		public StateData TH { get; set; }


		public void combine(Data data) {
			SH.combine(data.SH);
			HH.combine(data.HH);
			NI.combine(data.NI);
			HB.combine(data.HB);
			NW.combine(data.NW);
			HE.combine(data.HE);
			RP.combine(data.RP);
			BW.combine(data.BW);
			BY.combine(data.BY);
			SL.combine(data.SL);
			BE.combine(data.BE);
			BB.combine(data.BB);
			MV.combine(data.MV);
			SN.combine(data.SN);
			ST.combine(data.ST);
			TH.combine(data.TH);
		}
	}

	public class StateData {
		public int id { get; set; }
		public string name { get; set; }
		public List<DailyData> history { get; set; }

		internal void combine(StateData oStateDataForeign) {
			if (oStateDataForeign.id != id || oStateDataForeign.name != name) {
				throw new Exception("Data incompatible");
			}
			if (oStateDataForeign.history==null || oStateDataForeign.history.Count==0)
			{
				return;
			}
			if (history == null || history.Count == 0) {
				return;
			}
			//asume that all days have data (makes it easier to have no edge cases)
			foreach (var item  in history) {
				DailyData oDailyDataForeign = findItem(oStateDataForeign.history, item.date);
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
			throw  new Exception("Can not find item for date "+ dtNeedle);

		}
	}

	public class DailyData {
		public int? cases { get; set; }
		public int? deaths { get; set; }
		public DateTime date { get; set; }

		public JSONDailyData convert() {
			JSONDailyData oJSONDailyData = new JSONDailyData();
			
			oJSONDailyData.new_deaths = deaths ?? default(int);
			oJSONDailyData.new_cases = cases ?? default(int);

			oJSONDailyData.date = date.ToString("yyyy-MM-dd"); 
			return oJSONDailyData;
		}
	}
	public class Meta {
		public string source { get; set; }
		public string contact { get; set; }
		public string info { get; set; }
		public DateTime lastUpdate { get; set; }
		public DateTime lastCheckedForUpdate { get; set; }
	}
}