using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
