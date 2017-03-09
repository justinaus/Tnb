using System;
using System.Net;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using HtmlAgilityPack;
using System.Linq;

namespace Tnb
{
	public class BroadcastViewModel : BaseViewModel, IHandleViewAppearing
	{

		private BroadcastPage _view;
		private BroadcastHeaderView _broadcastHeaderView;

		public ObservableCollectionCustomized<BroadcastModelGroup> broadcastModelList = new ObservableCollectionCustomized<BroadcastModelGroup>();

		private NaverDataManager naverDataManager;

		private const int CALL_TIMEOUT_LIMIT = 15000;

		private bool hasNetworkProblem = false;



		public BroadcastViewModel( BroadcastPage view )
		{
			_view = view;
			_broadcastHeaderView = view.BroadcastHeaderView;

			view.NetWarningView.BindingContext = this;

			_broadcastHeaderView.DateChanged += onDateChanged;
		}


		public void OnViewAppearing(VisualElement view)
		{
			HasNetworkProblem = false;

			_broadcastHeaderView.SetToday();
		}


		private async void onDateChanged(object sender, DateChangedEventArgs e)
		{
			HasNetworkProblem = false;

			_broadcastHeaderView.IsEnabled = false;
			_view.ShowActivityIndicator(true);

			broadcastModelList.Clear();

			//await getDataTest( "http://m.sports.naver.com/basketball/schedule/index.nhn?category=nba&date=20170224" );
			await getSpotvDataAll(e.DateTimeTarget);

			BroadcastModelGroup group = await getSkySportsData(e.DateTimeTarget);
			if (group.Count > 0) broadcastModelList.Add(group);

			_view.ShowActivityIndicator(false);
			_broadcastHeaderView.IsEnabled = true;
		}


		private async Task<BroadcastModelGroup> getSkySportsData( DateTime dateTime )
		{
			HttpClient client = new HttpClient();

			client.Timeout = TimeSpan.FromMilliseconds( CALL_TIMEOUT_LIMIT );

			string result = "";

			try
			{
				HttpResponseMessage response = await client.GetAsync(SkySports.URL_SCHEDULE);

				HttpContent content = response.Content;

				result = await content.ReadAsStringAsync();
				result = WebUtility.HtmlDecode(result);
			}
			catch ( Exception e )
			{
				Debug.WriteLine( "skysports http call timeout!! : " + e );

				warnNetworking();
			}

			return MakeSkySportsData(result, dateTime);
		}


		private void warnNetworking()
		{
			HasNetworkProblem = true;
		}


		private BroadcastModelGroup MakeSkySportsData(string strHTML, DateTime dateTime)
		{
			BroadcastModelGroup group = new BroadcastModelGroup();
			group.Channel = SkySports.CHANNEL;
			group.ChannelShow = group.Channel;

			int startIdx = strHTML.IndexOf("<html", System.StringComparison.Ordinal);
			if (startIdx > 0) strHTML = strHTML.Substring(startIdx);

			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(strHTML);

			SkySportsModel model;

			int nDayCurrent = (int)dateTime.DayOfWeek;

			//IEnumerable<HtmlNode> trNode = doc.DocumentNode.Descendants( "tr" ).Where(x => x.ParentNode.Name == "tbody");

			foreach (HtmlNode element in doc.DocumentNode.Descendants("tr"))
			{
				if (element.ParentNode.Name != "tbody") continue;
				// 24 lines.

				int nSeq = 0;

				foreach (HtmlNode element2 in element.Descendants("td"))
				{
					if (element2.ParentNode != element) continue;

					++nSeq;

					if (nSeq == 1) continue;

					int nDay = nSeq - 2;

					bool isYesterday = nDay == nDayCurrent - 1;

					if ( !isYesterday && nDay != nDayCurrent) continue;

					if (element2.OuterHtml.IndexOf("NBA", StringComparison.Ordinal) == -1) continue;

					foreach (HtmlNode element3 in element2.Descendants("p"))
					{
						if (element3.OuterHtml.IndexOf("NBA", StringComparison.Ordinal) == -1) continue;

						model = new SkySportsModel();

						string strTime = element2.Descendants("p").Where(x => x.GetAttributeValue("class", "") == "dateTxt mb10").First().InnerText;

						model.Time = strTime;
						model.Channel = SkySports.CHANNEL;
						model.Kind = BroadcastStruct.RERUN;
						model.Title = element2.Descendants("span").First().InnerText;

						model.DayPart = SkySports.getDayPartToDisplay( model.Time );

						if( !getIsAlreadyHas( group, model ) )	group.Add(model);
					}
				}
			}

			return group;
		}


		private bool getIsAlreadyHas( BroadcastModelGroup group, SkySportsModel model  )
		{
			SkySportsModel modelAlready;

			for (int i = 0; i < group.Count; ++i)
			{
				modelAlready = (Tnb.SkySportsModel)group[i];

				if (modelAlready.Time == model.Time || modelAlready.Title == model.Title)	return true;
			}

			return false;
		}


		private async Task<ObservableCollectionCustomized<BroadcastModelGroup>> getSpotvDataAll(DateTime dateTime)
		{
			BroadcastModelGroup group = await getSpotvDataByChannel(dateTime, Spotv.SPOTV_ONE);
			if (group.Count > 0) broadcastModelList.Add(group);

			group = await getSpotvDataByChannel(dateTime, Spotv.SPOTV_TWO);
			if (group.Count > 0) broadcastModelList.Add(group);

			group = await getSpotvDataByChannel(dateTime, Spotv.SPOTV_PLUS);
			if (group.Count > 0) broadcastModelList.Add(group);

			return broadcastModelList;
		}


