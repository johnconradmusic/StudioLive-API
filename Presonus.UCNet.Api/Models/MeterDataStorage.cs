using Presonus.UCNet.Api.NewDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Models
{
	public class MeterDataStorage
	{
		private MeterData _meterData;

		public MeterDataStorage()
		{
			_meterData = new MeterData();
		}

		public void UpdateMeterData(MeterData newData)
		{
			if (newData == null)
			{
				throw new ArgumentNullException(nameof(newData));
			}

			// Update the _meterData object with the new data.
			_meterData = newData;
		}

		public MeterData GetMeterData()
		{
			// Return the current _meterData object.
			return _meterData;
		}

		public float[] GetInputData()
		{
			return _meterData?.Input;
		}

		public Dictionary<string, float[]> GetChannelStripData()
		{
			return _meterData?.ChannelStrip;
		}

		public float[] GetAuxMeteringData()
		{
			return _meterData?.AuxMetering;
		}

		public Dictionary<string, float[]> GetAuxChStripData()
		{
			return _meterData?.AuxChStrip;
		}

		public float[] GetMainMixFadersData()
		{
			return _meterData?.MainMixFaders;
		}

		public Dictionary<string, float[]> GetMainChStripData()
		{
			return _meterData?.MainChStrip;
		}

		public float[] GetMainData()
		{
			return _meterData?.Main;
		}

		public Dictionary<string, float[]> GetFxChStripData()
		{
			return _meterData?.FxChStrip;
		}

		public Dictionary<string, float[]> GetFxReturnStripData()
		{
			return _meterData?.FxReturnStrip;
		}
	}



}
