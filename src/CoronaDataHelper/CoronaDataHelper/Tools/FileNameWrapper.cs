using System;


using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaDataHelper.Tools {
	internal class FileNameWrapper :IComparable<FileNameWrapper> {
		internal DateTime m_dtFile;

		internal string m_strFileName;

		internal FileNameWrapper(string strFileName) {
			m_strFileName = strFileName;
			string[] arstrparts = System.IO.Path.GetFileNameWithoutExtension(strFileName).Split('-');
			int iMonth = Int16.Parse(arstrparts[0]);
			int iDay = Int16.Parse(arstrparts[1]);
			int iYear = Int16.Parse(arstrparts[2]);
			m_dtFile = new DateTime(iYear, iMonth, iDay);
		}

		public int CompareTo(FileNameWrapper other) {
			return m_dtFile.CompareTo(other.m_dtFile);
		}
	}
}
