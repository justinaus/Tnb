using System;
using Xamarin.Forms;

namespace Tnb
{
	public partial class StandingsPage : ContentPage
	{

		private const String URL_NBA_STANDING = "http://www.nba.com/standings";


		public StandingsPage()
		{
			InitializeComponent();

			WebViewerViewModel vm = webViewer.WebViewerVM;

			this.BindingContext = vm;

			vm.InitUrl = URL_NBA_STANDING;

			// 일단 미리 부르는걸로.
			vm.GoInitUrl();
		}

	}
}
