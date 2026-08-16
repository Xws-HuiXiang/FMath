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
        /// <summary>
        /// 是否为0
        /// </summary>
        internal bool IsZero => High == 0 && Low == 0;
        /// <summary>
        /// 是否为负数
        /// </summary>
        internal bool IsNegative => High < 0;
        /// <summary>
        /// 数字0
        /// </summary>
        internal static FInt128 Zero => default;

        /// <summary>
        /// 从 long 类型转换为 FInt128
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static FInt128 FromInt64(long value)
        {
            return new FInt128
            {
                High = value < 0 ? -1 : 0,
                Low = unchecked((ulong)value)
            };
        }

        /// <summary>
        /// 从 ulong 类型转换为 FInt128
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static FInt128 FromUInt64(ulong value)
        {
            return new FInt128
            {
                High = 0,
                Low = value
            };
        }

        /// <summary>
        /// 转换为 ulong 类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static ulong AbsToUInt64(long value)
        {
            //非负数就是值本身
            if (value >= 0)
                return (ulong)value;

            //long.MinValue直接取整数会溢出
            //所以，先用负数+1，转换为对应的正数后再+1
            return unchecked((ulong)(-(value + 1))) + 1UL;
        }

        /// <summary>
        /// 取绝对值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static FInt128 Abs(FInt128 value)
        {
            //如果是负数则取负，否则返回本身
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

        /// <summary>
        /// 有符号长整型乘法运算
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        internal static FInt128 MultiplySigned(long a, long b)
        {
            //先判断结果得符号。异或操作：同号得正，异号得负
            bool negative = (a < 0) ^ (b < 0);
            //两个值按照无符号进行乘法运算
            FInt128 result = MultiplyUnsigned(AbsToUInt64(a), AbsToUInt64(b));

            //如果结果是负数则取反
            if (negative)
                result = -result;

            return result;
        }

        /// <summary>
        /// 有符号整数除法
        /// </summary>
        /// <param name="dividend">被除数</param>
        /// <param name="divisor">除数</param>
        /// <returns></returns>
        /// <exception cref="DivideByZeroException"></exception>
        internal static FInt128 DivideSigned(FInt128 dividend, long divisor)
        {
            if (divisor == 0)
                throw new DivideByZeroException();

            //先判断结果得符号。异或操作：同号得正，异号得负
            bool negative = dividend.IsNegative ^ (divisor < 0);

            //先按无符号进行运算
            FInt128 absDividend = Abs(dividend);
            ulong absDivisor = AbsToUInt64(divisor);
            FInt128 quotient = DivideUnsigned(absDividend, absDivisor, out _);

            //还原结果得符号
            if (negative)
                quotient = -quotient;

            return quotient;
        }

        /// <summary>
        /// 无符号整数除法
        /// <para>采用二进制长除法，从最高位向最低位逐位试商</para>
        /// </summary>
        /// <param name="dividend">被除数</param>
        /// <param name="divisor">除数</param>
        /// <param name="remainder">余数</param>
        /// <returns></returns>
        /// <exception cref="DivideByZeroException"></exception>
        internal static FInt128 DivideUnsigned(FInt128 dividend, ulong divisor, out ulong remainder)
        {
            if (divisor == 0)
                throw new DivideByZeroException();

            FInt128 quotient = Zero;//累加构建的商
            FInt128 rem = Zero;//当前已经处理过的被除数高位部分形成的“部分余数”（也可以理解为当前试除的中间值）
            FInt128 divisorValue = FromUInt64(divisor);//数据格式转换，方便参与计算

            //手算二进制除法：
            //每一步，将当前的“余数”左移一位（相当于乘以2）
            //然后从被除数中“拉入”下一位
            //接着判断这个新值是否大于等于除数
            //如果够减，则商当前位为1，并从余数中减去除数；否则商当前位为0
            for (int i = 127; i >= 0; i--)
            {
                rem <<= 1;
                if (dividend.GetBit(i) != 0)
                    rem.Low |= 1UL;
                //比较rem和除数的大小
                if (CompareUnsigned(rem, divisorValue) >= 0)
                {
                    //当前位可以商1，因为rem够减除数
                    //从rem中减去除数，得到新的余数
                    rem -= divisorValue;
                    //将商的第i位设置为1
                    quotient.SetBit(i);
                }
            }

            //因为除数是ulong，且最后余数一定小于除数，所以余数一定在低位中，所以这里只取低位
            remainder = rem.Low;
            return quotient;
        }

        /// <summary>
        /// 获取指定位的值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal ulong GetBit(int index)
        {
            if (index < 64)
                return (Low >> index) & 1UL;

            return (unchecked((ulong)High) >> (index - 64)) & 1UL;
        }

        /// <summary>
        /// 将指定的位设置为1
        /// </summary>
        /// <param name="index"></param>
        internal void SetBit(int index)
        {
            if (index < 64)
            {
                Low |= 1UL << index;
                return;
            }

            High |= unchecked((long)(1UL << (index - 64)));
        }

        /// <summary>
        /// 将数据转换为long
        /// </summary>
        /// <returns></returns>
        /// <exception cref="OverflowException">如果数据超过long类型的上限，则抛出异常</exception>
        internal long ToInt64Checked()
        {
            if (High == 0)
            {
                //正数直接转换是安全的
                if (Low <= long.MaxValue)
                    return (long)Low;
            }
            else if (High == -1)
            {
                //Low = 0x8000000000000000 对应数值 -9223372036854775808（即 long.MinValue）
                //这是long能表示的最小的数
                //Low = 0xFFFFFFFFFFFFFFFF 对应数值 -1
                //因此，要能放入 long，负数的绝对值必须小于等于 2⁶³，即 Low 必须 大于等于 0x8000000000000000
                if (Low >= 0x8000000000000000UL)
                    return unchecked((long)Low);//这里不是做数值转换，在 C# 默认的 checked 上下文下，会抛出溢出异常，因为编译器认为将大于 long.MaxValue 的无符号数转为有符号数属于溢出。所以关闭溢出检查
            }

            throw new OverflowException("FInt128 的值超出了 Int64 的范围");
        }

        /// <summary>
        /// 有符号数值比较
        /// <para>如果left小于right，则返回-1；若left大于right，则返回1；否则返回0</para>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 无符号数值比较
        /// <para>如果left小于right，则返回-1；若left大于right，则返回1；否则返回0</para>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 加法运算
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 减法运算
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 取负值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FInt128 operator -(FInt128 value)
        {
            return Zero - value;
        }

        /// <summary>
        /// 按位左移
        /// </summary>
        /// <param name="value"></param>
        /// <param name="shift"></param>
        /// <returns></returns>
        public static FInt128 operator <<(FInt128 value, int shift)
        {
            if (shift <= 0)
                return shift == 0 ? value : value >> -shift;
            if (shift >= 128)
                return Zero;

            //因为是分为高低位的，同时处理高位和低位的位移操作
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

        /// <summary>
        /// 按位右移
        /// </summary>
        /// <param name="value"></param>
        /// <param name="shift"></param>
        /// <returns></returns>
        public static FInt128 operator >>(FInt128 value, int shift)
        {
            if (shift <= 0)
                return shift == 0 ? value : value << -shift;
            if (shift >= 128)
                return value.IsNegative ? new FInt128 { High = -1, Low = ulong.MaxValue } : Zero;

            //因为是分为高低位的，同时处理高位和低位的位移操作
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
