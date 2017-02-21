using System;
using System.Net;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Tnb
{
	public class BroadcastViewModel : BaseViewModel, IHandleViewAppearing
	{

		private DateTime dateTimeCurrent;

		public ObservableCollectionCustomized<BroadcastModelGroup> broadcastModelList = new ObservableCollectionCustomized<BroadcastModelGroup>();



		public BroadcastViewModel()
		{
			
		}


		public void OnViewAppearing()
		{
			//2017-02-12 오후 5:30:13
			DateTime dateTimeNow = DateTime.Now;

			if ( !getIsSameDate(dateTimeNow))
			{
				DateTimeCurrent = DateTime.Now;
			}
			else {
				Debug.WriteLine( "same date!!!!!!" );
			}
		}


		private async Task onChangedCurrentDate(DateTime dateTime)
		{
			await getSpotvDataAll( DateTimeCurrent );
		}


		private async Task<ObservableCollectionCustomized<BroadcastModelGroup>> getSpotvDataAll(DateTime dateTime) 
		{
			//broadcastModelList.Clear();

			BroadcastModelGroup group = await getSpotvDataByChannel( dateTime, Spotv1Type.CHANNEL );
			if ( group.Count > 0 )	broadcastModelList.Add(group);

			group = await getSpotvDataByChannel(dateTime, Spotv2Type.CHANNEL);
			if (group.Count > 0) broadcastModelList.Add(group);

			group = await getSpotvDataByChannel(dateTime, SpotvPlusType.CHANNEL);
			if (group.Count > 0) broadcastModelList.Add(group);

			return broadcastModelList;
		}


		private async Task<BroadcastModelGroup> getSpotvDataByChannel( DateTime dateTime, string strChannel )
		{
			BroadcastModelGroup group = new BroadcastModelGroup();
			group.Channel = strChannel;

			ObservableCollectionCustomized<IBroadcastModel> gotModelList = await getSpotvData( DateTimeCurrent, SpotvType.DAY_PART_MORNING, strChannel );
			group.AddRange( gotModelList );
			gotModelList = await getSpotvData(DateTimeCurrent, SpotvType.DAY_PART_EVENING, strChannel);
			group.AddRange(gotModelList);
			gotModelList = await getSpotvData(DateTimeCurrent, SpotvType.DAY_PART_NIGHT, strChannel);
			group.AddRange(gotModelList);

			group.Add(new SpotvModel( "hello world", "09", "30" ) );

			return group;
		}



		private async Task<ObservableCollectionCustomized<IBroadcastModel>> getSpotvData(DateTime dateTime, string strDayPart, string strChannel)
		{
			HttpClient client = new HttpClient();

			string strParams = "?y=" + dateTime.Year + "&m=" + dateTime.Month + "&d=" + dateTime.Day + "&dayPart=" + strDayPart + "&ch=" + strChannel;
			//HttpResponseMessage response = await client.GetAsync("http://www.spotv.net/data/json/schedule/daily.json2.asp?y=2017&m=2&d=14&dayPart=morning&ch=spotv2");
			HttpResponseMessage response = await client.GetAsync(SpotvType.URL_SPOTV + SpotvType.URL_SPOTV_DAILY + strParams);

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



		private bool getIsSameDate(DateTime dateTimeTarget)
		{
			return dateTimeTarget.Date == DateTimeCurrent.Date && dateTimeTarget.Month == DateTimeCurrent.Month;
		}


		private DateTime DateTimeCurrent
		{
			get
			{
				return dateTimeCurrent;
			}
			set
			{
				dateTimeCurrent = value;

				onChangedCurrentDate(value);
			}
		}




		public void OnViewDisappearing()
		{

		}

	}
}
