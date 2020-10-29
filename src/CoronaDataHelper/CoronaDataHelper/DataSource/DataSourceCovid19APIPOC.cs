using CoronaDataHelper.Interface;
using CoronaDataHelper.JSON;
using Covid19;
using Covid19.Classes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoronaDataHelper.DataSource {

	//Ony POC andnot used
	internal class DataSourceCovid19APIPOC : IDataSource {

		public JSONCoronaVirusData process() {
			Task t = processMethod2Async();
			t.Wait();
			return null;
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
					Console.WriteLine(item.UpdateDate + " " + (item.Confirmed - iOld));
					iOld = item.Confirmed;
				}
			} catch (Exception e) {
				Console.WriteLine("Error:" + e);
			}
		}
	}
}