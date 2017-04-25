using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace Tnb
{
	public partial class BroadcastPage : ContentPage
	{

		public BroadcastViewModel viewModel;


		public BroadcastPage()
		{
			InitializeComponent();

			Debug.WriteLine( "@@@@@@@only once@@@@@@" );

			viewModel = new BroadcastViewModel( this );

			listViewBroadcastGame.ItemsSource = viewModel.broadcastModelList;
			listViewBroadcastGame.ItemSelected += OnSelectedItem;

			viewModel.Start();
		}


		//protected override void OnAppearing()
		//{
		//	base.OnAppearing();

		//	viewModel.OnViewAppearing(this);
		//}


		private async void OnSelectedItem( object sender, SelectedItemChangedEventArgs e )
		{
			Debug.WriteLine( "### selected ###" + e.SelectedItem == null );

			if (e.SelectedItem == null)
			{
				return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
			}

			//DependencyService.Get<ITextToSpeech>().Speak("Hello from Xamarin Forms");
			//Debug.WriteLine( "%%%ORIEND" +  DependencyService.Get<IDeviceOrientation>().GetOrientation());
			//DependencyService.Get<IDeviceOrientation>().SetOrientation( false );

			IBroadcastModel model = listViewBroadcastGame.SelectedItem as IBroadcastModel;

			listViewBroadcastGame.SelectedItem = null;
			if( model.Kind != BroadcastStruct.LIVE )
			{
				return;
			}

			string goNaverUrl = await viewModel.GetLink( model );
			Debug.WriteLine( goNaverUrl );

			PopupWebviewPage webViewPage = new PopupWebviewPage();
			webViewPage.OpenURL( goNaverUrl );

			webViewPage.Closed -= OnClosed;
			webViewPage.Closed += OnClosed;

			await Navigation.PushModalAsync( webViewPage );

			//await DependencyService.Get<IAppHandler>().LaunchApp("naverplayer://" );

			//Device.OpenUri(new Uri("naverplayer://nlc_play?minAppVersion=1210&is_addr=is1.ncast.naver.com&is_port=11310&serviceID=12002&liveId=S2017041909ch5&qualityId=2000&title=+&video1=selected%3Dtrue%26%26title%3DHD%ED%99%94%EC%A7%88%26%26type%3Dnlivecast%26%26url%3Dhighch5%26%26id%3D2000&video2=selected%3Dfalse%26%26title%3D%EA%B3%A0%ED%99%94%EC%A7%88%26%26type%3Dnlivecast%26%26url%3Dlowch5%26%26id%3D800&video3=selected%3Dfalse%26%26title%3D%EC%A0%80%ED%99%94%EC%A7%88%26%26type%3Dcdn%26%26url%3Dhttp%3A%2F%2Fhls.live.m.nhn.gscdn.com%2Fch5%2F_definst_%2Fch5_300.stream%2Fplaylist.m3u8%26%26id%3D300&advertiseUrl=http%3A%2F%2Fams.rmcnmv.naver.com%2Fitem%2Fcreate%2F2002%2FNAVER%3Fams_ctgr%3DNSPORTS%26ams_cp%3DNHN%26svc%3Dsportslive%26st%3DLIVE%26ams_vodType%3DLIVE%26ams_chnl%3DLIVE_nba%26cl%3D%26ams_videoId%3D2017041909%26unit%3D1104B#Intent;scheme=naverplayer;action=android.intent.action.VIEW;category=android.intent.category.BROWSABLE;package=com.nhn.android.naverplayer;end"));
			//Device.OpenUri(new Uri( "daummaps://" ));
			//Device.OpenUri(new Uri( "naverplayer://" ));
			//Device.OpenUri(new Uri( "https://appsto.re/kr/rryj3.i" ));
		}

		private void OnClosed(object sender, EventArgs e)
		{
			//DependencyService.Get<IDeviceOrientation>().SetOrientation(true);
		}


		public void ShowActivityIndicator( bool bShow )
		{
			aiv.IsRunning = bShow;
		}


		public BroadcastHeaderView BroadcastHeaderView
		{
			get
			{
				return broadcastHeader;
			}
		}


		public NetworkWarningView NetWarningView
		{
			get
			{
				return netWarningView;
			}
		}

	}
}
