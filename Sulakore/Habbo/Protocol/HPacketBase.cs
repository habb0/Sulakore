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

namespace Sulakore.Habbo.Protocol
{
    public abstract class HPacketBase : IHPacket
    {
        public abstract ushort Header { get; set; }

        public abstract int Position { get; set; }
        public abstract HDestination Destination { get; set; }
        
        public abstract bool IsCorrupted{ get; }
        public abstract int Length { get; protected set; }
        public abstract byte[] Body { get; protected set; }

        public virtual int ReadInteger()
        {
            int index = Position;
            int value = ReadInteger(ref index);
            Position = index;
            return value;
        }
        public virtual int ReadInteger(int index) => ReadInteger(ref index);
        public abstract int ReadInteger(ref int index);

        public virtual ushort ReadShort()
        {
            int index = Position;
            ushort value = ReadShort(ref index);
            Position = index;
            return value;
        }
        public virtual ushort ReadShort(int index) => ReadShort(ref index);
        public abstract ushort ReadShort(ref int index);

        public virtual bool ReadBoolean()
        {
            int index = Position;
            bool value = ReadBoolean(ref index);
            Position = index;
            return value;
        }
        public virtual bool ReadBoolean(int index) => ReadBoolean(ref index);
        public abstract bool ReadBoolean(ref int index);

        public virtual string ReadString()
        {
            int index = Position;
            string value = ReadString(ref index);
            Position = index;
            return value;
        }
        public virtual string ReadString(int index) => ReadString(ref index);
        public abstract string ReadString(ref int index);
        
        public virtual byte[] ReadBytes(int length)
        {
            int index = Position;
            byte[] value = ReadBytes(length, ref index);
            Position = index;
            return value;
        }
        public virtual byte[] ReadBytes(int length, int index) => ReadBytes(length, ref index);
        public abstract byte[] ReadBytes(int length, ref int index);

        public virtual void Remove<T>()
        {
            Remove<T>(Position);
        }
        public abstract void Remove<T>(int index);

        public virtual bool CanRead<T>() => CanRead<T>(Position);
        public abstract bool CanRead<T>(int index);

        public virtual void Replace<T>(object chunk)
        {
            Replace<T>(Position, chunk);
        }
        public abstract void Replace<T>(int index, object chunk);
    }
}