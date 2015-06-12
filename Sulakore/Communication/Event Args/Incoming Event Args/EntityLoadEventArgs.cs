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
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;

using Sulakore.Habbo;
using Sulakore.Habbo.Protocol;

namespace Sulakore.Communication
{
    public class EntityLoadEventArgs : InterceptedEventArgs, IReadOnlyList<HEntity>
    {
        private readonly IReadOnlyList<HEntity> _entityLoadList;

        public int Count => _entityLoadList.Count;
        public HEntity this[int index] => _entityLoadList[index];

        public EntityLoadEventArgs(HMessage packet)
            : this(null, -1, packet)
        { }
        public EntityLoadEventArgs(int step, HMessage packet)
            : this(null, step, packet)
        { }
        public EntityLoadEventArgs(int step, byte[] data, HDestination destination)
            : this(null, step, new HMessage(data, destination))
        { }
        public EntityLoadEventArgs(Func<Task> continuation, int step, HMessage packet)
            : base(continuation, step, packet)
        {
            _entityLoadList = HEntity.Parse(packet);
        }
        public EntityLoadEventArgs(Func<Task> continuation, int step, byte[] data, HDestination destination)
            : this(continuation, step, new HMessage(data, destination))
        { }

        public IEnumerator<HEntity> GetEnumerator() =>
            _entityLoadList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            ((IEnumerable)_entityLoadList).GetEnumerator();

        public override string ToString() =>
            $"{nameof(Packet.Header)}: {Packet.Header}, {nameof(Count)}: {Count}";
    }
}