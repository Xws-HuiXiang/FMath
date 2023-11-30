using System;
using System.Collections.Generic;
using System.Text;

namespace FMath
{
    /// <summary>
    /// float 类型对应的定点数类型
    /// </summary>
    public struct FInt
    {
        /// <summary>
        /// 扩大数值的左移位数。值越大精度越高
        /// <para>定点数实现核心为扩大扩大浮点数的值，这个值即为原本数值左移的位数</para>
        /// <para>默认为10即左移10位，数值将扩大1024（2的10次方）倍</para>
        /// </summary>
        public static UInt16 BitMoveCount { get; set; } = 10;
        /// <summary>
        /// 扩大数值的倍数
        /// </summary>
        public static long MULTIPLER_FACTOR = 1 << BitMoveCount;

        private long scaledValue;
        /// <summary>
        /// 原本数值被缩放后的值
        /// </summary>
        public long ScaledValue
        {
            get { return scaledValue; }
            set { scaledValue = value; }
        }

        /// <summary>
        /// 内部使用的构造函数
        /// </summary>
        /// <param name="scaledValue">已经缩放完成的构造函数</param>
        private FInt(long scaledValue)
        {
            this.scaledValue = scaledValue;
        }

        public FInt(int value)
        {
            scaledValue = value * MULTIPLER_FACTOR;
        }

        public FInt(float value)
        {
            scaledValue = (long)Math.Round(value * MULTIPLER_FACTOR);
        }

        public FInt(double value)
        {
            scaledValue = (long)Math.Round(value * MULTIPLER_FACTOR);
        }

        /// <summary>
        /// 定点数对应的浮点数
        /// </summary>
        public readonly float Float
        {
            get { return scaledValue * 1.0f / MULTIPLER_FACTOR; }
        }

        /// <summary>
        /// 定点数对应的浮点数
        /// </summary>
        public readonly double Double
        {
            get { return scaledValue * 1.0 / MULTIPLER_FACTOR; }
        }

        /// <summary>
        /// 定点数对应的整数
        /// </summary>
        public readonly int Int
        {
            get { return (int)(scaledValue >> BitMoveCount); }
        }

        #region 运算符重载
        public static FInt operator -(FInt value)
        {
            return new FInt(-value.scaledValue);
        }

        public static bool operator ==(FInt left, FInt right)
        {
            return left.scaledValue == right.scaledValue;
        }

        public static bool operator !=(FInt left, FInt right)
        {
            return left.scaledValue != right.scaledValue;
        }

        public static bool operator >(FInt left, FInt right)
        {
            return left.scaledValue > right.scaledValue;
        }

        public static bool operator <(FInt left, FInt right)
        {
            return left.scaledValue < right.scaledValue;
        }

        public static bool operator >=(FInt left, FInt right)
        {
            return left.scaledValue >= right.scaledValue;
        }

        public static bool operator <=(FInt left, FInt right)
        {
            return left.scaledValue <= right.scaledValue;
        }

        public static FInt operator >>(FInt value, int bitMoveCount)
        {
            return new FInt(value.scaledValue >> bitMoveCount);
        }

        public static FInt operator <<(FInt value, int bitMoveCount)
        {
            return new FInt(value.scaledValue << bitMoveCount);
        }
        #endregion

        public override string ToString()
        {
            return this.Double.ToString();
        }

        #region 打印定点数的真实值
        /// <summary>
        /// 打印 int 类型的值
        /// </summary>
        public void DumpInt()
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log(Int.ToString());
#else
            Console.WriteLine(Int.ToString());
#endif
        }

        /// <summary>
        /// 打印 float 类型的值
        /// </summary>
        public void DumpFloat()
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log(Float.ToString());
#else
            Console.WriteLine(Float.ToString());
#endif
        }

        /// <summary>
        /// 打印 double 类型的值
        /// </summary>
        public void DumpDouble()
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log(Double.ToString());
#else
            Console.WriteLine(Double.ToString());
#endif
        }
        #endregion
    }
}
