/* GitHub(Source): https://GitHub.com/ArachisH

    .NET library for creating Habbo Hotel desktop applications.
    Copyright (C) 2015  Arachis

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
*/

using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Sulakore.Habbo.Web
{
    [DataContract]
    public class HUser
    {
        private static readonly DataContractJsonSerializer _serializer;

        [DataMember(Name = "uniqueId")]
        private readonly string _uniqueId;
        public string UniqueId
        {
            get { return _uniqueId; }
        }

        [DataMember(Name = "name")]
        private readonly string _name;
        public string Name
        {
            get { return _name; }
        }

        [DataMember(Name = "figureString")]
        private readonly string _figureId;
        public string FigureId
        {
            get { return _figureId; }
        }

        [DataMember(Name = "selectedBadges")]
        private readonly IList<HBadge> _selectedBadges;
        public IList<HBadge> SelectedBadges
        {
            get { return _selectedBadges; }
        }

        [DataMember(Name = "motto")]
        private readonly string _motto;
        public string Motto
        {
            get { return _motto; }
        }

        [DataMember(Name = "memberSince")]
        private readonly string _memberSince;
        public string MemberSince
        {
            get { return _memberSince; }
        }

        [DataMember(Name = "profileVisible")]
        private readonly bool _isProfileVisible;
        public bool IsProfileVisible
        {
            get { return _isProfileVisible; }
        }

        [DataMember(Name = "lastWebAccess")]
        private readonly string _lastWebAccess;
        public string LastWebAccess
        {
            get { return _lastWebAccess; }
        }

        static HUser()
        {
            _serializer = new DataContractJsonSerializer(typeof(HUser));
        }
        public HUser()
        {
            _uniqueId = string.Empty;
            _name = string.Empty;
            _figureId = string.Empty;
            _selectedBadges = new List<HBadge>(0);
            _motto = string.Empty;
            _memberSince = string.Empty;
            _isProfileVisible = false;
            _lastWebAccess = string.Empty;
        }

        public static HUser Load(string path)
        {
            return Create(File.ReadAllText(path));
        }
        public static HUser Create(string json)
        {
            byte[] data = Encoding.UTF8.GetBytes(json);
            using (var memoryStream = new MemoryStream(data))
                return (HUser)_serializer.ReadObject(memoryStream);
        }
    }
}