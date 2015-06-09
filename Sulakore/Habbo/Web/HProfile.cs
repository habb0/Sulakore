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
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Sulakore.Habbo.Web
{
    [DataContract]
    public class HProfile
    {
        private static readonly DataContractJsonSerializer _serializer;

        [DataMember(Name = "user")]
        private readonly HUser _user;
        public HUser User => _user;

        [DataMember(Name = "friends")]
        private readonly IList<HFriend> _friends;
        public IList<HFriend> Friends => _friends;

        [DataMember(Name = "groups")]
        private readonly IList<HGroup> _groups;
        public IList<HGroup> Groups => _groups;

        [DataMember(Name = "rooms")]
        private readonly IList<HRoom> _rooms;
        public IList<HRoom> Rooms => _rooms;

        [DataMember(Name = "badges")]
        private readonly IList<HBadge> _badges;
        public IList<HBadge> Badges => _badges;

        static HProfile()
        {
            _serializer = new DataContractJsonSerializer(typeof(HProfile));
        }
        public HProfile()
        {
            _user = new HUser();
            _friends = new List<HFriend>(0);
            _groups = new List<HGroup>(0);
            _rooms = new List<HRoom>(0);
            _badges = new List<HBadge>(0);
        }

        public static HProfile Load(string path) =>
            Create(File.ReadAllText(path));
        public static HProfile Create(string json)
        {
            byte[] data = Encoding.UTF8.GetBytes(json);
            using (var memoryStream = new MemoryStream(data))
                return (HProfile)_serializer.ReadObject(memoryStream);
        }
    }
}