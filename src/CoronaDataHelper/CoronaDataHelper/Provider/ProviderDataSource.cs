using CoronaDataHelper.DataSource;
using CoronaDataHelper.Interface;

namespace CoronaDataHelper.Processor {

	internal static class ProviderDataSource {

		internal enum EDataProvider {
			OurWorldInData
		}

		internal static IDataSource getDataSource(EDataProvider edataprovider) {
			switch (edataprovider) {
				case EDataProvider.OurWorldInData:
					return new DataSourceOurworldIndata();

				default:
					return null;
			}
		}
	}
}