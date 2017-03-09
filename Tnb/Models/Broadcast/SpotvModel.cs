using System;
using System.Diagnostics;

namespace Tnb
{
	public class SpotvModel : IBroadcastModel
	{
		
		public SpotvModel( string title = "", string scheduleHour = "", string scheduleMinute = "" )
		{
			//"kind": "재방송",
			//"sch_date": "2017-02-14",
			//"sch_hour": 5,
			//"sch_min": "30",
			//"title": "2014 WTA 카타르 토탈 오픈 결승 할렙:커버"

			if (title != "") Title = title;
			if (scheduleHour != "") ScheduleHour = scheduleHour;
			if (scheduleMinute != "") ScheduleMinute = scheduleMinute;
		}


		public string Kind { get; set; }
		public string ScheduleDate { get; set; }
		public string ScheduleHour { get; set; }
		public string ScheduleMinute { get; set; }
		public string Title { get; set; }

		//public DateTime ScheduleDateTime { get; set; }

		public string Channel { get; set; }

		public string DayPart { get; set; }

		public string Time
		{
			get
			{
				string hour = ScheduleHour;
				if (hour.Length < 2) hour = "0" + hour;

				return hour + ":" + ScheduleMinute;
			}
		}

		public string ImagePath
		{
			get
			{
				string strRet = "Images/today.png";

				switch ( Kind )
				{
					case BroadcastStruct.LIVE :
						strRet = "Images/today.png";
						break;
					case BroadcastStruct.REGULAR:
						strRet = "Images/schedule.png";
						break;
					case BroadcastStruct.RERUN:
						strRet = "Images/settings.png";
						break;
					case BroadcastStruct.DELAYED:
						strRet = "Images/settings.png";
						break;
				}

				return strRet;
			}
		}

	}
}
