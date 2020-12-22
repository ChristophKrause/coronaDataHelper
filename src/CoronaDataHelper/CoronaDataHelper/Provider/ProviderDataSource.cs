using System;
using CoronaDataHelper.DataSource;
using CoronaDataHelper.Interface;

namespace CoronaDataHelper.Processor {

	internal static class ProviderDataSource {

		internal enum EDataProvider {
			OurWorldInData,
			
			Worldometer
		}

		internal static IDataSource getDataSource(EDataProvider edataprovider) {
			switch (edataprovider) {
				case EDataProvider.OurWorldInData:
					Console.WriteLine("Using "+nameof(DataSourceOurworldIndata));
					return new DataSourceOurworldIndata();
				case EDataProvider.Worldometer:
					Console.WriteLine("Using " + nameof(DataSourceWorldometer));
					return new DataSourceWorldometer();
				
				default:
					return null;
			}
		}
	}
}