using CoronaDataHelper.Data;
using Covid19;
using Covid19.Classes;
using Newtonsoft.Json;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoronaDataHelper {

	internal class Program {
		private const string JSONURL = "https://covid.ourworldindata.org/data/owid-covid-data.json";

		private static void Main(string[] args) {
			processMethod1();
			//Task t = processMethod2Async();
			//t.Wait();
		}

		private static async System.Threading.Tasks.Task processMethod2Async() {
			try {
				CovidAPIClient client = new CovidAPIClient();
			Console.WriteLine("Get data");
			//Country slugs available at https://api.covid19api.com/countries
			List<Country> result = await client.GetTotalCountryDataAsync("spain");
			Console.WriteLine("Got data");
				int iOld = 0;
			foreach (var item in result) {
					
				Console.WriteLine(item.UpdateDate + " " + (item.Confirmed- iOld));
					iOld = item.Confirmed;
				}
			} catch (Exception e) {
				Console.WriteLine("Error:" + e);
			}
		}

		private static void processMethod1() {
			try {
				const string strFileNameJSON = "coronavirus-source-data.json";
				const string strFileNameExcelx = "coronadata.xlsx";

				//is using https://ourworldindata.org/coronavirus-source-data as source


				JSONCoronaVirusData oJSONCoronaVirusData = Util.getJSONData(JSONURL, strFileNameJSON);

				Console.WriteLine(oJSONCoronaVirusData.DEU.data.Length);

				processXLSXInfected(strFileNameExcelx, oJSONCoronaVirusData);
				processXLSXDeath(strFileNameExcelx, oJSONCoronaVirusData);
				//NEXT:
				//1)Push data from JSON to excel
				//2) Add formula to XLSX
			} catch (Exception e) {
				Console.WriteLine("Error:" + e);
			}
		}
		private static void processXLSXDeath(string strFileNameExcelx, JSONCoronaVirusData result) {
		
			//XLSX Layout:
			//Date	Italy	Spain 	USA 	Germany	France	Iran	UK	Netherlands	Belgium	Sweden	Brazil
			//Date ITA ESP USA  DEU  FRA  IRN  GBR NLD BEL SWE BRA

			Console.WriteLine("OPEN Excel " + strFileNameExcelx);
			SLDocument sl = new SLDocument(strFileNameExcelx, "death");
			//validate
			
			validateFile(sl);

			//	Console.WriteLine("strValue:" + strValue);
			//Console.WriteLine(result.DEU.data[1].date + ":" + result.DEU.data[1].new_cases);

			setData(sl, result.ITA, "B", ERow.death);
			setData(sl, result.ESP, "C", ERow.death);
			setData(sl, result.USA, "D", ERow.death);
			setData(sl, result.DEU, "E", ERow.death);
			setData(sl, result.FRA, "F", ERow.death);
			setData(sl, result.IRN, "G", ERow.death);
			setData(sl, result.GBR, "H", ERow.death);
			setData(sl, result.NLD, "I", ERow.death);
			setData(sl, result.BEL, "J", ERow.death);
			setData(sl, result.SWE, "K", ERow.death);
			setData(sl, result.BRA, "L", ERow.death);
			setData(sl, result.IRL, "M", ERow.death);

	
			Console.WriteLine("save Excel " + strFileNameExcelx);

			sl.Save();
		}

		internal enum ERow {
			death,
			infected
		}
		private static void processXLSXInfected(string strFileNameExcelx, JSONCoronaVirusData result) {
			//XLSX Layout:
			//Date	Italy	Spain 	USA 	Germany	France	Iran	UK	Netherlands	Belgium	Sweden	Brazil
			//Date ITA ESP USA  DEU  FRA  IRN  GBR NLD BEL SWE BRA

			Console.WriteLine("OPEN Excel " + strFileNameExcelx);
			SLDocument sl = new SLDocument(strFileNameExcelx, "infected");
			//validate
			
			validateFile(sl);

			//	Console.WriteLine("strValue:" + strValue);
			//Console.WriteLine(result.DEU.data[1].date + ":" + result.DEU.data[1].new_cases);

			setData(sl, result.ITA, "B", ERow.infected);
			setData(sl, result.ESP, "C", ERow.infected);
			setData(sl, result.USA, "D", ERow.infected);
			setData(sl, result.DEU, "E", ERow.infected);
			setData(sl, result.FRA, "F", ERow.infected);
			setData(sl, result.IRN, "G", ERow.infected);
			setData(sl, result.GBR, "H", ERow.infected);
			setData(sl, result.NLD, "I", ERow.infected);
			setData(sl, result.BEL, "J", ERow.infected);
			setData(sl, result.SWE, "K", ERow.infected);
			setData(sl, result.BRA, "L", ERow.infected);
			setData(sl, result.IRL, "M", ERow.infected);


			Console.WriteLine("save Excel " + strFileNameExcelx);

			sl.Save();
		}
		private static void setData(SLDocument sl, IRL oIRL, String strColumn, ERow eRow) {
			for (int i = 1; i < oIRL.data.Length; i++) {
				int iValue = (int)oIRL.data[i].new_cases;
				if (eRow == ERow.death) {
					iValue = (int)oIRL.data[i].new_deaths;
				}
				sl.SetCellValue(strColumn + "" + (i + 1), "=SUM(B2:B11)");
				sl.ClearCellContent(strColumn + "" + (i + 1), strColumn + "" + (i + 1));
				sl.SetCellValue(strColumn + "" + (i + 1), "11");
				sl.SetCellValue(strColumn + "" + (i + 1), iValue);
			}
		}
		private static void setData(SLDocument sl, ESP oESP, String strColumn, ERow eRow) {
			for (int i = 1; i < oESP.data.Length; i++) {
				int iValue = (int)oESP.data[i].new_cases;
				if (eRow == ERow.death) {
					iValue = (int)oESP.data[i].new_deaths;
				}
				sl.SetCellValue(strColumn + "" + (i + 1), "=SUM(B2:B11)");
				sl.ClearCellContent(strColumn + "" + (i + 1), strColumn + "" + (i + 1));
				sl.SetCellValue(strColumn + "" + (i + 1), "11");
				sl.SetCellValue(strColumn + "" + (i + 1), iValue);
			}
		}
		private static void setData(SLDocument sl, USA oUSA, String strColumn, ERow eRow) {
			for (int i = 1; i < oUSA.data.Length; i++) {
				int iValue = (int)oUSA.data[i].new_cases;
				if (eRow == ERow.death) {
					iValue = (int)oUSA.data[i].new_deaths;
				}
				sl.SetCellValue(strColumn + "" + (i + 1), "=SUM(B2:B11)");
				sl.ClearCellContent(strColumn + "" + (i + 1), strColumn + "" + (i + 1));
				sl.SetCellValue(strColumn + "" + (i + 1), "11");
				sl.SetCellValue(strColumn + "" + (i + 1), iValue);
			}
		}
		private static void setData(SLDocument sl, DEU oDEU, String strColumn, ERow eRow) {
			Console.WriteLine(" oDEU.data.Length:" + oDEU.data.Length);
			//sl.ClearCellContent(strColumn + "2", strColumn + "" + oDEU.data.Length);
			for (int i = 1; i < oDEU.data.Length; i++) {
				int iValue = (int)oDEU.data[i].new_cases;
				if (eRow == ERow.death) {
					iValue = (int)oDEU.data[i].new_deaths;
				}
				//Console.WriteLine(i + " " + sl.GetCellFormula(strColumn + "" + (i + 1)));
				sl.SetCellValue(strColumn + "" + (i + 1), "=SUM(B2:B11)");
				sl.ClearCellContent(strColumn + "" + (i + 1), strColumn + "" + (i + 1));
				sl.SetCellValue(strColumn + "" + (i + 1), "11");
			
			//	Console.WriteLine(i + " " + sl.GetCellFormula(strColumn + "" + (i + 1)) + " "+ sl.GetCellValueAsString(strColumn + "" + (i + 1)));
					sl.SetCellValue(strColumn + "" + (i + 1), iValue);
			}
		}
		private static void setData(SLDocument sl, FRA oFRA, String strColumn, ERow eRow) {
			for (int i = 1; i < oFRA.data.Length; i++) {
				int iValue = (int)oFRA.data[i].new_cases;
				if (eRow == ERow.death) {
					iValue = (int)oFRA.data[i].new_deaths;
				}
				sl.SetCellValue(strColumn + "" + (i + 1), "=SUM(B2:B11)");
				sl.ClearCellContent(strColumn + "" + (i + 1), strColumn + "" + (i + 1));
				sl.SetCellValue(strColumn + "" + (i + 1), "11");
				sl.SetCellValue(strColumn + "" + (i + 1), iValue);
			}
		}
		private static void setData(SLDocument sl, IRN oIRN, String strColumn, ERow eRow) {
			for (int i = 1; i < oIRN.data.Length; i++) {
				int iValue = (int)oIRN.data[i].new_cases;
				if (eRow == ERow.death) {
					iValue = (int)oIRN.data[i].new_deaths;
				}
				sl.SetCellValue(strColumn + "" + (i + 1), "=SUM(B2:B11)");
				sl.ClearCellContent(strColumn + "" + (i + 1), strColumn + "" + (i + 1));
				sl.SetCellValue(strColumn + "" + (i + 1), "11");
				sl.SetCellValue(strColumn + "" + (i + 1), iValue );
			}
		}
		private static void setData(SLDocument sl, GBR oGBR, String strColumn, ERow eRow) {
			for (int i = 1; i < oGBR.data.Length; i++) {
				int iValue = (int)oGBR.data[i].new_cases;
				if (eRow == ERow.death) {
					iValue = (int)oGBR.data[i].new_deaths;
				}
				sl.SetCellValue(strColumn + "" + (i + 1), "=SUM(B2:B11)");
				sl.ClearCellContent(strColumn + "" + (i + 1), strColumn + "" + (i + 1));
				sl.SetCellValue(strColumn + "" + (i + 1), "11");
				sl.SetCellValue(strColumn + "" + (i + 1), iValue + "");
			}
		}
		private static void setData(SLDocument sl, BEL oBEL, String strColumn, ERow eRow) {
			for (int i = 1; i < oBEL.data.Length; i++) {
				int iValue = (int)oBEL.data[i].new_cases;
				if (eRow == ERow.death) {
					iValue = (int)oBEL.data[i].new_deaths;
				}
				sl.SetCellValue(strColumn + "" + (i + 1), "=SUM(B2:B11)");
				sl.ClearCellContent(strColumn + "" + (i + 1), strColumn + "" + (i + 1));
				sl.SetCellValue(strColumn + "" + (i + 1), "11");
				sl.SetCellValue(strColumn + "" + (i + 1), iValue);
			}
		}
		private static void setData(SLDocument sl, SWE oSWE, String strColumn, ERow eRow) {
			for (int i = 1; i < oSWE.data.Length; i++) {
				int iValue = (int)oSWE.data[i].new_cases;
				if (eRow == ERow.death) {
					iValue = (int)oSWE.data[i].new_deaths;
				}
				sl.SetCellValue(strColumn + "" + (i + 1), "=SUM(B2:B11)");
				sl.ClearCellContent(strColumn + "" + (i + 1), strColumn + "" + (i + 1));
				sl.SetCellValue(strColumn + "" + (i + 1), "11");
				sl.SetCellValue(strColumn + "" + (i + 1), iValue);
			}
		}
		private static void setData(SLDocument sl, BRA oBRA, String strColumn, ERow eRow) {
			for (int i = 1; i < oBRA.data.Length; i++) {
				int iValue = (int)oBRA.data[i].new_cases;
				if (eRow == ERow.death) {
					iValue = (int)oBRA.data[i].new_deaths;
				}
				sl.SetCellValue(strColumn + "" + (i + 1), "=SUM(B2:B11)");
				sl.ClearCellContent(strColumn + "" + (i + 1), strColumn + "" + (i + 1));
				sl.SetCellValue(strColumn + "" + (i + 1), "11");
				sl.SetCellValue(strColumn + "" + (i + 1), iValue );
			}
		}
		private static void setData(SLDocument sl, NLD oNLD, String strColumn, ERow eRow) {
			for (int i = 1; i < oNLD.data.Length; i++) {
				int iValue = (int)oNLD.data[i].new_cases;
				if (eRow == ERow.death) {
					iValue = (int)oNLD.data[i].new_deaths;
				}
				sl.SetCellValue(strColumn + "" + (i + 1), "=SUM(B2:B11)");
				sl.ClearCellContent(strColumn + "" + (i + 1), strColumn + "" + (i + 1));
				sl.SetCellValue(strColumn + "" + (i + 1), "11");
				sl.SetCellValue(strColumn + "" + (i + 1), iValue );
			}
		}
		private static void setData(SLDocument sl, ITA oiTA, String strColumn, ERow eRow) {
			for (int i=1; i< oiTA.data.Length; i++) {
				//	Console.WriteLine(oiTA.data[i].date + " : " + oiTA.data[i].new_cases);
				int iValue = (int)oiTA.data[i].new_cases;
				if (eRow == ERow.death) {
					iValue = (int)oiTA.data[i].new_deaths;
				}
				sl.SetCellValue(strColumn + "" + (i + 1), "=SUM(B2:B11)");
				sl.ClearCellContent(strColumn + "" + (i + 1), strColumn + "" + (i + 1));
				sl.SetCellValue(strColumn + "" + (i + 1), "11");
				sl.SetCellValue(strColumn + "" + (i + 1), iValue );
			}
		
		}

		private static void validateFile(SLDocument sl) {
			var strValue = sl.GetCellValueAsString("A1").Trim();
			if (!strValue.Equals("Date", StringComparison.InvariantCultureIgnoreCase)) {
				throw new Exception("Invalid File:" + strValue + "!=Date");
			}
			strValue = sl.GetCellValueAsString("B1").Trim();
			if (!strValue.Equals("Italy", StringComparison.InvariantCultureIgnoreCase)) {
				throw new Exception("Invalid File:" + strValue + "!=Italy");
			}
			strValue = sl.GetCellValueAsString("C1").Trim();
			if (!strValue.Equals("Spain", StringComparison.InvariantCultureIgnoreCase)) {
				throw new Exception("Invalid File:" + strValue + "!=Spain");
			}
			strValue = sl.GetCellValueAsString("D1").Trim();
			if (!strValue.Equals("USA", StringComparison.InvariantCultureIgnoreCase)) {
				throw new Exception("Invalid File:" + strValue + "!=USA");
			}
			strValue = sl.GetCellValueAsString("E1").Trim();
			if (!strValue.Equals("Germany", StringComparison.InvariantCultureIgnoreCase)) {
				throw new Exception("Invalid File:" + strValue + "!=Germany");
			}
			strValue = sl.GetCellValueAsString("F1").Trim();
			if (!strValue.Equals("France", StringComparison.InvariantCultureIgnoreCase)) {
				throw new Exception("Invalid File:" + strValue + "!=France");
			}
			strValue = sl.GetCellValueAsString("G1").Trim();
			if (!strValue.Equals("Iran", StringComparison.InvariantCultureIgnoreCase)) {
				throw new Exception("Invalid File:" + strValue + "!=Iran");
			}
			strValue = sl.GetCellValueAsString("H1").Trim();
			if (!strValue.Equals("UK", StringComparison.InvariantCultureIgnoreCase)) {
				throw new Exception("Invalid File:" + strValue + "!=UK");
			}
			strValue = sl.GetCellValueAsString("I1").Trim();
			if (!strValue.Equals("Netherlands", StringComparison.InvariantCultureIgnoreCase)) {
				throw new Exception("Invalid File:" + strValue + "!=Netherlands");
			}
			strValue = sl.GetCellValueAsString("J1").Trim();
			if (!strValue.Equals("Belgium", StringComparison.InvariantCultureIgnoreCase)) {
				throw new Exception("Invalid File:" + strValue + "!=Belgium");
			}
			strValue = sl.GetCellValueAsString("K1").Trim();
			if (!strValue.Equals("Sweden", StringComparison.InvariantCultureIgnoreCase)) {
				throw new Exception("Invalid File:" + strValue + "!=Sweden");
			}
			strValue = sl.GetCellValueAsString("L1").Trim();
			if (!strValue.Equals("Brazil", StringComparison.InvariantCultureIgnoreCase)) {
				throw new Exception("Invalid File:" + strValue + "!=Brazil");
			}
			strValue = sl.GetCellValueAsString("M1").Trim();
			if (!strValue.Equals("Ireland", StringComparison.InvariantCultureIgnoreCase)) {
				throw new Exception("Invalid File:" + strValue + "!=Ireland");
			}
		}
	}
}