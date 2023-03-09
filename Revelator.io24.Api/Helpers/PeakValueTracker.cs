using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Helpers
{
	public class PeakValueTracker
	{
		private double[] buffer;
		private int bufferIndex;
		private int bufferLength;
		private double maxRms;

		public PeakValueTracker(int bufferSize)
		{
			buffer = new double[bufferSize];
			bufferIndex = 0;
			bufferLength = bufferSize;
			maxRms = 0;
		}

		public void AddSample(double sample)
		{
			buffer[bufferIndex] = sample;
			bufferIndex = (bufferIndex + 1) % bufferLength;

			// Calculate the RMS value of the buffer
			double sumOfSquares = 0;
			for (int i = 0; i < bufferLength; i++)
			{
				sumOfSquares += buffer[i] * buffer[i];
			}
			double meanOfSquares = sumOfSquares / bufferLength;
			double rms = Math.Sqrt(meanOfSquares);

			// Update the maximum RMS value if necessary
			if (rms > maxRms)
			{
				maxRms = rms;
			}
		}

		public double GetPeakValue()
		{
			return maxRms;
		}
	}

}
