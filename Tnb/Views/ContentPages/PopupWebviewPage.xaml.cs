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
				//WebViewerVM.OnClickedRefresh(sender, e);
				WebViewerVM.OnClickedHome(sender, e);
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


		public void OpenURL(string strURL)
		{
			WebViewerVM.InitUrl = strURL;
			WebViewerVM.GoUrl( strURL );
		}

	}
}
