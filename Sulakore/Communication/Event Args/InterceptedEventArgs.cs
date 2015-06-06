﻿/* Copyright

    GitHub(Source): https://GitHub.com/ArachisH/Sulakore

    .NET library for creating Habbo Hotel related desktop applications.
    Copyright (C) 2015 Arachis

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
using System.ComponentModel;
using System.Threading.Tasks;

using Sulakore.Habbo.Protocol;

namespace Sulakore.Communication
{
    public class InterceptedEventArgs : CancelEventArgs
    {
        private readonly Func<Task> _continuation;

        public int Step { get; }
        public HMessage Packet { get; set; }
        public bool WasContinued { get; private set; }

        public InterceptedEventArgs(HMessage packet)
            : this(null, -1, packet)
        { }
        public InterceptedEventArgs(int step, HMessage packet)
            : this(null, step, packet)
        { }
        public InterceptedEventArgs(int step, byte[] data, HDestination destination)
            : this(null, step, new HMessage(data, destination))
        { }
        public InterceptedEventArgs(Func<Task> continuation, int step, HMessage packet)
        {
            _continuation = continuation;

            Step = step;
            Packet = packet;
        }
        public InterceptedEventArgs(Func<Task> continuation, int step, byte[] data, HDestination destination)
            : this(continuation, step, new HMessage(data, destination))
        { }

        public void ContinueRead()
        {
            if (WasContinued) return;
            else WasContinued = true;

            if (_continuation == null)
                throw new NullReferenceException($"{nameof(_continuation)}");

            _continuation();
        }
    }
}