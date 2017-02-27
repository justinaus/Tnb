using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Tnb
{
	public interface IHandleViewAppearing
	{
		Task OnViewAppearingAsync(VisualElement view);
	}
}
