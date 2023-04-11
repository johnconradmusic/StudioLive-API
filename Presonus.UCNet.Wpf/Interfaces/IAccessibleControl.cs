using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Wpf.Interfaces
{
	public interface IAccessibleControl
	{
		string Caption { get; set; }
		string ValueString { get; }

		event EventHandler ValueChanged;
	}


}
