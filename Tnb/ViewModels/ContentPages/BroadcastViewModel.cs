using System;
using System.Net;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Tnb
{
	public class BroadcastViewModel : BaseViewModel
	{
		
		private ObservableCollectionCustomized<BroadcastModelGroup> broadcastModelList = new ObservableCollectionCustomized<BroadcastModelGroup>();

		private const int CALL_TIMEOUT_LIMIT = 15000;

		private bool hasNetworkProblem;
		private bool isBusy;


		public BroadcastViewModel()
		{
			HasNetworkProblem = false;
			IsBusy = false;
		}

		public async void OnDateChanged(object sender, DateChangedEventArgs e)
		{
			HasNetworkProblem = false;

			IsBusy = true;

			BroadcastModelList.Clear();

			//await getDataTest( "http://m.sports.naver.com/basketball/schedule/index.nhn?category=nba&date=20170224" );
			await getSpotvDataAll(e.DateTimeTarget);

			BroadcastModelGroup group = await getSkySportsData(e.DateTimeTarget);
			if (group.Count > 0) broadcastModelList.Add(group);

			IsBusy = false;
		}

		private async Task<ObservableCollectionCustomized<BroadcastModelGroup>> getSpotvDataAll(DateTime dateTime)
		{
			BroadcastModelGroup group = await getSpotvDataByChannel(dateTime, SpotvService.SPOTV_ONE);
			if (group.Count > 0) broadcastModelList.Add(group);

			group = await getSpotvDataByChannel(dateTime, SpotvService.SPOTV_TWO);
			if (group.Count > 0) broadcastModelList.Add(group);

			group = await getSpotvDataByChannel(dateTime, SpotvService.SPOTV_PLUS);
			if (group.Count > 0) broadcastModelList.Add(group);

			return broadcastModelList;
		}

		private async Task<BroadcastModelGroup> getSpotvDataByChannel(DateTime dateTime, string strChannel)
		{
			BroadcastModelGroup group = new BroadcastModelGroup();
			group.Channel = strChannel;
			group.ChannelShow = SpotvService.getChannelToDisplay(strChannel);

			// add yesterday midnight.
			ObservableCollectionCustomized<IBroadcastModel> gotModelList = await getSpotvData(dateTime.AddDays(-1), SpotvService.DAY_PART_NIGHT, strChannel, true);
			group.AddRange(gotModelList);

			gotModelList = await getSpotvData(dateTime, SpotvService.DAY_PART_MORNING, strChannel);
			group.AddRange(gotModelList);
			gotModelList = await getSpotvData(dateTime, SpotvService.DAY_PART_EVENING, strChannel);
			group.AddRange(gotModelList);
			gotModelList = await getSpotvData(dateTime, SpotvService.DAY_PART_NIGHT, strChannel);
			group.AddRange(gotModelList);

			return group;
		}

		private async Task<ObservableCollectionCustomized<IBroadcastModel>> getSpotvData(DateTime dateTime, string strDayPart, string strChannel, bool isYesterday = false)
		{
			string strParams = "?y=" + dateTime.Year + "&m=" + dateTime.Month + "&d=" + dateTime.Day + "&dayPart=" + strDayPart + "&ch=" + strChannel;
			//HttpResponseMessage response = await client.GetAsync("http://www.spotv.net/data/json/schedule/daily.json2.asp?y=2017&m=2&d=14&dayPart=morning&ch=spotv2");

			string result = await getData(SpotvService.URL_SPOTV + SpotvService.URL_SPOTV_DAILY + strParams, dateTime);

			ObservableCollectionCustomized<IBroadcastModel> listSpotv;

			if ( result == "" ) 
			{
				return new ObservableCollectionCustomized<IBroadcastModel>();
			}

			try
			{
				JArray jarrResult = JArray.Parse(result);

				listSpotv = getNBAPrettyData(this, jarrResult, strChannel, strDayPart, isYesterday);
			}
			catch (Exception e)
			{
				listSpotv = new ObservableCollectionCustomized<IBroadcastModel>();
			}

			return listSpotv;
		}

		private async Task<string> getData(string url, DateTime dateTime)
		{
			HttpClient client = new HttpClient();

			client.Timeout = TimeSpan.FromMilliseconds( CALL_TIMEOUT_LIMIT );

			string result = "";

			try
			{
				HttpResponseMessage response = await client.GetAsync(url);

				HttpContent content = response.Content;

				result = await content.ReadAsStringAsync();
				result = WebUtility.HtmlDecode(result);
			}
			catch ( Exception e )
			{
				Debug.WriteLine( "http call timeout!! : " + e );

				HasNetworkProblem = true;

				result = "";
			}

			return result;
		}

		private async Task<BroadcastModelGroup> getSkySportsData( DateTime dateTime )
		{
			string result = await getData( SkySportsService.URL_SCHEDULE, dateTime );

			SkySportsService skySportsService = new SkySportsService();

			return skySportsService.MakeSkySportsData(result, dateTime);
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
				model.DayPart = SpotvService.getDayPartToDisplay( strDayPart, model.ScheduleHour );

				listRet.Add(model);
			}

			return listRet;
		}

		private bool getIsValidNightHour( string strDayPart, bool isYesterday, int nHour )
		{
			if (strDayPart != SpotvService.DAY_PART_NIGHT) return true;

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

		public async Task<string> GetLink( IBroadcastModel model, DateTime dateTimeCurrent )
		{
			NaverService naverService = new NaverService();

			return await naverService.GetGameURL( dateTimeCurrent, model.Title );
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

		public bool IsBusy
		{
			get
			{
				return isBusy;
			}

			set
			{
				isBusy = value;

				OnPropertyChanged("IsBusy");
			}
		}

		public ObservableCollectionCustomized<BroadcastModelGroup> BroadcastModelList
		{
			get
			{
				return broadcastModelList;
			}
			set
			{
				broadcastModelList = value;
			}
		}

	}
}
