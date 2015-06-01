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

namespace Sulakore.Habbo
{
    /// <summary>
    /// Specifies the compass locations of an object/player in-game.
    /// </summary>
    public enum HDirection
    {
        /// <summary>
        /// Represents an object/player facing north.
        /// </summary>
        North = 0,
        /// <summary>
        /// Represents an object/player facing north east.
        /// </summary>
        NorthEast = 1,
        /// <summary>
        /// Represents an object/player facing east.
        /// </summary>
        East = 2,
        /// <summary>
        /// Represents an object/player facing south east.
        /// </summary>
        SouthEast = 3,
        /// <summary>
        /// Represents an object/player facing south.
        /// </summary>
        South = 4,
        /// <summary>
        /// Represents an object/player facing south west.
        /// </summary>
        SouthWest = 5,
        /// <summary>
        /// Represents an object/player facing west.
        /// </summary>
        West = 6,
        /// <summary>
        /// Represents an object/player facing north west.
        /// </summary>
        NorthWest = 7
    }
}