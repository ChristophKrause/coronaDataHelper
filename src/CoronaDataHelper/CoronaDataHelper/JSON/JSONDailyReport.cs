using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;

namespace CoronaDataHelper.JSON {
	public class JSONDailyReport {

		private static List<string> m_listMandatory = new List<string>(){ "Province_State" , "Country_Region", "Last_Update", "Confirmed", "Deaths" , "Recovered" , "Active" };
		public int? FIPS { get; set; }
        public string Admin2 { get; set; }
        public string Province_State { get; set; } //mandatory
		public string Country_Region { get; set; } //mandatory
		public DateTime Last_Update { get; set; } //mandatory
		public string Lat { get; set; }
        public string Long_ { get; set; }
        public int? Confirmed { get; set; } //mandatory
		public int? Deaths { get; set; } //mandatory
		public int? Recovered { get; set; } //mandatory
		public int? Active { get; set; } //mandatory
		public string Combined_Key { get; set; }
        public string Incident_Rate { get; set; }
        public string Case_Fatality_Ratio { get; set; }
        internal int new_cases { get; set; }
        internal int new_deaths { get; set; }

        public override string ToString() {
	        return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static void validate(CsvReader csv) {
			csv.Read();
			csv.ReadHeader();
			var header = csv.Context.HeaderRecord;
			List<string> listHeaders = new List<string>(header);
			
			foreach (var item in m_listMandatory) {
				if (!listHeaders.Contains(item)) {
					throw new Exception("Field not Found "+ item);
				}
			}
        }

        public JSONDailyData convert() {
			JSONDailyData oJSONDailyData =  new JSONDailyData();
			oJSONDailyData.total_deaths = Deaths ?? default(int);
			oJSONDailyData.total_cases = Confirmed ?? default(int);

			oJSONDailyData.new_deaths = new_deaths;
			oJSONDailyData.new_cases = new_cases;

			oJSONDailyData.date = Last_Update.AddDays(-1).ToString("yyyy-MM-dd"); //decrement day, because the update is not the data date!
			return oJSONDailyData;

        }
	}
}
