using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Tnb
{
	public interface IHandleViewAppearing
	{
		void OnViewAppearing(VisualElement view);
	}
}
