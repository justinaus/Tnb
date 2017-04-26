using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace Tnb
{

	public partial class BroadcastHeaderView : ContentView
	{

		public BroadcastHeaderViewModel ViewModel;


		public BroadcastHeaderView()
		{
			InitializeComponent();

			ViewModel = new BroadcastHeaderViewModel();
			this.BindingContext = ViewModel;

			IsEnabled = false;

			SetEvents();
		}

		public void SetToday()
		{
			ViewModel.SetToday();
		}

		private void SetEvents()
		{
			var tabGestureRecognizer = new TapGestureRecognizer();
			tabGestureRecognizer.Tapped += OnClicked;
			labelDate.GestureRecognizers.Add(tabGestureRecognizer);
			btnPrev.GestureRecognizers.Add(tabGestureRecognizer);
			btnNext.GestureRecognizers.Add(tabGestureRecognizer);
		}

		private void OnClicked(object sender, EventArgs e)
		{
			if (sender == labelDate)
			{
				SetToday();
			}
			else if (sender == btnPrev)
			{
				ViewModel.GoPrevDate();
			}
			else if (sender == btnNext)
			{
				ViewModel.GoNextDate();
			}
			else
			{

			}
		}
	}
}
