using System;
using System.Diagnostics;

namespace Tnb
{
	public class Spotv
	{
		public const string URL_SPOTV = "http://www.spotv.net/";
		public const string URL_SPOTV_DAILY = "data/json/schedule/daily.json2.asp";

		public const string DAY_PART_MORNING = "morning";
		public const string DAY_PART_EVENING = "evening";
		public const string DAY_PART_NIGHT = "night";

		public const string SPOTV_ONE = "spotv";
		public const string SPOTV_TWO = "spotv2";
		public const string SPOTV_PLUS = "spotvp";

		public const string SPOTV_ONE_SHOW = "spotv";
		public const string SPOTV_TWO_SHOW = "spotv two";
		public const string SPOTV_PLUS_SHOW = "spotv plus";



		public static string getDayPartToDisplay( string strDayPart, string strHour )
		{
			string strRet = "";

			int nHour = int.Parse( strHour );

			switch (strDayPart)
			{
				case DAY_PART_MORNING:
					if (nHour >= 12)
					{
						strRet = DayPartToDisplayStruct.EVENING;
					}
					else
					{
						strRet = DayPartToDisplayStruct.MORNING;
					}
					break;
				case DAY_PART_EVENING:
					strRet = DayPartToDisplayStruct.EVENING;
					break;
				case DAY_PART_NIGHT:
					strRet = DayPartToDisplayStruct.NIGHT;
					break;
			}

			return strRet;
		}


		public static string getChannelToDisplay( string strChannel )
		{
			string strRet = "";

			switch (strChannel)
			{
				case SPOTV_ONE:
					strRet = SPOTV_ONE_SHOW;
					break;
				case SPOTV_TWO:
					strRet = SPOTV_TWO_SHOW;
					break;
				case SPOTV_PLUS:
					strRet = SPOTV_PLUS_SHOW;
					break;
			}

			return strRet;
		}
	}
}
