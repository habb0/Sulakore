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

namespace Sulakore.Habbo.Protocol.Encoders
{
    public static class BigEndian
    {
        public static byte[] CypherShort(ushort value)
        {
            return new[] { (byte)(value >> 8), (byte)value };
        }
        public static byte[] CypherShort(byte[] source, int offset, ushort value)
        {
            offset = offset > source.Length ? source.Length : offset < 0 ? 0 : offset;

            var data = new byte[source.Length + 2];
            for (int i = 0, j = 0; j < data.Length; j++)
            {
                if (j != offset) data[j] = source[i++];
                else
                {
                    byte[] toInsert = CypherShort(value);
                    data[j++] = toInsert[0];
                    data[j] = toInsert[1];
                }
            }
            return data;
        }

        public static ushort DecypherShort(byte[] data)
        {
            return DecypherShort(data, 0);
        }
        public static ushort DecypherShort(string encoded)
        {
            return DecypherShort(new[] { (byte)encoded[0], (byte)encoded[1] }, 0);
        }
        public static ushort DecypherShort(byte[] data, int offset)
        {
            return (ushort)((data[offset] | data[offset + 1]) < 0 ? -1 : ((data[offset] << 8) + data[offset + 1]));
        }
        public static ushort DecypherShort(byte first, byte second)
        {
            return DecypherShort(new[] { first, second }, 0);
        }

        public static byte[] CypherInt(int value)
        {
            return new[] { (byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8), (byte)value };
        }
        public static byte[] CypherInt(byte[] source, int offset, int value)
        {
            offset = offset > source.Length ? source.Length : offset < 0 ? 0 : offset;

            var data = new byte[source.Length + 4];
            for (int i = 0, j = 0; j < data.Length; j++)
            {
                if (j != offset) data[j] = source[i++];
                else
                {
                    byte[] toInsert = CypherInt(value);
                    data[j++] = toInsert[0];
                    data[j++] = toInsert[1];
                    data[j++] = toInsert[2];
                    data[j] = toInsert[3];
                }
            }
            return data;
        }

        public static int DecypherInt(byte[] data)
        {
            return DecypherInt(data, 0);
        }
        public static int DecypherInt(string encoded)
        {
            return DecypherInt(new[] { (byte)encoded[0], (byte)encoded[1], (byte)encoded[2], (byte)encoded[3] }, 0);
        }
        public static int DecypherInt(byte[] data, int offset)
        {
            return ((data[offset] << 24) + (data[offset + 1] << 16) + (data[offset + 2] << 8) + data[offset + 3]);
        }
        public static int DecypherInt(byte first, byte second, byte third, byte fourth)
        {
            return DecypherInt(new[] { first, second, third, fourth }, 0);
        }
    }
}