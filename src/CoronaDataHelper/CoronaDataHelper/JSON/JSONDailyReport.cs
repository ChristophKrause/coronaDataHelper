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

        public override string ToString() {
	        return JsonConvert.SerializeObject(this);
        }

        public static void validate(CsvReader csv) {
			csv.Read();
			csv.ReadHeader();
			var header = csv.Context.HeaderRecord;
			List<string> listHeaders = new List<string>(header);

			//foreach (var item in listHeaders) {
			//	Console.WriteLine(item);
			//}
			//check invalid fields
			foreach (var item in m_listMandatory) {
				if (!listHeaders.Contains(item)) {
					throw new Exception("Field not Found "+ item);
				}
			}
        }
	}
	public class JSONDailyReportv2Map : ClassMap<JSONDailyReport> {
		public JSONDailyReportv2Map() {
			Map(m => m.FIPS).Name("FIPS");
			Map(m => m.Admin2).Name("Admin2");
			Map(m => m.Province_State).Name("Province_State");
			Map(m => m.Country_Region).Name("Country_Region");
			Map(m => m.Last_Update).Name("Last_Update");
			Map(m => m.Lat).Name("Lat");
			Map(m => m.Long_).Name("Long_");
			Map(m => m.Confirmed).Name("Confirmed");
			Map(m => m.Deaths).Name("Deaths");
			Map(m => m.Recovered).Name("Recovered");
			Map(m => m.Active).Name("Active");
			Map(m => m.Combined_Key).Name("Combined_Key");
			Map(m => m.Incident_Rate).Name("Incidence_Rate");
			Map(m => m.Incident_Rate).Name("Incident_Rate");
			Map(m => m.Case_Fatality_Ratio).Name("Case-Fatality_Ratio");
			Map(m => m.Case_Fatality_Ratio).Name("Case_Fatality_Ratio");
		}
	}


}
