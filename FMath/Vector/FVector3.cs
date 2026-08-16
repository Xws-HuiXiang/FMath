#if UNITY
using UnityEngine;
#endif

namespace FixedMath
{
    /// <summary>
    /// 定点数三维向量
    /// </summary>
    public struct FVector3
    {
        #region 常用向量
        /// <summary>
        /// 向量（0, 0, 0）
        /// </summary>
        public static FVector3 Zero { get { return new FVector3(0, 0, 0); } }
        /// <summary>
        /// 向量（1, 1, 1）
        /// </summary>
        public static FVector3 One { get { return new FVector3(1, 1, 1); } }
        /// <summary>
        /// 向量（0, 0, 1）
        /// </summary>
        public static FVector3 Forward { get { return new FVector3(0, 0, 1); } }
        /// <summary>
        /// 向量（0, 0, -1）
        /// </summary>
        public static FVector3 Back { get { return new FVector3(0, 0, -1); } }
        /// <summary>
        /// 向量（-1, 0, 0）
        /// </summary>
        public static FVector3 Left { get { return new FVector3(-1, 0, 0); } }
        /// <summary>
        /// 向量（1, 0, 0）
        /// </summary>
        public static FVector3 Right { get { return new FVector3(1, 0, 0); } }
        /// <summary>
        /// 向量（0, 1, 0）
        /// </summary>
        public static FVector3 Up { get { return new FVector3(0, 1, 0); } }
        /// <summary>
        /// 向量（0, -1, 0）
        /// </summary>
        public static FVector3 Down { get { return new FVector3(0, -1, 0); } }
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
        /// 使用定点数构造定点向量
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public FVector3(FFloat x, FFloat y, FFloat z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

#if UNITY
        /// <summary>
        /// 使用 unity 的 vector3 类型构造定点向量
        /// </summary>
        /// <param name="vector3"></param>
        public FVector3(Vector3 vector3)
        {
            this.x = new FFloat(vector3.x);
            this.y = new FFloat(vector3.y);
            this.z = new FFloat(vector3.z);
        }

        /// <summary>
        /// 返回 unity 的向量对象
        /// </summary>
        public Vector3 Vector3
        {
            get
            {
                return new Vector3(x.Float, y.Float, z.Float);
            }
        }
#endif

        /// <summary>
        /// 获取放大后的值数组
        /// </summary>
        /// <returns></returns>
        public long[] ConvertLongArray()
        {
            return new long[] { x.RawValue, y.RawValue, z.RawValue };
        }

#pragma warning disable IDE1006 // 命名样式
        /// <summary>
        /// 向量长度的平方
        /// </summary>
        public FFloat sqrMagnitude { get { return x * x + y * y + z * z; } }

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
        public static FFloat SqrMagnitude(FVector3 vector)
        {
            return vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
        }

        /// <summary>
        /// 计算向量长度
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static FFloat Magnitude(FVector3 vector)
        {
            return FMath.Sqrt(vector.sqrMagnitude);
        }

        /// <summary>
        /// 返回当前向量的单位向量
        /// </summary>
        public FVector3 Normalized
        {
            get
            {
                if (this.magnitude > 0)
                {
                    FFloat rate = FFloat.One / this.magnitude;

                    return new FVector3(x * rate, y * rate, z * rate);
                }
                else
                {
                    return FVector3.Zero;
                }
            }
        }

        /// <summary>
        /// 将当前向量转换为单位向量
        /// </summary>
        public void Normalize()
        {
            if(this.magnitude > 0)
            {
                FFloat rate = FFloat.One / this.magnitude;

                x *= rate;
                y *= rate;
                z *= rate;
            }
        }

        /// <summary>
        /// 计算指定向量的单位向量
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static FVector3 Normalize(FVector3 vector)
        {
            if (vector.magnitude > 0)
            {
                FFloat rate = FFloat.One / vector.magnitude;

                return new FVector3(vector.x * rate, vector.y * rate, vector.z * rate);
            }
            else
            {
                return FVector3.Zero;
            }
        }

        /// <summary>
        /// 向量点乘。结果大于0则两向量夹角小与90度；等于0则两个向量互相垂直；小与0则两向量加角在90~180度之间
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FFloat Dot(FVector3 left, FVector3 right)
        {
            return left.x * right.x + left.y * right.y + left.z * right.z;
        }

        /// <summary>
        /// 向量叉乘。结果为两个向量所在平面的法线向量（方向为右手法则确定）
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FVector3 Cross(FVector3 left, FVector3 right)
        {
            return new FVector3(left.y * right.z - left.z * right.y, left.z * right.x - left.x * right.z, left.x * right.y - left.y * right.x);
        }

        /// <summary>
        /// 计算两向量夹角（返回弧度值）
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>返回为弧度值</returns>
        public static FFloat Angle(FVector3 from, FVector3 to)
        {
            FFloat mod = from.magnitude * to.magnitude;
            if (mod == 0) return FFloat.Zero;
            FFloat dot = Dot(from, to);
            FFloat value = FMath.Clamp(dot / mod, -1, 1);

            return FMath.Acos(value);
        }

        /// <summary>
        /// 计算两向量有符号夹角（返回弧度值）
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="axis"></param>
        /// <returns>返回为弧度值</returns>
        public static FFloat SignedAngle(FVector3 from, FVector3 to, FVector3 axis)
        {
            if (from.sqrMagnitude == 0 || to.sqrMagnitude == 0 || axis.sqrMagnitude == 0)
                return FFloat.Zero;

            FVector3 cross = Cross(from, to);
            FFloat angle = FMath.Atan2(cross.magnitude, Dot(from, to));

            return Dot(axis, cross) < 0 ? -angle : angle;
        }

        /// <summary>
        /// 计算两点距离
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FFloat Distance(FVector3 left, FVector3 right)
        {
            return (left - right).magnitude;
        }

        /// <summary>
        /// 计算两点距离的平方
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FFloat SqrDistance(FVector3 left, FVector3 right)
        {
            return (left - right).sqrMagnitude;
        }

        /// <summary>
        /// 向量加法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FVector3 Add(FVector3 left, FVector3 right)
        {
            return left + right;
        }

        /// <summary>
        /// 向量减法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FVector3 Subtract(FVector3 left, FVector3 right)
        {
            return left - right;
        }

        /// <summary>
        /// 向量乘法
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FVector3 Multiply(FVector3 vector, FFloat value)
        {
            return vector * value;
        }

        /// <summary>
        /// 向量除法
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FVector3 Divide(FVector3 vector, FFloat value)
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
        public static FVector3 Lerp(FVector3 left, FVector3 right, FFloat t)
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
        public static FVector3 LerpUnclamped(FVector3 left, FVector3 right, FFloat t)
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
        public static FVector3 MoveTowards(FVector3 current, FVector3 target, FFloat maxDistanceDelta)
        {
            FVector3 delta = target - current;
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
        public static FVector3 Scale(FVector3 left, FVector3 right)
        {
            return new FVector3(left.x * right.x, left.y * right.y, left.z * right.z);
        }

        /// <summary>
        /// 按分量取最大值
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FVector3 Max(FVector3 left, FVector3 right)
        {
            return new FVector3(FMath.Max(left.x, right.x), FMath.Max(left.y, right.y), FMath.Max(left.z, right.z));
        }

        /// <summary>
        /// 按分量取最小值
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FVector3 Min(FVector3 left, FVector3 right)
        {
            return new FVector3(FMath.Min(left.x, right.x), FMath.Min(left.y, right.y), FMath.Min(left.z, right.z));
        }

        /// <summary>
        /// 限制向量长度
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static FVector3 ClampMagnitude(FVector3 vector, FFloat maxLength)
        {
            if (maxLength <= 0)
                return FVector3.Zero;

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
        public static FVector3 Project(FVector3 vector, FVector3 onNormal)
        {
            FFloat sqrMagnitude = onNormal.sqrMagnitude;
            if (sqrMagnitude == 0)
                return FVector3.Zero;

            return onNormal * (Dot(vector, onNormal) / sqrMagnitude);
        }

        /// <summary>
        /// 计算向量在指定平面上的投影
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="planeNormal"></param>
        /// <returns></returns>
        public static FVector3 ProjectOnPlane(FVector3 vector, FVector3 planeNormal)
        {
            return vector - Project(vector, planeNormal);
        }

        /// <summary>
        /// 计算反射向量
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="normal">必须为单位向量</param>
        /// <returns></returns>
        public static FVector3 Reflect(FVector3 vector, FVector3 normal)
        {
            return vector - normal * (2 * Dot(vector, normal));
        }

        /// <summary>
        /// 判断两个三维向量是否足够接近
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool Approximately(FVector3 a, FVector3 b, FFloat tolerance)
        {
            return (FMath.Abs(a.x - b.x) <= tolerance) &&
                (FMath.Abs(a.y - b.y) <= tolerance) &&
                (FMath.Abs(a.z - b.z) <= tolerance);
        }

        #region 运算符重载
        /// <summary>
        /// 向量加法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FVector3 operator +(FVector3 left, FVector3 right)
        {
            FFloat x = left.x + right.x;
            FFloat y = left.y + right.y;
            FFloat z = left.z + right.z;

            return new FVector3(x, y, z);
        }

        /// <summary>
        /// 向量减法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FVector3 operator -(FVector3 left, FVector3 right)
        {
            FFloat x = left.x - right.x;
            FFloat y = left.y - right.y;
            FFloat z = left.z - right.z;

            return new FVector3(x, y, z);
        }

        /// <summary>
        /// 向量乘法
        /// </summary>
        /// <param name="value"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public static FVector3 operator *(FVector3 left, FFloat value)
        {
            FFloat x = left.x * value;
            FFloat y = left.y * value;
            FFloat z = left.z * value;

            return new FVector3(x, y, z);
        }

        /// <summary>
        /// 向量乘法
        /// </summary>
        /// <param name="value"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public static FVector3 operator *(FFloat value, FVector3 left)
        {
            FFloat x = value * left.x;
            FFloat y = value * left.y;
            FFloat z = value * left.z;

            return new FVector3(x, y, z);
        }

        /// <summary>
        /// 向量除法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FVector3 operator /(FVector3 left, FFloat value)
        {
            FFloat x = left.x / value;
            FFloat y = left.y / value;
            FFloat z = left.z / value;

            return new FVector3(x, y, z);
        }

        /// <summary>
        /// 向量值取反
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static FVector3 operator -(FVector3 vector)
        {
            FFloat x = -vector.x;
            FFloat y = -vector.y;
            FFloat z = -vector.z;

            return new FVector3(x, y, z);
        }

        /// <summary>
        /// 判断向量相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(FVector3 left, FVector3 right)
        {
            return left.x == right.x && left.y == right.y && left.z == right.z;
        }

        /// <summary>
        /// 判断向量不等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(FVector3 left, FVector3 right)
        {
            return left.x != right.x || left.y != right.y || left.z != right.z;
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

            if (obj is FVector3 v)
            {
                return v.x == x && v.y == y && v.z == z;
            }

            return false;
        }

        /// <summary>
        /// 返回这个对象的 HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
        }

        /// <summary>
        /// 返回对象的 x、y 和 z 轴值的字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"({x},{y},{z})";
        }
    }
}
