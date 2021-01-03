using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using CoronaDataHelper.Interface;
using CoronaDataHelper.JSON;
using CoronaDataHelper.Tools;
using CsvHelper;
using Newtonsoft.Json;

namespace CoronaDataHelper.DataSource {
	class DataSourceGermanyJHUCSSEGIT : IDataSource {

		private const string PATH = @"c:\dev\COVID-19\csse_covid_19_data\csse_covid_19_daily_reports\";


		public object process() {

			udateGIT();
			string strFileNameJSON = "JSONDataGermany.json";
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
				return JsonConvert.DeserializeObject<JSONCoronaVirusDataGermany>(strJsonText);
			}

			var data = getData();


			JSONCoronaVirusDataGermany oJSONCoronaVirusDataGermany = new JSONCoronaVirusDataGermany();

			foreach (var itemKVP in data) {
				foreach (var item in itemKVP.Value) {
					oJSONCoronaVirusDataGermany.addData(item);
				}
			}
			string strJson = JsonConvert.SerializeObject(oJSONCoronaVirusDataGermany, Formatting.Indented, new JsonSerializerSettings {
				NullValueHandling = NullValueHandling.Ignore
			});

			File.WriteAllText(strFileNameJSON, strJson);
			Console.WriteLine("Got Data:" + data.Count);
			return oJSONCoronaVirusDataGermany;
		}

		private void udateGIT() {

			string gitCommand = "git.exe";
			string gitCheckoutArgument = @"checkout -B master remotes/origin/master --";
			string gitFetchArgument = "fetch -v --progress \"origin\"";

			Process oprocess = new Process();
			oprocess.StartInfo.FileName = gitCommand;
			oprocess.StartInfo.Arguments = gitFetchArgument;
			oprocess.StartInfo.WorkingDirectory = PATH;
			Console.WriteLine("update repository");
			oprocess.Start();
			oprocess.WaitForExit();
			oprocess = new Process();
			// Configure the process using the StartInfo properties.
			oprocess.StartInfo.FileName = gitCommand;
			oprocess.StartInfo.Arguments = gitCheckoutArgument;
			oprocess.StartInfo.WorkingDirectory = PATH;
			oprocess.Start();
			oprocess.WaitForExit();
			Console.WriteLine("update repository ... done");
		}


		private Dictionary<DateTime, List<JSONDailyReport>> getData() {
			var files = Directory.GetFiles(PATH, "*.csv");
			Dictionary<DateTime, List<JSONDailyReport>> dictResult = new Dictionary<DateTime, List<JSONDailyReport>>();
			//All files > 5-14-2020.csv
			DateTime dtstart = new DateTime(2020, 5, 14);

			Console.WriteLine("Found Files:" + files.Length);

			Dictionary<string, int> dictStateToDeath = new Dictionary<string, int>();
			Dictionary<string, int> dictStateToConfirmed = new Dictionary<string, int>();


			//files need to be sorted here to have a valid calculation.
			List<FileNameWrapper> listFileNameWrapper = new List<FileNameWrapper>();
			foreach (var file in files) {
				FileNameWrapper oFileNameWrapper = new FileNameWrapper(file);

				listFileNameWrapper.Add(oFileNameWrapper);

			}

			listFileNameWrapper.Sort();
			foreach (var oFileName in listFileNameWrapper) {
			
				if (oFileName.m_dtFile < dtstart) {
					Console.WriteLine("Skipping file:" + oFileName.m_strFileName);
					continue;
				}




				using (var reader = new StreamReader(oFileName.m_strFileName)) {
					try {
						using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) {
							csv.Configuration.HeaderValidated = null;
							string strException = "";

							try {

								csv.Configuration.MissingFieldFound = null;

								JSONDailyReport.validate(csv);

								var records = csv.GetRecords<JSONDailyReport>();
								List<JSONDailyReport> listJSONDailyReport = new List<JSONDailyReport>();
								foreach (var item in records) {
									if (!isValidItem(item)) {
										continue;
									}

									if (!dictStateToDeath.ContainsKey(item.Province_State)) {
										dictStateToDeath.Add(item.Province_State, 0);
										dictStateToConfirmed.Add(item.Province_State, 0);

									}
									//modify item

									item.new_cases = item.Confirmed.Value - dictStateToConfirmed[item.Province_State];
									item.new_deaths = item.Deaths.Value - dictStateToDeath[item.Province_State];

									dictStateToDeath[item.Province_State] = item.Deaths.Value;
									dictStateToConfirmed[item.Province_State] = item.Confirmed.Value;

									listJSONDailyReport.Add(item);

									if (item.Province_State.Equals("Bayern")) {
										Console.WriteLine("2 file:" + oFileName.m_strFileName);
										//	Debug.WriteLine("item:" + item);
									}
								}

								Debug.WriteLine(oFileName.m_dtFile + " records            :" + records.Count());
								Debug.WriteLine(oFileName.m_dtFile + " listJSONDailyReport:" + listJSONDailyReport.Count());
								dictResult.Add(oFileName.m_dtFile, listJSONDailyReport);
								continue;
							} catch (Exception e) {
								strException += e + Environment.NewLine;
							}
							Console.WriteLine("Invalid File:" + oFileName.m_strFileName);
							Debug.WriteLine("Invalid File:" + oFileName.m_strFileName);
							Debug.WriteLine("strException:" + strException);

						}
					} catch (Exception e) {
						Console.WriteLine("Error in file:" + oFileName.m_strFileName);
						Debug.WriteLine("Error in file:" + oFileName.m_strFileName);
						Debug.WriteLine("Error:" + e);
					}
				}
			}
			return dictResult;
		}

		private bool isValidItem(JSONDailyReport oJSONDailyReport) {
			switch (oJSONDailyReport.Province_State) {
				case "Brandenburg":
				case "Berlin":
				case "Baden-Wurttemberg":
				case "Bayern":
				case "Bremen":
				case "Hessen":
				case "Hamburg":
				case "Mecklenburg-Vorpommern":
				case "Niedersachsen":
				case "Nordrhein-Westfalen":
				case "Rheinland-Pfalz":
				case "Schleswig-Holstein":
				case "Saarland":
				case "Sachsen":
				case "Sachsen-Anhalt":
				case "Thuringen":
					return true;
				default:
					return false;

			}
		}

	}
}
