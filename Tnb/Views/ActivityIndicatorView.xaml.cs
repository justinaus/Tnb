using Xamarin.Forms;

namespace Tnb
{
	public partial class ActivityIndicatorView : ContentView
	{
		

		public ActivityIndicatorView()
		{
			InitializeComponent();

			IsRunning = false;
		}


		public bool IsRunning
		{
			get
			{
				return activityIndicator.IsRunning;
			}

			set
			{
				activityIndicator.IsRunning = value;
				IsVisible = value;
			}
		}

	}
}
