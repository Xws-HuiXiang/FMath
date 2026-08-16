using System;

namespace FixedMath
{
    /// <summary>
    /// 定点数四元数
    /// </summary>
    public struct FQuaternion
    {
        /// <summary>
        /// 单位四元数
        /// </summary>
        public static FQuaternion Identity { get { return new FQuaternion(0, 0, 0, 1); } }

        /// <summary>
        /// 四元数 x 轴的值
        /// </summary>
        public FFloat x;
        /// <summary>
        /// 四元数 y 轴的值
        /// </summary>
        public FFloat y;
        /// <summary>
        /// 四元数 z 轴的值
        /// </summary>
        public FFloat z;
        /// <summary>
        /// 四元数 w 轴的值
        /// </summary>
        public FFloat w;

        /// <summary>
        /// 构建定点数四元数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        public FQuaternion(FFloat x, FFloat y, FFloat z, FFloat w)
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

#pragma warning disable IDE1006 // 命名样式
        /// <summary>
        /// 四元数长度的平方
        /// </summary>
        public FFloat sqrMagnitude { get { return x * x + y * y + z * z + w * w; } }

        /// <summary>
        /// 四元数长度
        /// </summary>
        public FFloat magnitude { get { return FMath.Sqrt(sqrMagnitude); } }
#pragma warning restore IDE1006 // 命名样式

        /// <summary>
        /// 返回当前四元数的单位四元数
        /// </summary>
        public FQuaternion Normalized
        {
            get
            {
                return Normalize(this);
            }
        }

        /// <summary>
        /// 返回当前四元数对应的欧拉角，单位为弧度
        /// </summary>
        public FVector3 EulerRadians
        {
            get
            {
                return ToEuler(this);
            }
        }

        /// <summary>
        /// 返回当前四元数对应的欧拉角，单位为角度
        /// </summary>
        public FVector3 EulerAngles
        {
            get
            {
                return ToEulerAngle(this);
            }
        }

        /// <summary>
        /// 将当前四元数转换为单位四元数
        /// </summary>
        public void Normalize()
        {
            this = Normalize(this);
        }

        /// <summary>
        /// 计算指定四元数的单位四元数
        /// </summary>
        /// <param name="quaternion"></param>
        /// <returns></returns>
        public static FQuaternion Normalize(FQuaternion quaternion)
        {
            FFloat magnitude = quaternion.magnitude;
            if (magnitude <= FMath.Epsilon)
                return Identity;

            return quaternion / magnitude;
        }

        /// <summary>
        /// 计算共轭四元数
        /// </summary>
        /// <param name="quaternion"></param>
        /// <returns></returns>
        public static FQuaternion Conjugate(FQuaternion quaternion)
        {
            return new FQuaternion(-quaternion.x, -quaternion.y, -quaternion.z, quaternion.w);
        }

        /// <summary>
        /// 计算逆四元数
        /// </summary>
        /// <param name="quaternion"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static FQuaternion Inverse(FQuaternion quaternion)
        {
            FFloat sqrMagnitude = quaternion.sqrMagnitude;
            if (sqrMagnitude <= FMath.Epsilon)
                throw new InvalidOperationException("四元数不可逆");

            return Conjugate(quaternion) / sqrMagnitude;
        }

        /// <summary>
        /// 计算四元数点乘
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FFloat Dot(FQuaternion left, FQuaternion right)
        {
            return left.x * right.x + left.y * right.y + left.z * right.z + left.w * right.w;
        }

        /// <summary>
        /// 计算两个四元数之间的夹角，单位为弧度
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FFloat Angle(FQuaternion left, FQuaternion right)
        {
            FFloat dot = FMath.Abs(Dot(Normalize(left), Normalize(right)));
            dot = FMath.Clamp(dot, 0, 1);

            return FMath.Acos(dot) * 2;
        }

        /// <summary>
        /// 计算两个四元数之间的夹角，单位为角度
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FFloat AngleAngle(FQuaternion left, FQuaternion right)
        {
            return Angle(left, right) * FMath.Rad2Deg;
        }

