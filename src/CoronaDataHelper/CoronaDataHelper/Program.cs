using CoronaDataHelper.Interface;
using CoronaDataHelper.JSON;
using CoronaDataHelper.Processor;
using CoronaDataHelper.Provider;
using System;
using static CoronaDataHelper.Processor.ProviderDataSource;

namespace CoronaDataHelper {

	internal static class Program {

		private static void Main() {
			try {
				IDataSource oIDataSource = ProviderDataSource.getDataSource(EDataProvider.OurWorldInData);
				JSONCoronaVirusData oJSONCoronaVirusData = oIDataSource.process();

				IDataProcessor oIDataProcessor = ProviderProcessor.getDataProcessor(ProviderProcessor.EDataProcessor.Spreadsheetlight);
				oIDataProcessor.process(oJSONCoronaVirusData);
			} catch (Exception e) {
				Console.WriteLine("Error:" + e);
			}
		}
	}
}