		private async Task<BroadcastModelGroup> getSpotvDataByChannel(DateTime dateTime, string strChannel)
		{
			BroadcastModelGroup group = new BroadcastModelGroup();
			group.Channel = strChannel;
			group.ChannelShow = Spotv.getChannelToDisplay(strChannel);

			// add yesterday midnight.
			ObservableCollectionCustomized<IBroadcastModel> gotModelList = await getSpotvData(dateTime.AddDays( -1 ), Spotv.DAY_PART_NIGHT, strChannel, true);
			group.AddRange(gotModelList);

			gotModelList = await getSpotvData(dateTime, Spotv.DAY_PART_MORNING, strChannel);
			group.AddRange(gotModelList);
			gotModelList = await getSpotvData(dateTime, Spotv.DAY_PART_EVENING, strChannel);
			group.AddRange(gotModelList);
			gotModelList = await getSpotvData(dateTime, Spotv.DAY_PART_NIGHT, strChannel);
			group.AddRange(gotModelList);

			return group;
		}


		private async Task<ObservableCollectionCustomized<IBroadcastModel>> getSpotvData(DateTime dateTime, string strDayPart, string strChannel, bool isYesterday = false )
		{
			HttpClient client = new HttpClient();

			client.Timeout = TimeSpan.FromMilliseconds(CALL_TIMEOUT_LIMIT);

			string strParams = "?y=" + dateTime.Year + "&m=" + dateTime.Month + "&d=" + dateTime.Day + "&dayPart=" + strDayPart + "&ch=" + strChannel;
			//HttpResponseMessage response = await client.GetAsync("http://www.spotv.net/data/json/schedule/daily.json2.asp?y=2017&m=2&d=14&dayPart=morning&ch=spotv2");

			ObservableCollectionCustomized<IBroadcastModel> listSpotv;

			try
			{
				HttpResponseMessage response = await client.GetAsync(Spotv.URL_SPOTV + Spotv.URL_SPOTV_DAILY + strParams);

				HttpContent content = response.Content;

				string result = await content.ReadAsStringAsync();

				result = WebUtility.HtmlDecode(result);

				JArray jarrResult = JArray.Parse(result);

				listSpotv = getNBAPrettyData(this, jarrResult, strChannel, strDayPart, isYesterday);
			}
			catch ( Exception e )
			{
				Debug.WriteLine("spotv http call timeout!! : " + e);

				listSpotv = new ObservableCollectionCustomized<IBroadcastModel>();

				warnNetworking();
			}

			return listSpotv;
		}


		private ObservableCollectionCustomized<IBroadcastModel> getNBAPrettyData(BroadcastViewModel instance, JArray jarrRaw, string strChannel, string strDayPart, bool isYesterday = false )
		{
			ObservableCollectionCustomized<IBroadcastModel> listRet = new ObservableCollectionCustomized<IBroadcastModel>();

			JObject jobjRaw;
			SpotvModel model;

			//	"kind": "재방송",
			//"sch_date": "2017-02-14",
			//"sch_hour": 5,
			//"sch_min": "30",
			//"title": "2014 WTA 카타르 토탈 오픈 결승 할렙:커버"

			string title;
			string kind;
			string scheduleDate;
			string scheduleHour;
			string scheduleMinute;

			for (int i = 0; i < jarrRaw.Count; ++i)
			{
				jobjRaw = jarrRaw[i] as JObject;

				title = (string)(jobjRaw["title"]);

				if (!title.Contains("NBA") && !title.Contains("nba")) continue;

				kind = (string)(jobjRaw["kind"]);
				scheduleDate = (string)(jobjRaw["sch_date"]);
				scheduleHour = (string)(jobjRaw["sch_hour"]);
				scheduleMinute = (string)(jobjRaw["sch_min"]);

				if (!getIsValidNightHour(strDayPart, isYesterday, int.Parse(scheduleHour))) continue;

				model = new SpotvModel();

				model.Kind = kind;
				model.ScheduleDate = scheduleDate;
				model.ScheduleHour = scheduleHour;
				model.ScheduleMinute = scheduleMinute;
				model.Title = title;
				//model.ScheduleDateTime = dateTime;

				model.Channel = strChannel;

				// morning over 12, change to afternoon.
				model.DayPart = Spotv.getDayPartToDisplay( strDayPart, model.ScheduleHour );

				listRet.Add(model);
			}

			return listRet;
		}


		private bool getIsValidNightHour( string strDayPart, bool isYesterday, int nHour )
		{
			if (strDayPart != Spotv.DAY_PART_NIGHT) return true;

			if (isYesterday)
			{
				if (nHour < 24 && nHour > 9) return false;
			}
			else
			{
				if (nHour == 24 || nHour < 20) return false;
			}

			return true;
		}

		private bool getIsValidHourSkySports(bool isYesterday, int nHour)
		{
			if (isYesterday)
			{
				if (nHour >= 0 && nHour <= 5) return true;
			}
			else
			{
				if (nHour >= 6 || nHour <= 23) return true;
			}

			return false;
		}


		public async Task<string> GetLink( IBroadcastModel model )
		{
			if (naverDataManager == null)
			{
				naverDataManager = new NaverDataManager();
			}

			return await naverDataManager.GetGameURL( _broadcastHeaderView.DateTimeCurrent, model.Title );
		}


		public bool HasNetworkProblem
		{
			get
			{
				return hasNetworkProblem;
			}

			set
			{
				hasNetworkProblem = value;

				OnPropertyChanged( "HasNetworkProblem" );
			}
		}



	}
}
