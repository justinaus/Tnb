using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

//[assembly: ExportRenderer(typeof(TabbedPage), typeof(TabbedPageCustom))]
namespace Tnb.iOS
{
	public class TabbedPageCustom : TabbedRenderer
	{
		public TabbedPageCustom()
		{
			//Debug.WriteLine( "$$$$$$$$$4" + TabBar.TintColor );
		}
	}
}
