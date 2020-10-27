using CoronaDataHelper.Data;

using Newtonsoft.Json;
using SpreadsheetLight;
using System;
using System.IO;

namespace CoronaDataHelper {

	internal class Program {
		private const string JSONURL = "https://covid.ourworldindata.org/data/owid-covid-data.json";

		private static void Main(string[] args) {
			try {
				const string strFileNameJSON = "coronavirus-source-data.json";
				const string strFileNameExcelx = "coronadata.xlsx";

				//is using https://ourworldindata.org/coronavirus-source-data as source


				JSONCoronaVirusData oJSONCoronaVirusData = Util.getJSONData(JSONURL, strFileNameJSON);

				Console.WriteLine(oJSONCoronaVirusData.DEU.data.Length);

				processXLSX(strFileNameExcelx, oJSONCoronaVirusData);
				//NEXT:
				//1)Push data from JSON to excel
				//2) Add formula to XLSX
			} catch (Exception e) {
				Console.WriteLine("Error:" + e);
			}
		}

		private static void processXLSX(string strFileNameExcelx, JSONCoronaVirusData result) {
			//XLSX Layout:
			//Date	Italy	Spain 	USA 	Germany	France	Iran	UK	Netherlands	Belgium	Sweden	Brazil
			//Date ITA ESP USA  DEU  FRA  IRN  GBR NLD BEL SWE BRA

			Console.WriteLine("OPEN Excel " + strFileNameExcelx);
			SLDocument sl = new SLDocument(strFileNameExcelx);
			//validate
			var strValue = sl.GetCellValueAsString("E1");
			validateFile(sl);

			//	Console.WriteLine("strValue:" + strValue);
			//Console.WriteLine(result.DEU.data[1].date + ":" + result.DEU.data[1].new_cases);

			setData(sl, result.ITA,"B");
			setData(sl, result.ESP, "C");
			setData(sl, result.USA, "D");
			setData(sl, result.DEU, "E");
			setData(sl, result.FRA, "F");
			setData(sl, result.IRN, "G");
			setData(sl, result.GBR, "H");
			setData(sl, result.NLD, "I");
			setData(sl, result.BEL, "J");
			setData(sl, result.SWE, "K");
			setData(sl, result.BRA, "L");

			sl.SetCellValue("E2", (int)result.DEU.data[1].new_cases + "");
			Console.WriteLine("save Excel " + strFileNameExcelx);

			sl.Save();
		}
		private static void setData(SLDocument sl, ESP oESP, String strColumn) {
			for (int i = 1; i < oESP.data.Length; i++) {
				sl.SetCellValue(strColumn + "" + (i + 1), (int)oESP.data[i].new_cases + "");
			}
		}
		private static void setData(SLDocument sl, USA oUSA, String strColumn) {
			for (int i = 1; i < oUSA.data.Length; i++) {
				sl.SetCellValue(strColumn + "" + (i + 1), (int)oUSA.data[i].new_cases + "");
			}
		}
		private static void setData(SLDocument sl, DEU oDEU, String strColumn) {
			for (int i = 1; i < oDEU.data.Length; i++) {
				sl.SetCellValue(strColumn + "" + (i + 1), (int)oDEU.data[i].new_cases + "");
			}
		}
		private static void setData(SLDocument sl, FRA oFRA, String strColumn) {
			for (int i = 1; i < oFRA.data.Length; i++) {
				sl.SetCellValue(strColumn + "" + (i + 1), (int)oFRA.data[i].new_cases + "");
			}
		}
		private static void setData(SLDocument sl, IRN oIRN, String strColumn) {
			for (int i = 1; i < oIRN.data.Length; i++) {
				sl.SetCellValue(strColumn + "" + (i + 1), (int)oIRN.data[i].new_cases + "");
			}
		}
		private static void setData(SLDocument sl, GBR oGBR, String strColumn) {
			for (int i = 1; i < oGBR.data.Length; i++) {
				sl.SetCellValue(strColumn + "" + (i + 1), (int)oGBR.data[i].new_cases + "");
			}
		}
		private static void setData(SLDocument sl, BEL oBEL, String strColumn) {
			for (int i = 1; i < oBEL.data.Length; i++) {
				sl.SetCellValue(strColumn + "" + (i + 1), (int)oBEL.data[i].new_cases + "");
			}
		}
		private static void setData(SLDocument sl, SWE oSWE, String strColumn) {
			for (int i = 1; i < oSWE.data.Length; i++) {
				sl.SetCellValue(strColumn + "" + (i + 1), (int)oSWE.data[i].new_cases + "");
			}
		}
		private static void setData(SLDocument sl, BRA oBRA, String strColumn) {
			for (int i = 1; i < oBRA.data.Length; i++) {
				sl.SetCellValue(strColumn + "" + (i + 1), (int)oBRA.data[i].new_cases + "");
			}
		}
		private static void setData(SLDocument sl, NLD oNLD, String strColumn) {
			for (int i = 1; i < oNLD.data.Length; i++) {
				sl.SetCellValue(strColumn + "" + (i + 1), (int)oNLD.data[i].new_cases + "");
			}
		}
		private static void setData(SLDocument sl, ITA oiTA, String strColumn) {
			for (int i=1; i< oiTA.data.Length; i++) {
			//	Console.WriteLine(oiTA.data[i].date + " : " + oiTA.data[i].new_cases);
				sl.SetCellValue(strColumn+""+(i+1), (int)oiTA.data[i].new_cases + "");
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
			if (!sl.GetCellValueAsString("L1").Equals("Brazil", StringComparison.InvariantCultureIgnoreCase)) {
				throw new Exception("Invalid File:" + strValue + "!=Brazil");
			}
		}
	}
}