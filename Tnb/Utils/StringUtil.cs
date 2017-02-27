using System;
namespace Tnb
{
	public class StringUtil
	{
		public StringUtil()
		{
		}


		public static string Trim( string strOld )
		{
			return strOld.Trim().Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace(" ", "");
		}

	}
}
