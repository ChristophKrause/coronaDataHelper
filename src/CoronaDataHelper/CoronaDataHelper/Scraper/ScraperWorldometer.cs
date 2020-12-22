using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaDataHelper.Scraper {
	internal class ScraperWorldometer {


		internal Dictionary<DateTime, int> processUrl(string strUri, string strNeedle) {

			string strSource = Util.downloadPageSource(strUri);
			//Find needle
		//strNeedle = "name: 'Daily Cases',";

			//strNeedle = "name: 'Daily Deaths',";
			int iIndex = strSource.IndexOf(strNeedle);
			string strSourceShort = strSource.Substring((iIndex));
			//Debug.WriteLine(("strSourceShort:" + strSourceShort));
			//split by  [ and take 1
			string[] strSplit = strSourceShort.Split('[');
			Debug.WriteLine(("strSplit[1]:"+ strSplit[1]));
			strSplit = strSplit[1].Split(']');
			Debug.WriteLine(("strSplit[0]:" + strSplit[0]));
			string[] arStrData = strSplit[0].Split(',');
			DateTime dtData = new DateTime(2020,02,15);
			Dictionary<DateTime, int> dictDateToData = new Dictionary<DateTime, int>();
			foreach (var item in arStrData) {
				string strValue = item;
				if (strValue == "null") {
					strValue = "0";
				}

				int iValue = int.Parse(strValue);
				dictDateToData.Add(dtData, iValue);
				dtData = dtData.AddDays(1);
			}

			foreach (var item  in dictDateToData) {
				Debug.WriteLine(item.Key.ToString(("yy-MM-dd"))+" "+item.Value);
			}

			return dictDateToData;
		}
	}
}
