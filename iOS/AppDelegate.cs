using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace Tnb.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{

		public bool IsPortraitMode = true;


		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			//UITabBar.Appearance.SelectedImageTintColor = UIColor.FromRGB(0x91, 0xCA, 0x47);

			global::Xamarin.Forms.Forms.Init();

			// Code for starting up the Xamarin Test Cloud Agent
#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
#endif

			UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
			UIApplication.SharedApplication.SetStatusBarHidden(false, false);

			LoadApplication(new App());

			//UIApplication.SharedApplication.SetStatusBarStyle( UIStatusBarStyle.LightContent, false  );

			return base.FinishedLaunching(app, options);
		}


		//public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, UIWindow forWindow)
		//{
		//	if ( IsPortraitMode )
		//	{
		//		return UIInterfaceOrientationMask.Portrait;
		//	}

		//	return UIInterfaceOrientationMask.AllButUpsideDown;
		//}

	}
}
