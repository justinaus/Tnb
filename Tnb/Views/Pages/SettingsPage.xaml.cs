using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace Tnb
{
	public partial class SettingsPage : ContentPage
	{
		
		public List<PageTypeGroup> Groups = new List<PageTypeGroup> {
			new PageTypeGroup ("서비스 정보", "A"){
				new PageModel("버전", "v1.0")
			},
			new PageTypeGroup ("개발자 정보", "B"){
				new PageModel( "", "문의하기", true)
			}
		};


		public SettingsPage()
		{
			InitializeComponent();

			listViewSetting.ItemsSource = Groups;

			listViewSetting.ItemSelected += OnSelectedItem;
		}


		private async void OnSelectedItem(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)
			{
				return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
			}

			PageModel model = listViewSetting.SelectedItem as PageModel;

			listViewSetting.SelectedItem = null;

			switch ( model.SubTitle )
			{
				case "문의하기" :
					sendMail( model.Title );

					break;
				case "후원하기" :
					break;
			}
		}


		private void sendMail( string strMailAddress )
		{
			Device.OpenUri(new Uri("mailto:" + strMailAddress));
		}


	}


	public class PageTypeGroup : List<PageModel>
	{
		public string Title { get; set; }
		public string ShortName { get; set; } //will be used for jump lists
		public string Subtitle { get; set; }

		public  PageTypeGroup(string title, string shortName)
		{
			Title = title;
			ShortName = shortName;
		}

		public static IList<PageTypeGroup> All { private set; get; }
	}


	public class PageModel
	{
		public string Title { get; set; }
		public string SubTitle { get; set; }
		public bool HasImage { get; set; }

		public PageModel(string title, string subTitle = "", bool hasImage = false )
		{
			Title = title;
			SubTitle = subTitle;
			HasImage = hasImage;
		}

	}
}
