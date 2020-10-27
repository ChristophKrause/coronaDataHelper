using CoronaDataHelper.Data;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace CoronaDataHelper {

	internal class Util {

		internal static string downloadPageSource(String strURL) {
			using (WebClient client = new WebClient())
				return client.DownloadString(strURL);
		}

		internal static JSONCoronaVirusData getJSONData(string strjSONURL, string strFileNameJSON) {
			string strJSON;

			if (string.IsNullOrWhiteSpace(strFileNameJSON)) {
				Console.WriteLine("Downloading Data");
				strJSON = Util.downloadPageSource(strjSONURL);
			} else if (!File.Exists(strFileNameJSON)) {
				Console.WriteLine("Downloading Data");
				strJSON = Util.downloadPageSource(strjSONURL);
				File.WriteAllText(strFileNameJSON, strJSON);
			} else {
				Console.WriteLine("Read file Data");
				strJSON = File.ReadAllText(strFileNameJSON);
			}


			Console.WriteLine("deserialize Data");
			return JsonConvert.DeserializeObject<JSONCoronaVirusData>(strJSON);
		}
	}
}