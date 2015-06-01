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
    public class HostMoveFurnitureEventArgs : EventArgs, IHabboEvent
    {
        public ushort Header { get; }
        public HDestination Destination => HDestination.Server;

        public int Id { get; }
        public HPoint Tile { get; }
        public HDirection Direction { get; }

        public HostMoveFurnitureEventArgs(HMessage packet)
        {
            Header = packet.Header;

            Id = packet.ReadInteger(0);

            Tile = new HPoint(packet.ReadInteger(4),
                packet.ReadInteger(8));

            Direction = (HDirection)packet.ReadInteger(12);
        }

        public override string ToString() =>
            $"{nameof(Header)}: {Header}, {nameof(Id)}: {Id}, " +
            $"{nameof(Tile)}: {Tile}, {nameof(Direction)}: {Direction}";
    }
}