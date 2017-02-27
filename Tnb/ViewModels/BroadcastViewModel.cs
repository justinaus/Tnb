using System;
using System.Net;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Xamarin.Forms;
using System.ComponentModel;
using HtmlAgilityPack;
using System.Xml.Linq;
using System.Xml;

namespace Tnb
{
	public class BroadcastViewModel : BaseViewModel, IHandleViewAppearing
	{

		private DateTime DateTimeCurrent;
		private DateTime DateTimeToday;

		public ObservableCollectionCustomized<BroadcastModelGroup> broadcastModelList = new ObservableCollectionCustomized<BroadcastModelGroup>();

		private NaverDataManager naverDataManager;


		public BroadcastViewModel()
		{
			
		}


		public async Task OnViewAppearingAsync(VisualElement view)
		{
			//2017-02-12 오후 5:30:13
			DateTime DateTimeNow = DateTime.Now;

			if ( !getIsSameDate( DateTimeToday, DateTimeNow))
			{
				// day changed!
				DateTimeToday = DateTimeNow;

				DateTimeCurrent = DateTimeToday;

				await onChangedCurrentDate();
			}
			else {
				Debug.WriteLine( "same date!!!!!!" );
			}
		}


		private async Task<DateTime> onChangedCurrentDate()
		{
			OnPropertyChanged("CurrentDate");

			broadcastModelList.Clear();

			//await getDataTest( "http://m.sports.naver.com/basketball/schedule/index.nhn?category=nba&date=20170224" );
			await getSpotvDataAll(DateTimeCurrent);

			return DateTimeCurrent;
		}


		public async Task<string> GetLink( IBroadcastModel model )
		{
			if (naverDataManager == null)
			{
				naverDataManager = new NaverDataManager();
			}

			return await naverDataManager.GetGameURL( DateTimeCurrent, model.Title );
		}


		private async Task<DateTime> changeDate(DateTime dt)
		{
			DateTimeCurrent = dt;

			await onChangedCurrentDate();

			return DateTimeCurrent;
		}


		public async Task<DateTime> changeToday()
		{
			return await changeDate( DateTimeToday );
		}


		public async Task<DateTime> changeDate( bool bNext )
		{
			Debug.WriteLine( "click change" );

			if (bNext)
			{
				DateTimeCurrent = DateTimeCurrent.AddDays( 1 );
			}
			else {
				DateTimeCurrent = DateTimeCurrent.AddDays( -1 );
			}

			await onChangedCurrentDate();

			return DateTimeCurrent;
		}



		private async Task<ObservableCollectionCustomized<BroadcastModelGroup>> getSpotvDataAll(DateTime dateTime) 
		{
			//broadcastModelList.Clear();

			BroadcastModelGroup group = await getSpotvDataByChannel( dateTime, ChannelStruct.SPOTV_ONE );
			if ( group.Count > 0 )	broadcastModelList.Add(group);

			group = await getSpotvDataByChannel(dateTime, ChannelStruct.SPOTV_TWO );
			if (group.Count > 0) broadcastModelList.Add(group);

			group = await getSpotvDataByChannel(dateTime, ChannelStruct.SPOTV_PLUS );
			if (group.Count > 0) broadcastModelList.Add(group);

			return broadcastModelList;
		}


		private async Task<BroadcastModelGroup> getSpotvDataByChannel( DateTime dateTime, string strChannel )
		{
			BroadcastModelGroup group = new BroadcastModelGroup();
			group.Channel = strChannel;
			group.ChannelShow = getChannelShow(strChannel);

			ObservableCollectionCustomized<IBroadcastModel> gotModelList = await getSpotvData( DateTimeCurrent, SpotvURLStruct.DAY_PART_MORNING, strChannel );
			group.AddRange( gotModelList );
			gotModelList = await getSpotvData(DateTimeCurrent, SpotvURLStruct.DAY_PART_EVENING, strChannel);
			group.AddRange(gotModelList);
			gotModelList = await getSpotvData(DateTimeCurrent, SpotvURLStruct.DAY_PART_NIGHT, strChannel);
			group.AddRange(gotModelList);

			//group.Add(new SpotvModel( "hello world", "holly", "wood" ) );

			return group;
		}



		private async Task<ObservableCollectionCustomized<IBroadcastModel>> getSpotvData(DateTime dateTime, string strDayPart, string strChannel)
		{
			HttpClient client = new HttpClient();

			string strParams = "?y=" + dateTime.Year + "&m=" + dateTime.Month + "&d=" + dateTime.Day + "&dayPart=" + strDayPart + "&ch=" + strChannel;
			//HttpResponseMessage response = await client.GetAsync("http://www.spotv.net/data/json/schedule/daily.json2.asp?y=2017&m=2&d=14&dayPart=morning&ch=spotv2");
			HttpResponseMessage response = await client.GetAsync(SpotvURLStruct.URL_SPOTV + SpotvURLStruct.URL_SPOTV_DAILY + strParams);

			HttpContent content = response.Content;

			string result = await content.ReadAsStringAsync();
			result = WebUtility.HtmlDecode( result );

			JArray jarrResult = JArray.Parse(result);

			ObservableCollectionCustomized<IBroadcastModel> listSpotv = getNBAPrettyData(jarrResult, strChannel);

			//broadcastModelList.AddRange( listSpotv );

			return listSpotv;
		}


		private ObservableCollectionCustomized<IBroadcastModel> getNBAPrettyData( JArray jarrRaw, string strChannel )
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

			for (int i = 0; i < jarrRaw.Count; ++i)
			{
				jobjRaw = jarrRaw[ i ] as JObject;

				title = (string)(jobjRaw["title"]);

				if (!title.Contains("NBA") && !title.Contains("nba")) continue;

				model = new SpotvModel();

				model.Kind = (string)( jobjRaw["kind"] );
				model.ScheduleDate = (string)(jobjRaw["sch_date"]);
				model.ScheduleHour = (string)(jobjRaw["sch_hour"]);
				model.ScheduleMinute = (string)(jobjRaw["sch_min"]);
				model.Title = title;
				//model.ScheduleDateTime = dateTime;

				model.Channel = strChannel;

				listRet.Add( model );
			}

			return listRet;
		}


		private string getChannelShow( string strChannel )
		{
			string strRet = "";

			switch ( strChannel )
			{
				case ChannelStruct.SPOTV_ONE:
					strRet = ChannelStruct.SPOTV_ONE_SHOW;
					break;
				case ChannelStruct.SPOTV_TWO:
					strRet = ChannelStruct.SPOTV_TWO_SHOW;
					break;
				case ChannelStruct.SPOTV_PLUS:
					strRet = ChannelStruct.SPOTV_PLUS_SHOW;
					break;
			}

			return strRet;
		}



		private bool getIsSameDate(DateTime dateTimeTarget0, DateTime dateTimeTarget1)
		{
			return dateTimeTarget0.Date == dateTimeTarget1.Date && dateTimeTarget0.Month == dateTimeTarget1.Month;
		}




		public string CurrentDate
		{
			get
			{
				return DateTimeCurrent.Month + " / " + DateTimeCurrent.Day + " (" + DateUtil.getDayOfWeekForKorean(DateTimeCurrent.DayOfWeek) + ")";
			}
		}


	}
}
