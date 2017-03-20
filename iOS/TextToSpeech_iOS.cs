using System;
using System.Diagnostics;
using Tnb.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(TextToSpeech_iOS))]

namespace Tnb.iOS
{
	public class TextToSpeech_iOS : ITextToSpeech
	{
		public TextToSpeech_iOS()
		{
		}

		public void Speak(string text)
		{
			Debug.WriteLine( "speak ios" );
		}
	}
}
