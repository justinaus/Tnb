using System;
namespace Tnb
{
	public class SkySports
	{
		public const string URL_SCHEDULE = "https://tv.skylife.co.kr/skysports/timetable/by/channel";

		public const string CHANNEL = "skysports";


		public static string getDayPartToDisplay(string strTime)
		{
			const char DIV = ':';

			if (strTime.IndexOf(DIV) == -1) return "";

			string strHour = strTime.Split(DIV)[0];
			int nHour = int.Parse(strHour.Trim());

			string strRet = "";

			if ( nHour >= 6 && nHour < 12 )
			{
				// 06 ~ 13;
				strRet = DayPartToDisplay.MORNING;
			} else if ( nHour >= 12 && nHour < 23 )
			{
				// 13 ~ 23;
				strRet = DayPartToDisplay.EVENING;
			} else
			{
				strRet = DayPartToDisplay.NIGHT;
			}

			return strRet;
		}

	}
}
