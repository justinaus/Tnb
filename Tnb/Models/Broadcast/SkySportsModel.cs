using System;
namespace Tnb
{
	public class SkySportsModel : IBroadcastModel
	{
		public SkySportsModel()
		{
		}

		public string Title { get; set; }

		public string Time { get; set; }

		public string Kind { get; set; }

		public string Channel { get; set; }

		public string DayPart { get; set; }

		public string ImagePath
		{
			get
			{
				return "Images/Broadcast/rebroad.png";
			}
		}

		public bool IsLive
		{
			get
			{
				return Kind == BroadcastStruct.LIVE;
			}
		}


	}
}
