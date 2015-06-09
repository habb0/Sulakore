/* Copyright

    GitHub(Source): https://GitHub.com/ArachisH/Sulakore

    .NET library for creating Habbo Hotel related desktop applications.
    Copyright (C) 2015 ArachisH

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along
    with this program; if not, write to the Free Software Foundation, Inc.,
    51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.

    See License.txt in the project root for license information.
*/

using System;
using System.Timers;

using Sulakore.Habbo.Protocol;

namespace Sulakore.Components
{
    public class HSchedule : IDisposable
    {
        public event EventHandler<HScheduleTriggeredEventArgs> ScheduleTriggered;
        protected virtual void OnScheduleTriggered(HScheduleTriggeredEventArgs e)
        {
            EventHandler<HScheduleTriggeredEventArgs> handler = ScheduleTriggered;
            if (handler != null) handler(this, e);
            if (e.Cancel) IsRunning = false;
        }

        private readonly object _tickerLock;
        private readonly System.Timers.Timer _ticker;

        public int Interval
        {
            get { return (int)_ticker.Interval; }
            set { _ticker.Interval = value; }
        }
        public int Burst { get; set; }
        public HMessage Packet { get; set; }
        public bool IsRunning { get; private set; }

        public HSchedule(HMessage packet, int interval, int burst)
        {
            _ticker = new System.Timers.Timer(interval);
            _ticker.Elapsed += Ticker_Elapsed;

            _tickerLock = new object();

            Packet = packet;
            Burst = burst;
        }

        public void Stop()
        {
            if (!IsRunning) return;

            _ticker.Stop();
            IsRunning = false;
        }
        public void Start()
        {
            if (IsRunning) return;

            _ticker.Start();
            IsRunning = true;
        }
        public void Toggle()
        {
            if (IsRunning) Stop();
            else Start();
        }

        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            ScheduleTriggered = null;

            if (disposing)
            {
                Stop();
                _ticker.Dispose();
            }
        }

        private void Ticker_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (_tickerLock)
            {
                _ticker.Stop();
                int tmpBurst = Burst, burstCount;
                for (int i = 0; i < tmpBurst && IsRunning; i++)
                {
                    burstCount = i + 1;

                    OnScheduleTriggered(new HScheduleTriggeredEventArgs(Packet,
                        burstCount, tmpBurst - burstCount, burstCount >= tmpBurst));
                }
                if (IsRunning) _ticker.Start();
            }
        }
    }
}