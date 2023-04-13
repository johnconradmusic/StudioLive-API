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
		private ReductionData _reductionData;

		public MeterDataStorage()
		{

		}
		public void UpdateMeterData(ReductionData reductionData)
		{
			if (_reductionData == null)
			{
				_reductionData = reductionData;
				return;
			}
			UpdateValues(_reductionData.InputGateReduction, reductionData.InputGateReduction);

		}
		public void UpdateMeterData(MeterData newData)
		{
			if (_meterData == null)
			{
				_meterData = newData;
				return;
			}
			UpdateValues(_meterData.Input, newData.Input);
			UpdateValues(_meterData.AuxMetering, newData.AuxMetering);

			// Update other meter data arrays as needed.

			// Update ChannelStrip data
			foreach (var stripName in newData.ChannelStrip.Keys)
			{
				if (!_meterData.ChannelStrip.ContainsKey(stripName))
				{
					_meterData.ChannelStrip[stripName] = new float[newData.ChannelStrip[stripName].Length];
				}
				UpdateValues(_meterData.ChannelStrip[stripName], newData.ChannelStrip[stripName]);
			}
		}
		private void UpdateValues(float[] values, float[] newData, float decayFactor = 0.8f)
		{
			if (newData == null) return;
			int count = newData.Length;

			// Check if it's the first time updating or if the lengths don't match.
			if (values == null || values.Length != count)
			{
				values = newData;
				return;
			}

			for (int i = 0; i < count; i++)
			{
				float newVal = newData[i];

				// If the new value is greater than the previous value, use the new value.
				// Otherwise, apply the decay factor.
				if (newVal > values[i])
				{
					values[i] = newVal;
				}
				else
				{
					values[i] = values[i] * decayFactor + newVal * (1 - decayFactor);
				}
			}
		}

		public float[] GetInputGateReduction()
		{
			return _reductionData?.InputGateReduction;
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
