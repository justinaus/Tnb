using System;
using System.Diagnostics;
using Xamarin.Forms;
using Tnb.iOS;
using UIKit;

[assembly: Dependency(typeof(DeviceOrientationImplementation))]

namespace Tnb.iOS
{
	public class DeviceOrientationImplementation : IDeviceOrientation
	{
		public DeviceOrientationImplementation()
		{
		}


		public DeviceOrientations GetOrientation()
		{
			var currentOrientation = UIApplication.SharedApplication.StatusBarOrientation;
			bool isPortrait = currentOrientation == UIInterfaceOrientation.Portrait
				|| currentOrientation == UIInterfaceOrientation.PortraitUpsideDown;

			return isPortrait ? DeviceOrientations.Portrait : DeviceOrientations.Landscape;
		}

		public void SetOrientation(bool isPortrait)
		{
			AppDelegate appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;

			appDelegate.IsPortraitMode = isPortrait;
		}

	}
}
