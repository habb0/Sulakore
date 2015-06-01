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

using System.Runtime.Serialization;

namespace Sulakore.Habbo.Web
{
    [DataContract]
    public class HGroup
    {
        [DataMember(Name = "id")]
        private readonly string _id;
        public string Id => _id;

        [DataMember(Name = "name")]
        private readonly string _name;
        public string Name => _name;

        [DataMember(Name = "description")]
        private readonly string _description;
        public string Description => _description;

        [DataMember(Name = "type")]
        private readonly string _type;
        public string Type => _type;

        [DataMember(Name = "roomId")]
        private readonly string _roomId;
        public string RoomId => _roomId;

        [DataMember(Name = "badgeCode")]
        private readonly string _badgeCode;
        public string BadgeCode => _badgeCode;

        [DataMember(Name = "primaryColour")]
        private readonly string _primaryColor;
        public string PrimaryColor => _primaryColor;

        [DataMember(Name = "secondaryColour")]
        private readonly string _secondaryColor;
        public string SecondaryColor => _secondaryColor;

        [DataMember(Name = "isAdmin")]
        private readonly bool _isAdmin;
        public bool IsAdmin => _isAdmin;

        public HGroup(string id, string name, string description,
            string type, string roomId, string badgeCode,
            string primaryColor, string secondaryColor, bool isAdmin)
        {
            _id = id;
            _name = name;
            _description = description;
            _type = type;
            _roomId = roomId;
            _badgeCode = badgeCode;
            _primaryColor = primaryColor;
            _secondaryColor = secondaryColor;
            _isAdmin = isAdmin;
        }
    }
}