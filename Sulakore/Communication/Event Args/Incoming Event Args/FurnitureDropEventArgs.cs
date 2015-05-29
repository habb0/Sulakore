/* Copyright

    GitHub(Source): https://GitHub.com/ArachisH/Sulakore

    .NET library for creating Habbo Hotel desktop applications.
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
    public class FurnitureDropEventArgs : EventArgs, IHabboEvent
    {
        public ushort Header { get; }
        public HDestination Destination => HDestination.Client;

        public int Id { get; }
        public int TypeId { get; }
        public int OwnerId { get; }
        public HPoint Tile { get; }
        public bool IsRental { get; }
        public string OwnerName { get; }
        public HDirection Direction { get; }

        public FurnitureDropEventArgs(HMessage packet)
        {
            Header = packet.Header;

            Id = packet.ReadInteger();
            TypeId = packet.ReadInteger();

            int x = packet.ReadInteger();
            int y = packet.ReadInteger();

            Direction = (HDirection)packet.ReadInteger();
            Tile = new HPoint(x, y, double.Parse(packet.ReadString()));

            packet.ReadString();
            packet.ReadInteger();
            packet.ReadInteger();
            packet.ReadString();

            IsRental = (packet.ReadInteger() != 1);
            packet.ReadInteger();

            OwnerId = packet.ReadInteger();
            OwnerName = packet.ReadString();
        }

        public override string ToString() =>
            $"{nameof(Header)}: {Header}, {nameof(Id)}: {Id}, {nameof(TypeId)}: {TypeId}, " +
            $"{nameof(Tile)}: {Tile}, {nameof(Direction)}: {Direction}, {nameof(IsRental)}: {IsRental}, " +
            $"{nameof(OwnerId)}: {OwnerId}, {nameof(OwnerName)}: {OwnerName}";
    }
}