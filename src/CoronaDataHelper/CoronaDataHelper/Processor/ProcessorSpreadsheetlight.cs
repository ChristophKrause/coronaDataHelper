using CoronaDataHelper.Interface;
using CoronaDataHelper.JSON;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CoronaDataHelper.Processor {

	internal class ProcessorSpreadsheetlight : IDataProcessor {

		private static  readonly Dictionary<string, string> m_oDictCellToStateName = new Dictionary<string, string>() {
			{"A1","Date"},{"B1","Brandenburg"},{"C1","Berlin"},{"D1","Baden-Wurttemberg"},{"E1","Bayern"},{"F1","Bremen"},{"G1","Hessen"},
			{"H1","Hamburg"},{"I1","Mecklenburg-Vorpommern"},{"J1","Niedersachsen"},{"K1","Nordrhein-Westfalen"},{"L1","Rheinland-Pfalz"},{"M1","Schleswig-Holstein"},{"N1","Saarland"},
			{"O1","Sachsen"},
			{"P1","Sachsen-Anhalt"},
			{"Q1","Thuringen"}
		}; 
		private static readonly Dictionary<string, string> m_oDictCellToCountryName = new Dictionary<string, string>() {
			{"A1","Date"},{"B1","Italy"},{"C1","Spain"},{"D1","USA"},{"E1","Germany"},{"F1","France"},{"G1","Iran"},
			{"H1","UK"},{"I1","Netherlands"},{"J1","Belgium"},{"K1","Sweden"},{"L1","Brazil"},{"M1","Ireland"},{"N1","Canada"}
		};
		internal enum EDataType {
			death,
			infected
		}
	
		public bool process(string strFilename, object oJSONData) {
			if (!File.Exists(strFilename)) {
				throw new Exception("Can not find ExcelFile:" + strFilename);
			}

			if (oJSONData == null) {
				throw new Exception("data is null");
			}

			if (oJSONData is JSONCoronaVirusDataGermany oJSONCoronaVirusDataGermany) {
				processXLSX_ger(strFilename, oJSONCoronaVirusDataGermany, EDataType.infected);
				processXLSX_ger(strFilename, oJSONCoronaVirusDataGermany, EDataType.death);
				return true;
			}

			processXLSX(strFilename, (JSONCoronaVirusData)oJSONData, EDataType.infected);
			processXLSX(strFilename, (JSONCoronaVirusData)oJSONData, EDataType.death);

			return true;
		}


		private static void processXLSX_ger(string strFileNameExcelx, JSONCoronaVirusDataGermany oJSONCoronaVirusDataGermany, EDataType eDataType) {
			//XLSX Layout:
			//Date Baden-Württem­berg	Bayern	Berlin	Branden­burgBremen	France	Hamburg	Hessen	Mecklenburg Vorpommern	Niedersachsen	NRW	Rheinland-Pfalz	Saarland	Sachsen	Sachsen - Anhalt	Schleswig - Holstein	Thüringen

			//Date ITA ESP USA  DEU  FRA  IRN  GBR NLD BEL SWE BRA

			Console.WriteLine("OPEN Excel " + strFileNameExcelx);
			SLDocument sl = new SLDocument(strFileNameExcelx, eDataType.ToString());

			validateFile(sl, m_oDictCellToStateName);

			//TODO: Make these calls generic by a List with propertynames

			setData(sl, oJSONCoronaVirusDataGermany.BB, "B", eDataType);
			setData(sl, oJSONCoronaVirusDataGermany.BE, "C", eDataType);
			setData(sl, oJSONCoronaVirusDataGermany.BW, "D", eDataType);
			setData(sl, oJSONCoronaVirusDataGermany.BY, "E", eDataType);
			setData(sl, oJSONCoronaVirusDataGermany.HB, "F", eDataType);
			setData(sl, oJSONCoronaVirusDataGermany.HE, "G", eDataType);
			setData(sl, oJSONCoronaVirusDataGermany.HH, "H", eDataType);
			setData(sl, oJSONCoronaVirusDataGermany.MV, "I", eDataType);
			setData(sl, oJSONCoronaVirusDataGermany.NI, "J", eDataType);
			setData(sl, oJSONCoronaVirusDataGermany.NW, "K", eDataType);
			setData(sl, oJSONCoronaVirusDataGermany.RP, "L", eDataType);
			setData(sl, oJSONCoronaVirusDataGermany.SH, "M", eDataType);
			setData(sl, oJSONCoronaVirusDataGermany.SL, "N", eDataType);
			setData(sl, oJSONCoronaVirusDataGermany.SN, "O", eDataType);
			setData(sl, oJSONCoronaVirusDataGermany.ST, "P", eDataType);
			setData(sl, oJSONCoronaVirusDataGermany.TH, "Q", eDataType);

			Console.WriteLine("save Excel " + strFileNameExcelx);

			sl.Save();
		}

		private static void processXLSX(string strFileNameExcelx, JSONCoronaVirusData oJSONCoronaVirusData, EDataType eDataType) {
			//XLSX Layout:
			//Date	Italy	Spain 	USA 	Germany	France	Iran	UK	Netherlands	Belgium	Sweden	Brazil
			//Date ITA ESP USA  DEU  FRA  IRN  GBR NLD BEL SWE BRA

			Console.WriteLine("OPEN Excel " + strFileNameExcelx);
			SLDocument sl = new SLDocument(strFileNameExcelx, eDataType.ToString());

			validateFile(sl, m_oDictCellToCountryName);

			//TODO: Make these calls generic by a List with propertynames

			setData(sl, oJSONCoronaVirusData.ITA, "B", eDataType);
			setData(sl, oJSONCoronaVirusData.ESP, "C", eDataType);
			setData(sl, oJSONCoronaVirusData.USA, "D", eDataType);
			setData(sl, oJSONCoronaVirusData.DEU, "E", eDataType);
			setData(sl, oJSONCoronaVirusData.FRA, "F", eDataType);
			setData(sl, oJSONCoronaVirusData.IRN, "G", eDataType);
			setData(sl, oJSONCoronaVirusData.GBR, "H", eDataType);
			setData(sl, oJSONCoronaVirusData.NLD, "I", eDataType);
			setData(sl, oJSONCoronaVirusData.BEL, "J", eDataType);
			setData(sl, oJSONCoronaVirusData.SWE, "K", eDataType);
			setData(sl, oJSONCoronaVirusData.BRA, "L", eDataType);
			setData(sl, oJSONCoronaVirusData.IRL, "M", eDataType);
			setData(sl, oJSONCoronaVirusData.CAN, "N", eDataType);

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
			} else if (oData[0].date == "2020-02-04") {
				iRowModifier = 37;
			} else if (oData[0].date == "2020-02-15") {
				iRowModifier = 48;
			} else if (oData[0].date == "2020-02-19") {
				iRowModifier = 52;
			} else if (oData[0].date == "2020-02-26") {
				iRowModifier = 59;
			} else if (oData[0].date == "2020-02-29") {
				iRowModifier = 62;
			}else if (oData[0].date == "2020-05-14") {
				iRowModifier = 2;
			} else {
				throw new Exception("Invalid value:" + oData[0].date + " strColumn:" + strColumn);
			}

			for (int i = 0; i < oData.Count; i++) {
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

	

		private static void validateFile(SLDocument sl, Dictionary<string, string> oDictCellToCountryName) {
			foreach (var item in oDictCellToCountryName) {
				var strValue = sl.GetCellValueAsString(item.Key).Trim();
				if (!strValue.Equals(item.Value, StringComparison.InvariantCultureIgnoreCase)) {
					throw new Exception("Invalid File:" + strValue + "!=" + item.Value);
				}
			}
		}
	}
}