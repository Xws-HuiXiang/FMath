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
            return new long[] { x.RawValue, y.RawValue };
        }

#pragma warning disable IDE1006 // 命名样式
        /// <summary>
        /// 向量长度的平方
        /// </summary>
        public readonly FFloat sqrMagnitude { get { return x * x + y * y; } }

        /// <summary>
        /// 向量长度
        /// </summary>
        public FFloat magnitude { get { return FMath.Sqrt(this.sqrMagnitude); } }
#pragma warning restore IDE1006 // 命名样式

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
        /// 计算向量长度
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static FFloat Magnitude(FVector2 vector)
        {
            return FMath.Sqrt(vector.sqrMagnitude);
        }

        /// <summary>
        /// 返回当前向量的单位向量
        /// </summary>
        public FVector2 Normalized
        {
            get
            {
                if (this.magnitude > 0)
                {
                    FFloat rate = FFloat.One / this.magnitude;

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
            FFloat sqrMagnitude = this.sqrMagnitude;
            if (sqrMagnitude > FFloat.Zero)
            {
                FFloat magnitude = FMath.Sqrt(sqrMagnitude);
                FFloat rate = FFloat.One / magnitude;

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
            if (vector.sqrMagnitude > FFloat.Zero)
            {
                FFloat rate = FFloat.One / vector.magnitude;

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
        /// 向量叉乘。结果为两个向量所在平面的法线向量长度
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FFloat CrossValue(FVector2 left, FVector2 right)
        {
            return left.x * right.y - left.y * right.x;
        }

        /// <summary>
        /// 计算两向量夹角（返回弧度值）
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>返回为弧度值</returns>
        public static FFloat Angle(FVector2 from, FVector2 to)
        {
            if (from.sqrMagnitude == 0 || to.sqrMagnitude == 0)
                return FFloat.Zero;

            FFloat num = FMath.Sqrt(from.sqrMagnitude * to.sqrMagnitude);
            FFloat num2 = FMath.Clamp(Dot(from, to) / num, -1, 1);

            return FMath.Acos(num2);
        }

        /// <summary>
        /// 计算两向量有符号夹角（返回弧度值）
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>返回为弧度值</returns>
        public static FFloat SignedAngle(FVector2 from, FVector2 to)
        {
            if (from.sqrMagnitude == 0 || to.sqrMagnitude == 0)
                return FFloat.Zero;

            return FMath.Atan2(CrossValue(from, to), Dot(from, to));
        }

        /// <summary>
        /// 计算两点距离
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FFloat Distance(FVector2 left, FVector2 right)
        {
            return (left - right).magnitude;
        }

        /// <summary>
        /// 计算两点距离的平方
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FFloat SqrDistance(FVector2 left, FVector2 right)
        {
            return (left - right).sqrMagnitude;
        }

        /// <summary>
        /// 向量加法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FVector2 Add(FVector2 left, FVector2 right)
        {
            return left + right;
        }

        /// <summary>
        /// 向量减法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FVector2 Subtract(FVector2 left, FVector2 right)
        {
            return left - right;
        }

        /// <summary>
        /// 向量乘法
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FVector2 Multiply(FVector2 vector, FFloat value)
        {
            return vector * value;
        }

        /// <summary>
        /// 向量除法
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FVector2 Divide(FVector2 vector, FFloat value)
        {
            return vector / value;
        }

        /// <summary>
        /// 向量线性插值
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static FVector2 Lerp(FVector2 left, FVector2 right, FFloat t)
        {
            return LerpUnclamped(left, right, FMath.Clamp(t, 0, 1));
        }

        /// <summary>
        /// 向量线性插值
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static FVector2 LerpUnclamped(FVector2 left, FVector2 right, FFloat t)
        {
            return left + (right - left) * t;
        }

        /// <summary>
        /// 向目标移动指定距离
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="maxDistanceDelta"></param>
        /// <returns></returns>
        public static FVector2 MoveTowards(FVector2 current, FVector2 target, FFloat maxDistanceDelta)
        {
            FVector2 delta = target - current;
            FFloat sqrDistance = delta.sqrMagnitude;
            FFloat maxSqrDistance = maxDistanceDelta * maxDistanceDelta;

            if (sqrDistance == 0 || (maxDistanceDelta >= 0 && sqrDistance <= maxSqrDistance))
                return target;

            return current + delta / FMath.Sqrt(sqrDistance) * maxDistanceDelta;
        }

        /// <summary>
        /// 按分量相乘
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FVector2 Scale(FVector2 left, FVector2 right)
        {
            return new FVector2(left.x * right.x, left.y * right.y);
        }

        /// <summary>
        /// 按分量取最大值
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FVector2 Max(FVector2 left, FVector2 right)
        {
            return new FVector2(FMath.Max(left.x, right.x), FMath.Max(left.y, right.y));
        }

        /// <summary>
        /// 按分量取最小值
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FVector2 Min(FVector2 left, FVector2 right)
        {
            return new FVector2(FMath.Min(left.x, right.x), FMath.Min(left.y, right.y));
        }

        /// <summary>
        /// 限制向量长度
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static FVector2 ClampMagnitude(FVector2 vector, FFloat maxLength)
        {
            if (maxLength <= 0)
                return FVector2.Zero;

            FFloat maxSqrLength = maxLength * maxLength;
            if (vector.sqrMagnitude <= maxSqrLength)
                return vector;

            return vector.Normalized * maxLength;
        }

        /// <summary>
        /// 计算向量投影
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="onNormal"></param>
        /// <returns></returns>
        public static FVector2 Project(FVector2 vector, FVector2 onNormal)
        {
            FFloat sqrMagnitude = onNormal.sqrMagnitude;
            if (sqrMagnitude == 0)
                return FVector2.Zero;

            return onNormal * (Dot(vector, onNormal) / sqrMagnitude);
        }

        /// <summary>
        /// 计算反射向量
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="normal"></param>
        /// <returns></returns>
        public static FVector2 Reflect(FVector2 vector, FVector2 normal)
        {
            return vector - normal * (2 * Dot(vector, normal));
        }

        /// <summary>
        /// 计算垂直向量
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static FVector2 Perpendicular(FVector2 vector)
        {
            return new FVector2(-vector.y, vector.x);
        }

        /// <summary>
        /// 判断两个二维向量是否足够接近
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool Approximately(FVector2 a, FVector2 b, FFloat tolerance)
        {
            return (FMath.Abs(a.x - b.x) <= tolerance) && (FMath.Abs(a.y - b.y) <= tolerance);
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
