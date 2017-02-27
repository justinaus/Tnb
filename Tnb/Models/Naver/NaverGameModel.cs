using System;
namespace Tnb
{
	public class NaverGameModel
	{

		//<li >
		//		<a href = '/basketball/gamecenter/nba/index.nhn?tab=record&gameId=2017022319' onClick="nclk(this, 'sch.match', '', '');">
		//		<div class="dt_info">
		//		   <span>
		//			   09:00
		//		   </span>
		//		</div>
		//		<div class="gm_info">
		//			<span class="gm_stat over">종료</span>
		//					<div class="tm_info1">
		//						<span class="tm">올랜도</span>
		//							<span class="sc">103</span>
		//					</div>
		//					<div class="tm_info2">
		//						<span class="tm">포틀랜드</span>
		//							<span class="sc">112</span>
		//					</div>
		//		</div>
		//			<div class="bt_area">
		//				<span class="sch_btn rec">기록</span>
		//			</div>
		//		</a>
		//	</li>

		public string Link { get; set; }

		/// <summary>
		/// 이런 저런 파싱하기 귀찮아서 그냥 문자열 다 갖고 있자
		/// 그리고 나중에 문자열 중 검색
		/// </summary>
		/// <value>The values.</value>
		public string Values { get; set; }

		//public string Team1 { get; set; }
		//public string Team2 { get; set; }


		public NaverGameModel()
		{
		}
	}
}
