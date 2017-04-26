using System;
namespace Tnb
{
	public class BroadcastHeaderViewModel : BaseViewModel
	{

		private DateTime dateTimeCurrent;

		public delegate void DateChangedEventHandler(object sender, DateChangedEventArgs e);
		public event DateChangedEventHandler DateChanged;


		public BroadcastHeaderViewModel()
		{
		}


		public DateTime SetToday()
		{
			//2017-02-12 오후 5:30:13
			DateTime DateTimeNow = DateTime.Now;

			if (DateUtil.GetIsSameDate(DateTimeCurrent, DateTimeNow))
			{
				return DateTimeNow;
			}

			return SetDate(DateTimeNow);
		}


		public DateTime SetDate( DateTime dt )
		{
			DateTimeCurrent = dt;

			return DateTimeCurrent;
		}

		public DateTime GoNextDate()
		{
			DateTimeCurrent = DateTimeCurrent.AddDays(1);

			return DateTimeCurrent;
		}

		public DateTime GoPrevDate()
		{
			DateTimeCurrent = DateTimeCurrent.AddDays(-1);

			return DateTimeCurrent;
		}

		public DateTime DateTimeCurrent
		{
			get
			{
				return dateTimeCurrent;
			}

			private set
			{
				dateTimeCurrent = value;

				OnPropertyChanged("CurrentDate");

				if (DateChanged != null)
				{
					DateChanged(this, new DateChangedEventArgs(dateTimeCurrent));
				}
			}
		}

		public string CurrentDate
		{
			get
			{
				return DateTimeCurrent.Month + " / " + DateTimeCurrent.Day + " (" + DateUtil.GetDayOfWeekForKorean(DateTimeCurrent.DayOfWeek) + ")";
			}
		}

	}


	public class DateChangedEventArgs : EventArgs
	{

		private DateTime dateTimeTarget;

		public DateChangedEventArgs(DateTime dt)
		{
			dateTimeTarget = dt;
		}

		public DateTime DateTimeTarget
		{
			get { return dateTimeTarget; }
		}	
	}
}
