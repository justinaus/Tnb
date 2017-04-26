using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace Tnb
{
	public partial class SettingsPage : ContentPage
	{

		readonly SettingPageViewModel vm;


		public SettingsPage()
		{
			InitializeComponent();

			vm = new SettingPageViewModel();
			this.BindingContext = vm;
		}

		private void OnSelectedItem(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)
			{
				return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
			}

			PageModel model = listViewSetting.SelectedItem as PageModel;

			listViewSetting.SelectedItem = null;

			const string REFRESENTATIVE_MAIL = "justriz81@gmail.com";

			switch ( model.SubTitle )
			{
				case "문의하기" :
					SendMail( REFRESENTATIVE_MAIL );

					break;
				case "후원하기" :
					break;
			}
		}

		private void SendMail( string strMailAddress )
		{
			Device.OpenUri(new Uri("mailto:" + strMailAddress));
		}
	}
}
