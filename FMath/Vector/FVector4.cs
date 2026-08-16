using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace FixedMath.Vector
{
    /// <summary>
    /// 定点数四维向量
    /// </summary>
    public struct FVector4
    {
        #region 常用向量
        /// <summary>
        /// 向量（0, 0）
        /// </summary>
        public static FVector4 Zero { get { return new FVector4(0, 0, 0, 0); } }
        /// <summary>
        /// 向量（1, 1）
        /// </summary>
        public static FVector4 One { get { return new FVector4(1, 1, 1, 1); } }
        #endregion

        /// <summary>
        /// 向量 x 轴的值
        /// </summary>
        public FFloat x;
        /// <summary>
        /// 向量 y 轴的值
        /// </summary>
        public FFloat y;
        /// <summary>
        /// 向量 z 轴的值
        /// </summary>
        public FFloat z;
        /// <summary>
        /// 向量 w 轴的值
        /// </summary>
        public FFloat w;

        /// <summary>
        /// 构建定点数四维向量
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        public FVector4(FFloat x, FFloat y, FFloat z, FFloat w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        /// <summary>
        /// 获取放大后的值数组
        /// </summary>
        /// <returns></returns>
        public long[] ConvertLongArray()
        {
            return new long[] { x.RawValue, y.RawValue, z.RawValue, w.RawValue };
        }

        /// <summary>
        /// 向量长度的平方
        /// </summary>
        public FFloat sqrMagnitude { get { return x * x + y * y + z * z + w * w; } }

        /// <summary>
        /// 计算向量长度的平方
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static FFloat SqrMagnitude(FVector4 vector)
        {
            return vector.x * vector.x + vector.y * vector.y + vector.z * vector.z + vector.w * vector.w;
        }

        /// <summary>
        /// 计算向量长度
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static FFloat GetMagnitude(FVector4 vector)
        {
            return FMath.Sqrt(vector.sqrMagnitude);
        }

        /// <summary>
        /// 向量长度
        /// </summary>
        public FFloat Magnitude { get { return FMath.Sqrt(Dot(this, this)); } }

        /// <summary>
        /// 返回当前向量的单位向量
        /// </summary>
        public FVector4 normalized
        {
            get
            {
                if (this.Magnitude > 0)
                {
                    FFloat rate = FFloat.One / this.Magnitude;

                    return new FVector4(x * rate, y * rate, z * rate, w * rate);
                }
                else
                {
                    return FVector4.Zero;
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
                z *= rate;
                w *= rate;
            }
        }

        /// <summary>
        /// 计算指定向量的单位向量
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static FVector4 Normalize(FVector4 vector)
        {
            if (vector.Magnitude > 0)
            {
                FFloat rate = FFloat.One / vector.Magnitude;

                return new FVector4(vector.x * rate, vector.y * rate, vector.z * rate, vector.w * rate);
            }
            else
            {
                return FVector4.Zero;
            }
        }

        /// <summary>
        /// 向量点乘
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static FFloat Dot(FVector4 a, FVector4 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }

        /// <summary>
        /// 四维向量叉乘。结果与输入的三个向量垂直
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static FVector4 Cross(FVector4 a, FVector4 b, FVector4 c)
        {
            FFloat x = Determinant3(a.y, a.z, a.w, b.y, b.z, b.w, c.y, c.z, c.w);
            FFloat y = -Determinant3(a.x, a.z, a.w, b.x, b.z, b.w, c.x, c.z, c.w);
            FFloat z = Determinant3(a.x, a.y, a.w, b.x, b.y, b.w, c.x, c.y, c.w);
            FFloat w = -Determinant3(a.x, a.y, a.z, b.x, b.y, b.z, c.x, c.y, c.z);

            return new FVector4(x, y, z, w);
        }

        /// <summary>
        /// 计算两向量夹角（返回弧度值）
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>返回为弧度值</returns>
        public static FFloat Angle(FVector4 from, FVector4 to)
        {
            FFloat mod = from.Magnitude * to.Magnitude;
            if (mod == 0) return FFloat.Zero;

            FFloat value = FMath.Clamp(Dot(from, to) / mod, -1, 1);

            return FMath.Acos(value);
        }

        /// <summary>
        /// 计算两点距离
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FFloat Distance(FVector4 left, FVector4 right)
        {
            return (left - right).Magnitude;
        }

        /// <summary>
        /// 计算两点距离的平方
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FFloat SqrDistance(FVector4 left, FVector4 right)
        {
            return (left - right).sqrMagnitude;
        }

        /// <summary>
        /// 向量加法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FVector4 Add(FVector4 left, FVector4 right)
        {
            return left + right;
        }

        /// <summary>
        /// 向量减法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FVector4 Subtract(FVector4 left, FVector4 right)
        {
            return left - right;
        }

        /// <summary>
        /// 向量乘法
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FVector4 Multiply(FVector4 vector, FFloat value)
        {
            return vector * value;
        }

        /// <summary>
        /// 向量除法
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FVector4 Divide(FVector4 vector, FFloat value)
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
        public static FVector4 Lerp(FVector4 left, FVector4 right, FFloat t)
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
        public static FVector4 LerpUnclamped(FVector4 left, FVector4 right, FFloat t)
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
        public static FVector4 MoveTowards(FVector4 current, FVector4 target, FFloat maxDistanceDelta)
        {
            FVector4 delta = target - current;
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
        public static FVector4 Scale(FVector4 left, FVector4 right)
        {
            return new FVector4(left.x * right.x, left.y * right.y, left.z * right.z, left.w * right.w);
        }

        /// <summary>
        /// 按分量取最大值
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FVector4 Max(FVector4 left, FVector4 right)
        {
            return new FVector4(FMath.Max(left.x, right.x), FMath.Max(left.y, right.y), FMath.Max(left.z, right.z), FMath.Max(left.w, right.w));
        }

        /// <summary>
        /// 按分量取最小值
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FVector4 Min(FVector4 left, FVector4 right)
        {
            return new FVector4(FMath.Min(left.x, right.x), FMath.Min(left.y, right.y), FMath.Min(left.z, right.z), FMath.Min(left.w, right.w));
        }

        /// <summary>
        /// 限制向量长度
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static FVector4 ClampMagnitude(FVector4 vector, FFloat maxLength)
        {
            if (maxLength <= 0)
                return FVector4.Zero;

            FFloat maxSqrLength = maxLength * maxLength;
            if (vector.sqrMagnitude <= maxSqrLength)
                return vector;

            return vector.normalized * maxLength;
        }

        /// <summary>
        /// 计算向量投影
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="onNormal"></param>
        /// <returns></returns>
        public static FVector4 Project(FVector4 vector, FVector4 onNormal)
        {
            FFloat sqrMagnitude = onNormal.sqrMagnitude;
            if (sqrMagnitude == 0)
                return FVector4.Zero;

            return onNormal * (Dot(vector, onNormal) / sqrMagnitude);
        }

        /// <summary>
        /// 计算反射向量
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="normal"></param>
        /// <returns></returns>
        public static FVector4 Reflect(FVector4 vector, FVector4 normal)
        {
            return vector - normal * (2 * Dot(vector, normal));
        }

        private static FFloat Determinant3(
            FFloat a1, FFloat a2, FFloat a3,
            FFloat b1, FFloat b2, FFloat b3,
            FFloat c1, FFloat c2, FFloat c3)
        {
            return a1 * (b2 * c3 - b3 * c2) - a2 * (b1 * c3 - b3 * c1) + a3 * (b1 * c2 - b2 * c1);
        }

        #region 运算符重载
        /// <summary>
        /// 向量加法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FVector4 operator +(FVector4 left, FVector4 right)
        {
            FFloat x = left.x + right.x;
            FFloat y = left.y + right.y;
            FFloat z = left.z + right.z;
            FFloat w = left.w + right.w;

            return new FVector4(x, y, z, w);
        }

        /// <summary>
        /// 向量减法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FVector4 operator -(FVector4 left, FVector4 right)
        {
            FFloat x = left.x - right.x;
            FFloat y = left.y - right.y;
            FFloat z = left.z - right.z;
            FFloat w = left.w - right.w;

            return new FVector4(x, y, z, w);
        }

        /// <summary>
        /// 向量乘法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FVector4 operator *(FVector4 left, FFloat value)
        {
            FFloat x = left.x * value;
            FFloat y = left.y * value;
            FFloat z = left.z * value;
            FFloat w = left.w * value;

            return new FVector4(x, y, z, w);
        }

        /// <summary>
        /// 向量乘法
        /// </summary>
        /// <param name="value"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public static FVector4 operator *(FFloat value, FVector4 left)
        {
            FFloat x = value * left.x;
            FFloat y = value * left.y;
            FFloat z = value * left.z;
            FFloat w = value * left.w;

            return new FVector4(x, y, z, w);
        }

        /// <summary>
        /// 向量除法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FVector4 operator /(FVector4 left, FFloat value)
        {
            FFloat x = left.x / value;
            FFloat y = left.y / value;
            FFloat z = left.z / value;
            FFloat w = left.w / value;

            return new FVector4(x, y, z, w);
        }

        /// <summary>
        /// 向量值取反
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static FVector4 operator -(FVector4 vector)
        {
            FFloat x = -vector.x;
            FFloat y = -vector.y;
            FFloat z = -vector.z;
            FFloat w = -vector.w;

            return new FVector4(x, y, z, w);
        }

        /// <summary>
        /// 判断向量相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(FVector4 left, FVector4 right)
        {
            return left.x == right.x && left.y == right.y && left.z == right.z && left.w == right.w;
        }

        /// <summary>
        /// 判断向量不等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(FVector4 left, FVector4 right)
        {
            return left.x != right.x || left.y != right.y || left.z != right.z || left.w != right.w;
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

            if (obj is FVector4 v)
            {
                return v.x == x && v.y == y && v.z == z && v.w == w;
            }

            return false;
        }

        /// <summary>
        /// 返回这个对象的 HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode() ^ w.GetHashCode();
        }

        /// <summary>
        /// 返回对象的 x、y、z 和 w 轴值的字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"({x},{y},{z},{w})";
        }
    }
}
