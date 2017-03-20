﻿using System;
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

			viewModel = new BroadcastViewModel( this );

			listViewBroadcastGame.ItemsSource = viewModel.broadcastModelList;
			listViewBroadcastGame.ItemSelected += OnSelectedItem;

			viewModel.Start();
		}


		//protected override void OnAppearing()
		//{
		//	base.OnAppearing();

		//	viewModel.OnViewAppearing(this);
		//}


		private async void OnSelectedItem( object sender, SelectedItemChangedEventArgs e )
		{
			if (e.SelectedItem == null)
			{
				return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
			}

			//DependencyService.Get<ITextToSpeech>().Speak("Hello from Xamarin Forms");
			//Debug.WriteLine( "%%%ORIEND" +  DependencyService.Get<IDeviceOrientation>().GetOrientation());
			//DependencyService.Get<IDeviceOrientation>().SetOrientation( false );

			IBroadcastModel model = listViewBroadcastGame.SelectedItem as IBroadcastModel;

			listViewBroadcastGame.SelectedItem = null;
			if( model.Kind != BroadcastStruct.LIVE )
			//if (model.Title.IndexOf(":", StringComparison.Ordinal) == -1)
			{
				return;
			}

			string goNaverUrl = await viewModel.GetLink( model );
			Debug.WriteLine( goNaverUrl );

			PopupWebviewPage webViewPage = new PopupWebviewPage();
			webViewPage.OpenURL( goNaverUrl );

			//Application.Current.MainPage = webViewPage;

			webViewPage.Closed -= OnClosed;
			webViewPage.Closed += OnClosed;

			await Navigation.PushModalAsync( webViewPage );

			//Device.OpenUri(new Uri( goNaverUrl ));
			//Device.OpenUri(new Uri( "naverplayer://" ));
		}

		private void OnClosed(object sender, EventArgs e)
		{
			//DependencyService.Get<IDeviceOrientation>().SetOrientation(true);
		}


		public void ShowActivityIndicator( bool bShow )
		{
			aiv.IsRunning = bShow;
		}


		public BroadcastHeaderView BroadcastHeaderView
		{
			get
			{
				return broadcastHeader;
			}
		}


		public NetworkWarningView NetWarningView
		{
			get
			{
				return netWarningView;
			}
		}

	}
}
