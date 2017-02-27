using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Tnb
{
	public partial class ActivityIndicatorView : ContentView
	{
		public ActivityIndicatorView()
		{
			InitializeComponent();
		}


		public void OnLoading( bool isRunning )
		{
			actInd.IsRunning = isRunning;
		}
	}
}
