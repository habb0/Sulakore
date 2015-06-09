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
using System.Threading.Tasks;

using Sulakore.Habbo;
using Sulakore.Habbo.Protocol;

namespace Sulakore.Communication
{
    public class FurnitureDropEventArgs : InterceptedEventArgs
    {
        public int Id { get; }
        public int TypeId { get; }
        public int OwnerId { get; }
        public HPoint Tile { get; }
        public bool IsRental { get; }
        public string OwnerName { get; }
        public HDirection Direction { get; }

        public FurnitureDropEventArgs(HMessage packet)
            : this(null, -1, packet)
        { }
        public FurnitureDropEventArgs(int step, HMessage packet)
            : this(null, step, packet)
        { }
        public FurnitureDropEventArgs(int step, byte[] data, HDestination destination)
            : this(null, step, new HMessage(data, destination))
        { }
        public FurnitureDropEventArgs(Func<Task> continuation, int step, HMessage packet)
            : base(continuation, step, packet)
        {
            Id = Packet.ReadInteger();
            TypeId = Packet.ReadInteger();

            int x = Packet.ReadInteger();
            int y = Packet.ReadInteger();

            Direction = (HDirection)Packet.ReadInteger();
            Tile = new HPoint(x, y, double.Parse(Packet.ReadString()));

            Packet.ReadString();
            Packet.ReadInteger();
            Packet.ReadInteger();
            Packet.ReadString();

            IsRental = (Packet.ReadInteger() != 1);
            Packet.ReadInteger();

            OwnerId = Packet.ReadInteger();
            OwnerName = Packet.ReadString();
        }
        public FurnitureDropEventArgs(Func<Task> continuation, int step, byte[] data, HDestination destination)
            : this(continuation, step, new HMessage(data, destination))
        { }

        public override string ToString() =>
            $"{nameof(Packet.Header)}: {Packet.Header}, {nameof(Id)}: {Id}, {nameof(TypeId)}: {TypeId}, " +
            $"{nameof(Tile)}: {Tile}, {nameof(Direction)}: {Direction}, {nameof(IsRental)}: {IsRental}, " +
            $"{nameof(OwnerId)}: {OwnerId}, {nameof(OwnerName)}: {OwnerName}";
    }
}