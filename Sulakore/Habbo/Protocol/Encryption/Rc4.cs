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

using System;

namespace Sulakore.Habbo.Protocol.Encryption
{
    public class Rc4
    {
        private int _i, _j;
        private readonly int[] _table;

        public Rc4(byte[] key)
        {
            _table = new int[256];

            for (int i = 0; i < 256; i++)
                _table[i] = i;

            for (int j = 0, enX = 0; j < 256; j++)
                Swap(j, enX = (((enX + _table[j]) + (key[j % key.Length])) % 256));
        }

        public void Parse(byte[] data)
        {
            for (int k = 0; k < data.Length; k++)
            {
                Swap(_i = (++_i % 256), _j = ((_j + _table[_i]) % 256));
                data[k] ^= (byte)(_table[(_table[_i] + _table[_j]) % 256]);
            }
        }
        public byte[] SafeParse(byte[] data)
        {
            var dataCopy = new byte[data.Length];
            Buffer.BlockCopy(data, 0, dataCopy, 0, data.Length);

            Parse(dataCopy);
            return dataCopy;
        }

        private void Swap(int a, int b)
        {
            int temp = _table[a];
            _table[a] = _table[b];
            _table[b] = temp;
        }
    }
}