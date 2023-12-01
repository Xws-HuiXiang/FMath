using System;
using System.Collections.Generic;
using System.Text;

namespace FMath
{
    /// <summary>
    /// float 类型对应的定点数类型
    /// </summary>
    public struct FFloat
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
        private FFloat(long scaledValue)
        {
            this.scaledValue = scaledValue;
        }

        public FFloat(int value)
        {
            scaledValue = value * MULTIPLER_FACTOR;
        }

        public FFloat(float value)
        {
            scaledValue = (long)Math.Round(value * MULTIPLER_FACTOR);
        }

        public FFloat(double value)
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
            //因为负数在计算机中使用补码表示，而运算减法为“加一个对应的负数（加一个反码）”，加完以后所有位均为1，这时需要再加1即为对应的减法结果
            //所以这里多加了1需要处理掉
            get
            {
                if (scaledValue >= 0)
                    return (int)(scaledValue >> BitMoveCount);
                else
                    return -(int)(-scaledValue >> BitMoveCount);
            }
        }

        #region 运算符重载
        public static FFloat operator -(FFloat value)
        {
            return new FFloat(-value.scaledValue);
        }

        public static bool operator ==(FFloat left, FFloat right)
        {
            return left.scaledValue == right.scaledValue;
        }

        public static bool operator !=(FFloat left, FFloat right)
        {
            return left.scaledValue != right.scaledValue;
        }

        public static bool operator >(FFloat left, FFloat right)
        {
            return left.scaledValue > right.scaledValue;
        }

        public static bool operator <(FFloat left, FFloat right)
        {
            return left.scaledValue < right.scaledValue;
        }

        public static bool operator >=(FFloat left, FFloat right)
        {
            return left.scaledValue >= right.scaledValue;
        }

        public static bool operator <=(FFloat left, FFloat right)
        {
            return left.scaledValue <= right.scaledValue;
        }

        public static FFloat operator >>(FFloat value, int bitMoveCount)
        {
            if(value.scaledValue >= 0)
                return new FFloat(value.scaledValue >> bitMoveCount);
            else
                return new FFloat(-(-value.scaledValue) >> bitMoveCount);
        }

        public static FFloat operator <<(FFloat value, int bitMoveCount)
        {
            return new FFloat(value.scaledValue << bitMoveCount);
        }

        public static FFloat operator +(FFloat left, FFloat right)
        {
            return new FFloat(left.scaledValue + right.scaledValue);
        }

        public static FFloat operator -(FFloat left, FFloat right)
        {
            return new FFloat(left.scaledValue - right.scaledValue);
        }

        public static FFloat operator *(FFloat left, FFloat right)
        {
            //因为使用缩放后的值进行乘法会导致多乘一个倍数，所以这里要再缩小一次倍数
            long value = left.scaledValue * right.scaledValue;
            if (value >= 0)
                value >>= BitMoveCount;
            else
                value = -(-value >> BitMoveCount);

            return new FFloat(value);
        }

        public static FFloat operator /(FFloat left, FFloat right)
        {
            //因为使用缩放后的值进行除法会导致多除一个倍数，所以这里要再放大一次倍数
            if (right.scaledValue != 0)
                return new FFloat((left.scaledValue << BitMoveCount) / right.scaledValue);

            throw new DivideByZeroException();
        }
        #endregion

        #region 隐式转换和显示转换
        /// <summary>
        /// 显示转换。会损失精度
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator FFloat(float value)
        {
            return new FFloat((long)Math.Round(value * MULTIPLER_FACTOR));
        }

        /// <summary>
        /// 显示转换。会损失精度
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator FFloat(double value)
        {
            return new FFloat((long)Math.Round(value * MULTIPLER_FACTOR));
        }

        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator FFloat(int value)
        {
            return new FFloat(value);
        }
        #endregion

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is FFloat ff)
                return scaledValue == ff.scaledValue;

            return false;
        }

        public override int GetHashCode()
        {
            return scaledValue.GetHashCode();
        }

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
