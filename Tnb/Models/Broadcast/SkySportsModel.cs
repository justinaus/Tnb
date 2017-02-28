using System;
namespace Tnb
{
	public class SkySportsModel
	{
		public SkySportsModel()
		{
		}

		public string Title { get; set; }

		public string Time { get; set; }

		public string Kind { get; set; }

		public string Channel { get; set; }

		public string ImagePath
		{
			get
			{
				return "Images/settings.png";
			}
		}


	}
}
