using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace Tnb
{
	public class WebViewerViewModel : BaseViewModel
	{

		private WebView webView;

		private bool isBusy = false;

		private string navigatingUrl = "";

		private string initUrl;


		public WebViewerViewModel()
		{
			webView = new WebView();

			webView.Navigating += webView_Navigating;
		}

		public WebView GetWebView()
		{
			return webView;
		}

		public string InitUrl
		{
			get
			{
				return initUrl;
			}
			set
			{
				initUrl = value;
			}
		}

		public bool CanGoBack
		{
			get
			{
				return webView.CanGoBack;
			}
		}

		public double GoBackButtonOpacity
		{
			get
			{
				return CanGoBack ? 1 : 0.3;
			}
		}

		public bool CanGoForward
		{
			get
			{
				return webView.CanGoForward;
			}
		}

		public double GoForwardButtonOpacity
		{
			get
			{
				return CanGoForward ? 1 : 0.3;
			}
		}

		public bool IsBusy
		{
			get
			{
				return isBusy;
			}
			set
			{
				isBusy = value;

				OnPropertyChanged("IsBusy");
			}
		}

		public void GoUrl( string strUrl )
		{
			webView.Source = strUrl;
		}

		public void GoInitUrl()
		{
			webView.Source = InitUrl;
		}

		private void webView_Navigating(object sender, WebNavigatingEventArgs e)
		{
			Debug.WriteLine("navigating" + e.Url);

			navigatingUrl = e.Url;

			webView.Navigated += webView_Navigated;
			webView.Navigating -= webView_Navigating;

			if (!e.Url.StartsWith("http", StringComparison.Ordinal)) return;

			IsBusy = true;

			OnChangedWebViewCanMoveStatus();
		}


		private void OnChangedWebViewCanMoveStatus()
		{
            OnPropertyChanged("CanGoBack");
			OnPropertyChanged("GoBackButtonOpacity");
            OnPropertyChanged("CanGoForward");
			OnPropertyChanged("GoForwardButtonOpacity");
		}

		private void webView_Navigated(object sender, WebNavigatedEventArgs e)
		{
			Debug.WriteLine("navigated" + navigatingUrl + "/" + e.Result);

			if (e.Result == WebNavigationResult.Success)
			{

			}
			else if (e.Result == WebNavigationResult.Failure)
			{
				//if (CustomWebViewEvent != null)
				//{
					string goUrl = navigatingUrl == "" ? e.Url : navigatingUrl;

				//	//NeedToCancel = true;

				//	//Device.OpenUri(new Uri( "naverplayer://nlc_play?minAppVersion=1210&is_addr=is1.ncast.naver.com&is_port=11310&serviceID=12002&liveId=S2017042422ch5&qualityId=2000&title=+&video1=selected%3Dtrue%26%26title%3DHD%ED%99%94%EC%A7%88%26%26type%3Dnlivecast%26%26url%3Dhighch5%26%26id%3D2000&video2=selected%3Dfalse%26%26title%3D%EA%B3%A0%ED%99%94%EC%A7%88%26%26type%3Dnlivecast%26%26url%3Dlowch5%26%26id%3D800&video3=selected%3Dfalse%26%26title%3D%EC%A0%80%ED%99%94%EC%A7%88%26%26type%3Dcdn%26%26url%3Dhttp%3A%2F%2Fhls.live.m.nhn.gscdn.com%2Fch5%2F_definst_%2Fch5_300.stream%2Fplaylist.m3u8%26%26id%3D300&advertiseUrl=http%3A%2F%2Fams.rmcnmv.naver.com%2Fitem%2Fcreate%2F2002%2FNAVER%3Fams_ctgr%3DNSPORTS%26ams_cp%3DNHN%26svc%3Dsportslive%26st%3DLIVE%26ams_vodType%3DLIVE%26ams_chnl%3DLIVE_nba%26cl%3D%26ams_videoId%3D2017042422%26unit%3D1104B#Intent;scheme=naverplayer;action=android.intent.action.VIEW;category=android.intent.category.BROWSABLE;package=com.nhn.android.naverplayer;end" ));
				//	CustomWebViewEvent(this, new CustomWebViewEventArgs(CustomWebViewEventArgs.Types.NavigatedFailed, goUrl));

				//	//return;
				//}

				const string NAVERPLAYER_SCHEME = "naverplayer";

				if (goUrl.IndexOf(NAVERPLAYER_SCHEME, StringComparison.Ordinal) == 0)
				{
					DependencyService.Get<IAppHandler>().LaunchApp( goUrl );
				}
			}

			webView.Navigating += webView_Navigating;
			webView.Navigated -= webView_Navigated;

			IsBusy = false;

			navigatingUrl = "";

            OnChangedWebViewCanMoveStatus();
		}


		public void OnClickedHome(object sender, EventArgs e)
		{
			GoInitUrl();
		}

		public void OnClickedBack(object sender, EventArgs e)
		{
			if (webView.CanGoBack)
			{
				webView.GoBack();
			}
		}

		public void OnClickedForward(object sender, EventArgs e)
		{
			if (webView.CanGoForward)
			{
				webView.GoForward();
			}
		}

		public void OnClickedOpenWebBrowser(object sender, EventArgs e)
		{
			Device.OpenUri(new Uri( (webView.Source as UrlWebViewSource).Url ));
		}

		public void OnClickedRefresh(object sender, EventArgs e)
		{
            GoUrl( (webView.Source as UrlWebViewSource).Url );
		}

	}
}
