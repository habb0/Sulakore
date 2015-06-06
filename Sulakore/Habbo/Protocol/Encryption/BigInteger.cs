/* Copyright

    BigInteger Class Version 1.03
    Copyright (C) 2002 Chew Keong TAN (http://www.codeproject.com/Articles/2728/C-BigInteger-Class)

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
    /// <summary>
    /// Represents a large number.
    /// </summary>
    public class BigInteger : IDisposable
    {
        private uint[] _data;
        private const int MAX_LENGTH = 70;

        /// <summary>
        /// Number of characters used.
        /// </summary>
        public int DataLength { get; set; }

        /// <summary>
        /// Contains a list of primes that are under 2000.
        /// </summary>
        public static int[] PrimesBelow2000 { get; } =
        {
            2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97,
            101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199,
            211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281, 283, 293,
            307, 311, 313, 317, 331, 337, 347, 349, 353, 359, 367, 373, 379, 383, 389, 397,
            401, 409, 419, 421, 431, 433, 439, 443, 449, 457, 461, 463, 467, 479, 487, 491, 499,
            503, 509, 521, 523, 541, 547, 557, 563, 569, 571, 577, 587, 593, 599,
            601, 607, 613, 617, 619, 631, 641, 643, 647, 653, 659, 661, 673, 677, 683, 691,
            701, 709, 719, 727, 733, 739, 743, 751, 757, 761, 769, 773, 787, 797,
            809, 811, 821, 823, 827, 829, 839, 853, 857, 859, 863, 877, 881, 883, 887,
            907, 911, 919, 929, 937, 941, 947, 953, 967, 971, 977, 983, 991, 997,
            1009, 1013, 1019, 1021, 1031, 1033, 1039, 1049, 1051, 1061, 1063, 1069, 1087, 1091, 1093, 1097,
            1103, 1109, 1117, 1123, 1129, 1151, 1153, 1163, 1171, 1181, 1187, 1193,
            1201, 1213, 1217, 1223, 1229, 1231, 1237, 1249, 1259, 1277, 1279, 1283, 1289, 1291, 1297,
            1301, 1303, 1307, 1319, 1321, 1327, 1361, 1367, 1373, 1381, 1399,
            1409, 1423, 1427, 1429, 1433, 1439, 1447, 1451, 1453, 1459, 1471, 1481, 1483, 1487, 1489, 1493, 1499,
            1511, 1523, 1531, 1543, 1549, 1553, 1559, 1567, 1571, 1579, 1583, 1597,
            1601, 1607, 1609, 1613, 1619, 1621, 1627, 1637, 1657, 1663, 1667, 1669, 1693, 1697, 1699,
            1709, 1721, 1723, 1733, 1741, 1747, 1753, 1759, 1777, 1783, 1787, 1789,
            1801, 1811, 1823, 1831, 1847, 1861, 1867, 1871, 1873, 1877, 1879, 1889,
            1901, 1907, 1913, 1931, 1933, 1949, 1951, 1973, 1979, 1987, 1993, 1997, 1999
        };
        
        /// <summary>
        /// Initializes a new instance of the BigInteger class with the default value of 0.
        /// </summary>
        public BigInteger()
        {
            _data = new uint[MAX_LENGTH];
            DataLength = 1;
        }

        /// <summary>
        /// Initializes a new instance of the BigInteger class.
        /// </summary>
        /// <param name="value">The long value to initialize the instance with.</param>
        public BigInteger(long value)
        {
            _data = new uint[MAX_LENGTH];
            long tempVal = value;

            DataLength = 0;
            while (value != 0 && DataLength < MAX_LENGTH)
            {
                _data[DataLength] = (uint)(value & 0xFFFFFFFF);
                value >>= 32;
                DataLength++;
            }

            if (tempVal > 0)
            {
                if (value != 0 || (_data[MAX_LENGTH - 1] & 0x80000000) != 0)
                    throw (new ArithmeticException("Positive overflow in constructor."));
            }
            else if (tempVal < 0)
            {
                if (value != -1 || (_data[DataLength - 1] & 0x80000000) == 0)
                    throw (new ArithmeticException("Negative underflow in constructor."));
            }

            if (DataLength == 0)
                DataLength = 1;
        }

        /// <summary>
        /// Initializes a new instance of the BigInteger class.
        /// </summary>
        /// <param name="value">The ulong value to initialize the instance with.</param>
        public BigInteger(ulong value)
        {
            _data = new uint[MAX_LENGTH];

            DataLength = 0;
            while (value != 0 && DataLength < MAX_LENGTH)
            {
                _data[DataLength] = (uint)(value & 0xFFFFFFFF);
                value >>= 32;
                DataLength++;
            }

            if (value != 0 || (_data[MAX_LENGTH - 1] & 0x80000000) != 0)
                throw (new ArithmeticException("Positive overflow in constructor."));

            if (DataLength == 0)
                DataLength = 1;
        }

        /// <summary>
        /// Initializes a new instance of the BigInteger class.
        /// </summary>
        /// <param name="bigInteger">The BigInteger value to initialize the instance with.</param>
        public BigInteger(BigInteger bigInteger)
        {
            _data = new uint[MAX_LENGTH];

            DataLength = bigInteger.DataLength;

            for (int i = 0; i < DataLength; i++)
                _data[i] = bigInteger._data[i];
        }

        /// <summary>
        /// Initializes a new instance of the BigInteger class.
        /// </summary>
        /// <param name="value">The string value to initialize the instance with the provided base.</param>
        /// <param name="radix">The int value that dictates the base of the provided System.String.</param>
        public BigInteger(string value, int radix)
        {
            BigInteger multiplier = new BigInteger(1);
            BigInteger result = new BigInteger();
            value = (value.ToUpper()).Trim();
            int limit = 0;

            if (value[0] == '-')
                limit = 1;

            for (int i = value.Length - 1; i >= limit; i--)
            {
                int posVal = (int)value[i];

                if (posVal >= '0' && posVal <= '9')
                    posVal -= '0';
                else if (posVal >= 'A' && posVal <= 'Z')
                    posVal = (posVal - 'A') + 10;
                else
                    posVal = 9999999;


                if (posVal >= radix)
                    throw (new ArithmeticException("Invalid string in constructor."));
                else
                {
                    if (value[0] == '-')
                        posVal = -posVal;

                    result = result + (multiplier * posVal);

                    if ((i - 1) >= limit)
                        multiplier = multiplier * radix;
                }
            }

            if (value[0] == '-')
            {
                if ((result._data[MAX_LENGTH - 1] & 0x80000000) == 0)
                    throw (new ArithmeticException("Negative underflow in constructor."));
            }
            else
            {
                if ((result._data[MAX_LENGTH - 1] & 0x80000000) != 0)
                    throw (new ArithmeticException("Positive overflow in constructor."));
            }

            _data = new uint[MAX_LENGTH];
            for (int i = 0; i < result.DataLength; i++)
                _data[i] = result._data[i];

            DataLength = result.DataLength;
        }

        /// <summary>
        /// Initializes a new instance of the BigInteger class.
        /// </summary>
        /// <param name="data">The byte array value to initialize the instance with.</param>
        public BigInteger(byte[] data)
        {
            DataLength = data.Length >> 2;

            int leftOver = data.Length & 0x3;
            if (leftOver != 0)
                DataLength++;

            if (DataLength > MAX_LENGTH)
                throw (new ArithmeticException("Byte overflow in constructor."));

            _data = new uint[MAX_LENGTH];

            for (int i = data.Length - 1, j = 0; i >= 3; i -= 4, j++)
            {
                _data[j] = (uint)((data[i - 3] << 24) + (data[i - 2] << 16) +
                                 (data[i - 1] << 8) + data[i]);
            }

            if (leftOver == 1)
                _data[DataLength - 1] = (uint)data[0];
            else if (leftOver == 2)
                _data[DataLength - 1] = (uint)((data[0] << 8) + data[1]);
            else if (leftOver == 3)
                _data[DataLength - 1] = (uint)((data[0] << 16) + (data[1] << 8) + data[2]);


            while (DataLength > 1 && _data[DataLength - 1] == 0)
                DataLength--;
        }

        /// <summary>
        /// Initializes a new instance of the BigInteger class.
        /// </summary>
        /// <param name="data">The byte array value to initialize the instance with.</param>
        /// <param name="length">The int value that specifies the length to use for the provided byte array.</param>
        public BigInteger(byte[] data, int length)
        {
            DataLength = length >> 2;

            int leftOver = length & 0x3;
            if (leftOver != 0)
                DataLength++;

            if (DataLength > MAX_LENGTH || length > data.Length)
                throw (new ArithmeticException("Byte overflow in constructor."));

            _data = new uint[MAX_LENGTH];

            for (int i = length - 1, j = 0; i >= 3; i -= 4, j++)
            {
                _data[j] = (uint)((data[i - 3] << 24) + (data[i - 2] << 16) +
                                 (data[i - 1] << 8) + data[i]);
            }

            if (leftOver == 1)
                _data[DataLength - 1] = (uint)data[0];
            else if (leftOver == 2)
                _data[DataLength - 1] = (uint)((data[0] << 8) + data[1]);
            else if (leftOver == 3)
                _data[DataLength - 1] = (uint)((data[0] << 16) + (data[1] << 8) + data[2]);

            if (DataLength == 0)
                DataLength = 1;

            while (DataLength > 1 && _data[DataLength - 1] == 0)
                DataLength--;
        }

        /// <summary>
        /// Initializes a new instance of the BigInteger class.
        /// </summary>
        /// <param name="data">The uint array value to initialize the instance with.</param>
        public BigInteger(uint[] data)
        {
            DataLength = data.Length;

            if (DataLength > MAX_LENGTH)
                throw (new ArithmeticException("Byte overflow in constructor."));

            _data = new uint[MAX_LENGTH];

            for (int i = DataLength - 1, j = 0; i >= 0; i--, j++)
                _data[j] = data[i];

            while (DataLength > 1 && _data[DataLength - 1] == 0)
                DataLength--;
        }
        
        public static implicit operator BigInteger(long value) => (new BigInteger(value));
        public static implicit operator BigInteger(ulong value) => (new BigInteger(value));
        public static implicit operator BigInteger(int value) => (new BigInteger(value));
        public static implicit operator BigInteger(uint value) => (new BigInteger((ulong)value));
        
        public static BigInteger operator --(BigInteger instance)
        {
            BigInteger result = new BigInteger(instance);

            long val;
            bool carryIn = true;
            int index = 0;

            while (carryIn && index < MAX_LENGTH)
            {
                val = (long)(result._data[index]);
                val--;

                result._data[index] = (uint)(val & 0xFFFFFFFF);

                if (val >= 0)
                    carryIn = false;

                index++;
            }

            if (index > result.DataLength)
                result.DataLength = index;

            while (result.DataLength > 1 && result._data[result.DataLength - 1] == 0)
                result.DataLength--;

            int lastPos = MAX_LENGTH - 1;

            if ((instance._data[lastPos] & 0x80000000) != 0 &&
               (result._data[lastPos] & 0x80000000) != (instance._data[lastPos] & 0x80000000))
            {
                throw (new ArithmeticException("Underflow in --."));
            }

            return result;
        }
        public static BigInteger operator ++(BigInteger instance)
        {
            BigInteger result = new BigInteger(instance);

            long val, carry = 1;
            int index = 0;

            while (carry != 0 && index < MAX_LENGTH)
            {
                val = (long)(result._data[index]);
                val++;

                result._data[index] = (uint)(val & 0xFFFFFFFF);
                carry = val >> 32;

                index++;
            }

            if (index > result.DataLength)
                result.DataLength = index;
            else
            {
                while (result.DataLength > 1 && result._data[result.DataLength - 1] == 0)
                    result.DataLength--;
            }

            int lastPos = MAX_LENGTH - 1;

            if ((instance._data[lastPos] & 0x80000000) == 0 &&
               (result._data[lastPos] & 0x80000000) != (instance._data[lastPos] & 0x80000000))
            {
                throw (new ArithmeticException("Overflow in ++."));
            }
            return result;
        }
        public static BigInteger operator ~(BigInteger instance)
        {
            BigInteger result = new BigInteger(instance);

            for (int i = 0; i < MAX_LENGTH; i++)
                result._data[i] = (uint)(~(instance._data[i]));

            result.DataLength = MAX_LENGTH;

            while (result.DataLength > 1 && result._data[result.DataLength - 1] == 0)
                result.DataLength--;

            return result;
        }
        public static BigInteger operator -(BigInteger instance)
        {
            if (instance.DataLength == 1 && instance._data[0] == 0)
                return (new BigInteger());

            BigInteger result = new BigInteger(instance);

            for (int i = 0; i < MAX_LENGTH; i++)
                result._data[i] = (uint)(~(instance._data[i]));

            long val, carry = 1;
            int index = 0;

            while (carry != 0 && index < MAX_LENGTH)
            {
                val = (long)(result._data[index]);
                val++;

                result._data[index] = (uint)(val & 0xFFFFFFFF);
                carry = val >> 32;

                index++;
            }

            if ((instance._data[MAX_LENGTH - 1] & 0x80000000) == (result._data[MAX_LENGTH - 1] & 0x80000000))
                throw (new ArithmeticException("Overflow in negation.\n"));

            result.DataLength = MAX_LENGTH;

            while (result.DataLength > 1 && result._data[result.DataLength - 1] == 0)
                result.DataLength--;
            return result;
        }

        public static BigInteger operator /(BigInteger left, BigInteger right)
        {
            BigInteger quotient = new BigInteger();
            BigInteger remainder = new BigInteger();

            int lastPos = MAX_LENGTH - 1;
            bool divisorNeg = false, dividendNeg = false;

            if ((left._data[lastPos] & 0x80000000) != 0)
            {
                left = -left;
                dividendNeg = true;
            }
            if ((right._data[lastPos] & 0x80000000) != 0)
            {
                right = -right;
                divisorNeg = true;
            }

            if (left < right)
            {
                return quotient;
            }

            else
            {
                if (right.DataLength == 1)
                    SingleByteDivide(left, right, quotient, remainder);
                else
                    MultipleByteDivide(left, right, quotient, remainder);

                if (dividendNeg != divisorNeg)
                    return -quotient;

                return quotient;
            }
        }
        public static BigInteger operator %(BigInteger left, BigInteger right)
        {
            BigInteger quotient = new BigInteger();
            BigInteger remainder = new BigInteger(left);

            int lastPos = MAX_LENGTH - 1;
            bool dividendNeg = false;

            if ((left._data[lastPos] & 0x80000000) != 0)
            {
                left = -left;
                dividendNeg = true;
            }
            if ((right._data[lastPos] & 0x80000000) != 0)
                right = -right;

            if (left < right)
            {
                return remainder;
            }

            else
            {
                if (right.DataLength == 1)
                    SingleByteDivide(left, right, quotient, remainder);
                else
                    MultipleByteDivide(left, right, quotient, remainder);

                if (dividendNeg)
                    return -remainder;

                return remainder;
            }
        }
        public static BigInteger operator &(BigInteger left, BigInteger right)
        {
            BigInteger result = new BigInteger();
            int len = (left.DataLength > right.DataLength) ? left.DataLength : right.DataLength;

            for (int i = 0; i < len; i++)
            {
                uint sum = (uint)(left._data[i] & right._data[i]);
                result._data[i] = sum;
            }

            result.DataLength = MAX_LENGTH;

            while (result.DataLength > 1 && result._data[result.DataLength - 1] == 0)
                result.DataLength--;

            return result;
        }
        public static BigInteger operator |(BigInteger left, BigInteger right)
        {
            BigInteger result = new BigInteger();
            int len = (left.DataLength > right.DataLength) ? left.DataLength : right.DataLength;

            for (int i = 0; i < len; i++)
            {
                uint sum = (uint)(left._data[i] | right._data[i]);
                result._data[i] = sum;
            }

            result.DataLength = MAX_LENGTH;

            while (result.DataLength > 1 && result._data[result.DataLength - 1] == 0)
                result.DataLength--;

            return result;
        }
        public static BigInteger operator ^(BigInteger left, BigInteger right)
        {
            BigInteger result = new BigInteger();
            int len = (left.DataLength > right.DataLength) ? left.DataLength : right.DataLength;

            for (int i = 0; i < len; i++)
            {
                uint sum = (uint)(left._data[i] ^ right._data[i]);
                result._data[i] = sum;
            }

            result.DataLength = MAX_LENGTH;

            while (result.DataLength > 1 && result._data[result.DataLength - 1] == 0)
                result.DataLength--;

            return result;
        }

        public static BigInteger operator +(BigInteger left, BigInteger right)
        {
            BigInteger result = new BigInteger();

            result.DataLength = (left.DataLength > right.DataLength) ? left.DataLength : right.DataLength;

            long carry = 0;
            for (int i = 0; i < result.DataLength; i++)
            {
                long sum = (long)left._data[i] + (long)right._data[i] + carry;
                carry = sum >> 32;
                result._data[i] = (uint)(sum & 0xFFFFFFFF);
            }

            if (carry != 0 && result.DataLength < MAX_LENGTH)
            {
                result._data[result.DataLength] = (uint)(carry);
                result.DataLength++;
            }

            while (result.DataLength > 1 && result._data[result.DataLength - 1] == 0)
                result.DataLength--;

            int lastPos = MAX_LENGTH - 1;
            if ((left._data[lastPos] & 0x80000000) == (right._data[lastPos] & 0x80000000) &&
               (result._data[lastPos] & 0x80000000) != (left._data[lastPos] & 0x80000000))
            {
                throw (new ArithmeticException());
            }

            return result;
        }
        public static BigInteger operator -(BigInteger left, BigInteger right)
        {
            BigInteger result = new BigInteger();

            result.DataLength = (left.DataLength > right.DataLength) ? left.DataLength : right.DataLength;

            long carryIn = 0;
            for (int i = 0; i < result.DataLength; i++)
            {
                long diff;

                diff = (long)left._data[i] - (long)right._data[i] - carryIn;
                result._data[i] = (uint)(diff & 0xFFFFFFFF);

                if (diff < 0)
                    carryIn = 1;
                else
                    carryIn = 0;
            }

            if (carryIn != 0)
            {
                for (int i = result.DataLength; i < MAX_LENGTH; i++)
                    result._data[i] = 0xFFFFFFFF;
                result.DataLength = MAX_LENGTH;
            }

            while (result.DataLength > 1 && result._data[result.DataLength - 1] == 0)
                result.DataLength--;

            int lastPos = MAX_LENGTH - 1;
            if ((left._data[lastPos] & 0x80000000) != (right._data[lastPos] & 0x80000000) &&
               (result._data[lastPos] & 0x80000000) != (left._data[lastPos] & 0x80000000))
            {
                throw (new ArithmeticException());
            }

            return result;
        }
        public static BigInteger operator *(BigInteger left, BigInteger right)
        {
            int lastPos = MAX_LENGTH - 1;
            bool bi1Neg = false, bi2Neg = false;

            try
            {
                if ((left._data[lastPos] & 0x80000000) != 0)
                {
                    bi1Neg = true; left = -left;
                }
                if ((right._data[lastPos] & 0x80000000) != 0)
                {
                    bi2Neg = true; right = -right;
                }
            }
            catch (Exception) { }

            BigInteger result = new BigInteger();

            try
            {
                for (int i = 0; i < left.DataLength; i++)
                {
                    if (left._data[i] == 0) continue;

                    ulong mcarry = 0;
                    for (int j = 0, k = i; j < right.DataLength; j++, k++)
                    {
                        ulong val = ((ulong)left._data[i] * (ulong)right._data[j]) +
                                     (ulong)result._data[k] + mcarry;

                        result._data[k] = (uint)(val & 0xFFFFFFFF);
                        mcarry = (val >> 32);
                    }

                    if (mcarry != 0)
                        result._data[i + right.DataLength] = (uint)mcarry;
                }
            }
            catch (Exception)
            {
                throw (new ArithmeticException("Multiplication overflow."));
            }


            result.DataLength = left.DataLength + right.DataLength;
            if (result.DataLength > MAX_LENGTH)
                result.DataLength = MAX_LENGTH;

            while (result.DataLength > 1 && result._data[result.DataLength - 1] == 0)
                result.DataLength--;

            if ((result._data[lastPos] & 0x80000000) != 0)
            {
                if (bi1Neg != bi2Neg && result._data[lastPos] == 0x80000000)
                {
                    if (result.DataLength == 1)
                        return result;
                    else
                    {
                        bool isMaxNeg = true;
                        for (int i = 0; i < result.DataLength - 1 && isMaxNeg; i++)
                        {
                            if (result._data[i] != 0)
                                isMaxNeg = false;
                        }

                        if (isMaxNeg)
                            return result;
                    }
                }

                throw (new ArithmeticException("Multiplication overflow."));
            }

            if (bi1Neg != bi2Neg)
                return -result;

            return result;
        }

        public static BigInteger operator <<(BigInteger instance, int shift)
        {
            BigInteger result = new BigInteger(instance);
            result.DataLength = ShiftLeft(result._data, shift);

            return result;
        }
        private static int ShiftLeft(uint[] buffer, int shift)
        {
            int shiftAmount = 32;
            int bufLen = buffer.Length;

            while (bufLen > 1 && buffer[bufLen - 1] == 0)
                bufLen--;

            for (int count = shift; count > 0;)
            {
                if (count < shiftAmount)
                    shiftAmount = count;

                ulong carry = 0;
                for (int i = 0; i < bufLen; i++)
                {
                    ulong val = ((ulong)buffer[i]) << shiftAmount;
                    val |= carry;

                    buffer[i] = (uint)(val & 0xFFFFFFFF);
                    carry = val >> 32;
                }

                if (carry != 0)
                {
                    if (bufLen + 1 <= buffer.Length)
                    {
                        buffer[bufLen] = (uint)carry;
                        bufLen++;
                    }
                }
                count -= shiftAmount;
            }
            return bufLen;
        }

        public static BigInteger operator >>(BigInteger instance, int shift)
        {
            BigInteger result = new BigInteger(instance);
            result.DataLength = ShiftRight(result._data, shift);


            if ((instance._data[MAX_LENGTH - 1] & 0x80000000) != 0)
            {
                for (int i = MAX_LENGTH - 1; i >= result.DataLength; i--)
                    result._data[i] = 0xFFFFFFFF;

                uint mask = 0x80000000;
                for (int i = 0; i < 32; i++)
                {
                    if ((result._data[result.DataLength - 1] & mask) != 0)
                        break;

                    result._data[result.DataLength - 1] |= mask;
                    mask >>= 1;
                }
                result.DataLength = MAX_LENGTH;
            }

            return result;
        }
        private static int ShiftRight(uint[] buffer, int shift)
        {
            int shiftAmount = 32;
            int invShift = 0;
            int bufLen = buffer.Length;

            while (bufLen > 1 && buffer[bufLen - 1] == 0)
                bufLen--;

            for (int count = shift; count > 0;)
            {
                if (count < shiftAmount)
                {
                    shiftAmount = count;
                    invShift = 32 - shiftAmount;
                }

                ulong carry = 0;
                for (int i = bufLen - 1; i >= 0; i--)
                {
                    ulong val = ((ulong)buffer[i]) >> shiftAmount;
                    val |= carry;

                    carry = ((ulong)buffer[i]) << invShift;
                    buffer[i] = (uint)(val);
                }

                count -= shiftAmount;
            }

            while (bufLen > 1 && buffer[bufLen - 1] == 0)
                bufLen--;

            return bufLen;
        }

        public static bool operator ==(BigInteger left, BigInteger right)
        {
            if (object.ReferenceEquals(left, right)) return true;
            if (((object)left == null) || ((object)right == null)) return false;

            return left.Equals(right);
        }
        public static bool operator !=(BigInteger left, BigInteger right) => !(left == right);

        public static bool operator >(BigInteger left, BigInteger right)
        {
            int pos = MAX_LENGTH - 1;

            if ((left._data[pos] & 0x80000000) != 0 && (right._data[pos] & 0x80000000) == 0)
                return false;

            else if ((left._data[pos] & 0x80000000) == 0 && (right._data[pos] & 0x80000000) != 0)
                return true;

            int len = (left.DataLength > right.DataLength) ? left.DataLength : right.DataLength;
            for (pos = len - 1; pos >= 0 && left._data[pos] == right._data[pos]; pos--) ;

            if (pos >= 0)
            {
                if (left._data[pos] > right._data[pos])
                    return true;
                return false;
            }
            return false;
        }
        public static bool operator <(BigInteger left, BigInteger right)
        {
            int pos = MAX_LENGTH - 1;

            if ((left._data[pos] & 0x80000000) != 0 && (right._data[pos] & 0x80000000) == 0)
                return true;

            else if ((left._data[pos] & 0x80000000) == 0 && (right._data[pos] & 0x80000000) != 0)
                return false;

            int len = (left.DataLength > right.DataLength) ? left.DataLength : right.DataLength;
            for (pos = len - 1; pos >= 0 && left._data[pos] == right._data[pos]; pos--) ;

            if (pos >= 0)
            {
                if (left._data[pos] < right._data[pos])
                    return true;
                return false;
            }
            return false;
        }
        public static bool operator >=(BigInteger left, BigInteger right) => (left == right || left > right);
        public static bool operator <=(BigInteger left, BigInteger right) => (left == right || left < right);
        
        private bool LucasStrongTestHelper(BigInteger thisVal)
        {
            long D = 5, sign = -1, dCount = 0;
            bool done = false;

            while (!done)
            {
                int Jresult = Jacobi(D, thisVal);

                if (Jresult == -1)
                    done = true;
                else
                {
                    if (Jresult == 0 && Math.Abs(D) < thisVal)
                        return false;

                    if (dCount == 20)
                    {
                        BigInteger root = thisVal.Sqrt();
                        if (root * root == thisVal)
                            return false;
                    }
                    D = (Math.Abs(D) + 2) * sign;
                    sign = -sign;
                }
                dCount++;
            }

            long Q = (1 - D) >> 2;

            BigInteger p_add1 = thisVal + 1;
            int s = 0;

            for (int index = 0; index < p_add1.DataLength; index++)
            {
                uint mask = 0x01;

                for (int i = 0; i < 32; i++)
                {
                    if ((p_add1._data[index] & mask) != 0)
                    {
                        index = p_add1.DataLength;
                        break;
                    }
                    mask <<= 1;
                    s++;
                }
            }

            BigInteger t = p_add1 >> s;

            BigInteger constant = new BigInteger();

            int nLen = thisVal.DataLength << 1;
            constant._data[nLen] = 0x00000001;
            constant.DataLength = nLen + 1;

            constant = constant / thisVal;

            BigInteger[] lucas = LucasSequenceHelper(1, Q, t, thisVal, constant, 0);
            bool isPrime = false;

            if ((lucas[0].DataLength == 1 && lucas[0]._data[0] == 0) ||
               (lucas[1].DataLength == 1 && lucas[1]._data[0] == 0))
            {
                isPrime = true;
            }

            for (int i = 1; i < s; i++)
            {
                if (!isPrime)
                {
                    lucas[1] = thisVal.BarrettReduction(lucas[1] * lucas[1], thisVal, constant);
                    lucas[1] = (lucas[1] - (lucas[2] << 1)) % thisVal;

                    lucas[1] = ((lucas[1] * lucas[1]) - (lucas[2] << 1)) % thisVal;

                    if ((lucas[1].DataLength == 1 && lucas[1]._data[0] == 0))
                        isPrime = true;
                }

                lucas[2] = thisVal.BarrettReduction(lucas[2] * lucas[2], thisVal, constant);
            }


            if (isPrime)
            {
                BigInteger g = thisVal.Gcd(Q);
                if (g.DataLength == 1 && g._data[0] == 1)
                {
                    if ((lucas[2]._data[MAX_LENGTH - 1] & 0x80000000) != 0)
                        lucas[2] += thisVal;

                    BigInteger temp = (Q * BigInteger.Jacobi(Q, thisVal)) % thisVal;
                    if ((temp._data[MAX_LENGTH - 1] & 0x80000000) != 0)
                        temp += thisVal;

                    if (lucas[2] != temp)
                        isPrime = false;
                }
            }

            return isPrime;
        }

        private static void SingleByteDivide(BigInteger bi1, BigInteger bi2, BigInteger outQuotient, BigInteger outRemainder)
        {
            uint[] result = new uint[MAX_LENGTH];
            int resultPos = 0;

            for (int i = 0; i < MAX_LENGTH; i++)
                outRemainder._data[i] = bi1._data[i];
            outRemainder.DataLength = bi1.DataLength;

            while (outRemainder.DataLength > 1 && outRemainder._data[outRemainder.DataLength - 1] == 0)
                outRemainder.DataLength--;

            ulong divisor = (ulong)bi2._data[0];
            int pos = outRemainder.DataLength - 1;
            ulong dividend = (ulong)outRemainder._data[pos];

            if (dividend >= divisor)
            {
                ulong quotient = dividend / divisor;
                result[resultPos++] = (uint)quotient;

                outRemainder._data[pos] = (uint)(dividend % divisor);
            }
            pos--;

            while (pos >= 0)
            {
                dividend = ((ulong)outRemainder._data[pos + 1] << 32) + (ulong)outRemainder._data[pos];
                ulong quotient = dividend / divisor;
                result[resultPos++] = (uint)quotient;

                outRemainder._data[pos + 1] = 0;
                outRemainder._data[pos--] = (uint)(dividend % divisor);
            }

            outQuotient.DataLength = resultPos;
            int j = 0;
            for (int i = outQuotient.DataLength - 1; i >= 0; i--, j++)
                outQuotient._data[j] = result[i];
            for (; j < MAX_LENGTH; j++)
                outQuotient._data[j] = 0;

            while (outQuotient.DataLength > 1 && outQuotient._data[outQuotient.DataLength - 1] == 0)
                outQuotient.DataLength--;

            if (outQuotient.DataLength == 0)
                outQuotient.DataLength = 1;

            while (outRemainder.DataLength > 1 && outRemainder._data[outRemainder.DataLength - 1] == 0)
                outRemainder.DataLength--;
        }
        private static BigInteger[] LucasSequenceHelper(BigInteger P, BigInteger Q, BigInteger k, BigInteger n, BigInteger constant, int s)
        {
            BigInteger[] result = new BigInteger[3];

            if ((k._data[0] & 0x00000001) == 0)
                throw (new ArgumentException("Argument k must be odd."));

            int numbits = k.BitCount();
            uint mask = (uint)0x1 << ((numbits & 0x1F) - 1);

            BigInteger v = 2 % n, Q_k = 1 % n,
                       v1 = P % n, u1 = Q_k;
            bool flag = true;

            for (int i = k.DataLength - 1; i >= 0; i--)
            {
                while (mask != 0)
                {
                    if (i == 0 && mask == 0x00000001)
                        break;

                    if ((k._data[i] & mask) != 0)
                    {
                        u1 = (u1 * v1) % n;

                        v = ((v * v1) - (P * Q_k)) % n;
                        v1 = n.BarrettReduction(v1 * v1, n, constant);
                        v1 = (v1 - ((Q_k * Q) << 1)) % n;

                        if (flag)
                            flag = false;
                        else
                            Q_k = n.BarrettReduction(Q_k * Q_k, n, constant);

                        Q_k = (Q_k * Q) % n;
                    }
                    else
                    {
                        u1 = ((u1 * v) - Q_k) % n;

                        v1 = ((v * v1) - (P * Q_k)) % n;
                        v = n.BarrettReduction(v * v, n, constant);
                        v = (v - (Q_k << 1)) % n;

                        if (flag)
                        {
                            Q_k = Q % n;
                            flag = false;
                        }
                        else
                            Q_k = n.BarrettReduction(Q_k * Q_k, n, constant);
                    }

                    mask >>= 1;
                }
                mask = 0x80000000;
            }

            u1 = ((u1 * v) - Q_k) % n;
            v = ((v * v1) - (P * Q_k)) % n;
            if (flag)
                flag = false;
            else
                Q_k = n.BarrettReduction(Q_k * Q_k, n, constant);

            Q_k = (Q_k * Q) % n;


            for (int i = 0; i < s; i++)
            {
                u1 = (u1 * v) % n;
                v = ((v * v) - (Q_k << 1)) % n;

                if (flag)
                {
                    Q_k = Q % n;
                    flag = false;
                }
                else
                    Q_k = n.BarrettReduction(Q_k * Q_k, n, constant);
            }

            result[0] = u1;
            result[1] = v;
            result[2] = Q_k;

            return result;
        }
        private static void MultipleByteDivide(BigInteger inDividend, BigInteger inDivisor, BigInteger outQuotient, BigInteger outRemainder)
        {
            uint[] result = new uint[MAX_LENGTH];

            int remainderLen = inDividend.DataLength + 1;
            uint[] remainder = new uint[remainderLen];

            uint mask = 0x80000000;
            uint val = inDivisor._data[inDivisor.DataLength - 1];
            int shift = 0, resultPos = 0;

            while (mask != 0 && (val & mask) == 0)
            {
                shift++; mask >>= 1;
            }

            for (int i = 0; i < inDividend.DataLength; i++)
                remainder[i] = inDividend._data[i];
            ShiftLeft(remainder, shift);
            inDivisor = inDivisor << shift;

            int j = remainderLen - inDivisor.DataLength;
            int pos = remainderLen - 1;

            ulong firstDivisorByte = inDivisor._data[inDivisor.DataLength - 1];
            ulong secondDivisorByte = inDivisor._data[inDivisor.DataLength - 2];

            int divisorLen = inDivisor.DataLength + 1;
            uint[] dividendPart = new uint[divisorLen];

            while (j > 0)
            {
                ulong dividend = ((ulong)remainder[pos] << 32) + (ulong)remainder[pos - 1];

                ulong q_hat = dividend / firstDivisorByte;
                ulong r_hat = dividend % firstDivisorByte;

                bool done = false;
                while (!done)
                {
                    done = true;

                    if (q_hat == 0x100000000 ||
                       (q_hat * secondDivisorByte) > ((r_hat << 32) + remainder[pos - 2]))
                    {
                        q_hat--;
                        r_hat += firstDivisorByte;

                        if (r_hat < 0x100000000)
                            done = false;
                    }
                }

                for (int h = 0; h < divisorLen; h++)
                    dividendPart[h] = remainder[pos - h];

                BigInteger kk = new BigInteger(dividendPart);
                BigInteger ss = inDivisor * (long)q_hat;

                while (ss > kk)
                {
                    q_hat--;
                    ss -= inDivisor;
                }
                BigInteger yy = kk - ss;

                for (int h = 0; h < divisorLen; h++)
                    remainder[pos - h] = yy._data[inDivisor.DataLength - h];

                result[resultPos++] = (uint)q_hat;
                pos--;
                j--;
            }

            outQuotient.DataLength = resultPos;
            int y = 0;
            for (int x = outQuotient.DataLength - 1; x >= 0; x--, y++)
                outQuotient._data[y] = result[x];
            for (; y < MAX_LENGTH; y++)
                outQuotient._data[y] = 0;

            while (outQuotient.DataLength > 1 && outQuotient._data[outQuotient.DataLength - 1] == 0)
                outQuotient.DataLength--;

            if (outQuotient.DataLength == 0)
                outQuotient.DataLength = 1;

            outRemainder.DataLength = ShiftRight(remainder, shift);

            for (y = 0; y < outRemainder.DataLength; y++)
                outRemainder._data[y] = remainder[y];
            for (; y < MAX_LENGTH; y++)
                outRemainder._data[y] = 0;
        }
        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _data = null;
            GC.Collect();
        }

        /// <summary>
        /// Returns a string that represents the current object in the specified base.
        /// </summary>
        /// <param name="radix">The base type of the string representation of the current object.</param>
        /// <returns>A string that represents the current object in the specified base.</returns>
        public string ToString(int radix)
        {
            if (radix < 2 || radix > 36)
                throw (new ArgumentException("Radix must be >= 2 and <= 36"));

            string charSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string result = "";

            BigInteger a = this;

            bool negative = false;
            if ((a._data[MAX_LENGTH - 1] & 0x80000000) != 0)
            {
                negative = true;
                try
                {
                    a = -a;
                }
                catch (Exception) { }
            }

            BigInteger quotient = new BigInteger();
            BigInteger remainder = new BigInteger();
            BigInteger biRadix = new BigInteger(radix);

            if (a.DataLength == 1 && a._data[0] == 0)
                result = "0";
            else
            {
                while (a.DataLength > 1 || (a.DataLength == 1 && a._data[0] != 0))
                {
                    SingleByteDivide(a, biRadix, quotient, remainder);

                    if (remainder._data[0] < 10)
                        result = remainder._data[0] + result;
                    else
                        result = charSet[(int)remainder._data[0] - 10] + result;

                    a = quotient;
                }
                if (negative)
                    result = "-" + result;
            }

            return result;
        }

        /// <summary>
        /// Returns a hex string showing the contents of the current instance.
        /// </summary>
        /// <returns></returns>
        public string ToHexString()
        {
            string result = _data[DataLength - 1].ToString("X");

            for (int i = DataLength - 2; i >= 0; i--)
            {
                result += _data[i].ToString("X8");
            }

            return result.ToLower();
        }

        /// <summary>
        /// Modulo Exponentiation.
        /// </summary>
        /// <param name="exponent"></param>
        /// <param name="modulus"></param>
        /// <returns></returns>
        public BigInteger ModPow(BigInteger exponent, BigInteger modulus)
        {
            if ((exponent._data[MAX_LENGTH - 1] & 0x80000000) != 0)
                throw (new ArithmeticException("Positive exponents only."));

            BigInteger resultNum = 1;
            BigInteger tempNum;
            bool thisNegative = false;

            if ((_data[MAX_LENGTH - 1] & 0x80000000) != 0)
            {
                tempNum = -this % modulus;
                thisNegative = true;
            }
            else
                tempNum = this % modulus;

            if ((modulus._data[MAX_LENGTH - 1] & 0x80000000) != 0)
                modulus = -modulus;

            BigInteger constant = new BigInteger();

            int i = modulus.DataLength << 1;
            constant._data[i] = 0x00000001;
            constant.DataLength = i + 1;

            constant = constant / modulus;
            int totalBits = exponent.BitCount();
            int count = 0;

            for (int pos = 0; pos < exponent.DataLength; pos++)
            {
                uint mask = 0x01;

                for (int index = 0; index < 32; index++)
                {
                    if ((exponent._data[pos] & mask) != 0)
                        resultNum = BarrettReduction(resultNum * tempNum, modulus, constant);

                    mask <<= 1;

                    tempNum = BarrettReduction(tempNum * tempNum, modulus, constant);


                    if (tempNum.DataLength == 1 && tempNum._data[0] == 1)
                    {
                        if (thisNegative && (exponent._data[0] & 0x1) != 0)
                            return -resultNum;
                        return resultNum;
                    }
                    count++;
                    if (count == totalBits)
                        break;
                }
            }

            if (thisNegative && (exponent._data[0] & 0x1) != 0)    //odd exp
                return -resultNum;

            return resultNum;
        }

        /// <summary>
        /// Fast calculation of modular reduction using Barrett's reduction.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="n"></param>
        /// <param name="constant"></param>
        /// <returns></returns>
        private BigInteger BarrettReduction(BigInteger x, BigInteger n, BigInteger constant)
        {
            int k = n.DataLength,
                kPlusOne = k + 1,
                kMinusOne = k - 1;

            BigInteger q1 = new BigInteger();

            for (int i = kMinusOne, j = 0; i < x.DataLength; i++, j++)
                q1._data[j] = x._data[i];
            q1.DataLength = x.DataLength - kMinusOne;
            if (q1.DataLength <= 0)
                q1.DataLength = 1;


            BigInteger q2 = q1 * constant;
            BigInteger q3 = new BigInteger();

            for (int i = kPlusOne, j = 0; i < q2.DataLength; i++, j++)
                q3._data[j] = q2._data[i];
            q3.DataLength = q2.DataLength - kPlusOne;
            if (q3.DataLength <= 0)
                q3.DataLength = 1;

            BigInteger r1 = new BigInteger();
            int lengthToCopy = (x.DataLength > kPlusOne) ? kPlusOne : x.DataLength;
            for (int i = 0; i < lengthToCopy; i++)
                r1._data[i] = x._data[i];
            r1.DataLength = lengthToCopy;

            BigInteger r2 = new BigInteger();
            for (int i = 0; i < q3.DataLength; i++)
            {
                if (q3._data[i] == 0) continue;

                ulong mcarry = 0;
                int t = i;
                for (int j = 0; j < n.DataLength && t < kPlusOne; j++, t++)
                {
                    ulong val = ((ulong)q3._data[i] * (ulong)n._data[j]) +
                                 (ulong)r2._data[t] + mcarry;

                    r2._data[t] = (uint)(val & 0xFFFFFFFF);
                    mcarry = (val >> 32);
                }

                if (t < kPlusOne)
                    r2._data[t] = (uint)mcarry;
            }
            r2.DataLength = kPlusOne;
            while (r2.DataLength > 1 && r2._data[r2.DataLength - 1] == 0)
                r2.DataLength--;

            r1 -= r2;
            if ((r1._data[MAX_LENGTH - 1] & 0x80000000) != 0)
            {
                BigInteger val = new BigInteger();
                val._data[kPlusOne] = 0x00000001;
                val.DataLength = kPlusOne + 1;
                r1 += val;
            }

            while (r1 >= n)
                r1 -= n;

            return r1;
        }

        /// <summary>
        /// Returns the greatest common denominator against the specified BigInteger instance.
        /// </summary>
        /// <param name="bi">The BigInteger instance to use with the current instance.</param>
        /// <returns></returns>
        public BigInteger Gcd(BigInteger bi)
        {
            BigInteger x, y;

            if ((_data[MAX_LENGTH - 1] & 0x80000000) != 0)
                x = -this;
            else
                x = this;

            if ((bi._data[MAX_LENGTH - 1] & 0x80000000) != 0)
                y = -bi;
            else
                y = bi;

            BigInteger g = y;

            while (x.DataLength > 1 || (x.DataLength == 1 && x._data[0] != 0))
            {
                g = x;
                x = y % x;
                y = g;
            }

            return g;
        }

        /// <summary>
        /// Populates the current instance with the specified amount of random bytes.
        /// </summary>
        /// <param name="bits">The amount of bits to populate the current instance.</param>
        /// <param name="rand">The System.Random instance to generate the random bytes.</param>
        public void GenRandomBits(int bits, Random rand)
        {
            int dwords = bits >> 5;
            int remBits = bits & 0x1F;

            if (remBits != 0)
                dwords++;

            if (dwords > MAX_LENGTH)
                throw (new ArithmeticException("Number of required bits > maxLength."));

            for (int i = 0; i < dwords; i++)
                _data[i] = (uint)(rand.NextDouble() * 0x100000000);

            for (int i = dwords; i < MAX_LENGTH; i++)
                _data[i] = 0;

            if (remBits != 0)
            {
                uint mask = (uint)(0x01 << (remBits - 1));
                _data[dwords - 1] |= mask;

                mask = (uint)(0xFFFFFFFF >> (32 - remBits));
                _data[dwords - 1] &= mask;
            }
            else
                _data[dwords - 1] |= 0x80000000;

            DataLength = dwords;

            if (DataLength == 0)
                DataLength = 1;
        }

        /// <summary>
        /// Returns the position of the most significant bit in the current instance.
        /// </summary>
        /// <returns>the most significant bit</returns>
        public int BitCount()
        {
            while (DataLength > 1 && _data[DataLength - 1] == 0)
                DataLength--;

            uint value = _data[DataLength - 1];
            uint mask = 0x80000000;
            int bits = 32;

            while (bits > 0 && (value & mask) == 0)
            {
                bits--;
                mask >>= 1;
            }
            bits += ((DataLength - 1) << 5);

            return bits;
        }

        /// <summary>
        /// Probabilistic prime test based on Rabin-Miller's test.
        /// </summary>
        /// <param name="confidence">The amount of times/iterations to check the primality of the current instance.</param>
        /// <returns>true if the current instance is a strong pseudo-prime to randomly chosen bases, otherwise false if not prime.</returns>
        public bool RabinMillerTest(int confidence)
        {
            BigInteger thisVal;
            if ((_data[MAX_LENGTH - 1] & 0x80000000) != 0)
                thisVal = -this;
            else
                thisVal = this;

            if (thisVal.DataLength == 1)
            {
                if (thisVal._data[0] == 0 || thisVal._data[0] == 1)
                    return false;
                else if (thisVal._data[0] == 2 || thisVal._data[0] == 3)
                    return true;
            }

            if ((thisVal._data[0] & 0x1) == 0)
                return false;

            BigInteger p_sub1 = thisVal - (new BigInteger(1));
            int s = 0;

            for (int index = 0; index < p_sub1.DataLength; index++)
            {
                uint mask = 0x01;

                for (int i = 0; i < 32; i++)
                {
                    if ((p_sub1._data[index] & mask) != 0)
                    {
                        index = p_sub1.DataLength;
                        break;
                    }
                    mask <<= 1;
                    s++;
                }
            }

            BigInteger t = p_sub1 >> s;

            int bits = thisVal.BitCount();
            BigInteger a = new BigInteger();
            Random rand = new Random();

            for (int round = 0; round < confidence; round++)
            {
                bool done = false;

                while (!done)
                {
                    int testBits = 0;

                    while (testBits < 2)
                        testBits = (int)(rand.NextDouble() * bits);

                    a.GenRandomBits(testBits, rand);

                    int byteLen = a.DataLength;

                    if (byteLen > 1 || (byteLen == 1 && a._data[0] != 1))
                        done = true;
                }

                BigInteger gcdTest = a.Gcd(thisVal);
                if (gcdTest.DataLength == 1 && gcdTest._data[0] != 1)
                    return false;

                BigInteger b = a.ModPow(t, thisVal);

                bool result = false;

                if (b.DataLength == 1 && b._data[0] == 1)
                    result = true;

                for (int j = 0; result == false && j < s; j++)
                {
                    if (b == p_sub1)
                    {
                        result = true;
                        break;
                    }

                    b = (b * b) % thisVal;
                }

                if (result == false)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Determines whether a number is probably prime using the Rabin-Miller's test.
        /// </summary>
        /// <param name="confidence">The amount of times/iterations to check the primality of the current instance.</param>
        /// <returns>true if the instance has a large probability of being prime, otherwise false.</returns>
        public bool IsProbablePrime(int confidence)
        {
            BigInteger thisVal;
            if ((this._data[MAX_LENGTH - 1] & 0x80000000) != 0)
                thisVal = -this;
            else
                thisVal = this;

            for (int p = 0; p < PrimesBelow2000.Length; p++)
            {
                BigInteger divisor = PrimesBelow2000[p];

                if (divisor >= thisVal)
                    break;

                BigInteger resultNum = thisVal % divisor;
                if (resultNum.IntValue() == 0)
                    return false;
            }

            if (thisVal.RabinMillerTest(confidence)) return true;
            else return false;
        }

        /// <summary>
        /// Determines whether this BigInteger is probably prime using a combination of base 2 strong pseudo-prime tests, and Lucas strong pseudo-prime tests.
        /// </summary>
        /// <returns>true if the instance has a large probability of being prime, otherwise false.</returns>
        public bool IsProbablePrime()
        {
            BigInteger thisVal;
            if ((this._data[MAX_LENGTH - 1] & 0x80000000) != 0)
                thisVal = -this;
            else
                thisVal = this;

            if (thisVal.DataLength == 1)
            {
                if (thisVal._data[0] == 0 || thisVal._data[0] == 1)
                    return false;
                else if (thisVal._data[0] == 2 || thisVal._data[0] == 3)
                    return true;
            }

            if ((thisVal._data[0] & 0x1) == 0)
                return false;

            for (int p = 0; p < PrimesBelow2000.Length; p++)
            {
                BigInteger divisor = PrimesBelow2000[p];

                if (divisor >= thisVal)
                    break;

                BigInteger resultNum = thisVal % divisor;
                if (resultNum.IntValue() == 0)
                    return false;
            }

            BigInteger p_sub1 = thisVal - (new BigInteger(1));
            int s = 0;

            for (int index = 0; index < p_sub1.DataLength; index++)
            {
                uint mask = 0x01;

                for (int i = 0; i < 32; i++)
                {
                    if ((p_sub1._data[index] & mask) != 0)
                    {
                        index = p_sub1.DataLength;
                        break;
                    }
                    mask <<= 1;
                    s++;
                }
            }

            BigInteger t = p_sub1 >> s;

            int bits = thisVal.BitCount();
            BigInteger a = 2;

            BigInteger b = a.ModPow(t, thisVal);
            bool result = false;

            if (b.DataLength == 1 && b._data[0] == 1)
                result = true;

            for (int j = 0; result == false && j < s; j++)
            {
                if (b == p_sub1)
                {
                    result = true;
                    break;
                }

                b = (b * b) % thisVal;
            }

            if (result)
                result = LucasStrongTestHelper(thisVal);

            return result;
        }

        /// <summary>
        /// Returns the lowest System.Int32 from the byte array of the current instance. (4 bytes)
        /// </summary>
        /// <returns></returns>
        public int IntValue() => (int)_data[0];

        /// <summary>
        /// Returns the Jacobi symbol for the provided BigInteger values.
        /// </summary>
        /// <param name="a">The BigInteger value to use with the calculation.</param>
        /// <param name="b">The BigInteger value to use with the calculation.</param>
        /// <returns></returns>
        public static int Jacobi(BigInteger a, BigInteger b)
        {
            if ((b._data[0] & 0x1) == 0)
                throw (new ArgumentException("Jacobi defined only for odd integers."));

            if (a >= b) a %= b;
            if (a.DataLength == 1 && a._data[0] == 0) return 0;
            if (a.DataLength == 1 && a._data[0] == 1) return 1;

            if (a < 0)
            {
                if ((((b - 1)._data[0]) & 0x2) == 0)
                    return Jacobi(-a, b);
                else
                    return -Jacobi(-a, b);
            }

            int e = 0;
            for (int index = 0; index < a.DataLength; index++)
            {
                uint mask = 0x01;

                for (int i = 0; i < 32; i++)
                {
                    if ((a._data[index] & mask) != 0)
                    {
                        index = a.DataLength;
                        break;
                    }
                    mask <<= 1;
                    e++;
                }
            }

            BigInteger a1 = a >> e;

            int s = 1;
            if ((e & 0x1) != 0 && ((b._data[0] & 0x7) == 3 || (b._data[0] & 0x7) == 5))
                s = -1;

            if ((b._data[0] & 0x3) == 3 && (a1._data[0] & 0x3) == 3)
                s = -s;

            if (a1.DataLength == 1 && a1._data[0] == 1)
                return s;
            else
                return (s * Jacobi(b % a1, a1));
        }

        /// <summary>
        /// Returns a positive BigInteger that is probably prime.
        /// </summary>
        /// <param name="bits">The bit size of the number you wish to generate.</param>
        /// <param name="confidence">The amount of times/iterations to check the primality of the generated number.</param>
        /// <param name="rand">The System.Random instance to extract pseudo values to contribute to the probable prime number.</param>
        /// <returns></returns>
        public static BigInteger GenPseudoPrime(int bits, int confidence, Random rand)
        {
            BigInteger result = new BigInteger();
            bool done = false;

            while (!done)
            {
                result.GenRandomBits(bits, rand);
                result._data[0] |= 0x01;
                done = result.IsProbablePrime(confidence);
            }
            return result;
        }

        /// <summary>
        /// Returns the modulo inverse of the current instance against the provided modulus parameter.
        /// </summary>
        /// <param name="modulus">The modulus needed for the calculation.</param>
        /// <returns></returns>
        public BigInteger ModInverse(BigInteger modulus)
        {
            BigInteger[] p = { 0, 1 };
            BigInteger[] q = new BigInteger[2];
            BigInteger[] r = { 0, 0 };

            int step = 0;

            BigInteger a = modulus;
            BigInteger b = this;

            while (b.DataLength > 1 || (b.DataLength == 1 && b._data[0] != 0))
            {
                BigInteger quotient = new BigInteger();
                BigInteger remainder = new BigInteger();

                if (step > 1)
                {
                    BigInteger pval = (p[0] - (p[1] * q[0])) % modulus;
                    p[0] = p[1];
                    p[1] = pval;
                }

                if (b.DataLength == 1)
                    SingleByteDivide(a, b, quotient, remainder);
                else
                    MultipleByteDivide(a, b, quotient, remainder);

                q[0] = q[1];
                r[0] = r[1];
                q[1] = quotient; r[1] = remainder;

                a = b;
                b = remainder;

                step++;
            }

            if (r[0].DataLength > 1 || (r[0].DataLength == 1 && r[0]._data[0] != 1))
                throw (new ArithmeticException("No inverse!"));

            BigInteger result = ((p[0] - (p[1] * q[0])) % modulus);

            if ((result._data[MAX_LENGTH - 1] & 0x80000000) != 0)
                result += modulus;

            return result;
        }

        /// <summary>
        /// Returns this instance in a byte array, the lowest index contains the most smallest byte.
        /// </summary>
        /// <returns>the byte array</returns>
        public byte[] ToBytes()
        {
            int numBits = BitCount();

            int numBytes = numBits >> 3;
            if ((numBits & 0x7) != 0)
                numBytes++;

            byte[] result = new byte[numBytes];

            int pos = 0;
            uint tempVal, val = _data[DataLength - 1];

            if ((tempVal = (val >> 24 & 0xFF)) != 0)
                result[pos++] = (byte)tempVal;
            if ((tempVal = (val >> 16 & 0xFF)) != 0)
                result[pos++] = (byte)tempVal;
            if ((tempVal = (val >> 8 & 0xFF)) != 0)
                result[pos++] = (byte)tempVal;
            if ((tempVal = (val & 0xFF)) != 0)
                result[pos++] = (byte)tempVal;

            for (int i = DataLength - 2; i >= 0; i--, pos += 4)
            {
                val = _data[i];
                result[pos + 3] = (byte)(val & 0xFF);
                val >>= 8;
                result[pos + 2] = (byte)(val & 0xFF);
                val >>= 8;
                result[pos + 1] = (byte)(val & 0xFF);
                val >>= 8;
                result[pos] = (byte)(val & 0xFF);
            }

            return result;
        }

        /// <summary>
        /// Returns the square root of this instance.
        /// </summary>
        /// <returns>the square root</returns>
        public BigInteger Sqrt()
        {
            uint numBits = (uint)this.BitCount();

            if ((numBits & 0x1) != 0)
                numBits = (numBits >> 1) + 1;
            else
                numBits = (numBits >> 1);

            uint bytePos = numBits >> 5;
            byte bitPos = (byte)(numBits & 0x1F);

            uint mask;

            BigInteger result = new BigInteger();
            if (bitPos == 0)
                mask = 0x80000000;
            else
            {
                mask = (uint)1 << bitPos;
                bytePos++;
            }
            result.DataLength = (int)bytePos;

            for (int i = (int)bytePos - 1; i >= 0; i--)
            {
                while (mask != 0)
                {
                    result._data[i] ^= mask;

                    if ((result * result) > this)
                        result._data[i] ^= mask;

                    mask >>= 1;
                }
                mask = 0x80000000;
            }
            return result;
        }

        /// <summary>
        /// Returns a string that represents the current object in base 10.
        /// </summary>
        /// <returns>A string that represents the current object in base 10.</returns>
        public override string ToString() => ToString(10);
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current System.Object.</returns>
        public override int GetHashCode() => ToString().GetHashCode();
        /// <summary>
        /// Determines whether the specified System.Object is equal to the current System.Object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified System.Object is equal to the current System.Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            BigInteger bi = (BigInteger)obj;

            if (DataLength != bi.DataLength)
                return false;

            for (int i = 0; i < DataLength; i++)
            {
                if (_data[i] != bi._data[i])
                    return false;
            }
            return true;
        }
    }
}