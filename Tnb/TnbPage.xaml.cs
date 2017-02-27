using Xamarin.Forms;

namespace Tnb
{
	public partial class TnbPage : TabbedPage
	{

		private static TnbPage instance = null;


		public TnbPage()
		{
			InitializeComponent();

			//SetValue(NavigationPage.BarTextColorProperty, Color.Blue);

			instance = this;

			//IsBusy = true;
		}


		public static TnbPage Instance { 
			get
			{
				return instance;
			}
		}


	}
}
