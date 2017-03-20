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

		private bool NeedToCancel = false;

		Button btnBack;
		Button btnForward;


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
			btnBack = _page.FindByName<Button>("btnBack");
			btnForward = _page.FindByName<Button>("btnForward");
			Button btnRefresh = _page.FindByName<Button>("btnRefresh");
			Button btnWebBrowser = _page.FindByName<Button>("btnWebBrowser");
			Button btnClose = _page.FindByName<Button>("btnClose");
			Button btnHome = _page.FindByName<Button>("btnHome");

			if (btnBack != null)
			{
				btnBack.Clicked += backClicked;
			}
			if (btnForward != null)
			{
				btnForward.Clicked += forwardClicked;
			}
			if (btnRefresh != null)
			{
				btnRefresh.Clicked += refreshClicked;
			}
			if (btnWebBrowser != null)
			{
				btnWebBrowser.Clicked += openSafariClicked;
			}
			if (btnClose != null)
			{
				btnClose.Clicked += closeClicked;
			}
			if (btnHome != null)
			{
				btnHome.Clicked += HomeClicked;
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

			if (NeedToCancel && e.Url.StartsWith("https://itunes.apple.com", StringComparison.Ordinal))
			{
				e.Cancel = true;
				NeedToCancel = false;

				return;
			}

			NeedToCancel = false;

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
			Debug.WriteLine("navigated" + _navigatingUrl );

			if (e.Result == WebNavigationResult.Success)
			{
				CustomWebViewEvent(this, new CustomWebViewEventArgs(CustomWebViewEventArgs.Types.NavigatedSuccess, e.Url));
			} else if (e.Result == WebNavigationResult.Failure)
			{
				Debug.WriteLine( "fail" + _navigatingUrl );

				if (CustomWebViewEvent != null)
				{
					string goUrl = _navigatingUrl == "" ? e.Url : _navigatingUrl;

					NeedToCancel = true;

					CustomWebViewEvent(this, new CustomWebViewEventArgs(CustomWebViewEventArgs.Types.NavigatedFailed, goUrl));
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
			Debug.WriteLine( "back clicked" );

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
