using System;
using Foundation;
using Tnb.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(PackageInfo_iOS))]

namespace Tnb.iOS
{
	public class PackageInfo_iOS : IPackageInfo
	{
		public PackageInfo_iOS()
		{
		}

		string IPackageInfo.Version
		{
			get
			{
				string strRet = "";

				if ( NSBundle.MainBundle.InfoDictionary != null && NSBundle.MainBundle != null)
				{
					strRet = NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();
				}

				return strRet;
			}
		}
	}
}
