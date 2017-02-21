using System;
using System.Collections.Generic;

using System.Diagnostics;

using Xamarin.Forms;

namespace Tnb
{
	public partial class GamePage : ContentPage
	{

		private WebView webview;

		private const String URL_NBA_GAME = "https://watch.nba.com";


		public GamePage()
		{
			InitializeComponent();

			webview = new WebView();

			Content = webview;
		}


		protected override void OnAppearing()
		{
			base.OnAppearing();

			webview.Source = URL_NBA_GAME;
		}
	}
}
