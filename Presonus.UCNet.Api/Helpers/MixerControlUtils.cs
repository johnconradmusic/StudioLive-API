
using System;
using System.Collections.Generic;
using System.Linq;


namespace Presonus.UCNet.Api.Helpers
{
	public static class MixerControlUtils
	{
		public delegate void CancelTransitionFn();

		public static int LogVolumeToLinear(double db)
		{
			int CurveFunction(double x) => (int)Math.Truncate(
				72.5204177782 + 2.4734739920 * x + 0.0265675570 * Math.Pow(x, 2) + 0.0000880866 * Math.Pow(x, 3));

			var inputBounds = (-84, 10);
			var outputBounds = (0, 100);

			db = Clamp(db, inputBounds.Item1, inputBounds.Item2);

			if (db == inputBounds.Item1) return outputBounds.Item1;
			if (db == inputBounds.Item2) return outputBounds.Item2;
			var result = (int)Clamp(CurveFunction(db), outputBounds.Item1, outputBounds.Item2);

			return result;
		}

		public static double Clamp(double val, double min, double max)
		{
			return Math.Max(min, Math.Min(max, val));
		}

		public static CancelTransitionFn TransitionValue(double from, double to, int duration, Action<double> fn, Action callback = null)
		{
			if (duration <= 0 || from == to)
			{
				fn(to);
				callback?.Invoke();
				return () => { };
			}

			const int minInterval = 10;

			double CurveFunction(double position)
			{
				return -(Math.Cos(Math.PI * position) - 1) / 2;
			}

			int interval = Math.Max(duration / 100, minInterval);

			var bounds = (0.0, 1.0);
			double step = Clamp((double)interval / duration, bounds.Item1, bounds.Item2);

			double progress = 0;

			void Tick()
			{
				fn(from + (to - from) * CurveFunction(progress));

				if (progress == bounds.Item2)
				{
					CancelTransition();
					callback?.Invoke();
				}
				else
				{
					progress = Clamp(progress + step, bounds.Item1, bounds.Item2);
				}
			}

			var timer = new System.Timers.Timer(interval);
			timer.Elapsed += (sender, e) => Tick();
			timer.Start();
			Tick();

			void CancelTransition()
			{
				timer.Stop();
			}

			return CancelTransition;
		}
	}
	public class ChunkSet
	{
		public int max;
		public byte[] data;
	}
	public class UniqueRandom
	{
		private static readonly Dictionary<int, UniqueRandom> instances = new Dictionary<int, UniqueRandom>();

		
		public static UniqueRandom Get(int bits)
		{
			if (!instances.ContainsKey(bits)) instances[bits] = new UniqueRandom(bits);
			return instances[bits];
		}

		private readonly int max;
		private readonly List<int> active;

		private UniqueRandom(int bits)
		{
			max = (int)Math.Pow(2, bits) - 1;
			active = new List<int>();
		}

		public int Request()
		{
			int current;
			while (active.Contains((current = new Random().Next(max + 1)))) continue;
			active.Add(current);
			return current;
		}

		public void Release(int value)
		{
			active.Remove(value);
		}

		public List<int> Active
		{
			get { return new List<int>(active); }
		}
	}
}
