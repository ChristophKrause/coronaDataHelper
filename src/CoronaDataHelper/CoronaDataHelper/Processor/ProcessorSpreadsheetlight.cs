using CoronaDataHelper.Interface;
using CoronaDataHelper.JSON;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CoronaDataHelper.Processor {

	internal class ProcessorSpreadsheetlight : IDataProcessor {

		internal enum EDataType {
			death,
			infected
		}

		private const string FILENAMEEXCEL = "coronadata.xlsx";

		public bool process(JSONCoronaVirusData oJSONCoronaVirusData) {
			if (!File.Exists(FILENAMEEXCEL)) {
				throw new Exception("Can not find ExcelFile:" + FILENAMEEXCEL);
			}

			processXLSX(FILENAMEEXCEL, oJSONCoronaVirusData, EDataType.infected);
			processXLSX(FILENAMEEXCEL, oJSONCoronaVirusData, EDataType.death);

			return true;
		}

		private static void processXLSX(string strFileNameExcelx, JSONCoronaVirusData result, EDataType eDataType) {
			//XLSX Layout:
			//Date	Italy	Spain 	USA 	Germany	France	Iran	UK	Netherlands	Belgium	Sweden	Brazil
			//Date ITA ESP USA  DEU  FRA  IRN  GBR NLD BEL SWE BRA

			Console.WriteLine("OPEN Excel " + strFileNameExcelx);
			SLDocument sl = new SLDocument(strFileNameExcelx, eDataType.ToString());

			validateFile(sl);

			//TODO: Make these calls generic by a List with propertynames

			setData(sl, result.ITA, "B", eDataType);
			setData(sl, result.ESP, "C", eDataType);
			setData(sl, result.USA, "D", eDataType);
			setData(sl, result.DEU, "E", eDataType);
			setData(sl, result.FRA, "F", eDataType);
			setData(sl, result.IRN, "G", eDataType);
			setData(sl, result.GBR, "H", eDataType);
			setData(sl, result.NLD, "I", eDataType);
			setData(sl, result.BEL, "J", eDataType);
			setData(sl, result.SWE, "K", eDataType);
			setData(sl, result.BRA, "L", eDataType);
			setData(sl, result.IRL, "M", eDataType);
			setData(sl, result.CAN, "N", eDataType);

			Console.WriteLine("save Excel " + strFileNameExcelx);

			sl.Save();
		}

		private static void setData(SLDocument sl, JSONCountry oJSONCountry, String strColumn, EDataType eRow) {
			Console.WriteLine("column: " + strColumn+" setData: " + oJSONCountry.location);

			var oData = oJSONCountry.data;

			int iRowModifier = 0;
			//not generic to get a message if the data is changed 
			if (oData[0].date == "2019-12-31") {
				iRowModifier = 2;
			} else if (oData[0].date == "2020-01-22") {
				iRowModifier = 24;
			} else if (oData[0].date == "2020-01-23") {
				iRowModifier = 25;
			} else if (oData[0].date == "2020-01-24") {
				iRowModifier = 26;
			} else if (oData[0].date == "2020-01-26") {
				iRowModifier = 28;
			} else if (oData[0].date == "2020-01-27") {
				iRowModifier = 29;
			} else if (oData[0].date == "2020-01-31") {
				iRowModifier = 33;
			} else if (oData[0].date == "2020-02-01") {
				iRowModifier = 34;
			} else if (oData[0].date == "2020-02-02") {
				iRowModifier = 35;
			} else if (oData[0].date == "2020-02-19") {
				iRowModifier = 52;
			} else if (oData[0].date == "2020-02-26") {
				iRowModifier = 59;
			} else if (oData[0].date == "2020-02-29") {
				iRowModifier = 62;
			} else {
				throw new Exception("Invalid value:" + oData[0].date + " strColumn:" + strColumn);
			}

			for (int i = 0; i < oData.Length; i++) {
				int iValue = (int)oData[i].new_cases;
				if (eRow == EDataType.death) {
					iValue = (int)oData[i].new_deaths;
				}

				

				string strrowIndex = strColumn + "" + (i + iRowModifier);
				if (strColumn == "E") {
					Debug.WriteLine(strrowIndex+" "+ iValue + "    oData[0].date:" + oData[i].date);
				}
				sl.ClearCellContent(strrowIndex, strrowIndex);
				sl.SetCellValue(strrowIndex, iValue);
			}
		}

		private static readonly Dictionary<string, string> s_oDictCellToCountryName = new Dictionary<string, string>() {
			{"A1","Date"},{"B1","Italy"},{"C1","Spain"},{"D1","USA"},{"E1","Germany"},{"F1","France"},{"G1","Iran"},
			{"H1","UK"},{"I1","Netherlands"},{"J1","Belgium"},{"K1","Sweden"},{"L1","Brazil"},{"M1","Ireland"},{"N1","Canada"}
		};

		private static void validateFile(SLDocument sl) {
			foreach (var item in s_oDictCellToCountryName) {
				var strValue = sl.GetCellValueAsString(item.Key).Trim();
				if (!strValue.Equals(item.Value, StringComparison.InvariantCultureIgnoreCase)) {
					throw new Exception("Invalid File:" + strValue + "!=" + item.Value);
				}
			}
		}
	}
}