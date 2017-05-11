using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Tnb
{
	public class SettingPageViewModel : BaseViewModel
	{

		private ObservableCollectionCustomized<PageTypeGroup> groups = new ObservableCollectionCustomized<PageTypeGroup> {
			new PageTypeGroup ("서비스 정보", "A"){
				new PageModel("버전", "1.0.2")
			},
			new PageTypeGroup ("개발자 정보", "B"){
				new PageModel( "", "문의하기", true)
			}
		};

		public ObservableCollectionCustomized<PageTypeGroup> Groups
		{
			get
			{
				return groups;
			}
			set
			{
				groups = value;

                OnPropertyChanged("Groups");
			}
		}

		public SettingPageViewModel()
		{
			PageModel model = groups[0][0];
			model.SubTitle = DependencyService.Get<IPackageInfo>().Version;
		}
	}


	public class PageTypeGroup : List<PageModel>
	{
		public string Title { get; set; }
		public string ShortName { get; set; } //will be used for jump lists
		public string Subtitle { get; set; }

		public PageTypeGroup(string title, string shortName)
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

		public PageModel(string title, string subTitle = "", bool hasImage = false)
		{
			Title = title;
			SubTitle = subTitle;
			HasImage = hasImage;
		}
	}

}
