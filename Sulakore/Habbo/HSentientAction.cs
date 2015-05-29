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

using System;
using System.Collections.Generic;

using Sulakore.Habbo.Protocol;

namespace Sulakore.Habbo
{
    /// <summary>
    /// Represents a sentient object performing actions.
    /// </summary>
    public class HSentientAction
    {
        /// <summary>
        /// Gets the room index value of the sentient object.
        /// </summary>
        public int Index { get; }
        /// <summary>
        /// Gets a value that determines whether the sentient object has privileges.
        /// </summary>
        public bool IsEmpowered { get; }

        /// <summary>
        /// Gets the <see cref="HPoint"/> of where the sentient object is currently on.
        /// </summary>
        public HPoint Tile { get; }
        /// <summary>
        /// Gets the <see cref="HPoint"/> of where the sentient object will move to next.
        /// </summary>
        public HPoint MovingTo { get; }

        /// <summary>
        /// Gets the <see cref="HSign"/> that the sentient object has raised.
        /// </summary>
        public HSign Sign { get; }
        /// <summary>
        /// Gets the current <see cref="HStance"/> of the sentient object.
        /// </summary>
        public HStance Stance { get; }
        /// <summary>
        /// Gets the <see cref="HAction"/> that the sentient object has recently done.
        /// </summary>
        public HAction LastAction { get; }
        /// <summary>
        /// Gets the <see cref="HDirection"/> of the sentient object's head.
        /// </summary>
        public HDirection HeadDirection { get; }
        /// <summary>
        /// Gets the <see cref="HDirection"/> of the sentient object's body.
        /// </summary>
        public HDirection BodyDirection { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HSentientAction"/> class with the specified sentient object data.
        /// </summary>
        /// <param name="isEmpowered">The value that dermines whether the sentient object has privileges.</param>
        /// <param name="index">The room index value of the sentient object.</param>
        /// <param name="tile">The <see cref="HPoint"/> of where the sentient object is currently on.</param>
        /// <param name="movingTo">The <see cref="HPoint"/> of where the sentient object will move to next.</param>
        /// <param name="sign">The <see cref="HSign"/> that the sentient object has raised.</param>
        /// <param name="stance">The current <see cref="HStance"/> of the sentient object.</param>
        /// <param name="headDirection">The <see cref="HDirection"/> of the sentient object's head.</param>
        /// <param name="bodyDirection">The <see cref="HDirection"/> of the sentient object's body.</param>
        /// <param name="lastAction">The <see cref="HAction"/> that the sentient object has recently done.</param>
        public HSentientAction(bool isEmpowered, int index, HPoint tile, HPoint movingTo,
            HSign sign, HStance stance, HDirection headDirection, HDirection bodyDirection, HAction lastAction)
        {
            Index = index;
            IsEmpowered = isEmpowered;

            Tile = tile;
            MovingTo = movingTo;

            Sign = sign;
            Stance = stance;

            HeadDirection = headDirection;
            BodyDirection = bodyDirection;

            LastAction = lastAction;
        }

        /// <summary>
        /// Returns a <see cref="IReadOnlyList{T}"/> containing a list of actions performed by sentient objects found in the <see cref="HMessage"/>.
        /// </summary>
        /// <param name="packet">The <see cref="HMessage"/> that contains the sentient object's action data to parse.</param>
        /// <returns></returns>
        public static IReadOnlyList<HSentientAction> Parse(HMessage packet)
        {
            int position = packet.Position;
            int sentientActionCount = packet.ReadInteger();
            var sentientActionList = new List<HSentientAction>(sentientActionCount);

            for (int i = 0; i < sentientActionList.Capacity; i++)
            {
                int index = packet.ReadInteger();
                int x = packet.ReadInteger();
                int y = packet.ReadInteger();
                double z = double.Parse(packet.ReadString());
                var headDirection = (HDirection)packet.ReadInteger();
                var bodyDirection = (HDirection)packet.ReadInteger();

                string actionString = packet.ReadString();
                string[] actionData = actionString.Split(new[] { '/' },
                    StringSplitOptions.RemoveEmptyEntries);

                HSign sign = HSign.One;
                HAction action = HAction.None;
                HStance stance = HStance.Stand;

                double movingToZ = 0.0;
                bool isEmpowered = false;
                int movingToX = 0, movingToY = 0;

                foreach (string actionInfo in actionData)
                {
                    string[] actionValues = actionInfo.Split(' ');
                    if (actionValues.Length < 2 || string.IsNullOrWhiteSpace(actionValues[0]))
                        continue;

                    #region Switch Statement: actionValues[]
                    switch (actionValues[0])
                    {
                        case "flatctrl":
                        {
                            isEmpowered = true;
                            break;
                        }
                        case "mv":
                        {
                            string[] movingToValues = actionValues[1].Split(',');
                            if (movingToValues.Length >= 3)
                            {
                                movingToX = int.Parse(movingToValues[0]);
                                movingToY = int.Parse(movingToValues[1]);
                                movingToZ = double.Parse(movingToValues[2]);
                            }
                            action = HAction.Move;
                            break;
                        }
                        case "sit":
                        {
                            action = HAction.Sit;
                            stance = HStance.Sit;
                            break;
                        }
                        case "lay":
                        {
                            action = HAction.Lay;
                            stance = HStance.Lay;
                            break;
                        }
                        case "sign":
                        {
                            sign = (HSign)int.Parse(actionValues[1]);
                            action = HAction.Sign;
                            break;
                        }
                    }
                    #endregion
                }
                
                var sentientAction = new HSentientAction(isEmpowered, index, new HPoint(x, y, z),
                    new HPoint(movingToX, movingToY, movingToZ), sign, stance, headDirection, bodyDirection, action);

                packet.Position = position;
                sentientActionList.Add(sentientAction);
            }
            return sentientActionList;
        }

        /// <summary>
        /// Converts this <see cref="HSentientAction"/> to a human-readable string.
        /// </summary>
        /// <returns></returns>
        public override string ToString() =>
            $"{nameof(IsEmpowered)}: {IsEmpowered}, {nameof(Index)}: {Index}, " +
            $"{nameof(Tile)}: {Tile}, {nameof(MovingTo)}: {MovingTo}, {nameof(HeadDirection)}: {HeadDirection}, " +
            $"{nameof(BodyDirection)}: {BodyDirection}, {nameof(LastAction)}: {LastAction}";
    }
}