using CoronaDataHelper.JSON;
using CoronaDataHelper.Processor;

namespace CoronaDataHelper.Interface {

	internal interface IDataProcessor {

		bool process(string strFilename, object oJsondata);
	}
}