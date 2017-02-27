using System;
using System.Collections.Generic;

namespace Tnb
{
	public class NaverGameModelDailyList : List<NaverGameModel>
	{

		public int Month { get; set; }
		public int Day { get; set; }


		public NaverGameModelDailyList()
		{
		}
	}
}
