using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HtmlAgilityPack;

namespace Tnb
{
	public class SkySportsService
	{
		
		public const string URL_SCHEDULE = "https://tv.skylife.co.kr/skysports/timetable/by/channel";

		public const string CHANNEL = "skysports";


		public SkySportsService()
		{
		}



		public static string getDayPartToDisplay(string strTime)
		{
			const char DIV = ':';

			if (strTime.IndexOf(DIV) == -1) return "";

			string strHour = strTime.Split(DIV)[0];
			int nHour = int.Parse(strHour.Trim());

			string strRet = "";

			if (nHour >= 6 && nHour < 12)
			{
				// 06 ~ 13;
				strRet = DayPartToDisplayStruct.MORNING;
			}
			else if (nHour >= 12 && nHour < 23)
			{
				// 13 ~ 23;
				strRet = DayPartToDisplayStruct.EVENING;
			}
			else
			{
				strRet = DayPartToDisplayStruct.NIGHT;
			}

			return strRet;
		}






		public BroadcastModelGroup MakeSkySportsData(string strHTML, DateTime dateTime)
		{
			BroadcastModelGroup group = new BroadcastModelGroup();
			group.Channel = SkySportsService.CHANNEL;
			group.ChannelShow = group.Channel;

			int startIdx = strHTML.IndexOf("<html", System.StringComparison.Ordinal);
			if (startIdx > 0) strHTML = strHTML.Substring(startIdx);

			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(strHTML);

			List<DateTime> dateTimeSkySports = getDateTimeSkySports(doc);

			if (!getHasDate(dateTime, dateTimeSkySports)) return group;

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

					if (!isYesterday && nDay != nDayCurrent) continue;

					if (element2.OuterHtml.IndexOf("NBA", StringComparison.Ordinal) == -1) continue;

					foreach (HtmlNode element3 in element2.Descendants("p"))
					{
						if (element3.OuterHtml.IndexOf("NBA", StringComparison.Ordinal) == -1) continue;

						model = new SkySportsModel();

						string strTime = element2.Descendants("p").Where(x => x.GetAttributeValue("class", "") == "dateTxt mb10").First().InnerText;

						model.Time = strTime;
						model.Channel = SkySportsService.CHANNEL;
						model.Kind = BroadcastStruct.RERUN;
						model.Title = element2.Descendants("span").First().InnerText;

						model.DayPart = SkySportsService.getDayPartToDisplay(model.Time);

						if (!getIsAlreadyHas(group, model)) group.Add(model);
					}
				}
			}

			Debug.WriteLine(group.Count);

			return group;
		}

		private List<DateTime> getDateTimeSkySports(HtmlDocument doc)
		{
			List<DateTime> dtSkySports = new List<DateTime>();

			DateTime dt;

			foreach (HtmlNode element in doc.DocumentNode.Descendants("th"))
			{
				if (element.ParentNode.Name != "tr") continue;
				if (element.ParentNode.ParentNode.Name != "thead") continue;
				// 24 lines.

				int nSeq = 0;

				foreach (HtmlNode element2 in element.Descendants("p"))
				{
					if (element2.ParentNode != element) continue;

					++nSeq;

					//<p class="mb5">2017년 03월 12일</p>
					//2:::< p class="mb0 fs15 fcolor9">일요일</p>

					if (element2.GetAttributeValue("class", "") != "mb5") continue;

					string strDate = element2.InnerText;

					dt = Convert.ToDateTime(strDate);

					dtSkySports.Add(dt);
				}
			}

			return dtSkySports;
		}

		private bool getIsAlreadyHas(BroadcastModelGroup group, SkySportsModel model)
		{
			SkySportsModel modelAlready;

			for (int i = 0; i < group.Count; ++i)
			{
				modelAlready = (Tnb.SkySportsModel)group[i];

				if (modelAlready.Time == model.Time || modelAlready.Title == model.Title) return true;
			}

			return false;
		}

		private bool getHasDate(DateTime dateTime, List<DateTime> dateTimeSkySports)
		{
			for (int i = 0; i < dateTimeSkySports.Count; ++i)
			{
				if (DateUtil.GetIsSameDate(dateTime, dateTimeSkySports[i])) return true;
			}

			return false;
		}

	}
}
