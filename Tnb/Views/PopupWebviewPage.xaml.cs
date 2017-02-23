using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Tnb
{
	public partial class PopupWebviewPage : ContentPage
	{

		private WebView webview;


		public PopupWebviewPage()
		{
			InitializeComponent();

			webview = new WebView();

			cv.Content = webview;
		}




		public void OpenURL( string strURL )
		{
			webview.Source = strURL;
		}

		public void closeClicked( object sender, EventArgs e )
		{

		}
		public void backClicked(object sender, EventArgs e)
		{
			if (webview.CanGoBack)
			{
				webview.GoBack();
			}
		}
		public void forwardClicked(object sender, EventArgs e)
		{
			if (webview.CanGoForward)
			{
				webview.GoForward();
			}
		}
		public void openSafariClicked(object sender, EventArgs e)
		{

		}
	}
}
