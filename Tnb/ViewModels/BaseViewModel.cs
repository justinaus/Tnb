using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;




namespace Tnb
{
	public class BaseViewModel : INotifyPropertyChanged
	{

		public event PropertyChangedEventHandler PropertyChanged;


		public BaseViewModel()
		{
			
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}


	}
}
