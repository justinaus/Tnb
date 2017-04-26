using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace Tnb
{
	public partial class PopupWebviewPage : ContentPage
	{

		public WebViewerViewModel WebViewerVM;

		public delegate void CloseEventHandler(object sender, EventArgs e);
		public event CloseEventHandler Closed;


		public PopupWebviewPage()
		{
			InitializeComponent();

			//_webViewControl.CustomWebViewEvent += OnCustomWebViewHandler;

			WebViewerVM = new WebViewerViewModel();
			this.BindingContext = WebViewerVM;

			WebView webView = WebViewerVM.GetWebView();
			cv.Content = webView;

			setEvents();
		}

		private void setEvents()
		{
			var tabGestureRecognizer = new TapGestureRecognizer();
			tabGestureRecognizer.Tapped += OnClicked;

			btnBack.GestureRecognizers.Add(tabGestureRecognizer);
			btnForward.GestureRecognizers.Add(tabGestureRecognizer);
			btnRefresh.GestureRecognizers.Add(tabGestureRecognizer);
			btnWebBrowser.GestureRecognizers.Add(tabGestureRecognizer);
			btnClose.GestureRecognizers.Add(tabGestureRecognizer);
		}

		private void OnClicked(object sender, EventArgs e)
		{
			if (sender == btnBack)
			{
				WebViewerVM.OnClickedBack(sender, e);
			}
			else if (sender == btnForward)
			{
				WebViewerVM.OnClickedForward(sender, e);
			}
			else if (sender == btnRefresh)
			{
				WebViewerVM.OnClickedRefresh(sender, e);
			}
			else if (sender == btnWebBrowser)
			{
				WebViewerVM.OnClickedOpenWebBrowser(sender, e);
			}
			else if (sender == btnClose)
			{
				close();
			}
			else
			{
			}
		}

		private void close()
		{
			if (Closed != null)
			{
                Closed(this, new EventArgs());
			}

			Navigation.PopModalAsync();
		}

		//private void OnCustomWebViewHandler(object sender, CustomWebViewEventArgs e)
		//{
		//	Debug.WriteLine( "event listened" + e.TargetUrl );

		//	switch ( e.WebViewEventType )
		//	{
		//		case CustomWebViewEventArgs.Types.Closed :
		//			close();

		//			break;
		//		case CustomWebViewEventArgs.Types.NavigatedFailed :
		//			if (e.TargetUrl.IndexOf(NAVERPLAYER_SCHEME, StringComparison.Ordinal) == 0)
		//			{
		//				//Device.OpenUri(new Uri(e.TargetUrl));
		//				DependencyService.Get<IAppHandler>().LaunchApp( e.TargetUrl );
		//			}

		//			break;
		//	}
		//}

		public void OpenURL(string strURL)
		{
			WebViewerVM.GoUrl( strURL );
		}
	}
}
