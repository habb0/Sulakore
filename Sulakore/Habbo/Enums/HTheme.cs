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

namespace Sulakore.Habbo
{
    /// <summary>
    /// Specifies the different types of speech bubble themes found in-game.
    /// </summary>
    public enum HTheme
    {
        /// <summary>
        /// Represents a random speech bubble consisting of:
        /// <para><see cref="White"/>, <see cref="Red"/>, <see cref="Blue"/>, <see cref="Yellow"/>, <see cref="Green"/>, <see cref="Black"/></para>
        /// </summary>
        Random = -1,
        /// <summary>
        /// Represents the default(white) speech bubble.
        /// </summary>
        White = 0,
        /// <summary>
        /// Represents the red speech bubble.
        /// </summary>
        Red = 3,
        /// <summary>
        /// Represents the blue speech bubble.
        /// </summary>
        Blue = 4,
        /// <summary>
        /// Represents the yellow speech bubble.
        /// </summary>
        /// 
        Yellow = 5,
        /// <summary>
        /// Represents the green speech bubble.
        /// </summary>
        Green = 6,
        /// <summary>
        /// Represents the black speech bubble.
        /// </summary>
        Black = 7,
        /// <summary>
        /// Represents the light-blue(ice) speech bubble.
        /// </summary>
        Ice = 11,
        /// <summary>
        /// Represents the pink speech bubble.
        /// </summary>
        Pink = 12,
        /// <summary>
        /// Represents the purple speech bubble.
        /// </summary>
        Purple = 13,
        /// <summary>
        /// Represents the gold speech bubble.
        /// </summary>
        Gold = 14,
        /// <summary>
        /// Represents the turquoise speech bubble.
        /// </summary>
        Turquoise = 15,
        /// <summary>
        /// Represents a speech bubble with hearts in the background.
        /// </summary>
        Hearts = 16,
        /// <summary>
        /// Represents a speech bubble with roses in the background.
        /// </summary>
        Roses = 17,
        /// <summary>
        /// Represents a speech bubble with a pig in the background.
        /// </summary>
        Pig = 19,
        /// <summary>
        /// Represents a speech bubble with a dog in the background.
        /// </summary>
        Dog = 20,
        /// <summary>
        /// Represents a speech bubble with swords in the background.
        /// </summary>
        Swords = 29
    }
}