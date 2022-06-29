using System;
using System.Collections.Generic;

namespace Presonus.StudioLive32.Api.Models.Monitor
{
    public class ValuesMonitorModel
    {
        public event EventHandler ValuesUpdated;

        public float[] Line { get; set; } = new float[32];


        public void RaiseModelUpdated()
        {
            //Normaly all values gets updated at the same time with this model.
            ValuesUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}
