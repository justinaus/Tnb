using System;
namespace Tnb
{
	public interface ITextToSpeech
	{
		void Speak(string text); //note that interface members are public by default
	}
}
