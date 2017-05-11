using System;
using Android.Content;
using Tnb.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(PackageInfo_Android))]

namespace Tnb.Droid
{
	public class PackageInfo_Android : IPackageInfo
	{
		public PackageInfo_Android()
		{
		}

		public string Version
		{
			get
			{
				Context context = Forms.Context;
				var version = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;

				return version;
			}
		}
	}
}
