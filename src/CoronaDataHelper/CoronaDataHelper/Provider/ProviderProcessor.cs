using CoronaDataHelper.Interface;
using CoronaDataHelper.Processor;

namespace CoronaDataHelper.Provider {

	internal static class ProviderProcessor {

		internal enum EDataProcessor {
			Spreadsheetlight
		}

		internal static IDataProcessor getDataProcessor(EDataProcessor eDataProcessor) {
			switch (eDataProcessor) {
				case EDataProcessor.Spreadsheetlight:
					return new ProcessorSpreadsheetlight();

				default:
					return null;
			}
		}
	}
}