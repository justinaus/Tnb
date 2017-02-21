using System;
namespace Tnb
{
	public sealed class DateManager
	{

		private static readonly DateManager instance = new DateManager();

		private static DateTime dateTimeNow;



		public static DateManager Instance
		{
			get
			{
				return instance;
			}
		}


		public static DateTime DateTimeNow {
			get {
				return dateTimeNow;
			}
			set
			{
				dateTimeNow = value;
			}
		}
			
			

	}
}