        /// <summary>
        /// 通过轴和弧度构建四元数
        /// </summary>
        /// <param name="radians">弧度值</param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static FQuaternion AngleAxis(FFloat radians, FVector3 axis)
        {
            if (axis.sqrMagnitude <= FMath.Epsilon)
                return Identity;

            FVector3 normalizedAxis = FVector3.Normalize(axis);
            FMath.SinCos(radians / 2, out FFloat sin, out FFloat cos);

            return Normalize(new FQuaternion(
                normalizedAxis.x * sin,
                normalizedAxis.y * sin,
                normalizedAxis.z * sin,
                cos));
        }

        /// <summary>
        /// 通过轴和角度构建四元数
        /// </summary>
        /// <param name="angle">角度值</param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static FQuaternion AngleAxisAngle(FFloat angle, FVector3 axis)
        {
            return AngleAxis(angle * FMath.Deg2Rad, axis);
        }

        /// <summary>
        /// 通过欧拉角构建四元数，单位为弧度
        /// <para>旋转顺序为Z * Y * X，适配当前矩阵和向量的列向量乘法约定</para>
        /// </summary>
        /// <param name="x">绕x轴旋转的弧度值</param>
        /// <param name="y">绕y轴旋转的弧度值</param>
        /// <param name="z">绕z轴旋转的弧度值</param>
        /// <returns></returns>
        public static FQuaternion Euler(FFloat x, FFloat y, FFloat z)
        {
            FQuaternion qx = AngleAxis(x, FVector3.Right);
            FQuaternion qy = AngleAxis(y, FVector3.Up);
            FQuaternion qz = AngleAxis(z, FVector3.Forward);

            return Normalize(qz * qy * qx);
        }

        /// <summary>
        /// 通过欧拉角构建四元数，单位为弧度
        /// </summary>
        /// <param name="euler"></param>
        /// <returns></returns>
        public static FQuaternion Euler(FVector3 euler)
        {
            return Euler(euler.x, euler.y, euler.z);
        }

        /// <summary>
        /// 通过欧拉角构建四元数，单位为角度
        /// </summary>
        /// <param name="x">绕x轴旋转的角度值</param>
        /// <param name="y">绕y轴旋转的角度值</param>
        /// <param name="z">绕z轴旋转的角度值</param>
        /// <returns></returns>
        public static FQuaternion EulerAngle(FFloat x, FFloat y, FFloat z)
        {
            return Euler(x * FMath.Deg2Rad, y * FMath.Deg2Rad, z * FMath.Deg2Rad);
        }

        /// <summary>
        /// 通过欧拉角构建四元数，单位为角度
        /// </summary>
        /// <param name="euler"></param>
        /// <returns></returns>
        public static FQuaternion EulerAngle(FVector3 euler)
        {
            return EulerAngle(euler.x, euler.y, euler.z);
        }

        /// <summary>
        /// 将四元数转换为欧拉角，单位为弧度
        /// </summary>
        /// <returns></returns>
        public FVector3 ToEuler()
        {
            return ToEuler(this);
        }

        /// <summary>
        /// 将四元数转换为欧拉角，单位为弧度
        /// </summary>
        /// <param name="quaternion"></param>
        /// <returns></returns>
        public static FVector3 ToEuler(FQuaternion quaternion)
        {
            FQuaternion q = Normalize(quaternion);

            FFloat sinXCosY = 2 * ((q.w * q.x) + (q.y * q.z));
            FFloat cosXCosY = 1 - (2 * ((q.x * q.x) + (q.y * q.y)));
            FFloat x = FMath.Atan2(sinXCosY, cosXCosY);

            FFloat sinY = 2 * ((q.w * q.y) - (q.z * q.x));
            FFloat y;
            if (sinY >= 1)
                y = FMath.HalfPI;
            else if (sinY <= -1)
                y = -FMath.HalfPI;
            else
                y = FMath.Asin(sinY);

            FFloat sinZCosY = 2 * ((q.w * q.z) + (q.x * q.y));
            FFloat cosZCosY = 1 - (2 * ((q.y * q.y) + (q.z * q.z)));
            FFloat z = FMath.Atan2(sinZCosY, cosZCosY);

            return new FVector3(x, y, z);
        }

        /// <summary>
        /// 将四元数转换为欧拉角，单位为角度
        /// </summary>
        /// <returns></returns>
        public FVector3 ToEulerAngle()
        {
            return ToEulerAngle(this);
        }

