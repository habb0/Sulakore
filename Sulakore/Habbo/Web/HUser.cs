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
        public string UniqueId => _uniqueId;

        [DataMember(Name = "name")]
        private readonly string _name;
        public string Name => _name;

        [DataMember(Name = "figureString")]
        private readonly string _figureId;
        public string FigureId => _figureId;

        [DataMember(Name = "selectedBadges")]
        private readonly IList<HBadge> _selectedBadges;
        public IList<HBadge> SelectedBadges => _selectedBadges;

        [DataMember(Name = "motto")]
        private readonly string _motto;
        public string Motto => _motto;

        [DataMember(Name = "memberSince")]
        private readonly string _memberSince;
        public string MemberSince => _memberSince;

        [DataMember(Name = "profileVisible")]
        private readonly bool _isProfileVisible;
        public bool IsProfileVisible => _isProfileVisible;

        [DataMember(Name = "lastWebAccess")]
        private readonly string _lastWebAccess;
        public string LastWebAccess => _lastWebAccess;

        [DataMember(Name = "sessionLogId", EmitDefaultValue = false)]
        private readonly long _sessionLogId;
        public long SessionLogId => _sessionLogId;

        [DataMember(Name = "loginLogId", EmitDefaultValue = false)]
        private readonly long _loginLogId;
        public long LoginLodId => _loginLogId;

        [DataMember(Name = "email", EmitDefaultValue = false)]
        private readonly string _email;
        public string Email => _email;

        [DataMember(Name = "identityId", EmitDefaultValue = false)]
        private readonly int _identityId;
        public int IdentityId => _identityId;

        [DataMember(Name = "emailVerified", EmitDefaultValue = false)]
        private readonly bool? _isEmailVerified;
        public bool IsEmailVerified => (bool)_isEmailVerified;

        [DataMember(Name = "trusted", EmitDefaultValue = false)]
        private readonly bool? _isTrusted;
        public bool IsTrusted => (bool)_isTrusted;

        [DataMember(Name = "accountId", EmitDefaultValue = false)]
        private readonly int _accountId;
        public int AccountId => _accountId;

        [DataMember(Name = "country", EmitDefaultValue = false)]
        private readonly string _country;
        public string Country => _country;

        [DataMember(Name = "traits", EmitDefaultValue = false)]
        private readonly string _traits;
        public string Traits => _traits;

        [DataMember(Name = "partner", EmitDefaultValue = false)]
        private readonly string _partner;
        public string Partner => _partner;

        static HUser()
        {
            _serializer = new DataContractJsonSerializer(typeof(HUser));
        }
        public HUser()
        {
            _uniqueId = null;
            _name = null;
            _figureId = null;
            _selectedBadges = new List<HBadge>(0);
            _motto = null;
            _memberSince = null;
            _isProfileVisible = false;
            _lastWebAccess = null;
            _sessionLogId = default(long);
            _loginLogId = default(long);
            _email = null;
            _identityId = default(int);
            _isEmailVerified = null;
            _isTrusted = null;
            _accountId = default(int);
            _country = null;
            _traits = null;
            _partner = null;
        }
        
        public static HUser Load(string path) =>
            Create(File.ReadAllText(path));
        
        public static HUser Create(string json)
        {
            byte[] data = Encoding.UTF8.GetBytes(json);
            using (var memoryStream = new MemoryStream(data))
                return (HUser)_serializer.ReadObject(memoryStream);
        }
        
        public static HUser Create(Stream stream) =>
            (HUser)_serializer.ReadObject(stream);
    }
}