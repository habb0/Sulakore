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

using System.Collections.Generic;

using Sulakore.Habbo.Protocol;

namespace Sulakore.Habbo
{
    /// <summary>
    /// Represents an in-game object that probably has feelings.
    /// </summary>
    public class HSentient
    {
        /// <summary>
        /// Gets the id of the sentient object.
        /// </summary>
        public int Id { get; }
        /// <summary>
        /// Gets the room index value of the sentient object.
        /// </summary>
        public int Index { get; }
        /// <summary>
        /// Gets the name of the sentient object.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Gets the <see cref="HPoint"/> of the sentient object.
        /// </summary>
        public HPoint Tile { get; }
        /// <summary>
        /// Gets the motto of the sentient object.
        /// </summary>
        public string Motto { get; }
        /// <summary>
        /// Gets the gender of the sentient object.
        /// </summary>
        public HGender Gender { get; }
        /// <summary>
        /// Gets the figure id of the sentient object.
        /// </summary>
        public string FigureId { get; }
        /// <summary>
        /// Gets the favorite group badge of the sentient object.
        /// </summary>
        public string FavoriteGroup { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HSentient"/> class with the specified sentient object data.
        /// </summary>
        /// <param name="id">The id of the sentient object.</param>
        /// <param name="index">The room index value of the sentient object.</param>
        /// <param name="name">The name of the sentient object.</param>
        /// <param name="tile">The <see cref="HPoint"/> of the sentient object.</param>
        /// <param name="motto">The motto of the sentien object.</param>
        /// <param name="gender">The <see cref="HGender"/> of the sentient object.</param>
        /// <param name="figureId">The figure id of the sentient object.</param>
        /// <param name="favoriteGroup">The favorite group badge of the sentient object.</param>
        public HSentient(int id, int index, string name, HPoint tile,
            string motto, HGender gender, string figureId, string favoriteGroup)
        {
            Id = id;
            Index = index;
            Name = name;
            Tile = tile;
            Motto = motto;
            Gender = gender;
            FigureId = figureId;
            FavoriteGroup = favoriteGroup;
        }

        /// <summary>
        /// Returns a <see cref="IReadOnlyList{T}"/> containing a list of sentient objects found in the <see cref="HMessage"/>.
        /// </summary>
        /// <param name="packet">The <see cref="HMessage"/> that contains the sentient object data to parse.</param>
        /// <returns></returns>
        public static IReadOnlyList<HSentient> Parse(HMessage packet)
        {
            int position = packet.Position;
            int sentientCount = packet.ReadInteger();
            var sentientList = new List<HSentient>(sentientCount);

            for (int i = 0; i < sentientList.Capacity; i++)
            {
                int id = packet.ReadInteger();
                string name = packet.ReadString();
                string motto = packet.ReadString();
                string figureId = packet.ReadString();
                int index = packet.ReadInteger();
                int x = packet.ReadInteger();
                int y = packet.ReadInteger();
                double z = double.Parse(packet.ReadString());

                packet.ReadInteger();
                int type = packet.ReadInteger();

                HGender gender = HGender.Unknown;
                string favoriteGroup = string.Empty;
                #region Switch Statement: type
                switch (type)
                {
                    case 1:
                    {
                        gender = SKore.ToGender(packet.ReadString());
                        packet.ReadInteger();
                        packet.ReadInteger();
                        favoriteGroup = packet.ReadString();
                        packet.ReadString();
                        packet.ReadInteger();
                        packet.ReadBoolean();

                        break;
                    }
                    case 2:
                    {
                        packet.ReadInteger();
                        packet.ReadInteger();
                        packet.ReadString();
                        packet.ReadInteger();
                        packet.ReadBoolean();
                        packet.ReadBoolean();
                        packet.ReadBoolean();
                        packet.ReadBoolean();
                        packet.ReadBoolean();
                        packet.ReadBoolean();
                        packet.ReadInteger();
                        packet.ReadString();
                        break;
                    }
                    case 4:
                    {
                        packet.ReadString();
                        packet.ReadInteger();
                        packet.ReadString();

                        for (int j = packet.ReadInteger(); j > 0; j--)
                            packet.ReadUShort();

                        break;
                    }
                }
                #endregion

                var sentient = new HSentient(id, index, name,
                    new HPoint(x, y, z), motto, gender, figureId, favoriteGroup);

                sentientList.Add(sentient);
            }

            packet.Position = position;
            return sentientList;
        }

        /// <summary>
        /// Converts this <see cref="HSentient"/> to a human-readable string.
        /// </summary>
        /// <returns></returns>
        public override string ToString() =>
            $"{nameof(Id)}: {Id}, {nameof(Index)}: {Index}, {nameof(Name)}: {Name}, " +
            $"{nameof(Tile)}: {Tile}, {nameof(Motto)}: {Motto}, {nameof(Gender)}: {Gender}, " +
            $"{nameof(FigureId)}: {FigureId}, {nameof(FavoriteGroup)}: {FavoriteGroup}";
    }
}