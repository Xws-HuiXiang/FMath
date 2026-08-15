using System;

namespace FixedMath.BaseType
{
    /// <summary>
    /// 定点数数学库内部用于大整数乘除法的中间数据结构
    /// </summary>
    internal struct FInt128
    {
        /// <summary>
        /// 高64位
        /// </summary>
        public long High;
        /// <summary>
        /// 低64位
        /// </summary>
        public ulong Low;

        internal bool IsZero => High == 0 && Low == 0;
        internal bool IsNegative => High < 0;

        internal static FInt128 Zero => default;

        internal static FInt128 FromInt64(long value)
        {
            return new FInt128
            {
                High = value < 0 ? -1 : 0,
                Low = unchecked((ulong)value)
            };
        }

        internal static FInt128 FromUInt64(ulong value)
        {
            return new FInt128
            {
                High = 0,
                Low = value
            };
        }

        internal static ulong AbsToUInt64(long value)
        {
            if (value >= 0)
                return (ulong)value;

            return unchecked((ulong)(-(value + 1))) + 1UL;
        }

        internal static FInt128 Abs(FInt128 value)
        {
            if (value.IsNegative)
                return -value;

            return value;
        }

        /// <summary>
        /// 无符号长整型乘法运算
        /// <para>手动运算LongA*LongB的核心：</para>
        /// <para>LongA = Ahigh * 2^32 + Alow；LongB = Bhigt * 2^32 + Blow</para>
        /// <para>所以：LongA*LongB = Ahigh * Bhigh * 2^64 + (Ahigh * Blow + Alow * Bhigh) * 2^32 + Alow * Blow</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        internal static FInt128 MultiplyUnsigned(ulong a, ulong b)
        {
            ulong aLow = (uint)a;
            ulong aHigh = a >> 32;

            ulong bLow = (uint)b;
            ulong bHigh = b >> 32;

            ulong p00 = aLow * bLow;
            ulong p01 = aLow * bHigh;
            ulong p10 = aHigh * bLow;
            ulong p11 = aHigh * bHigh;

            ulong middle = (p00 >> 32) + (uint)p01 + (uint)p10;
            ulong low = (p00 & 0xFFFFFFFFUL) | (middle << 32);
            ulong high = p11 + (p01 >> 32) + (p10 >> 32) + (middle >> 32);

            return new FInt128
            {
                High = (long)high,
                Low = low
            };
        }

        internal static FInt128 MultiplySigned(long a, long b)
        {
            bool negative = (a < 0) ^ (b < 0);
            FInt128 result = MultiplyUnsigned(AbsToUInt64(a), AbsToUInt64(b));

            if (negative)
                result = -result;

            return result;
        }

        internal static FInt128 DivideSigned(FInt128 dividend, long divisor)
        {
            if (divisor == 0)
                throw new DivideByZeroException();

            bool negative = dividend.IsNegative ^ (divisor < 0);
            FInt128 absDividend = Abs(dividend);
            ulong absDivisor = AbsToUInt64(divisor);

            FInt128 quotient = DivideUnsigned(absDividend, absDivisor, out _);

            if (negative)
                quotient = -quotient;

            return quotient;
        }

        internal static FInt128 DivideUnsigned(FInt128 dividend, ulong divisor, out ulong remainder)
        {
            if (divisor == 0)
                throw new DivideByZeroException();

            FInt128 quotient = Zero;
            FInt128 rem = Zero;
            FInt128 divisorValue = FromUInt64(divisor);

            for (int i = 127; i >= 0; i--)
            {
                rem <<= 1;
                if (dividend.GetBit(i) != 0)
                    rem.Low |= 1UL;

                if (CompareUnsigned(rem, divisorValue) >= 0)
                {
                    rem -= divisorValue;
                    quotient.SetBit(i);
                }
            }

            remainder = rem.Low;
            return quotient;
        }

        internal ulong GetBit(int index)
        {
            if (index < 64)
                return (Low >> index) & 1UL;

            return (unchecked((ulong)High) >> (index - 64)) & 1UL;
        }

        internal void SetBit(int index)
        {
            if (index < 64)
            {
                Low |= 1UL << index;
                return;
            }

            High |= unchecked((long)(1UL << (index - 64)));
        }

        internal long ToInt64Checked()
        {
            if (High == 0)
            {
                if (Low <= long.MaxValue)
                    return (long)Low;
            }
            else if (High == -1)
            {
                if (Low >= 0x8000000000000000UL)
                    return unchecked((long)Low);
            }

            throw new OverflowException("FInt128 的值超出了 Int64 的范围");
        }

        internal static int Compare(FInt128 left, FInt128 right)
        {
            if (left.High < right.High)
                return -1;
            if (left.High > right.High)
                return 1;

            if (left.Low < right.Low)
                return -1;
            if (left.Low > right.Low)
                return 1;

            return 0;
        }

        internal static int CompareUnsigned(FInt128 left, FInt128 right)
        {
            ulong leftHigh = unchecked((ulong)left.High);
            ulong rightHigh = unchecked((ulong)right.High);

            if (leftHigh < rightHigh)
                return -1;
            if (leftHigh > rightHigh)
                return 1;

            if (left.Low < right.Low)
                return -1;
            if (left.Low > right.Low)
                return 1;

            return 0;
        }

        public static FInt128 operator +(FInt128 left, FInt128 right)
        {
            unchecked
            {
                ulong low = left.Low + right.Low;
                long high = left.High + right.High;

                if (low < left.Low)
                    high++;

                return new FInt128
                {
                    High = high,
                    Low = low
                };
            }
        }

        public static FInt128 operator -(FInt128 left, FInt128 right)
        {
            unchecked
            {
                ulong low = left.Low - right.Low;
                long high = left.High - right.High;

                if (left.Low < right.Low)
                    high--;

                return new FInt128
                {
                    High = high,
                    Low = low
                };
            }
        }

        public static FInt128 operator -(FInt128 value)
        {
            return Zero - value;
        }

        public static FInt128 operator <<(FInt128 value, int shift)
        {
            if (shift <= 0)
                return shift == 0 ? value : value >> -shift;
            if (shift >= 128)
                return Zero;

            if (shift >= 64)
            {
                int innerShift = shift - 64;
                ulong newHighBits = value.Low << innerShift;

                return new FInt128
                {
                    High = unchecked((long)newHighBits),
                    Low = 0
                };
            }

            ulong highBits = unchecked((ulong)value.High);
            ulong newLow = value.Low << shift;
            ulong newHigh = (highBits << shift) | (value.Low >> (64 - shift));

            return new FInt128
            {
                High = unchecked((long)newHigh),
                Low = newLow
            };
        }

        public static FInt128 operator >>(FInt128 value, int shift)
        {
            if (shift <= 0)
                return shift == 0 ? value : value << -shift;
            if (shift >= 128)
                return value.IsNegative ? new FInt128 { High = -1, Low = ulong.MaxValue } : Zero;

            if (shift >= 64)
            {
                int innerShift = shift - 64;
                long newHigh = value.High < 0 ? -1L : 0L;
                ulong newLow = unchecked((ulong)(value.High >> innerShift));

                return new FInt128
                {
                    High = newHigh,
                    Low = newLow
                };
            }

            long shiftedHigh = value.High >> shift;
            ulong highBits = unchecked((ulong)value.High);
            ulong newLowBits = (value.Low >> shift) | (highBits << (64 - shift));

            return new FInt128
            {
                High = shiftedHigh,
                Low = newLowBits
            };
        }
    }
}
