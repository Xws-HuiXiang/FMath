#if UNITY
using UnityEngine;
#endif

namespace FixedMath
{
    /// <summary>
    /// 定点数二维向量
    /// </summary>
    public struct FVector2
    {
        /// <summary>
        /// 向量 x 轴的值
        /// </summary>
        public FFloat x;
        /// <summary>
        /// 向量 y 轴的值
        /// </summary>
        public FFloat y;

        /// <summary>
        /// 构造定点数二维向量
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public FVector2(FFloat x, FFloat y)
        {
            this.x = x;
            this.y = y;
        }

        #region 常用向量
        /// <summary>
        /// 向量（0, 0）
        /// </summary>
        public static FVector2 Zero { get { return new FVector2(0, 0); } }
        /// <summary>
        /// 向量（1, 1）
        /// </summary>
        public static FVector2 One { get { return new FVector2(1, 1); } }
        /// <summary>
        /// 向量（-1, 0）
        /// </summary>
        public static FVector2 Left { get { return new FVector2(-1, 0); } }
        /// <summary>
        /// 向量（1, 0）
        /// </summary>
        public static FVector2 Right { get { return new FVector2(1, 0); } }
        /// <summary>
        /// 向量（0, 1）
        /// </summary>
        public static FVector2 Up { get { return new FVector2(0, 1); } }
        /// <summary>
        /// 向量（0, -1）
        /// </summary>
        public static FVector2 Down { get { return new FVector2(0, -1); } }
        #endregion

        /// <summary>
        /// 获取放大后的值数组
        /// </summary>
        /// <returns></returns>
        public long[] ConvertLongArray()
        {
            return new long[] { x.ScaledValue, y.ScaledValue };
        }

        /// <summary>
        /// 向量长度的平方
        /// </summary>
        public FFloat sqrMagnitude { get { return x * x + y * y; } }

        /// <summary>
        /// 计算向量长度的平方
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static FFloat SqrMagnitude(FVector2 vector)
        {
            return vector.x * vector.x + vector.y * vector.y;
        }

        /// <summary>
        /// 向量长度
        /// </summary>
        public FFloat Magnitude { get { return FMath.Sqrt(this.sqrMagnitude); } }

        /// <summary>
        /// 返回当前向量的单位向量
        /// </summary>
        public FVector2 normalized
        {
            get
            {
                if (this.Magnitude > 0)
                {
                    FFloat rate = FFloat.One / this.Magnitude;

                    return new FVector2(x * rate, y * rate);
                }
                else
                {
                    return FVector2.Zero;
                }
            }
        }

        /// <summary>
        /// 将当前向量转换为单位向量
        /// </summary>
        public void Normalize()
        {
            if (this.Magnitude > 0)
            {
                FFloat rate = FFloat.One / this.Magnitude;

                x *= rate;
                y *= rate;
            }
        }

        /// <summary>
        /// 计算指定向量的单位向量
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static FVector2 Normalize(FVector2 vector)
        {
            if (vector.Magnitude > 0)
            {
                FFloat rate = FFloat.One / vector.Magnitude;

                return new FVector2(vector.x * rate, vector.y * rate);
            }
            else
            {
                return FVector2.Zero;
            }
        }

        /// <summary>
        /// 向量点乘。结果大于0则两向量夹角小与90度；等于0则两个向量互相垂直；小与0则两向量加角在90~180度之间
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FFloat Dot(FVector2 left, FVector2 right)
        {
            return left.x * right.x + left.y * right.y;
        }

        /// <summary>
        /// 向量叉乘。结果为两个向量所在平面的法线向量
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FVector2 Cross(FVector2 left, FVector2 right)
        {
            return new FVector2(0, left.x * right.y - left.x * right.y);
        }

        /// <summary>
        /// 计算两向量夹角（返回弧度值）
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>返回为弧度值</returns>
        public static FFloat Angle(FVector2 from, FVector2 to)
        {
            FFloat num = FMath.Sqrt(from.sqrMagnitude * to.sqrMagnitude);
            FFloat num2 = FMath.Clamp(Dot(from, to) / num, -1, 1);

            return FMath.Acos(num2);
        }

        #region 运算符重载
        /// <summary>
        /// 向量加法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FVector2 operator +(FVector2 left, FVector2 right)
        {
            FFloat x = left.x + right.x;
            FFloat y = left.y + right.y;

            return new FVector2(x, y);
        }

        /// <summary>
        /// 向量减法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FVector2 operator -(FVector2 left, FVector2 right)
        {
            FFloat x = left.x - right.x;
            FFloat y = left.y - right.y;

            return new FVector2(x, y);
        }

        /// <summary>
        /// 向量乘法
        /// </summary>
        /// <param name="value"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public static FVector2 operator *(FVector2 left, FFloat value)
        {
            FFloat x = left.x * value;
            FFloat y = left.y * value;

            return new FVector2(x, y);
        }

        /// <summary>
        /// 向量乘法
        /// </summary>
        /// <param name="value"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public static FVector2 operator *(FFloat value, FVector2 left)
        {
            FFloat x = value * left.x;
            FFloat y = value * left.y;

            return new FVector2(x, y);
        }

        /// <summary>
        /// 向量除法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FVector2 operator /(FVector2 left, FFloat value)
        {
            FFloat x = left.x / value;
            FFloat y = left.y / value;

            return new FVector2(x, y);
        }

        /// <summary>
        /// 向量值取反
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static FVector2 operator -(FVector2 vector)
        {
            FFloat x = -vector.x;
            FFloat y = -vector.y;

            return new FVector2(x, y);
        }

        /// <summary>
        /// 判断向量相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(FVector2 left, FVector2 right)
        {
            return left.x == right.x && left.y == right.y;
        }

        /// <summary>
        /// 判断向量不等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(FVector2 left, FVector2 right)
        {
            return left.x != right.x || left.y != right.y;
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

            if (obj is FVector2 v)
            {
                return v.x == x && v.y == y;
            }

            return false;
        }

        /// <summary>
        /// 返回这个对象的 HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode();
        }

        /// <summary>
        /// 返回对象的 x、y 轴的值的字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"({x},{y})";
        }
    }
}
