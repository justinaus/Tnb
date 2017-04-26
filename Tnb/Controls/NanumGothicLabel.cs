using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace Tnb
{

	public enum FontWeightType { Normal=0, Light, Bold };  


	public class NanumGothicLabel : Label
	{

		private FontWeightType fontWeight;


		public NanumGothicLabel()
		{
			FontWeight = FontWeightType.Normal;
		}


		public FontWeightType FontWeight
		{
			get
			{
				return fontWeight;
			}
			set
			{
				fontWeight = value;

				OnChangedFontWeight(value);
			}
		}

		private void OnChangedFontWeight( FontWeightType enumType )
		{
				string strIOS = "NanumGothic";
				string strAnd = "fonts/NanumGothic.ttf#NanumGothic";

				if (enumType == FontWeightType.Bold)
				{
					strIOS = "NanumGothicBold";
					strAnd = "fonts/NanumGothicBold.ttf#NanumGothicBold";
				}
				else if (enumType == FontWeightType.Light)
				{
					strIOS = "NanumGothicLight";
					strAnd = "fonts/NanumGothicLight.ttf#NanumGothicLight";
				}
				else
				{
					
				}

				FontFamily = Device.OnPlatform(
					strIOS,
					strAnd,
					null
				);
		}
	}
}
