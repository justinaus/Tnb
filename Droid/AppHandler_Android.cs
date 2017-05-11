using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Android.Content;
using Tnb.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppHandler_Android))]

namespace Tnb.Droid
{
	public class AppHandler_Android : IAppHandler
	{
		public AppHandler_Android()
		{
			
		}

		public Task<bool> LaunchApp(string uri)
		{
			var aUri = Android.Net.Uri.Parse(uri.ToString());
			var intent = new Intent(Intent.ActionView, aUri);

		    try
		    {
		       	Forms.Context.StartActivity(intent);
		    }
		    catch (ActivityNotFoundException)
		    {
				uri = "market://details?id=com.nhn.android.naverplayer";
				//uri = "https://play.google.com/store/apps/details?id=com.nhn.android.naverplayer";
				aUri = Android.Net.Uri.Parse(uri.ToString());
				intent = new Intent(Intent.ActionView, aUri);
				Forms.Context.StartActivity(intent);
		    }

			return Task.FromResult(true);
		}
	}
}
