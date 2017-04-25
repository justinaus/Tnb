using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Foundation;
using Tnb.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppHandler_iOS))]

namespace Tnb.iOS
{
	public class AppHandler_iOS : IAppHandler
	{
		public AppHandler_iOS()
		{
		}

		public Task<bool> LaunchApp(string uri)
		{
			//bool canOpen = UIApplication.SharedApplication.CanOpenUrl(new NSUrl(uri));
			bool canOpen = UIApplication.SharedApplication.CanOpenUrl(new NSUrl("naverplayer://"));

			if (!canOpen)
			{
				uri = "https://appsto.re/kr/rryj3.i";
			}

			return Task.FromResult(UIApplication.SharedApplication.OpenUrl(new NSUrl(uri)));
		}
	}
}
