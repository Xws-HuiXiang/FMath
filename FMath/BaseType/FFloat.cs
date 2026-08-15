using FixedMath.BaseType;
using System;
using System.Globalization;

namespace FixedMath
{
    /// <summary>
    /// float 类型对应的定点数类型
    /// </summary>
    public struct FFloat
    {
        /// <summary>
        /// 定点数 0
        /// </summary>
        public static readonly FFloat Zero = 0;
        /// <summary>
        /// 定点数 1
        /// </summary>
        public static readonly FFloat One = 1;

        /// <summary>
        /// 扩大数值的左移位数。值越大精度越高
        /// <para>定点数实现核心为扩大扩大浮点数的值，这个值即为原本数值左移的位数</para>
        /// <para>默认为16即左移16位，数值将扩大65536（2的16次方）倍</para>
        /// </summary>
        public const int BitMoveCount = 16;
        /// <summary>
        /// 扩大数值的倍数
        /// </summary>
        public const long MULTIPLER_FACTOR = 1L << BitMoveCount;

        private long rawValue;
        /// <summary>
        /// 定点数实际值
        /// </summary>
        public readonly long RawValue => rawValue;

        #region 构造函数
        /// <summary>
        /// 使用 int 类型构造定点数
        /// </summary>
        /// <param name="value"></param>
        public FFloat(int value) => this.rawValue = value * MULTIPLER_FACTOR;

        /// <summary>
        /// 使用 float 类型构造定点数
        /// </summary>
        /// <param name="value"></param>
        public FFloat(float value) => this.rawValue = (long)Math.Round(value * MULTIPLER_FACTOR);

        /// <summary>
        /// 使用 double 类型构造定点数
        /// </summary>
        /// <param name="value"></param>
        public FFloat(double value) => this.rawValue = (long)Math.Round(value * MULTIPLER_FACTOR);
        
        /// <summary>
        /// 根据原始值构建定点数
        /// </summary>
        /// <param name="rawValue">实际值</param>
        /// <param name="isRaw">是否需要缩放</param>
        private FFloat(long rawValue, bool isRaw)
        {
            if (isRaw)
                this.rawValue = rawValue;
            else
                this.rawValue = rawValue * MULTIPLER_FACTOR;
        }
        #endregion

        /// <summary>
        /// 从原始值构建定点数
        /// </summary>
        /// <param name="rawValue"></param>
        /// <returns></returns>
        public static FFloat FromRaw(long rawValue) => new FFloat(rawValue, true);
        /// <summary>
        /// 从原始值构建定点数
        /// </summary>
        /// <param name="rawValue"></param>
        /// <param name="isRaw"></param>
        /// <returns></returns>
        public static FFloat FromRaw(long rawValue, bool isRaw) => new FFloat(rawValue, isRaw);

        /// <summary>
        /// 定点数对应的浮点数
        /// </summary>
        public readonly float Float => rawValue / (float)MULTIPLER_FACTOR;

        /// <summary>
        /// 定点数对应的双精度浮点数
        /// </summary>
        public readonly double Double => rawValue / (double)MULTIPLER_FACTOR;

        /// <summary>
        /// 定点数对应的整数（向0截断）
        /// </summary>
        public readonly int Int => (int)(rawValue / MULTIPLER_FACTOR);

        /// <summary>
        /// 定点数对应的整数（向负无穷截断）
        /// </summary>
        public readonly int FloorToInt => (int)(rawValue >> BitMoveCount);

        /// <summary>
        /// 取整数。使用IEEE规范，为“四舍六入五取偶”
        /// </summary>
        public readonly int RoundToInt
        {
            get
            {
                long integerPart = rawValue / MULTIPLER_FACTOR;
                long remainder = rawValue % MULTIPLER_FACTOR;

                // C# % 对负数结果也是负数。
                // 统一转换成绝对的小数部分。
                long absRemainder = remainder >= 0
                    ? remainder
                    : -remainder;

                long half = MULTIPLER_FACTOR / 2;

                // 小于 0.5，直接取整数部分
                if (absRemainder < half)
                    return (int)integerPart;

                // 大于 0.5，向远离 0 的方向进一
                if (absRemainder > half)
                    return (int)(integerPart + (rawValue >= 0 ? 1 : -1));

                // 恰好 0.5：取偶数
                if ((integerPart & 1) == 0)
                    return (int)integerPart;

                return (int)(integerPart + (rawValue >= 0 ? 1 : -1));
            }
        }

