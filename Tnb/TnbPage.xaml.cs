using Xamarin.Forms;

namespace Tnb
{
	public partial class TnbPage : TabbedPage
	{

		private TnbPageViewModel viewModel;


		public TnbPage()
		{
			InitializeComponent();

			//SetValue(NavigationPage.BarTextColorProperty, Color.Blue);

			viewModel = new TnbPageViewModel();
		}


	}
}
