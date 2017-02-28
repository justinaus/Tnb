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
using System.Linq;

namespace Tnb
{
	public class BroadcastViewModel : BaseViewModel, IHandleViewAppearing
	{

		private BroadcastPage _view;
		private BroadcastHeaderView _broadcastHeaderView;

		public ObservableCollectionCustomized<BroadcastModelGroup> broadcastModelList = new ObservableCollectionCustomized<BroadcastModelGroup>();

		private NaverDataManager naverDataManager;


		public BroadcastViewModel( BroadcastPage view, BroadcastHeaderView headerView )
		{
			_view = view;
			_broadcastHeaderView = headerView;

			_broadcastHeaderView.DateChanged += onDateChanged;
		}


		public void OnViewAppearing(VisualElement view)
		{
			_broadcastHeaderView.SetToday();
		}






		private async Task getSkySportsData( DateTime dateTime )
		{
			HttpClient client = new HttpClient();

			HttpResponseMessage response = await client.GetAsync("https://tv.skylife.co.kr/skysports/timetable/by/channel");

			HttpContent content = response.Content;

			string result = await content.ReadAsStringAsync();
			result = WebUtility.HtmlDecode(result);

			BroadcastViewModel.MakeData(result, dateTime);
		}


		private static void MakeData(string strHTML, DateTime dateTime)
		{
			int startIdx = strHTML.IndexOf("<html", System.StringComparison.Ordinal);
			if (startIdx > 0) strHTML = strHTML.Substring(startIdx);

			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(strHTML);

			SkySportsModel model;

			int nDayCurrent = (int)dateTime.DayOfWeek;

			Debug.WriteLine( "TTTTTT" + nDayCurrent );

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

					if (nDay != nDayCurrent) continue;

					if (element2.OuterHtml.IndexOf("NBA", StringComparison.Ordinal) == -1) continue;

					foreach (HtmlNode element3 in element2.Descendants("p"))
					{
						if (element3.OuterHtml.IndexOf("NBA", StringComparison.Ordinal) == -1) continue;

						model = new SkySportsModel();

						model.Time = element2.Descendants("p").Where(x => x.GetAttributeValue("class", "") == "dateTxt mb10").First().InnerText;
						model.Channel = ChannelStruct.SKY_SPORTS;
						model.Kind = SpotvBroadcastKindStruct.RERUN;
						model.Title = element2.Descendants("span").First().InnerText;

						Debug.WriteLine( model.Time + "///" + model.Title);
					}





					//new HtmlAttribute().

					//doc.DocumentNode.Descendants("tr").Where(x => x.ParentNode.Name == "tbody");


					//Debug.WriteLine( element2.Descendants( "span" ).First().OuterHtml );

					//model.Link = element2.Attributes( "class" );

					//model.Values = StringUtil.Trim(element2.Value);

					//dailyList.Add(model);
				}



					//Debug.WriteLine("%%" + nCount);
			}

		}


		private async void onDateChanged( object sender, DateChangedEventArgs e )
		{
			_broadcastHeaderView.IsEnabled = false;
			_view.ShowActivityIndicator( true );

			broadcastModelList.Clear();

			//await getDataTest( "http://m.sports.naver.com/basketball/schedule/index.nhn?category=nba&date=20170224" );
			await getSpotvDataAll( e.DateTimeTarget );

			_view.ShowActivityIndicator(false);
			_broadcastHeaderView.IsEnabled = true;

			// testtest!!!
			await getSkySportsData( e.DateTimeTarget );
		}


		private async Task<ObservableCollectionCustomized<BroadcastModelGroup>> getSpotvDataAll(DateTime dateTime)
		{
			BroadcastModelGroup group = await getSpotvDataByChannel(dateTime, ChannelStruct.SPOTV_ONE);
			if (group.Count > 0) broadcastModelList.Add(group);

			group = await getSpotvDataByChannel(dateTime, ChannelStruct.SPOTV_TWO);
			if (group.Count > 0) broadcastModelList.Add(group);

			group = await getSpotvDataByChannel(dateTime, ChannelStruct.SPOTV_PLUS);
			if (group.Count > 0) broadcastModelList.Add(group);

			return broadcastModelList;
		}


		private async Task<BroadcastModelGroup> getSpotvDataByChannel(DateTime dateTime, string strChannel)
		{
			BroadcastModelGroup group = new BroadcastModelGroup();
			group.Channel = strChannel;
			group.ChannelShow = getChannelShow(strChannel);

			ObservableCollectionCustomized<IBroadcastModel> gotModelList = await getSpotvData(dateTime, SpotvURLStruct.DAY_PART_MORNING, strChannel);
			group.AddRange(gotModelList);
			gotModelList = await getSpotvData(dateTime, SpotvURLStruct.DAY_PART_EVENING, strChannel);
			group.AddRange(gotModelList);
			gotModelList = await getSpotvData(dateTime, SpotvURLStruct.DAY_PART_NIGHT, strChannel);
			group.AddRange(gotModelList);

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
			result = WebUtility.HtmlDecode(result);

			JArray jarrResult = JArray.Parse(result);

			ObservableCollectionCustomized<IBroadcastModel> listSpotv = getNBAPrettyData(jarrResult, strChannel);

			return listSpotv;
		}


		private ObservableCollectionCustomized<IBroadcastModel> getNBAPrettyData(JArray jarrRaw, string strChannel)
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
				jobjRaw = jarrRaw[i] as JObject;

				title = (string)(jobjRaw["title"]);

				if (!title.Contains("NBA") && !title.Contains("nba")) continue;

				model = new SpotvModel();

				model.Kind = (string)(jobjRaw["kind"]);
				model.ScheduleDate = (string)(jobjRaw["sch_date"]);
				model.ScheduleHour = (string)(jobjRaw["sch_hour"]);
				model.ScheduleMinute = (string)(jobjRaw["sch_min"]);
				model.Title = title;
				//model.ScheduleDateTime = dateTime;

				model.Channel = strChannel;

				listRet.Add(model);
			}

			return listRet;
		}


		public async Task<string> GetLink( IBroadcastModel model )
		{
			if (naverDataManager == null)
			{
				naverDataManager = new NaverDataManager();
			}

			return await naverDataManager.GetGameURL( _broadcastHeaderView.DateTimeCurrent, model.Title );
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


	}
}
