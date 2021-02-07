using CoronaDataHelper.Interface;
using CoronaDataHelper.JSON;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using static CoronaDataHelper.Processor.ProviderDataSource;

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

		private static readonly Dictionary<string, string> m_oDictCellToProvider = new Dictionary<string, string>() {
			{"A1","Date"},{"B1","RKI"},{"C1","JHU"},{"D1","Worldometer"}
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
			} else if (oJSONData is Dictionary<EDataProvider, JSONCoronaVirusData> dictoJSONCoronaVirusData) {
				foreach (var item in dictoJSONCoronaVirusData) {
					Console.WriteLine("Found Data for:" + item.Key);
				}

				processXLSX(strFilename, dictoJSONCoronaVirusData, EDataType.infected);
				processXLSX(strFilename, dictoJSONCoronaVirusData, EDataType.death);
				return true;
			} else if (oJSONData is JSONCoronaVirusData oJSONCoronaVirusData) {
				processXLSX(strFilename, oJSONCoronaVirusData, EDataType.infected);
				processXLSX(strFilename, oJSONCoronaVirusData, EDataType.death);
				return true;
			} else {
				Console.WriteLine("Wrong data");
				return false;
			}
		}
		private static void processXLSX(string strFileNameExcelx, Dictionary<EDataProvider, JSONCoronaVirusData> dictoJSONCoronaVirusData, EDataType eDataType) {
			//XLSX Layout:
			//Date	RKI	JHU	Worldometer

			Console.WriteLine("OPEN Excel " + strFileNameExcelx);
			SLDocument sl = new SLDocument(strFileNameExcelx, eDataType.ToString());

			validateFile(sl, m_oDictCellToProvider);

			//TODO: Make these calls generic by a List with propertynames

			setData(
				sl: sl,
				oJSONCountry: dictoJSONCoronaVirusData[EDataProvider.GermanyOnlyMarlonLueckert].DEU,
				strColumn: "B",
				eRow: eDataType);
			setData(
				sl: sl,
				oJSONCountry: dictoJSONCoronaVirusData[EDataProvider.OurWorldInData].DEU,
				strColumn: "C",
				eRow: eDataType);
			setData(
				sl: sl,
				oJSONCountry: dictoJSONCoronaVirusData[EDataProvider.Worldometer].DEU,
				strColumn: "D",
				eRow: eDataType);		
			Console.WriteLine("save Excel " + strFileNameExcelx);

			sl.Save();
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
			setData(sl, oJSONCoronaVirusData.ISR, "O", eDataType);
			Console.WriteLine("save Excel " + strFileNameExcelx);

			sl.Save();
		}

		private static void setData(SLDocument sl, JSONCountry oJSONCountry, String strColumn, EDataType eRow) {
			Console.WriteLine("column: " + strColumn+" setData: " + oJSONCountry.location);

			var oData = oJSONCountry.data;
			for (int i = 0; i < oData.Count; i++) {
			
			//	Debug.WriteLine("Date: " + oData[i].date + " "  + oJSONCountry.location);

				float? fValue = oData[i].new_cases;
				if (eRow == EDataType.death) {
					fValue = oData[i].new_deaths;
				}

				int iValue = 0;
				if (fValue != null) {
					iValue = (int)fValue.Value;
				}
				int iRow = getRowIndexForDate(oData[i].date);
			//	Debug.WriteLine("Row for Date: "+ oData[0].date+" "+ iRow + " "+ oJSONCountry.location);
				
				string strrowIndex = strColumn + "" + iRow;


				//if (strColumn == "E") {
				//	Debug.WriteLine(strrowIndex+" "+ iValue + "    oData[0].date:" + oData[i].date);
				//}
				sl.ClearCellContent(strrowIndex, strrowIndex);
				sl.SetCellValue(strrowIndex, iValue);
			}
		}



		private static int getRowIndexForDate(string strDateTime) {
			string[] arstrparts = strDateTime.Split('-');
			int iYear = Int16.Parse(arstrparts[0]);
			int iMonth = Int16.Parse(arstrparts[1]);
			int iDay = Int16.Parse(arstrparts[2]);
			if (iMonth > 12 || iMonth<1) {
				throw  new Exception("Invalid month:"+ iMonth);
			}
			if (iDay > 31 || iDay < 1) {
				throw new Exception("Invalid day:" + iMonth);
			}
			if (iYear > 2030 || iYear < 2019) {
				throw new Exception("Invalid year:" + iMonth);
			}
			DateTime dtRow2 = new DateTime(2019, 12, 31);

			DateTime dtCurrent = new DateTime(iYear, iMonth, iDay);

			int iDiffDays = Convert.ToInt32((dtCurrent - dtRow2).TotalDays);

			return 2 + iDiffDays;

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