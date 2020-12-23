using CoronaDataHelper.Interface;
using CoronaDataHelper.JSON;
using CoronaDataHelper.Processor;
using CoronaDataHelper.Provider;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using CoronaDataHelper.Scraper;
using CsvHelper;
using DocumentFormat.OpenXml.Drawing;
using Newtonsoft.Json;
using static CoronaDataHelper.Processor.ProviderDataSource;
using Path = DocumentFormat.OpenXml.Drawing.Path;

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
				EDataProvider eDataProvider;
				if (args[1].Equals(EDataProvider.Worldometer.ToString())) {
					eDataProvider = EDataProvider.Worldometer;
					oIDataSource = ProviderDataSource.getDataSource(EDataProvider.Worldometer);
				} else if (args[1].Equals(EDataProvider.OurWorldInData.ToString())) {
					eDataProvider = EDataProvider.OurWorldInData;
					oIDataSource = ProviderDataSource.getDataSource(EDataProvider.OurWorldInData);
				} else if (args[1].Equals(EDataProvider.GermanyJHUCSSEGIT.ToString())) {
					eDataProvider = EDataProvider.GermanyJHUCSSEGIT; 
					oIDataSource = ProviderDataSource.getDataSource(EDataProvider.GermanyJHUCSSEGIT);
				} else {
					throw new Exception("Unknown data provider" + args[1]);
				}
				
			
				process(oIDataSource.process(), strFilename);
				
			
			} catch (Exception e) {
				Console.WriteLine("Error:" + e);
			}
		}

		private static void process( object data, string strFilename) {
			IDataProcessor oIDataProcessor = ProviderProcessor.getDataProcessor(ProviderProcessor.EDataProcessor.Spreadsheetlight);
				oIDataProcessor.process(strFilename, data);
				return;
			
			
		}



	}
}