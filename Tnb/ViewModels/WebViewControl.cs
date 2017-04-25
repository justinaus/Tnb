using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace Tnb
{
	
	public class WebViewControl : BaseViewModel
	{

		private WebView _webView;

		private bool _isBusy = false;

		private ContentPage _page;

		private string _navigatingUrl = "";

		public delegate void CustomWebViewEventHandler(object sender, CustomWebViewEventArgs e);
		public event CustomWebViewEventHandler CustomWebViewEvent;

		//private bool NeedToCancel = false;

		Image btnBack;
		Image btnForward;
		Image btnRefresh;
		Image btnWebBrowser;
		Image btnClose;
		Image btnHome;


		public WebViewControl( ContentPage page )
		{
			_page = page;

			_webView = new WebView();

			//_webView.Navigated += webView_Navigated;
			_webView.Navigating += webView_Navigating;

			setEvents();

			CanGoFoward = CanGoFoward;
			CanGoBack = CanGoBack;
		}


		private void setEvents()
		{
			btnBack = _page.FindByName<Image>("btnBack");
			btnForward = _page.FindByName<Image>("btnForward");
			btnRefresh = _page.FindByName<Image>("btnRefresh");
			btnWebBrowser = _page.FindByName<Image>("btnWebBrowser");
			btnClose = _page.FindByName<Image>("btnClose");
			btnHome = _page.FindByName<Image>("btnHome");

			var tabGestureRecognizer = new TapGestureRecognizer();
			tabGestureRecognizer.Tapped += OnClicked;
			//labelDate.GestureRecognizers.Add(tabGestureRecognizer);

			if (btnBack != null)
			{
				//btnBack.Clicked += backClicked;
				btnBack.GestureRecognizers.Add(tabGestureRecognizer);
			}
			if (btnForward != null)
			{
				//btnForward.Clicked += forwardClicked;
				btnForward.GestureRecognizers.Add(tabGestureRecognizer);
			}
			if (btnRefresh != null)
			{
				//btnRefresh.Clicked += refreshClicked;
				btnRefresh.GestureRecognizers.Add(tabGestureRecognizer);
			}
			if (btnWebBrowser != null)
			{
				//btnWebBrowser.Clicked += openSafariClicked;
				btnWebBrowser.GestureRecognizers.Add(tabGestureRecognizer);
			}
			if (btnClose != null)
			{
				//btnClose.Clicked += closeClicked;
				btnClose.GestureRecognizers.Add(tabGestureRecognizer);
			}
			if (btnHome != null)
			{
				//btnHome.Clicked += HomeClicked;
				btnHome.GestureRecognizers.Add(tabGestureRecognizer);
			}
		}


		private void OnClicked(object sender, EventArgs e)
		{
			Debug.WriteLine( "clicked" + ( sender == btnClose ) );

			if (sender == btnBack)
			{
				backClicked(sender, e);
			}
			else if (sender == btnForward)
			{
				forwardClicked(sender, e);
			}
			else if (sender == btnRefresh)
			{
				refreshClicked(sender, e);
			}
			else if (sender == btnWebBrowser)
			{
				openSafariClicked(sender, e);
			}
			else if (sender == btnClose)
			{
				closeClicked(sender, e);
			}
			else if (sender == btnHome)
			{
				HomeClicked(sender, e);
			}
			else 
			{

			}
		}


		public WebView GetWebView()
		{
			return _webView;
		}


		public bool IsBusy
		{
			get
			{
				return _isBusy;
			}

			set
			{
				_isBusy = value;

				OnPropertyChanged( "IsBusy" );
			}
		}


		public bool CanGoBack
		{
			get
			{
				return _webView.CanGoBack;
			}

			set
			{
				if (btnBack != null)
				{
					btnBack.Opacity = value ? 1 : 0.3;

					OnPropertyChanged("CanGoBack");
				}
			}
		}

		public bool CanGoFoward
		{
			get
			{
				return _webView.CanGoForward;
			}

			set
			{
				if (btnForward != null)
				{
					btnForward.Opacity = value ? 1 : 0.3;

					OnPropertyChanged("CanGoFoward");
				}
			}
		}

		private void webView_Navigating(object sender, WebNavigatingEventArgs e)
		{
			Debug.WriteLine( "navigating"+ e.Url );

			//if (NeedToCancel)
			////if (NeedToCancel && e.Url.StartsWith("https://itunes.apple.com", StringComparison.Ordinal))
			//{
			//	e.Cancel = true;
			//	//NeedToCancel = false;

			//	return;
			//}

			//NeedToCancel = false;

			_navigatingUrl = e.Url;

			_webView.Navigated += webView_Navigated;
			_webView.Navigating -= webView_Navigating;

			if (!e.Url.StartsWith("http", StringComparison.Ordinal)) return;

			IsBusy = true;

			CanGoFoward = CanGoFoward;
			CanGoBack = CanGoBack;
		}


		private void webView_Navigated(object sender, WebNavigatedEventArgs e)
		{
			Debug.WriteLine("navigated" + _navigatingUrl + "/" + e.Result );

			if (e.Result == WebNavigationResult.Success)
			{
				
			} else if (e.Result == WebNavigationResult.Failure)
			{
				if (CustomWebViewEvent != null)
				{
					string goUrl = _navigatingUrl == "" ? e.Url : _navigatingUrl;

					//NeedToCancel = true;

					//Device.OpenUri(new Uri( "naverplayer://nlc_play?minAppVersion=1210&is_addr=is1.ncast.naver.com&is_port=11310&serviceID=12002&liveId=S2017042422ch5&qualityId=2000&title=+&video1=selected%3Dtrue%26%26title%3DHD%ED%99%94%EC%A7%88%26%26type%3Dnlivecast%26%26url%3Dhighch5%26%26id%3D2000&video2=selected%3Dfalse%26%26title%3D%EA%B3%A0%ED%99%94%EC%A7%88%26%26type%3Dnlivecast%26%26url%3Dlowch5%26%26id%3D800&video3=selected%3Dfalse%26%26title%3D%EC%A0%80%ED%99%94%EC%A7%88%26%26type%3Dcdn%26%26url%3Dhttp%3A%2F%2Fhls.live.m.nhn.gscdn.com%2Fch5%2F_definst_%2Fch5_300.stream%2Fplaylist.m3u8%26%26id%3D300&advertiseUrl=http%3A%2F%2Fams.rmcnmv.naver.com%2Fitem%2Fcreate%2F2002%2FNAVER%3Fams_ctgr%3DNSPORTS%26ams_cp%3DNHN%26svc%3Dsportslive%26st%3DLIVE%26ams_vodType%3DLIVE%26ams_chnl%3DLIVE_nba%26cl%3D%26ams_videoId%3D2017042422%26unit%3D1104B#Intent;scheme=naverplayer;action=android.intent.action.VIEW;category=android.intent.category.BROWSABLE;package=com.nhn.android.naverplayer;end" ));
					CustomWebViewEvent(this, new CustomWebViewEventArgs(CustomWebViewEventArgs.Types.NavigatedFailed, goUrl));

					//return;
				}
			}

			_webView.Navigating += webView_Navigating;
			_webView.Navigated -= webView_Navigated;

			IsBusy = false;

			_navigatingUrl = "";

			CanGoBack = CanGoBack;
			CanGoFoward = CanGoFoward;
		}


		public void HomeClicked(object sender, EventArgs e)
		{
			//Navigation.PopModalAsync();
			if (CustomWebViewEvent != null)
			{
				CustomWebViewEvent(this, new CustomWebViewEventArgs(CustomWebViewEventArgs.Types.RefreshDefaultPage));
			}
		}


		public void closeClicked(object sender, EventArgs e)
		{
			//Navigation.PopModalAsync();
			if (CustomWebViewEvent != null)
			{
				CustomWebViewEvent(this, new CustomWebViewEventArgs( CustomWebViewEventArgs.Types.Closed ));
			}
		}
		public void backClicked(object sender, EventArgs e)
		{
			if (_webView.CanGoBack)
			{
				_webView.GoBack();
			}
		}
		public void forwardClicked(object sender, EventArgs e)
		{
			if (_webView.CanGoForward)
			{
				_webView.GoForward();
			}
		}
		public void openSafariClicked(object sender, EventArgs e)
		{
			Device.OpenUri(new Uri((_webView.Source as UrlWebViewSource).Url));
		}
		public void refreshClicked(object sender, EventArgs e)
		{
			_webView.Source = (_webView.Source as UrlWebViewSource).Url;
		}

	}

	public class CustomWebViewEventArgs : EventArgs
	{

		private Types _type;

		private string _TargetUrl;

		public enum Types { Closed, NavigatedFailed, NavigatedSuccess, RefreshDefaultPage };


		public CustomWebViewEventArgs(Types types, string strUrl = "")
		{
			WebViewEventType = types;

			TargetUrl = strUrl;
		}

		public Types WebViewEventType
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}

		public string TargetUrl
		{
			get
			{
				return _TargetUrl;
			}
			set
			{
				_TargetUrl = value;
			}
		}

	}
}
