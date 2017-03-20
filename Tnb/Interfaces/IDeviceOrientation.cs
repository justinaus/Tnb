using System;
namespace Tnb
{

	public enum DeviceOrientations
	{
		Undefined,
		Landscape,
		Portrait
	}


	public interface IDeviceOrientation
	{
		DeviceOrientations GetOrientation();

		void SetOrientation( bool isPortrait );
	}


}
