using System;
using System.Collections.Generic;

using Xamarin.Forms;

using System.Diagnostics;

namespace Tnb
{
	public partial class StandingsPage : ContentPage
	{

		private WebViewControl _webViewControl;
		private WebView _webView;

		private const String URL_NBA_STANDING = "http://www.nba.com/standings";


		public StandingsPage()
		{
			InitializeComponent();

			_webViewControl = new WebViewControl(this);
			_webView = _webViewControl.GetWebView();

			cv.Content = _webView;

			_webViewControl.CustomWebViewEvent += OnCustomWebViewHandler;

			this.BindingContext = WebViewCtl;

			_webView.Source = URL_NBA_STANDING;
			aiv.IsRunning = true;
		}


		private void OnCustomWebViewHandler(object sender, CustomWebViewEventArgs e)
		{
			switch (e.WebViewEventType)
			{
				case CustomWebViewEventArgs.Types.NavigatedSuccess:
					if (aiv.IsRunning) aiv.IsRunning = false;

					break;
				case CustomWebViewEventArgs.Types.RefreshDefaultPage:
					_webView.Source = URL_NBA_STANDING;

					break;
			}
		}


		public WebViewControl WebViewCtl
		{
			get
			{
				return _webViewControl;
			}
		}


		//protected override void OnAppearing()
		//{
		//	base.OnAppearing();

		//	if (_webView.Source == null || (_webView.Source as UrlWebViewSource).Url == "")
		//	{
		//		_webView.Source = URL_NBA_STANDING;

		//		aiv.IsRunning = true;
		//	}
		//}

	}
}
