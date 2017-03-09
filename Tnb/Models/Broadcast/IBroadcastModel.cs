using System;
namespace Tnb
{
	public interface IBroadcastModel
	{
		string Kind { get; set; }
		//string ScheduleDate { get; set; }
		//string ScheduleHour { get; set; }
		//string ScheduleMinute { get; set; }
		string Title { get; set; }

		//public DateTime ScheduleDateTime { get; set; }

		string Channel { get; set; }

		string Time { get; }

		string ImagePath { get; }

		string DayPart { get; set; }
	}
}
