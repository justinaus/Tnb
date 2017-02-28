using System;
namespace Tnb
{
	public class TnbPageViewModel
	{

		private static TnbPageViewModel instance = null;

		private ActivityIndicatorView activityIndicatorView;


		public TnbPageViewModel()
		{
			instance = this;
		}


		public static TnbPageViewModel Instance
		{
			get
			{
				return instance;
			}
		}

		/// <summary>
		/// Gets the activity indicator view.
		/// Only one indicator view.
		/// </summary>
		/// <returns>The activity indicator view.</returns>
		public ActivityIndicatorView GetActivityIndicatorView()
		{
			if (activityIndicatorView == null)
			{
				activityIndicatorView = new ActivityIndicatorView();
			}

			return activityIndicatorView;
		}


		public bool IsActivityIndicatorRunning
		{
			get
			{
				return activityIndicatorView.IsRunning;
			}

			set
			{
				activityIndicatorView.IsRunning = value;
			}
		}


	}
}
