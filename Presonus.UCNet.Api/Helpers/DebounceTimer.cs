using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Presonus.UCNet.Api.Helpers
{
	public class DebounceTimer
	{
		private readonly Thread _thread;
		private readonly Stopwatch _stopwatch = new Stopwatch();
		private readonly int _interval;
		private readonly Action _action;
		private bool _isRunning;

		public DebounceTimer(int interval, Action action)
		{
			_interval = interval;
			_action = action;
			_thread = new Thread(() =>
			{
				while (_isRunning)
				{
					_stopwatch.Restart();
					Thread.Sleep(_interval);
					_stopwatch.Stop();
					if (_stopwatch.ElapsedMilliseconds >= _interval)
					{
						_action.Invoke();
					}
				}
			});
		}

		public void Start()
		{
			if (_isRunning) return;

			_isRunning = true;
			_thread.Start();
		}

		public void Stop()
		{
			_isRunning = false;

			if (_thread != null)
			{
				_thread.Join();
			}
		}

		public bool IsRunning
		{
			get { return _isRunning; }
		}
	}


}