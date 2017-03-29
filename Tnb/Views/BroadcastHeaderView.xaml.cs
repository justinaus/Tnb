using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace Tnb
{

	public partial class BroadcastHeaderView : ContentView
	{
		
		private DateTime dateTimeCurrent;

		public delegate void DateChangedEventHandler(object sender, DateChangedEventArgs e);
		// public function funcTest;
		public event DateChangedEventHandler DateChanged;


		public BroadcastHeaderView()
		{
			InitializeComponent();

			IsEnabled = false;

			SetEvents();

			BindingContext = this;
		}


		public void SetToday()
		{
			//2017-02-12 오후 5:30:13
			DateTime DateTimeNow = DateTime.Now;

			if (!DateUtil.GetIsSameDate(DateTimeCurrent, DateTimeNow))
			{
				DateTimeCurrent = DateTimeNow;
			}
		}

		private void SetEvents()
		{
			//btnPrev.Clicked += OnPrevClicked;
			//btnNext.Clicked += OnNextClicked;

			var tabGestureRecognizer = new TapGestureRecognizer();
			//tabGestureRecognizer.Tapped += OnTodayClicked;
			tabGestureRecognizer.Tapped += OnClicked;
			labelDate.GestureRecognizers.Add(tabGestureRecognizer);
			btnPrev.GestureRecognizers.Add(tabGestureRecognizer);
			btnNext.GestureRecognizers.Add(tabGestureRecognizer);
		}


		private void OnClicked(object sender, EventArgs e)
		{
			if (sender == labelDate)
			{
				OnTodayClicked(sender, e);
			}
			else if (sender == btnPrev)
			{
				OnPrevClicked(sender, e);
			}
			else if (sender == btnNext)
			{
				OnNextClicked(sender, e);
			}
			else
			{

			}
		}


		private void OnPrevClicked(object sender, EventArgs e)
		{
			if (!IsEnabled) return;

			DateTimeCurrent = DateTimeCurrent.AddDays(-1);
		}

		private void OnNextClicked(object sender, EventArgs e)
		{
			if (!IsEnabled) return;

			DateTimeCurrent = DateTimeCurrent.AddDays(1);
		}


		private void OnTodayClicked(object sender, EventArgs e)
		{
			SetToday();
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

				OnPropertyChanged( "CurrentDate" );

				if ( DateChanged != null )
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
