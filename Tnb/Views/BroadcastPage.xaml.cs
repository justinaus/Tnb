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


		private void OnSelectedItem( object sender, SelectedItemChangedEventArgs e )
		{
			if (e.SelectedItem == null)
			{
				return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
			}

			listViewBroadcastGame.SelectedItem = null;

			if (webViewPage == null) webViewPage = new PopupWebviewPage();

			const string URL = "https://watch.nba.com";
			webViewPage.OpenURL(URL);

			Debug.WriteLine("123");

			//Application.Current.MainPage = webViewPage;
			Navigation.PushModalAsync( webViewPage );

			Debug.WriteLine( "456" + listViewBroadcastGame );

			Debug.WriteLine("789" + listViewBroadcastGame.SelectedItem);


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
