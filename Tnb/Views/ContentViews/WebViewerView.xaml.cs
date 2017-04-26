using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Tnb
{
	public partial class WebViewerView : ContentView
	{

		public WebViewerViewModel WebViewerVM;


		public WebViewerView()
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
			btnHome.GestureRecognizers.Add(tabGestureRecognizer);
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
			else if (sender == btnHome)
			{
				WebViewerVM.OnClickedHome(sender, e);
			}
			else
			{

			}
		}

	}
}
