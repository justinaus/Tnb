using System;

using System.Collections.ObjectModel;
using System.Diagnostics;

using Xamarin.Forms;

namespace Tnb
{
	public partial class BroadcastPage : ContentPage
	{

		BroadcastViewModel viewModel;


		public BroadcastPage()
		{
			InitializeComponent();

			viewModel = new BroadcastViewModel();

			listViewBroadcastGame.ItemsSource = viewModel.broadcastModelList;
		}


		protected override void OnAppearing()
		{
			base.OnAppearing();

			viewModel.OnViewAppearing();
		}

	}
}
