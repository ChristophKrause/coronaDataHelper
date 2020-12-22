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
					return new DataSourceOurworldIndata();
				case EDataProvider.Worldometer:
					return new DataSourceWorldometer();
				
				default:
					return null;
			}
		}
	}
}