        /// <summary>
        /// 将四元数转换为欧拉角，单位为角度
        /// </summary>
        /// <param name="quaternion"></param>
        /// <returns></returns>
        public static FVector3 ToEulerAngle(FQuaternion quaternion)
        {
            FVector3 euler = ToEuler(quaternion);

            return new FVector3(euler.x * FMath.Rad2Deg, euler.y * FMath.Rad2Deg, euler.z * FMath.Rad2Deg);
        }

        /// <summary>
        /// 将四元数转换为欧拉角，单位为角度
        /// </summary>
        /// <returns></returns>
        public FVector3 ToEulerAngles()
        {
            return ToEulerAngle();
        }

        /// <summary>
        /// 将四元数转换为欧拉角，单位为角度
        /// </summary>
        /// <param name="quaternion"></param>
        /// <returns></returns>
        public static FVector3 ToEulerAngles(FQuaternion quaternion)
        {
            return ToEulerAngle(quaternion);
        }

        /// <summary>
        /// 将四元数转换为3x3旋转矩阵
        /// </summary>
        /// <returns></returns>
        public FMatrix3x3 ToMatrix3x3()
        {
            return ToMatrix3x3(this);
        }

        /// <summary>
        /// 将四元数转换为3x3旋转矩阵
        /// </summary>
        /// <param name="quaternion"></param>
        /// <returns></returns>
        public static FMatrix3x3 ToMatrix3x3(FQuaternion quaternion)
        {
            FQuaternion q = Normalize(quaternion);

            FFloat xx = q.x * q.x;
            FFloat yy = q.y * q.y;
            FFloat zz = q.z * q.z;
            FFloat xy = q.x * q.y;
            FFloat xz = q.x * q.z;
            FFloat yz = q.y * q.z;
            FFloat wx = q.w * q.x;
            FFloat wy = q.w * q.y;
            FFloat wz = q.w * q.z;

            return new FMatrix3x3(
                1 - (2 * (yy + zz)), 2 * (xy - wz), 2 * (xz + wy),
                2 * (xy + wz), 1 - (2 * (xx + zz)), 2 * (yz - wx),
                2 * (xz - wy), 2 * (yz + wx), 1 - (2 * (xx + yy)));
        }

        /// <summary>
        /// 将四元数转换为4x4旋转矩阵
        /// </summary>
        /// <returns></returns>
        public FMatrix4x4 ToMatrix4x4()
        {
            return ToMatrix4x4(this);
        }

        /// <summary>
        /// 将四元数转换为4x4旋转矩阵
        /// </summary>
        /// <param name="quaternion"></param>
        /// <returns></returns>
        public static FMatrix4x4 ToMatrix4x4(FQuaternion quaternion)
        {
            FMatrix3x3 matrix = ToMatrix3x3(quaternion);

            return new FMatrix4x4(
                matrix.m00, matrix.m01, matrix.m02, 0,
                matrix.m10, matrix.m11, matrix.m12, 0,
                matrix.m20, matrix.m21, matrix.m22, 0,
                0, 0, 0, 1);
        }

        /// <summary>
        /// 通过旋转矩阵构建四元数
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static FQuaternion FromMatrix3x3(FMatrix3x3 matrix)
        {
            FFloat trace = matrix.m00 + matrix.m11 + matrix.m22;
            FQuaternion result;

            if (trace > 0)
            {
                FFloat s = FMath.Sqrt(trace + 1) * 2;
                result = new FQuaternion(
                    (matrix.m21 - matrix.m12) / s,
                    (matrix.m02 - matrix.m20) / s,
                    (matrix.m10 - matrix.m01) / s,
                    s / 4);
            }
            else if (matrix.m00 > matrix.m11 && matrix.m00 > matrix.m22)
            {
                FFloat s = FMath.Sqrt(1 + matrix.m00 - matrix.m11 - matrix.m22) * 2;
                result = new FQuaternion(
                    s / 4,
                    (matrix.m01 + matrix.m10) / s,
                    (matrix.m02 + matrix.m20) / s,
                    (matrix.m21 - matrix.m12) / s);
            }
            else if (matrix.m11 > matrix.m22)
            {
                FFloat s = FMath.Sqrt(1 + matrix.m11 - matrix.m00 - matrix.m22) * 2;
                result = new FQuaternion(
                    (matrix.m01 + matrix.m10) / s,
                    s / 4,
                    (matrix.m12 + matrix.m21) / s,
                    (matrix.m02 - matrix.m20) / s);
            }
            else
            {
                FFloat s = FMath.Sqrt(1 + matrix.m22 - matrix.m00 - matrix.m11) * 2;
                result = new FQuaternion(
                    (matrix.m02 + matrix.m20) / s,
                    (matrix.m12 + matrix.m21) / s,
                    s / 4,
                    (matrix.m10 - matrix.m01) / s);
            }

            return Normalize(result);
        }

