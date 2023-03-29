using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Helpers
{
	public class PeakValueTracker
	{
		private Queue<float> buffer;
		private float peakValue;
		private float decayFactor;
		private int bufferSize;
		public PeakValueTracker(int bufferSize = 10, float decayFactor = 0.99f)
		{
			buffer = new Queue<float>(bufferSize);
			this.bufferSize = bufferSize;
			this.decayFactor = decayFactor;
		}

		public void AddValue(float value)
		{
			buffer.Enqueue(value);
			if (buffer.Count > bufferSize)
			{
				buffer.Dequeue();
			}
		}

		public float GetPeakValue()
		{
			if (buffer.Count == 0)
			{
				peakValue = 0;
			}
			else
			{
				float highestInBuffer = buffer.Max();
				peakValue = Math.Max(highestInBuffer, peakValue * decayFactor);
			}
			return peakValue;
		}
	}


}