        #region 运算符重载
        /// <summary>
        /// 定点数取反
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FFloat operator -(FFloat value) => FromRaw(-value.rawValue);

        /// <summary>
        /// 判断等于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(FFloat left, FFloat right) => left.rawValue == right.rawValue;

        /// <summary>
        /// 判断不等于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(FFloat left, FFloat right) => left.rawValue != right.rawValue;

        /// <summary>
        /// 判断大于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >(FFloat left, FFloat right) => left.rawValue > right.rawValue;

        /// <summary>
        /// 判断小与
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <(FFloat left, FFloat right) => left.rawValue < right.rawValue;

        /// <summary>
        /// 判断大于等于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >=(FFloat left, FFloat right) => left.rawValue >= right.rawValue;

        /// <summary>
        /// 判断小于等于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <=(FFloat left, FFloat right) => left.rawValue <= right.rawValue;

        /// <summary>
        /// 定点数右移
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bitMoveCount"></param>
        /// <returns></returns>
        public static FFloat operator >>(FFloat value, int bitMoveCount) => FromRaw(value.rawValue >> bitMoveCount);

        /// <summary>
        /// 定点数左移
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bitMoveCount"></param>
        /// <returns></returns>
        public static FFloat operator <<(FFloat value, int bitMoveCount) => FromRaw(value.rawValue << bitMoveCount);

        /// <summary>
        /// 定点数加法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FFloat operator +(FFloat left, FFloat right) => FromRaw(left.rawValue + right.rawValue);

        /// <summary>
        /// 定点数减法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FFloat operator -(FFloat left, FFloat right) => FromRaw(left.rawValue - right.rawValue);

        /// <summary>
        /// 定点数乘法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FFloat operator *(FFloat left, FFloat right)
        {
            long value = left.rawValue * right.rawValue;
            value /= MULTIPLER_FACTOR;

            return FromRaw(value);

            //使用FInt128作为乘法中间的缓冲区
            //FInt128 result = FInt128.MultiplyUnsigned(left.rawValue, right.rawValue);
            //result >>= BitMoveCount;

            //return FromRaw(result.ToInt64());
        }

        /// <summary>
        /// 定点数除法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        /// <exception cref="DivideByZeroException"></exception>
        public static FFloat operator /(FFloat left, FFloat right)
        {
            if (right.rawValue == 0)
                throw new DivideByZeroException();

            long value = (left.rawValue * MULTIPLER_FACTOR) / right.rawValue;

            return FromRaw(value);
        }

        /// <summary>
        /// 定点数取余
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FFloat operator %(FFloat left, FFloat right)
        {
            if (right.rawValue == 0)
                throw new DivideByZeroException();

            return FromRaw(left.rawValue % right.rawValue);
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

        /// <summary>
        /// 定点数显示转换为浮点数
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator float(FFloat value)
        {
            return value.Float;
        }

        /// <summary>
        /// 定点数显示转换为双精度浮点数
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator double(FFloat value)
        {
            return value.Double;
        }

        /// <summary>
        /// 定点数显示转换为整数
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator int(FFloat value)
        {
            return value.Int;
        }
        #endregion

        /// <summary>
        /// 判断对象是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is FFloat ff)
                return rawValue == ff.rawValue;

            return false;
        }

        /// <summary>
        /// 返回这个对象的 HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => rawValue.GetHashCode();

        /// <summary>
        /// 返回对象的 double 值的字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Double.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 返回对象的 double 值的字符串并保留指定位数的小数部分
        /// </summary>
        /// <param name="decimalPlaces"></param>
        /// <returns></returns>
        public string ToString(int decimalPlaces)
        {
            return Double.ToString($"F{decimalPlaces}", CultureInfo.InvariantCulture);
        }
        #region 打印定点数的真实值
        /// <summary>
        /// int 类型的值
        /// </summary>
        public string DumpInt() => Int.ToString();

        /// <summary>
        /// float 类型的值
        /// </summary>
        public string DumpFloat() => Float.ToString();

        /// <summary>
        /// double 类型的值
        /// </summary>
        public string DumpDouble() => Double.ToString();
        #endregion
    }
}
