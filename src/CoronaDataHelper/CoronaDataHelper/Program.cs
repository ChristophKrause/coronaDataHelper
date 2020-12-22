using CoronaDataHelper.Interface;
using CoronaDataHelper.JSON;
using CoronaDataHelper.Processor;
using CoronaDataHelper.Provider;
using System;
using System.IO;
using static CoronaDataHelper.Processor.ProviderDataSource;

namespace CoronaDataHelper {

	internal static class Program {

		private static void Main(string[] args) {
			try {

				string strFilename = args[0];
				if (string.IsNullOrWhiteSpace((strFilename))) {
					throw new  Exception("No Filename");
				}
				if (!File.Exists(strFilename)) {
					throw new Exception("Can not find ExcelFile:" + strFilename);
				}
				IDataSource oIDataSource = ProviderDataSource.getDataSource(EDataProvider.OurWorldInData);
				JSONCoronaVirusData oJSONCoronaVirusData = oIDataSource.process();

				IDataProcessor oIDataProcessor = ProviderProcessor.getDataProcessor(ProviderProcessor.EDataProcessor.Spreadsheetlight);
				oIDataProcessor.process(strFilename,oJSONCoronaVirusData);
			} catch (Exception e) {
				Console.WriteLine("Error:" + e);
			}
		}
	}
}