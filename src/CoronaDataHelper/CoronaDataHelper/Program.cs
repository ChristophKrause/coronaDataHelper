﻿using CoronaDataHelper.Interface;
using CoronaDataHelper.JSON;
using CoronaDataHelper.Processor;
using CoronaDataHelper.Provider;
using System;
using System.IO;
using CoronaDataHelper.Scraper;
using static CoronaDataHelper.Processor.ProviderDataSource;

namespace CoronaDataHelper {

	internal static class Program {

		private static void Main(string[] args) {
			try {

				if (args.Length < 2) {
					Console.WriteLine("Usage:CoronaDataHelper.exe <EXCELFILE> <EDataProvider>");
					Console.WriteLine("EDataProvider:");
					string[] arstrDataProvider = Enum.GetNames(typeof(EDataProvider));
					foreach (string strDataProvider in arstrDataProvider) {
						Console.WriteLine(strDataProvider);
					}

					Console.WriteLine("Example:CoronaDataHelper.exe  coronadata_worldometer.xlsx " + EDataProvider.Worldometer);
					return;
				}
				//ScraperWorldometer oScraperWorldometer = new ScraperWorldometer();
				//oScraperWorldometer.processUrl("https://www.worldometers.info/coronavirus/country/germany/");
				//	return;
				string strFilename = args[0];
				if (string.IsNullOrWhiteSpace((strFilename))) {
					throw new Exception("No Filename");
				}
				if (!File.Exists(strFilename)) {
					throw new Exception("Can not find ExcelFile:" + strFilename);
				}

				IDataSource oIDataSource;
				if (args[1].Equals(EDataProvider.Worldometer.ToString())) {
					oIDataSource = ProviderDataSource.getDataSource(EDataProvider.Worldometer);
				} else if (args[1].Equals(EDataProvider.OurWorldInData.ToString())) {
					oIDataSource = ProviderDataSource.getDataSource(EDataProvider.OurWorldInData);
				} else {
					throw new Exception("Unknown data provider" + args[1]);
				}
				
				JSONCoronaVirusData oJSONCoronaVirusData = oIDataSource.process();

				IDataProcessor oIDataProcessor = ProviderProcessor.getDataProcessor(ProviderProcessor.EDataProcessor.Spreadsheetlight);
				oIDataProcessor.process(strFilename, oJSONCoronaVirusData);
			} catch (Exception e) {
				Console.WriteLine("Error:" + e);
			}
		}
	}
}