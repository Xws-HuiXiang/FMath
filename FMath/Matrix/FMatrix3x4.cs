using System;

namespace FixedMath
{
    /// <summary>
    /// 定点数三行四列仿射矩阵，隐含最后一行为（0, 0, 0, 1）
    /// </summary>
    public struct FMatrix3x4
    {
        /// <summary>
        /// 矩阵00位置的值
        /// </summary>
        public FFloat m00;
        /// <summary>
        /// 矩阵01位置的值
        /// </summary>
        public FFloat m01;
        /// <summary>
        /// 矩阵02位置的值
        /// </summary>
        public FFloat m02;
        /// <summary>
        /// 矩阵03位置的值
        /// </summary>
        public FFloat m03;
        /// <summary>
        /// 矩阵10位置的值
        /// </summary>
        public FFloat m10;
        /// <summary>
        /// 矩阵11位置的值
        /// </summary>
        public FFloat m11;
        /// <summary>
        /// 矩阵12位置的值
        /// </summary>
        public FFloat m12;
        /// <summary>
        /// 矩阵13位置的值
        /// </summary>
        public FFloat m13;
        /// <summary>
        /// 矩阵20位置的值
        /// </summary>
        public FFloat m20;
        /// <summary>
        /// 矩阵21位置的值
        /// </summary>
        public FFloat m21;
        /// <summary>
        /// 矩阵22位置的值
        /// </summary>
        public FFloat m22;
        /// <summary>
        /// 矩阵23位置的值
        /// </summary>
        public FFloat m23;

        /// <summary>
        /// 构建三行四列仿射矩阵
        /// </summary>
        /// <param name="m00"></param>
        /// <param name="m01"></param>
        /// <param name="m02"></param>
        /// <param name="m03"></param>
        /// <param name="m10"></param>
        /// <param name="m11"></param>
        /// <param name="m12"></param>
        /// <param name="m13"></param>
        /// <param name="m20"></param>
        /// <param name="m21"></param>
        /// <param name="m22"></param>
        /// <param name="m23"></param>
        public FMatrix3x4(
            FFloat m00, FFloat m01, FFloat m02, FFloat m03,
            FFloat m10, FFloat m11, FFloat m12, FFloat m13,
            FFloat m20, FFloat m21, FFloat m22, FFloat m23)
        {
            this.m00 = m00;
            this.m01 = m01;
            this.m02 = m02;
            this.m03 = m03;
            this.m10 = m10;
            this.m11 = m11;
            this.m12 = m12;
            this.m13 = m13;
            this.m20 = m20;
            this.m21 = m21;
            this.m22 = m22;
            this.m23 = m23;
        }

        /// <summary>
        /// 仿射矩阵，所有值均为0
        /// </summary>
        public static FMatrix3x4 Zero { get { return new FMatrix3x4(); } }

        /// <summary>
        /// 仿射矩阵的单位矩阵
        /// </summary>
        public static FMatrix3x4 Identity
        {
            get
            {
                return new FMatrix3x4(
                    1, 0, 0, 0,
                    0, 1, 0, 0,
                    0, 0, 1, 0);
            }
        }