        /// <summary>
        /// 通过旋转矩阵构建四元数
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static FQuaternion FromMatrix4x4(FMatrix4x4 matrix)
        {
            return FromMatrix3x3(new FMatrix3x3(
                matrix.m00, matrix.m01, matrix.m02,
                matrix.m10, matrix.m11, matrix.m12,
                matrix.m20, matrix.m21, matrix.m22));
        }

        /// <summary>
        /// 计算从一个方向旋转到另一个方向的四元数
        /// </summary>
        /// <param name="fromDirection"></param>
        /// <param name="toDirection"></param>
        /// <returns></returns>
        public static FQuaternion FromToRotation(FVector3 fromDirection, FVector3 toDirection)
        {
            if (fromDirection.sqrMagnitude <= FMath.Epsilon || toDirection.sqrMagnitude <= FMath.Epsilon)
                return Identity;

            FVector3 from = FVector3.Normalize(fromDirection);
            FVector3 to = FVector3.Normalize(toDirection);
            FFloat dot = FMath.Clamp(FVector3.Dot(from, to), -1, 1);

            if (dot >= 1 - FMath.Epsilon)
                return Identity;

            if (dot <= -1 + FMath.Epsilon)
            {
                FVector3 axis = FVector3.Cross(FVector3.Right, from);
                if (axis.sqrMagnitude <= FMath.Epsilon)
                    axis = FVector3.Cross(FVector3.Up, from);

                return AngleAxis(FMath.PI, axis);
            }

            FVector3 cross = FVector3.Cross(from, to);
            return Normalize(new FQuaternion(cross.x, cross.y, cross.z, 1 + dot));
        }

        /// <summary>
        /// 通过前方向和上方向构建四元数
        /// </summary>
        /// <param name="forward"></param>
        /// <param name="up"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static FQuaternion LookRotation(FVector3 forward, FVector3 up)
        {
            FVector3 zAxis = FVector3.Normalize(forward);
            if (zAxis.sqrMagnitude <= FMath.Epsilon)
                throw new ArgumentException("forward方向不能为零向量", nameof(forward));

            FVector3 xAxis = FVector3.Normalize(FVector3.Cross(up, zAxis));
            if (xAxis.sqrMagnitude <= FMath.Epsilon)
                throw new ArgumentException("up方向不能与forward方向平行", nameof(up));

            FVector3 yAxis = FVector3.Cross(zAxis, xAxis);

            return FromMatrix3x3(new FMatrix3x3(
                xAxis.x, yAxis.x, zAxis.x,
                xAxis.y, yAxis.y, zAxis.y,
                xAxis.z, yAxis.z, zAxis.z));
        }

        /// <summary>
        /// 通过前方向和上方向构建四元数
        /// </summary>
        /// <param name="forward"></param>
        /// <returns></returns>
        public static FQuaternion LookRotation(FVector3 forward)
        {
            return LookRotation(forward, FVector3.Up);
        }

        /// <summary>
        /// 线性插值四元数
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static FQuaternion Lerp(FQuaternion left, FQuaternion right, FFloat t)
        {
            return LerpUnclamped(left, right, FMath.Clamp(t, 0, 1));
        }

        /// <summary>
        /// 线性插值四元数
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static FQuaternion LerpUnclamped(FQuaternion left, FQuaternion right, FFloat t)
        {
            FQuaternion target = Dot(left, right) < 0 ? -right : right;

            return Normalize(left + (target - left) * t);
        }

        /// <summary>
        /// 球面插值四元数
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static FQuaternion Slerp(FQuaternion left, FQuaternion right, FFloat t)
        {
            return SlerpUnclamped(left, right, FMath.Clamp(t, 0, 1));
        }

