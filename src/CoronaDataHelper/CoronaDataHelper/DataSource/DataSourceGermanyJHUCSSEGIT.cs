using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoronaDataHelper.Interface;
using CoronaDataHelper.JSON;
using CsvHelper;

namespace CoronaDataHelper.DataSource {
	class DataSourceGermanyJHUCSSEGIT : IDataSource {

		private const string PATH = @"E:\dev\COVID-19\csse_covid_19_data\csse_covid_19_daily_reports\";
		public object process() {
			var data = getData();

			Console.WriteLine("Got Data:" + data.Count);
			return data;
		}


		private Dictionary<DateTime, IEnumerable<JSONDailyReport>> getData() {
			var files = Directory.GetFiles(PATH, "*.csv");
			Dictionary<DateTime, IEnumerable<JSONDailyReport>> dictResult = new Dictionary<DateTime, IEnumerable<JSONDailyReport>>();
			//All files > 5-14-2020.csv
			DateTime dtstart = new DateTime(2020, 5, 14);

			Console.WriteLine("Found Files:" + files.Length);
			foreach (var file in files) {
				string[] arstrparts = System.IO.Path.GetFileNameWithoutExtension(file).Split('-');
				int iMonth = Int16.Parse(arstrparts[0]);
				int iDay = Int16.Parse(arstrparts[1]);
				int iYear = Int16.Parse(arstrparts[2]);

				DateTime dt = new DateTime(iYear, iMonth, iDay);
				if (dt < dtstart) {
					Console.WriteLine("Skipping file:" + file);
					continue;
				}


				using (var reader = new StreamReader(file)) {
					try {
						using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) {
							csv.Configuration.HeaderValidated = null;
							string strException = "";

							try {
								csv.Configuration.RegisterClassMap<JSONDailyReportv2Map>();
								csv.Configuration.MissingFieldFound = null;

								JSONDailyReport.validate(csv);

								var records = csv.GetRecords<JSONDailyReport>();

								foreach (var item in records) {
									if (item.Province_State.Equals("Bayern")) {
										Console.WriteLine("2 file:" + file);
										//Console.WriteLine("item:" + item);
									}
								}
								dictResult.Add(dt, records);
								continue;
							} catch (Exception e) {
								strException += e + Environment.NewLine;
							}
							Console.WriteLine("Invalid File:" + file);
							Debug.WriteLine("Invalid File:" + file);
							Debug.WriteLine("strException:" + strException);

						}
					} catch (Exception e) {
						Console.WriteLine("Error in file:" + file);
						Debug.WriteLine("Error in file:" + file);
						Debug.WriteLine("Error:" + e);
					}
				}
			}
			return dictResult;
		}

	}
}