        /// <summary>
        /// 根据索引获取对应位置的值
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public FFloat this[int row, int column]
        {
            get
            {
                switch ((row * 4) + column)
                {
                    case 0: return m00;
                    case 1: return m01;
                    case 2: return m02;
                    case 3: return m03;
                    case 4: return m10;
                    case 5: return m11;
                    case 6: return m12;
                    case 7: return m13;
                    case 8: return m20;
                    case 9: return m21;
                    case 10: return m22;
                    case 11: return m23;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch ((row * 4) + column)
                {
                    case 0: m00 = value; break;
                    case 1: m01 = value; break;
                    case 2: m02 = value; break;
                    case 3: m03 = value; break;
                    case 4: m10 = value; break;
                    case 5: m11 = value; break;
                    case 6: m12 = value; break;
                    case 7: m13 = value; break;
                    case 8: m20 = value; break;
                    case 9: m21 = value; break;
                    case 10: m22 = value; break;
                    case 11: m23 = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// 构建矩阵中的旋转和缩放的部分
        /// </summary>
        public FMatrix3x3 RotationScale
        {
            get
            {
                return new FMatrix3x3(
                    m00, m01, m02,
                    m10, m11, m12,
                    m20, m21, m22);
            }
        }

        /// <summary>
        /// 当前矩阵表示平移的部分
        /// </summary>
        public FVector3 Translation
        {
            get { return new FVector3(m03, m13, m23); }
        }

        /// <summary>
        /// 转换为4x4矩阵，最后一行为(0,0,0,1)
        /// </summary>
        /// <returns></returns>
        public FMatrix4x4 ToMatrix4x4()
        {
            return new FMatrix4x4(
                m00, m01, m02, m03,
                m10, m11, m12, m13,
                m20, m21, m22, m23,
                0, 0, 0, 1);
        }

        /// <summary>
        /// 矩阵与点相乘
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public FVector3 MultiplyPoint(FVector3 point)
        {
            return new FVector3(
                (m00 * point.x) + (m01 * point.y) + (m02 * point.z) + m03,
                (m10 * point.x) + (m11 * point.y) + (m12 * point.z) + m13,
                (m20 * point.x) + (m21 * point.y) + (m22 * point.z) + m23);
        }

        /// <summary>
        /// 矩阵与向量相乘
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public FVector3 MultiplyVector(FVector3 vector)
        {
            return new FVector3(
                (m00 * vector.x) + (m01 * vector.y) + (m02 * vector.z),
                (m10 * vector.x) + (m11 * vector.y) + (m12 * vector.z),
                (m20 * vector.x) + (m21 * vector.y) + (m22 * vector.z));
        }

        /// <summary>
        /// 获取指定行
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public FVector4 GetRow(int row)
        {
            switch (row)
            {
                case 0: return new FVector4(m00, m01, m02, m03);
                case 1: return new FVector4(m10, m11, m12, m13);
                case 2: return new FVector4(m20, m21, m22, m23);
                default: throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// 获取指定列
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public FVector3 GetColumn(int column)
        {
            switch (column)
            {
                case 0: return new FVector3(m00, m10, m20);
                case 1: return new FVector3(m01, m11, m21);
                case 2: return new FVector3(m02, m12, m22);
                case 3: return new FVector3(m03, m13, m23);
                default: throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// 逆矩阵
        /// </summary>
        public FMatrix3x4 Inversed
        {
            get { return Inverse(this); }
        }

        /// <summary>
        /// 计算矩阵的逆矩阵
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static FMatrix3x4 Inverse(FMatrix3x4 matrix)
        {
            FMatrix3x3 inv3 = FMatrix3x3.Inverse(matrix.RotationScale);
            FVector3 invTranslation = -(inv3 * matrix.Translation);

            return new FMatrix3x4(
                inv3.m00, inv3.m01, inv3.m02, invTranslation.x,
                inv3.m10, inv3.m11, inv3.m12, invTranslation.y,
                inv3.m20, inv3.m21, inv3.m22, invTranslation.z);
        }

        /// <summary>
        /// 构建变换矩阵
        /// </summary>
        /// <param name="translation"></param>
        /// <returns></returns>
        public static FMatrix3x4 Translate(FVector3 translation)
        {
            return new FMatrix3x4(
                1, 0, 0, translation.x,
                0, 1, 0, translation.y,
                0, 0, 1, translation.z);
        }

        /// <summary>
        /// 构建缩放矩阵
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static FMatrix3x4 Scale(FVector3 scale)
        {
            return new FMatrix3x4(
                scale.x, 0, 0, 0,
                0, scale.y, 0, 0,
                0, 0, scale.z, 0);
        }

        /// <summary>
        /// 构建一个绕x轴旋转的矩阵
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static FMatrix3x4 RotateX(FFloat radians)
        {
            return FromRotationScale(FMatrix3x3.RotateX(radians));
        }

        /// <summary>
        /// 构建一个绕y轴旋转的矩阵
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static FMatrix3x4 RotateY(FFloat radians)
        {
            return FromRotationScale(FMatrix3x3.RotateY(radians));
        }

        /// <summary>
        /// 构建一个绕z轴旋转的矩阵
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static FMatrix3x4 RotateZ(FFloat radians)
        {
            return FromRotationScale(FMatrix3x3.RotateZ(radians));
        }

        /// <summary>
        /// 构建变换、旋转和缩放的矩阵
        /// </summary>
        /// <param name="translation"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static FMatrix3x4 TRS(FVector3 translation, FMatrix3x3 rotation, FVector3 scale)
        {
            FMatrix3x3 rotationScale = rotation * FMatrix3x3.Scale(scale);

            return new FMatrix3x4(
                rotationScale.m00, rotationScale.m01, rotationScale.m02, translation.x,
                rotationScale.m10, rotationScale.m11, rotationScale.m12, translation.y,
                rotationScale.m20, rotationScale.m21, rotationScale.m22, translation.z);
        }

        /// <summary>
        /// 构建旋转和缩放的矩阵
        /// </summary>
        /// <param name="rotationScale"></param>
        /// <returns></returns>
        public static FMatrix3x4 FromRotationScale(FMatrix3x3 rotationScale)
        {
            return new FMatrix3x4(
                rotationScale.m00, rotationScale.m01, rotationScale.m02, 0,
                rotationScale.m10, rotationScale.m11, rotationScale.m12, 0,
                rotationScale.m20, rotationScale.m21, rotationScale.m22, 0);
        }

        /// <summary>
        /// 矩阵加法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FMatrix3x4 operator +(FMatrix3x4 left, FMatrix3x4 right)
        {
            return new FMatrix3x4(
                left.m00 + right.m00, left.m01 + right.m01, left.m02 + right.m02, left.m03 + right.m03,
                left.m10 + right.m10, left.m11 + right.m11, left.m12 + right.m12, left.m13 + right.m13,
                left.m20 + right.m20, left.m21 + right.m21, left.m22 + right.m22, left.m23 + right.m23);
        }

        /// <summary>
        /// 矩阵减法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FMatrix3x4 operator -(FMatrix3x4 left, FMatrix3x4 right)
        {
            return new FMatrix3x4(
                left.m00 - right.m00, left.m01 - right.m01, left.m02 - right.m02, left.m03 - right.m03,
                left.m10 - right.m10, left.m11 - right.m11, left.m12 - right.m12, left.m13 - right.m13,
                left.m20 - right.m20, left.m21 - right.m21, left.m22 - right.m22, left.m23 - right.m23);
        }

        /// <summary>
        /// 矩阵乘法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FMatrix3x4 operator *(FMatrix3x4 left, FMatrix3x4 right)
        {
            return new FMatrix3x4(
                (left.m00 * right.m00) + (left.m01 * right.m10) + (left.m02 * right.m20),
                (left.m00 * right.m01) + (left.m01 * right.m11) + (left.m02 * right.m21),
                (left.m00 * right.m02) + (left.m01 * right.m12) + (left.m02 * right.m22),
                (left.m00 * right.m03) + (left.m01 * right.m13) + (left.m02 * right.m23) + left.m03,
                (left.m10 * right.m00) + (left.m11 * right.m10) + (left.m12 * right.m20),
                (left.m10 * right.m01) + (left.m11 * right.m11) + (left.m12 * right.m21),
                (left.m10 * right.m02) + (left.m11 * right.m12) + (left.m12 * right.m22),
                (left.m10 * right.m03) + (left.m11 * right.m13) + (left.m12 * right.m23) + left.m13,
                (left.m20 * right.m00) + (left.m21 * right.m10) + (left.m22 * right.m20),
                (left.m20 * right.m01) + (left.m21 * right.m11) + (left.m22 * right.m21),
                (left.m20 * right.m02) + (left.m21 * right.m12) + (left.m22 * right.m22),
                (left.m20 * right.m03) + (left.m21 * right.m13) + (left.m22 * right.m23) + left.m23);
        }

        /// <summary>
        /// 矩阵乘法
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static FVector3 operator *(FMatrix3x4 matrix, FVector3 point)
        {
            return matrix.MultiplyPoint(point);
        }

        /// <summary>
        /// 判断两个矩阵是否近似相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool Approximately(FMatrix3x4 left, FMatrix3x4 right, FFloat tolerance)
        {
            return
                FMath.Abs(left.m00 - right.m00) <= tolerance &&
                FMath.Abs(left.m01 - right.m01) <= tolerance &&
                FMath.Abs(left.m02 - right.m02) <= tolerance &&
                FMath.Abs(left.m03 - right.m03) <= tolerance &&
                FMath.Abs(left.m10 - right.m10) <= tolerance &&
                FMath.Abs(left.m11 - right.m11) <= tolerance &&
                FMath.Abs(left.m12 - right.m12) <= tolerance &&
                FMath.Abs(left.m13 - right.m13) <= tolerance &&
                FMath.Abs(left.m20 - right.m20) <= tolerance &&
                FMath.Abs(left.m21 - right.m21) <= tolerance &&
                FMath.Abs(left.m22 - right.m22) <= tolerance &&
                FMath.Abs(left.m23 - right.m23) <= tolerance;
        }

        /// <summary>
        /// 矩阵相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(FMatrix3x4 left, FMatrix3x4 right)
        {
            return
                left.m00 == right.m00 && left.m01 == right.m01 && left.m02 == right.m02 && left.m03 == right.m03 &&
                left.m10 == right.m10 && left.m11 == right.m11 && left.m12 == right.m12 && left.m13 == right.m13 &&
                left.m20 == right.m20 && left.m21 == right.m21 && left.m22 == right.m22 && left.m23 == right.m23;
        }

        /// <summary>
        /// 矩阵不等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(FMatrix3x4 left, FMatrix3x4 right)
        {
            return !(left == right);
        }

        /// <summary>
        /// 判断矩阵相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is FMatrix3x4 matrix && this == matrix;
        }

        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return m00.GetHashCode() ^ m01.GetHashCode() ^ m02.GetHashCode() ^ m03.GetHashCode() ^
                m10.GetHashCode() ^ m11.GetHashCode() ^ m12.GetHashCode() ^ m13.GetHashCode() ^
                m20.GetHashCode() ^ m21.GetHashCode() ^ m22.GetHashCode() ^ m23.GetHashCode();
        }

        /// <summary>
        /// 转为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"(({m00},{m01},{m02},{m03}),({m10},{m11},{m12},{m13}),({m20},{m21},{m22},{m23}))";
        }
    }
}
