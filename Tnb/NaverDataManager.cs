using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using HtmlAgilityPack;

namespace Tnb
{
	public class NaverDataManager
	{
		
		public const string URL = "http://m.sports.naver.com";
		//20170223
		public const string URL_SCHEDULE = "http://m.sports.naver.com/basketball/schedule/index.nhn?category=nba&date=";

		List<NaverGameModelDailyList> list;


		public NaverDataManager()
		{
		}


		public async Task<string> GetGameURL( DateTime dateTime, string gameTitle )
		{
			if ( list == null ) list = new List<NaverGameModelDailyList>();

			NaverGameModelDailyList dailyGameList = GetDailyGameListByDate( dateTime );

			if (dailyGameList == null)
			{
				dailyGameList = await GetHttpDailyGameList( dateTime );
				dailyGameList.Month = dateTime.Month;
				dailyGameList.Day = dateTime.Day;

				list.Add( dailyGameList );
			}

			return URL + FindLinkByGameTitle( dailyGameList, gameTitle );
		}



		public NaverGameModelDailyList MakeNaverData(string strXML)
		{
			NaverGameModelDailyList dailyList = new NaverGameModelDailyList();

			int startIdx = strXML.IndexOf("<html", System.StringComparison.Ordinal);
			if (startIdx > 0) strXML = strXML.Substring(startIdx);

			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(strXML);

			System.IO.StringWriter sw = new System.IO.StringWriter();

			XmlWriter xw = XmlWriter.Create(sw);

			doc.Save(xw);
			string result = sw.ToString();

			XDocument xd = XDocument.Parse(result);

			NaverGameModel model;

			foreach (XElement element in xd.Descendants("li"))
			{
				if (!element.Parent.HasAttributes) continue;
				if (element.Parent.Attribute("class") == null) continue;
				string strParentValue = element.Parent.Attribute("class").Value;
				if (strParentValue.IndexOf("sch_lst", StringComparison.Ordinal) == -1) continue;

				model = new NaverGameModel();

				foreach (XElement element2 in element.Descendants("a"))
				{
					///basketball/gamecenter/nba/index.nhn?tab=record&gameId=2017022319
					model.Link = element2.Attribute("href").Value;

					model.Values = StringUtil.Trim(element2.Value);

					dailyList.Add(model);
				}
			}

			return dailyList;
		}


		private async Task<NaverGameModelDailyList> GetHttpDailyGameList( DateTime dateTime )
		{
			HttpClient client = new HttpClient();

			HttpResponseMessage response = await client.GetAsync( URL_SCHEDULE + dateTime.Date.ToString("yyyyMMdd") );

			HttpContent content = response.Content;

			string strHTML = await content.ReadAsStringAsync();
			strHTML = WebUtility.HtmlDecode(strHTML);

			//Debug.WriteLine("response : " + strHTML);

			return MakeNaverData(strHTML);
		}


		/// <summary>
		/// if there is no same game data, return "".
		/// </summary>
		/// <returns>The link by game title.</returns>
		/// <param name="dailyGameList">Daily game list.</param>
		/// <param name="gameTitle">Game title.</param>
		private string FindLinkByGameTitle( NaverGameModelDailyList dailyGameList, string gameTitle )
		{
			NaverGameModel model;

			for (int i = 0; i < dailyGameList.Count; ++i)
			{
				model = dailyGameList[ i ];

				if (getIsSameGame(model, gameTitle)) return model.Link;
			}

			return "";
		}


		private bool getIsSameGame( NaverGameModel model, string gameTitle )
		{
			const char DIV = ':';

			string[] arrTitle = gameTitle.Split( DIV );
			//16-17 NBA LA 클리퍼스:골든스테이트

			string[] teamTemp = arrTitle[0].Split(' ');
			string team1 = teamTemp[ teamTemp.Length - 1 ];

			teamTemp = arrTitle[1].Split(' ');
			string team2 = teamTemp[0];

			Debug.WriteLine( team1 + "/" + team2 );

			if (model.Values.IndexOf(team1, StringComparison.Ordinal) == -1) return false;
			if (model.Values.IndexOf(team2, StringComparison.Ordinal) == -1) return false;

			return true;
		}


		private NaverGameModelDailyList GetDailyGameListByDate( DateTime dateTime )
		{
			NaverGameModelDailyList gameModel;

			for (int i = 0; i < list.Count; ++i)
			{
				gameModel = list[i];

				if (gameModel.Month == dateTime.Month && gameModel.Day == dateTime.Day)
				{
					return gameModel;
				}
			}

			return null;
		}

	}
}
