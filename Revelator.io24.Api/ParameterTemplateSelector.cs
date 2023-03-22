using Presonus.UCNet.Api.Models;

using System.Windows;
using System.Windows.Controls;

namespace Presonus.UCNet.Api
{
	public class ParameterTemplateSelector : DataTemplateSelector
	{
		public DataTemplate StringTemplate { get; set; }
		public DataTemplate FloatTemplate { get; set; }
		public DataTemplate StringArrayTemplate { get; set; }

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			var parameter = item as Parameter;
			if (parameter != null)
			{
				if (parameter.Type == typeof(string))
					return StringTemplate;
				else if (parameter.Type == typeof(float))
					return FloatTemplate;
				else if (parameter.Type == typeof(string[]))
					return StringArrayTemplate;
			}
			return base.SelectTemplate(item, container);
		}
	}

}