        /// <summary>
        /// 球面插值四元数
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static FQuaternion SlerpUnclamped(FQuaternion left, FQuaternion right, FFloat t)
        {
            FQuaternion from = Normalize(left);
            FQuaternion to = Normalize(right);
            FFloat dot = Dot(from, to);

            if (dot < 0)
            {
                to = -to;
                dot = -dot;
            }

            dot = FMath.Clamp(dot, 0, 1);
            if (dot > new FFloat(0.9995))
                return LerpUnclamped(from, to, t);

            FFloat theta0 = FMath.Acos(dot);
            FFloat theta = theta0 * t;
            FMath.SinCos(theta, out FFloat sinTheta, out FFloat cosTheta);
            FFloat sinTheta0 = FMath.Sin(theta0);

            FFloat s0 = cosTheta - dot * sinTheta / sinTheta0;
            FFloat s1 = sinTheta / sinTheta0;

            return Normalize((from * s0) + (to * s1));
        }

        /// <summary>
        /// 判断两个四元数是否足够接近
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool Approximately(FQuaternion left, FQuaternion right, FFloat tolerance)
        {
            return
                FMath.Abs(left.x - right.x) <= tolerance &&
                FMath.Abs(left.y - right.y) <= tolerance &&
                FMath.Abs(left.z - right.z) <= tolerance &&
                FMath.Abs(left.w - right.w) <= tolerance;
        }

        #region 运算符重载
        /// <summary>
        /// 四元数加法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FQuaternion operator +(FQuaternion left, FQuaternion right)
        {
            return new FQuaternion(left.x + right.x, left.y + right.y, left.z + right.z, left.w + right.w);
        }

        /// <summary>
        /// 四元数减法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FQuaternion operator -(FQuaternion left, FQuaternion right)
        {
            return new FQuaternion(left.x - right.x, left.y - right.y, left.z - right.z, left.w - right.w);
        }

        /// <summary>
        /// 四元数取反
        /// </summary>
        /// <param name="quaternion"></param>
        /// <returns></returns>
        public static FQuaternion operator -(FQuaternion quaternion)
        {
            return new FQuaternion(-quaternion.x, -quaternion.y, -quaternion.z, -quaternion.w);
        }

        /// <summary>
        /// 四元数乘法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FQuaternion operator *(FQuaternion left, FQuaternion right)
        {
            return new FQuaternion(
                (left.w * right.x) + (left.x * right.w) + (left.y * right.z) - (left.z * right.y),
                (left.w * right.y) - (left.x * right.z) + (left.y * right.w) + (left.z * right.x),
                (left.w * right.z) + (left.x * right.y) - (left.y * right.x) + (left.z * right.w),
                (left.w * right.w) - (left.x * right.x) - (left.y * right.y) - (left.z * right.z));
        }

        /// <summary>
        /// 四元数旋转向量
        /// </summary>
        /// <param name="quaternion"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static FVector3 operator *(FQuaternion quaternion, FVector3 vector)
        {
            FQuaternion q = Normalize(quaternion);
            FVector3 u = new FVector3(q.x, q.y, q.z);
            FFloat s = q.w;

            return (2 * FVector3.Dot(u, vector) * u) +
                ((s * s - FVector3.Dot(u, u)) * vector) +
                (2 * s * FVector3.Cross(u, vector));
        }

        /// <summary>
        /// 四元数乘法
        /// </summary>
        /// <param name="quaternion"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FQuaternion operator *(FQuaternion quaternion, FFloat value)
        {
            return new FQuaternion(quaternion.x * value, quaternion.y * value, quaternion.z * value, quaternion.w * value);
        }

        /// <summary>
        /// 四元数乘法
        /// </summary>
        /// <param name="value"></param>
        /// <param name="quaternion"></param>
        /// <returns></returns>
        public static FQuaternion operator *(FFloat value, FQuaternion quaternion)
        {
            return quaternion * value;
        }

        /// <summary>
        /// 四元数除法
        /// </summary>
        /// <param name="quaternion"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FQuaternion operator /(FQuaternion quaternion, FFloat value)
        {
            return new FQuaternion(quaternion.x / value, quaternion.y / value, quaternion.z / value, quaternion.w / value);
        }

        /// <summary>
        /// 判断四元数相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(FQuaternion left, FQuaternion right)
        {
            return left.x == right.x && left.y == right.y && left.z == right.z && left.w == right.w;
        }

        /// <summary>
        /// 判断四元数不等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(FQuaternion left, FQuaternion right)
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

            if (obj is FQuaternion quaternion)
                return quaternion == this;

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
