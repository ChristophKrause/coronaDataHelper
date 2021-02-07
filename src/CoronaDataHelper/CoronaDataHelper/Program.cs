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


				if (args[1].Equals(EDataProvider.Worldometer.ToString())) {
					IDataSource oIDataSource = ProviderDataSource.getDataSource(EDataProvider.Worldometer);
					process(oIDataSource.process(), strFilename);
				} else if (args[1].Equals(EDataProvider.OurWorldInData.ToString())) {
					IDataSource oIDataSource = ProviderDataSource.getDataSource(EDataProvider.OurWorldInData);
					process(oIDataSource.process(), strFilename);
				} else if (args[1].Equals(EDataProvider.GermanyJHUCSSEGIT.ToString())) {
					IDataSource oIDataSource = ProviderDataSource.getDataSource(EDataProvider.GermanyJHUCSSEGIT);
					process(oIDataSource.process(), strFilename);
				} else if (args[1].Equals(EDataProvider.GermanyMarlonLueckert.ToString())) {
					IDataSource oIDataSource = ProviderDataSource.getDataSource(EDataProvider.GermanyMarlonLueckert);
					process(oIDataSource.process(), strFilename);
				} else if (args[1].Equals(EDataProvider.GermanyOnly.ToString())) {
					Dictionary<EDataProvider, IDataSource> dictoIDataSource = new Dictionary<EDataProvider, IDataSource>();
					IDataSource oIDataSourceWorldometer = ProviderDataSource.getDataSource(EDataProvider.Worldometer);
					IDataSource oIDataSource2JHU = ProviderDataSource.getDataSource(EDataProvider.OurWorldInData);
					IDataSource oIDataSourceRKI = ProviderDataSource.getDataSource(EDataProvider.GermanyOnlyMarlonLueckert);
					dictoIDataSource.Add(EDataProvider.Worldometer, oIDataSourceWorldometer);
					dictoIDataSource.Add(EDataProvider.OurWorldInData, oIDataSource2JHU);
					dictoIDataSource.Add(EDataProvider.GermanyOnlyMarlonLueckert, oIDataSourceRKI);

					Dictionary<EDataProvider, JSONCoronaVirusData> dictoJSONCoronaVirusData = new Dictionary<EDataProvider, JSONCoronaVirusData>();
					foreach (var item in dictoIDataSource) {
						JSONCoronaVirusData oJSONCoronaVirusData = (JSONCoronaVirusData)item.Value.process();
						dictoJSONCoronaVirusData.Add(item.Key, oJSONCoronaVirusData);
					}
					process(dictoJSONCoronaVirusData, strFilename);
				} else {
					throw new Exception("Unknown data provider" + args[1]);
				}
			} catch (Exception e) {
				Console.WriteLine("Error:" + e);
			}
		}

		private static void process(object data, string strFilename) {
			IDataProcessor oIDataProcessor = ProviderProcessor.getDataProcessor(ProviderProcessor.EDataProcessor.Spreadsheetlight);
			oIDataProcessor.process(strFilename, data);
			return;


		}



	}
}