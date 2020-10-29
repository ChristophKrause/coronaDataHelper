using CoronaDataHelper.JSON;

namespace CoronaDataHelper.Interface {

	internal interface IDataProcessor {

		bool process(JSONCoronaVirusData oJSONCoronaVirusData);
	}
}