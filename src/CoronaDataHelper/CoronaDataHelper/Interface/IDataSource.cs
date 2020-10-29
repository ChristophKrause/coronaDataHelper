using CoronaDataHelper.JSON;

namespace CoronaDataHelper.Interface {

	internal interface IDataSource {

		JSONCoronaVirusData process();
	}
}