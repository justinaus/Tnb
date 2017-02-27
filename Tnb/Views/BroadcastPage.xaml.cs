using System;

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Tnb
{
	public partial class BroadcastPage : ContentPage
	{

		BroadcastViewModel viewModel;

		//PopupWebviewPage webViewPage = null;


		public BroadcastPage()
		{
			InitializeComponent();

			viewModel = new BroadcastViewModel();

			listViewBroadcastGame.ItemsSource = viewModel.broadcastModelList;

			listViewBroadcastGame.ItemSelected += OnSelectedItem;

			labelDate.BindingContext = viewModel;

			btnPrev.Clicked += OnPrevClicked;
			btnNext.Clicked += OnNextClicked;

			var labelTab = new TapGestureRecognizer();
			labelTab.Tapped += OnTodayClicked;
			labelDate.GestureRecognizers.Add(labelTab);
		}


		protected override async void OnAppearing()
		{
			base.OnAppearing();

			await viewModel.OnViewAppearingAsync(this);
		}






		private async void OnSelectedItem( object sender, SelectedItemChangedEventArgs e )
		{
			if (e.SelectedItem == null)
			{
				return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
			}

			IBroadcastModel model = listViewBroadcastGame.SelectedItem as IBroadcastModel;

			listViewBroadcastGame.SelectedItem = null;

			if (model.Title.IndexOf(":", StringComparison.Ordinal) == -1)
			{
				return;
			}

			string goNaverUrl = await viewModel.GetLink( model );
			Debug.WriteLine( goNaverUrl );

			//openWebView();

			//Device.OpenUri(new Uri("instagram://"));
			//http://sports.news.naver.com/tv/index.nhn?category=etc&gameId=20170227KBOSC
			//Device.OpenUri(new Uri("naverplayer://"));
			Device.OpenUri(new Uri( goNaverUrl ));
		}

		private async void OnTodayClicked(object sender, EventArgs e)
		{
			//var aiView = new ActivityIndicatorView();

			//slMain.Children.Add( aiView );

			//aiView.OnLoading( true );



			TnbPage.Instance.IsBusy = true;

			await viewModel.changeToday();
		}

		private async void OnPrevClicked( object sender, EventArgs e )
		{
			await viewModel.changeDate( false );
		}

		private async void OnNextClicked(object sender, EventArgs e)
		{
			await viewModel.changeDate(true);
		}







		//private async Task openWebView( string url )
		//{
		//	if (webViewPage == null) webViewPage = new PopupWebviewPage();

		//	//http://sports.news.naver.com/tv/index.nhn?category=etc&gameId=20170227KBOSC

		//	//Device.OpenUri(new Uri( url ));

		//	//Application.Current.MainPage = webViewPage;
		//	return await Navigation.PushModalAsync( webViewPage );
		//}

	}
}
