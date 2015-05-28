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

namespace Sulakore.Habbo
{
    /// <summary>
    /// Specifies the different types of dances your player can perform in-game.
    /// </summary>
    public enum HDance
    {
        /// <summary>
        /// Represents a non-dancing player.
        /// </summary>
        None = 0,
        /// <summary>
        /// Represents the default dance any player can perform.
        /// </summary>
        Normal = 1,
        /// <summary>
        /// Represents the duck funk dance. (HC Only)
        /// </summary>
        PogoMogo = 2,
        /// <summary>
        /// Represents the pogo mogo dance. (HC Only).
        /// </summary>
        DuckFunk = 3,
        /// <summary>
        /// Represents the rollie dance. (HC Only)
        /// </summary>
        TheRollie = 4
    }
}