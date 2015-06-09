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
    public class Outgoing
    {
        private static DataContractJsonSerializer _serializer;

        public static Outgoing Global { get; }

        public const ushort CLIENT_CONNECT = 4000;

        public ushort InitiateHandshake { get; set; }
        public ushort ClientPublicKey { get; set; }
        public ushort FlashClientUrl { get; set; }
        public ushort ClientSsoTicket { get; set; }

        public ushort PetScratch { get; set; }
        public ushort PlayerEffect { get; set; }

        public ushort BanPlayer { get; set; }
        public ushort KickPlayer { get; set; }
        public ushort MutePlayer { get; set; }
        public ushort TradePlayer { get; set; }
        public ushort ClickPlayer { get; set; }

        public ushort UpdateMotto { get; set; }
        public ushort UpdateStance { get; set; }
        public ushort UpdateClothes { get; set; }

        public ushort Walk { get; set; }
        public ushort Dance { get; set; }
        public ushort Gesture { get; set; }
        public ushort RaiseSign { get; set; }

        public ushort HostExitRoom { get; set; }
        public ushort NavigateRoom { get; set; }
        public ushort MoveFurniture { get; set; }
        public ushort ShopObjectGet { get; set; }

        public ushort Say { get; set; }
        public ushort Shout { get; set; }
        public ushort Whisper { get; set; }

        static Outgoing()
        {
            Global = new Outgoing();
            _serializer = new DataContractJsonSerializer(typeof(Outgoing));
        }

        public void Save(string path)
        {
            using (var fileStream = File.Open(path, FileMode.Create))
                _serializer.WriteObject(fileStream, this);
        }
        public static Outgoing Load(string path)
        {
            using (var fileStream = File.Open(path, FileMode.Open))
                return (Outgoing)_serializer.ReadObject(fileStream);
        }
    }
}