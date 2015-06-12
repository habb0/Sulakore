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

using System.Collections.Generic;

using Sulakore.Habbo.Protocol;

namespace Sulakore.Habbo
{
    /// <summary>
    /// Represents an in-game object that provides special information that makes it unique in a room.
    /// </summary>
    public class HEntity : IHEntity
    {
        /// <summary>
        /// Gets the id of the <see cref="HEntity"/>.
        /// </summary>
        public int Id { get; }
        /// <summary>
        /// Gets the room index value of the <see cref="HEntity"/>.
        /// </summary>
        public int Index { get; }
        /// <summary>
        /// Gets the name of the <see cref="HEntity"/>.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Gets the motto of the <see cref="HEntity"/>.
        /// </summary>
        public string Motto { get; }
        /// <summary>
        /// Gets the figure id of the <see cref="HEntity"/>.
        /// </summary>
        public string FigureId { get; }
        /// <summary>
        /// Gets the favorite group badge of the <see cref="HEntity"/>.
        /// </summary>
        public string FavoriteGroup { get; }

        /// <summary>
        /// Gets the <see cref="HPoint"/> of the <see cref="HEntity"/>.
        /// </summary>
        public HPoint Tile { get; }
        /// <summary>
        /// Gets the gender of the <see cref="HEntity"/>.
        /// </summary>
        public HGender Gender { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HEntity"/> class with the specified information,
        /// </summary>
        /// <param name="id">The id of the <see cref="HEntity"/>.</param>
        /// <param name="index">The room index value of the <see cref="HEntity"/>.</param>
        /// <param name="name">The name of the <see cref="HEntity"/>.</param>
        /// <param name="tile">The <see cref="HPoint"/> of the <see cref="HEntity"/>.</param>
        /// <param name="motto">The motto of the <see cref="HEntity"/>.</param>
        /// <param name="gender">The <see cref="HGender"/> of the <see cref="HEntity"/>.</param>
        /// <param name="figureId">The figure id of the <see cref="HEntity"/>.</param>
        /// <param name="favoriteGroup">The favorite group badge of the <see cref="HEntity"/>.</param>
        public HEntity(int id, int index, string name, HPoint tile,
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
        /// Returns a <see cref="IReadOnlyList{T}"/> of type <see cref="HEntity"/> found in the <see cref="HMessage"/>.
        /// </summary>
        /// <param name="packet">The <see cref="HMessage"/> that contains the <see cref="HEntity"/> data to parse.</param>
        /// <returns></returns>
        public static IReadOnlyList<HEntity> Parse(HMessage packet)
        {
            int entityCount = packet.ReadInteger();
            var entityList = new List<HEntity>(entityCount);

            for (int i = 0; i < entityList.Capacity; i++)
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
                            packet.ReadShort();

                        break;
                    }
                }
                #endregion

                var entity = new HEntity(id, index, name,
                    new HPoint(x, y, z), motto, gender, figureId, favoriteGroup);

                entityList.Add(entity);
            }

            return entityList;
        }

        /// <summary>
        /// Converts the <see cref="HEntity"/> to a human-readable string.
        /// </summary>
        /// <returns></returns>
        public override string ToString() =>
            $"{nameof(Id)}: {Id}, {nameof(Index)}: {Index}, {nameof(Name)}: {Name}, " +
            $"{nameof(Tile)}: {Tile}, {nameof(Motto)}: {Motto}, {nameof(Gender)}: {Gender}, " +
            $"{nameof(FigureId)}: {FigureId}, {nameof(FavoriteGroup)}: {FavoriteGroup}";
    }
}