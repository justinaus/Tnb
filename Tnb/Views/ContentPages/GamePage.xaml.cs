using System;
using Xamarin.Forms;

namespace Tnb
{
	public partial class GamePage : ContentPage
	{

		private const String URL_NBA_GAME = "https://watch.nba.com";


		public GamePage()
		{
			InitializeComponent();

			WebViewerViewModel vm = webViewer.WebViewerVM;

			this.BindingContext = vm;

			vm.InitUrl = URL_NBA_GAME;

			// 일단 미리 부르는걸로.
			vm.GoInitUrl();
		}

	}
}
