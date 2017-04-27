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

		const string NAVERPLAYER_SCHEME = "naverplayer";


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
				Debug.WriteLine( "FAILURE" );

				string goUrl = navigatingUrl == "" ? e.Url : navigatingUrl;

				if ( goUrl.IndexOf("intent://", StringComparison.Ordinal) == 0 )
				{
					goUrl = goUrl.Replace("intent://", "naverplayer://");
				}

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
