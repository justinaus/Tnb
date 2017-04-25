using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace Tnb
{
	public partial class PopupWebviewPage : ContentPage
	{

		private WebViewControl _webViewControl;
		private WebView _webView;

		private const string NAVERPLAYER_SCHEME = "naverplayer";

		private bool IsFirst = true;

		public delegate void CloseEventHandler(object sender, EventArgs e);
		public event CloseEventHandler Closed;


		public PopupWebviewPage()
		{
			InitializeComponent();

			_webViewControl = new WebViewControl(this);
			_webView = _webViewControl.GetWebView();

			_webViewControl.CustomWebViewEvent += OnCustomWebViewHandler;

			cv.Content = _webView;

			this.BindingContext = WebViewCtl;
		}


		public WebViewControl WebViewCtl
		{
			get
			{
				return _webViewControl;
			}
		}


		private void OnCustomWebViewHandler(object sender, CustomWebViewEventArgs e)
		{
			Debug.WriteLine( "event listened" + e.TargetUrl );

			switch ( e.WebViewEventType )
			{
				case CustomWebViewEventArgs.Types.Closed :
					if (Closed != null)
					{
						Closed(this, new EventArgs());
					}

					Navigation.PopModalAsync();

					break;
				case CustomWebViewEventArgs.Types.NavigatedFailed :
					if (e.TargetUrl.IndexOf(NAVERPLAYER_SCHEME, StringComparison.Ordinal) == 0)
					{
						//Device.OpenUri(new Uri(e.TargetUrl));
						DependencyService.Get<IAppHandler>().LaunchApp( e.TargetUrl );
					}

					break;
			}
		}


		public void OpenURL(string strURL)
		{
			_webView.Source = strURL;
		}

	}
}
