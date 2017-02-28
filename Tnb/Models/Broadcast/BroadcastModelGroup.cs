using System;
using System.Collections.Generic;

namespace Tnb
{
	public class BroadcastModelGroup : ObservableCollectionCustomized<IBroadcastModel>
	{

		public string Channel { get; set; }
		public string ChannelShow { get; set; }
		public string ShortName { get; set; } //will be used for jump lists


		public BroadcastModelGroup( string channel = "" )
		{
			if (channel != "") Channel = channel;
		}

	}
}
