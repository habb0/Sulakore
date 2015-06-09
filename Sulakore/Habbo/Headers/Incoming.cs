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

using System.IO;
using System.Runtime.Serialization.Json;

namespace Sulakore.Habbo.Headers
{
    public class Incoming
    {
        private static readonly DataContractJsonSerializer _serializer;
        
        public static Incoming Global { get; }

        public const ushort SERVER_DISCONNECT = 4000;

        public ushort RoomMapLoaded { get; set; }
        public ushort LocalHotelAlert { get; set; }
        public ushort GlobalHotelAlert { get; set; }

        public ushort SentienceLoad { get; set; }
        public ushort FurnitureLoad { get; set; }

        public ushort PlayerUpdate { get; set; }
        public ushort PlayerUpdateStance { get; set; }

        public ushort PlayerDance { get; set; }
        public ushort PlayerGesture { get; set; }
        public ushort PlayerKickHost { get; set; }

        public ushort FurnitureDrop { get; set; }
        public ushort FurnitureMove { get; set; }

        public ushort PlayerSay { get; set; }
        public ushort PlayerShout { get; set; }
        public ushort PlayerWhisper { get; set; }

        static Incoming()
        {
            Global = new Incoming();
            _serializer = new DataContractJsonSerializer(typeof(Incoming));
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