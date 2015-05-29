/* Copyright

    GitHub(Source): https://GitHub.com/ArachisH/Sulakore

    .NET library for creating Habbo Hotel desktop applications.
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
    public class HFriend
    {
        [DataMember(Name = "name")]
        private readonly string _name;
        public string Name => _name;

        [DataMember(Name = "motto")]
        private readonly string _motto;
        public string Motto => _motto;

        [DataMember(Name = "uniqueId")]
        private readonly string _uniqueId;
        public string UniqueId => _uniqueId;

        [DataMember(Name = "figureString")]
        private readonly string _figureId;
        public string FigureId => _figureId;

        public HFriend(string name, string motto,
            string uniqueId, string figureId)
        {
            _name = name;
            _motto = motto;
            _uniqueId = uniqueId;
            _figureId = figureId;
        }
    }
}