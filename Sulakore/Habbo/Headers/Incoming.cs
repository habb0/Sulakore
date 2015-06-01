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
using System.IO;
using System.Runtime.Serialization.Json;

namespace Sulakore.Habbo.Headers
{
    public class Incoming
    {
        private static readonly Type _incomingType;
        private static readonly DataContractJsonSerializer _serializer;

        private static readonly Incoming _global;
        public static Incoming Global => _global;

        public const ushort CLIENT_DISCONNECT = 4000;

        public static ushort RoomMapLoaded { get; set; }
        public static ushort LocalHotelAlert { get; set; }
        public static ushort GlobalHotelAlert { get; set; }

        public static ushort SentienceLoad { get; set; }
        public static ushort FurnitureLoad { get; set; }

        public static ushort PlayerUpdate { get; set; }
        public static ushort PlayerUpdateStance { get; set; }

        public static ushort PlayerDance { get; set; }
        public static ushort PlayerGesture { get; set; }
        public static ushort PlayerKickHost { get; set; }

        public static ushort FurnitureDrop { get; set; }
        public static ushort FurnitureMove { get; set; }

        public static ushort PlayerSay { get; set; }
        public static ushort PlayerShout { get; set; }
        public static ushort PlayerWhisper { get; set; }

        static Incoming()
        {
            _global = new Incoming();
            _incomingType = typeof(Incoming);
            _serializer = new DataContractJsonSerializer(_incomingType);
        }

        public void Save(string path)
        {
            using (var fileStream = File.Open(path, FileMode.Create))
                _serializer.WriteObject(fileStream, this);
        }
        public static Incoming Load(string path)
        {
            using (var fileStream = File.Open(path, FileMode.Open))
                return (Incoming)_serializer.ReadObject(fileStream);
        }
    }
}