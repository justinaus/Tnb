using System;
using System.Collections.Generic;

namespace Tnb
{
	public class DateUtil
	{
		public DateUtil()
		{
		}

		/*
		 * SortAscending
6/19/1979 12:00:00 AM
5/5/1980 12:00:00 AM
10/20/1982 12:00:00 AM
1/4/1984 12:00:00 AM
*/

		/// <summary>
		/// Sorts the ascending.
		/// </summary>
		/// <returns>The ascending.</returns>
		/// <param name="list">List.</param>
		/// <example>6/19/1979 12:00:00 AM
		/// 5/5/1980 12:00:00 AM
		/// 10/20/1982 12:00:00 AM
		/// 1/4/1984 12:00:00 AM</example>
		public static List<DateTime> SortAscending(List<DateTime> list)
		{
			list.Sort((a, b) => a.CompareTo(b));
			return list;
		}

		public static List<DateTime> SortDescending(List<DateTime> list)
		{
			list.Sort((a, b) => b.CompareTo(a));
			return list;
		}

		public static string getDayOfWeekForKorean( DayOfWeek dayOfWeekEng )
		{
			string strRet = "";

			switch (dayOfWeekEng)
			{
				case DayOfWeek.Monday:
					strRet = "월";
					break;
				case DayOfWeek.Tuesday:
					strRet = "화";
					break;
				case DayOfWeek.Wednesday:
					strRet = "수";
					break;
				case DayOfWeek.Thursday:
					strRet = "목";
					break;
				case DayOfWeek.Friday:
					strRet = "금";
					break;
				case DayOfWeek.Saturday:
					strRet = "토";
					break;
				case DayOfWeek.Sunday:
					strRet = "일";
					break;
				default:
					break;
			}
			return strRet;
		}

	}
}
