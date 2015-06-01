/* Copyright

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

using Sulakore.Habbo;
using Sulakore.Habbo.Protocol;

namespace Sulakore.Communication
{
    public class HostShoutEventArgs : EventArgs, IHabboEvent
    {
        public ushort Header { get; }
        public HDestination Destination => HDestination.Server;

        public HTheme Theme { get; }
        public string Message { get; }

        public HostShoutEventArgs(HMessage packet)
        {
            Header = packet.Header;

            Message = packet.ReadString(0);
            Theme = (HTheme)packet.ReadInteger(packet.Length - 6);
        }

        public override string ToString() =>
            $"{nameof(Header)}: {Header}, " +
            $"{nameof(Message)}: {Message}, {nameof(Theme)}: {Theme}";
    }
}