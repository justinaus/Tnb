using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace Tnb
{
	public partial class BroadcastPage : ContentPage
	{

		public BroadcastViewModel viewModel;


		public BroadcastPage()
		{
			InitializeComponent();

			viewModel = new BroadcastViewModel( this, broadcastHeader );

			listViewBroadcastGame.ItemsSource = viewModel.broadcastModelList;
			listViewBroadcastGame.ItemSelected += OnSelectedItem;
		}


		protected override void OnAppearing()
		{
			base.OnAppearing();

			viewModel.OnViewAppearing(this);
		}


		private async void OnSelectedItem( object sender, SelectedItemChangedEventArgs e )
		{
			if (e.SelectedItem == null)
			{
				return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
			}

			IBroadcastModel model = listViewBroadcastGame.SelectedItem as IBroadcastModel;

			listViewBroadcastGame.SelectedItem = null;

			if (model.Title.IndexOf(":", StringComparison.Ordinal) == -1)
			{
				return;
			}

			string goNaverUrl = await viewModel.GetLink( model );
			Debug.WriteLine( goNaverUrl );

			Device.OpenUri(new Uri( goNaverUrl ));
		}


		public void ShowActivityIndicator( bool bShow )
		{
			aiv.IsRunning = bShow;
		}

	}
}
