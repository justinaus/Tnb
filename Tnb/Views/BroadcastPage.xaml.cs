using System;

using System.Collections.ObjectModel;
using System.Diagnostics;

using Xamarin.Forms;

namespace Tnb
{
	public partial class BroadcastPage : ContentPage
	{

		BroadcastViewModel viewModel;

		PopupWebviewPage webViewPage = null;


		public BroadcastPage()
		{
			InitializeComponent();

			viewModel = new BroadcastViewModel();

			listViewBroadcastGame.ItemsSource = viewModel.broadcastModelList;

			listViewBroadcastGame.ItemSelected += OnSelectedItem;

			labelDate.BindingContext = viewModel;

			btnPrev.Clicked += OnPrevClicked;
			btnNext.Clicked += OnNextClicked;
		}


		private void OnSelectedItem( object sender, EventArgs e )
		{
			if (webViewPage == null) webViewPage = new PopupWebviewPage();

			//Application.Current.MainPage = webViewPage;

			Navigation.PushModalAsync( webViewPage );

			const string URL = "https://watch.nba.com";

			webViewPage.OpenURL( URL );
		}


		private void OnPrevClicked( object sender, EventArgs e )
		{
			viewModel.changeDate( false );
		}

		private void OnNextClicked(object sender, EventArgs e)
		{
			viewModel.changeDate(true);
		}


		protected override void OnAppearing()
		{
			base.OnAppearing();

			viewModel.OnViewAppearing();


		}

	}
}
