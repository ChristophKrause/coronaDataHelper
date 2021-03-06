﻿using System;
using CoronaDataHelper.DataSource;
using CoronaDataHelper.Interface;

namespace CoronaDataHelper.Processor {

	internal static class ProviderDataSource {

		internal enum EDataProvider {
			OurWorldInData,
			Worldometer,
			GermanyJHUCSSEGIT,
			GermanyMarlonLueckert,
			GermanyOnlyMarlonLueckert,
			GermanyOnly
		}

		internal static IDataSource getDataSource(EDataProvider edataprovider) {
			switch (edataprovider) {
				case EDataProvider.OurWorldInData:
					Console.WriteLine("Using "+nameof(DataSourceOurworldIndata));
					return new DataSourceOurworldIndata();
				case EDataProvider.Worldometer:
					Console.WriteLine("Using " + nameof(DataSourceWorldometer));
					return new DataSourceWorldometer();
				case EDataProvider.GermanyJHUCSSEGIT:
					Console.WriteLine("Using " + nameof(DataSourceGermanyJHUCSSEGIT));
					return new DataSourceGermanyJHUCSSEGIT();
				case EDataProvider.GermanyMarlonLueckert:
					Console.WriteLine("Using " + nameof(DataSourceMarlonLueckert));
					return new DataSourceMarlonLueckert();
				case EDataProvider.GermanyOnlyMarlonLueckert:
					Console.WriteLine("Using " + nameof(DataSourceMarlonLueckertGermany));
					return new DataSourceMarlonLueckertGermany();
				default:
					Console.WriteLine("Not found " + edataprovider);
					return null;
			}
		}
	}
}