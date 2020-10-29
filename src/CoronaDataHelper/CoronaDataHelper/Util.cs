using System;
using System.Net;

namespace CoronaDataHelper {

	internal static class Util {

		internal static string downloadPageSource(String strURL) {
			using (WebClient client = new WebClient())
				return client.DownloadString(strURL);
		}
	}
}