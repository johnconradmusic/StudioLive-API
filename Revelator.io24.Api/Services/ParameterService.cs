using Presonus.StudioLive32.Api;
using Presonus.UC.Api.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Services
{
	public class ParameterService
	{
		RawService rawService;

		public ParameterService(RawService rawService)
		{
			this.rawService = rawService;
		}



		public void SetValue(Param param, object value)
		{
			string route = ""; //get route from param structure? 
							   //how will we know which object it on?
							   //will each param be an instance and thereby have a complete path based on channel num, etc..?

			rawService.SetValue(route, (float)value);
		}

		public static void GetParamPath(Param param)
		{

			var fullPath = GetFullPath(() => param);
			
		}
		public static string GetFullPath<T>(Expression<Func<T>> action)
		{
			return action.Body.ToString();
		}

	}


}
