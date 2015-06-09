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
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

using Sulakore.Habbo.Protocol;

namespace Sulakore.Components
{
    [DesignerCategory("Code")]
    public class SKoreScheduler : SKoreListView
    {
        public event EventHandler<HScheduleTriggeredEventArgs> ScheduleTriggered;

        private readonly Dictionary<HSchedule, ListViewItem> _items;
        private readonly Dictionary<ListViewItem, HSchedule> _schedules;
        private readonly Dictionary<ListViewItem, string> _descriptions;

        private const string RUNNING = "Running", STOPPED = "Stopped";

        [Browsable(false)]
        [DefaultValue(true)]
        public bool AutoStart { get; set; }

        public int Running => _items.Keys.Count(x => x.IsRunning);

        public SKoreScheduler()
        {
            _items = new Dictionary<HSchedule, ListViewItem>();
            _descriptions = new Dictionary<ListViewItem, string>();
            _schedules = new Dictionary<ListViewItem, HSchedule>();

            AutoStart = true;
            CheckBoxes = true;
        }

        public void ClearItems()
        {
            foreach (ListViewItem item in Items)
                RemoveItem(item);
        }
        protected override void RemoveItem(ListViewItem listViewItem)
        {
            if (_schedules.ContainsKey(listViewItem))
            {
                HSchedule schedule = _schedules[listViewItem];
                _items.Remove(schedule);

                schedule.Dispose();
                _schedules.Remove(listViewItem);

                _descriptions.Remove(listViewItem);
            }

            base.RemoveItem(listViewItem);
        }
        public void AddSchedule(HMessage packet, int burst, int interval, string description)
        {
            if (packet.IsCorrupted)
                throw new Exception("Provided packet is either corrupted, or has Destination set to 'Unknown'.");

            var item = new ListViewItem(new[] { packet.ToString(),
                packet.Destination.ToString(), burst.ToString(), interval.ToString(), AutoStart ? RUNNING : STOPPED });

            var schedule = new HSchedule(packet, interval, burst);
            schedule.ScheduleTriggered += OnScheduleTriggered;

            _items.Add(schedule, item);
            _schedules.Add(item, schedule);
            _descriptions.Add(item, description);

            item.Checked = AutoStart;
            item.ToolTipText = description;

            FocusAdd(item);
        }

        public void StopAllSchedules()
        {
            foreach (ListViewItem item in _schedules.Keys)
                item.Checked = false;
        }
        public void StartAllSchedules()
        {
            foreach (ListViewItem item in _schedules.Keys)
                item.Checked = true;
        }

        public int GetItemBurst() => SelectedItems.Count > 0 ?
                _schedules[SelectedItems[0]].Burst : 0;
        public void SetItemBurst(int burst)
        {
            if (SelectedItems.Count < 1) return;

            ListViewItem item = SelectedItems[0];
            _schedules[item].Burst = burst;
            item.SubItems[2].Text = burst.ToString();
        }

        public int GetItemInterval() => SelectedItems.Count > 0 ?
                _schedules[SelectedItems[0]].Interval : 0;
        public void SetItemInterval(int interval)
        {
            if (SelectedItems.Count < 1) return;

            ListViewItem item = SelectedItems[0];
            _schedules[item].Interval = interval;
            item.SubItems[2].Text = interval.ToString();
        }

        public string GetItemDescription() => SelectedItems.Count > 0 ?
                _descriptions[SelectedItems[0]] : string.Empty;
        public void SetItemDescription(string description)
        {
            if (SelectedItems.Count < 1) return;

            ListViewItem item = SelectedItems[0];
            item.ToolTipText = description;
            _descriptions[item] = description;
        }

        public HMessage GetItemPacket() => SelectedItems.Count > 0 ?
                _schedules[SelectedItems[0]].Packet : null;
        public void SetItemPacket(HMessage packet)
        {
            if (SelectedItems.Count < 1) return;

            ListViewItem item = SelectedItems[0];
            _schedules[item].Packet = packet;
            item.SubItems[0].Text = packet.ToString();
            item.SubItems[1].Text = packet.Destination.ToString();
        }

        public HDestination GetItemDestination() => SelectedItems.Count > 0 ?
                _schedules[SelectedItems[0]].Packet.Destination : HDestination.Client;
        public void SetItemDestination(HDestination destination)
        {
            if (SelectedItems.Count < 1) return;

            ListViewItem item = SelectedItems[0];
            _schedules[item].Packet.Destination = destination;
            item.SubItems[1].Text = destination.ToString();
        }

        protected override void OnItemChecked(ItemCheckedEventArgs e)
        {
            if (!_schedules.ContainsKey(e.Item)) return;

            HSchedule schedule = _schedules[e.Item];
            e.Item.SubItems[4].Text = e.Item.Checked ? RUNNING : STOPPED;

            if (e.Item.Checked)
                schedule.Start();
            else if (schedule.IsRunning)
                schedule.Stop();

            base.OnItemChecked(e);
        }
        protected virtual void OnScheduleTriggered(object sender, HScheduleTriggeredEventArgs e)
        {
            EventHandler<HScheduleTriggeredEventArgs> handler = ScheduleTriggered;
            if (handler != null)
            {
                try { handler(sender, e); }
                catch { e.Cancel = true; }
                finally
                {
                    if (e.Cancel)
                    {
                        Invoke(new MethodInvoker(() =>
                        {
                            ListViewItem item = _items[(HSchedule)sender];
                            item.SubItems[4].Text = STOPPED;
                            item.Checked = false;
                        }));
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (HSchedule schedule in _schedules.Values)
                    schedule.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}