using System;

namespace FixedMath
{
    /// <summary>
    /// 定点数三阶矩阵
    /// </summary>
    public struct FMatrix3x3
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
        /// 构建三阶矩阵
        /// </summary>
        /// <param name="m00"></param>
        /// <param name="m01"></param>
        /// <param name="m02"></param>
        /// <param name="m10"></param>
        /// <param name="m11"></param>
        /// <param name="m12"></param>
        /// <param name="m20"></param>
        /// <param name="m21"></param>
        /// <param name="m22"></param>
        public FMatrix3x3(
            FFloat m00, FFloat m01, FFloat m02,
            FFloat m10, FFloat m11, FFloat m12,
            FFloat m20, FFloat m21, FFloat m22)
        {
            this.m00 = m00;
            this.m01 = m01;
            this.m02 = m02;
            this.m10 = m10;
            this.m11 = m11;
            this.m12 = m12;
            this.m20 = m20;
            this.m21 = m21;
            this.m22 = m22;
        }

        /// <summary>
        /// 返回所有行和所有列都是0的矩阵
        /// </summary>
        public static FMatrix3x3 Zero { get { return new FMatrix3x3(); } }

        /// <summary>
        /// 三阶单位矩阵
        /// </summary>
        public static FMatrix3x3 Identity
        {
            get
            {
                return new FMatrix3x3(
                    1, 0, 0,
                    0, 1, 0,
                    0, 0, 1);
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
                switch ((row * 3) + column)
                {
                    case 0: return m00;
                    case 1: return m01;
                    case 2: return m02;
                    case 3: return m10;
                    case 4: return m11;
                    case 5: return m12;
                    case 6: return m20;
                    case 7: return m21;
                    case 8: return m22;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch ((row * 3) + column)
                {
                    case 0: m00 = value; break;
                    case 1: m01 = value; break;
                    case 2: m02 = value; break;
                    case 3: m10 = value; break;
                    case 4: m11 = value; break;
                    case 5: m12 = value; break;
                    case 6: m20 = value; break;
                    case 7: m21 = value; break;
                    case 8: m22 = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// 行列式
        /// </summary>
        public FFloat Determinant
        {
            get
            {
                return
                    m00 * ((m11 * m22) - (m12 * m21)) -
                    m01 * ((m10 * m22) - (m12 * m20)) +
                    m02 * ((m10 * m21) - (m11 * m20));
            }
        }

        /// <summary>
        /// 转置矩阵
        /// </summary>
        public FMatrix3x3 Transposed
        {
            get
            {
                return new FMatrix3x3(
                    m00, m10, m20,
                    m01, m11, m21,
                    m02, m12, m22);
            }
        }

        /// <summary>
        /// 计算三阶矩阵的转置矩阵
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static FMatrix3x3 Transpose(FMatrix3x3 matrix)
        {
            return matrix.Transposed;
        }

        /// <summary>
        /// 逆矩阵
        /// </summary>
        public readonly FMatrix3x3 Inversed => Inverse(this);

        /// <summary>
        /// 计算矩阵的逆矩阵
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static FMatrix3x3 Inverse(FMatrix3x3 matrix)
        {
            FFloat det = matrix.Determinant;
            if (FMath.Abs(det) <= FMath.Epsilon)
                throw new InvalidOperationException("矩阵不可逆");

            FFloat invDet = FFloat.One / det;

            return new FMatrix3x3(
                ((matrix.m11 * matrix.m22) - (matrix.m12 * matrix.m21)) * invDet,
                ((matrix.m02 * matrix.m21) - (matrix.m01 * matrix.m22)) * invDet,
                ((matrix.m01 * matrix.m12) - (matrix.m02 * matrix.m11)) * invDet,
                ((matrix.m12 * matrix.m20) - (matrix.m10 * matrix.m22)) * invDet,
                ((matrix.m00 * matrix.m22) - (matrix.m02 * matrix.m20)) * invDet,
                ((matrix.m02 * matrix.m10) - (matrix.m00 * matrix.m12)) * invDet,
                ((matrix.m10 * matrix.m21) - (matrix.m11 * matrix.m20)) * invDet,
                ((matrix.m01 * matrix.m20) - (matrix.m00 * matrix.m21)) * invDet,
                ((matrix.m00 * matrix.m11) - (matrix.m01 * matrix.m10)) * invDet);
        }

        /// <summary>
        /// 计算矩阵与向量的乘法
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
        /// 变换法线
        /// <para>法线需要使用矩阵的逆转置进行变换，才能正确处理非均匀缩放</para>
        /// </summary>
        /// <param name="normal"></param>
        /// <returns></returns>
        public FVector3 TransformNormal(FVector3 normal)
        {
            FMatrix3x3 inverse = Inverse(this);
            FVector3 result = new FVector3(
                (inverse.m00 * normal.x) + (inverse.m10 * normal.y) + (inverse.m20 * normal.z),
                (inverse.m01 * normal.x) + (inverse.m11 * normal.y) + (inverse.m21 * normal.z),
                (inverse.m02 * normal.x) + (inverse.m12 * normal.y) + (inverse.m22 * normal.z));

            return FVector3.Normalize(result);
        }

        /// <summary>
        /// 获取指定行
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public FVector3 GetRow(int row)
        {
            switch (row)
            {
                case 0: return new FVector3(m00, m01, m02);
                case 1: return new FVector3(m10, m11, m12);
                case 2: return new FVector3(m20, m21, m22);
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
                default: throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// 缩放矩阵
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static FMatrix3x3 Scale(FVector3 scale)
        {
            return new FMatrix3x3(
                scale.x, 0, 0,
                0, scale.y, 0,
                0, 0, scale.z);
        }

        /// <summary>
        /// 绕x轴旋转的旋转矩阵
        /// </summary>
        /// <param name="radians">旋转的弧度值</param>
        /// <returns></returns>
        public static FMatrix3x3 RotateX(FFloat radians)
        {
            FMath.SinCos(radians, out FFloat sin, out FFloat cos);

            return new FMatrix3x3(
                1, 0, 0,
                0, cos, -sin,
                0, sin, cos);
        }

        /// <summary>
        /// 绕y轴旋转的旋转矩阵
        /// </summary>
        /// <param name="radians">旋转的弧度值</param>
        /// <returns></returns>
        public static FMatrix3x3 RotateY(FFloat radians)
        {
            FMath.SinCos(radians, out FFloat sin, out FFloat cos);

            return new FMatrix3x3(
                cos, 0, sin,
                0, 1, 0,
                -sin, 0, cos);
        }

        /// <summary>
        /// 绕z轴旋转的旋转矩阵
        /// </summary>
        /// <param name="radians">旋转的弧度值</param>
        /// <returns></returns>
        public static FMatrix3x3 RotateZ(FFloat radians)
        {
            FMath.SinCos(radians, out FFloat sin, out FFloat cos);

            return new FMatrix3x3(
                cos, -sin, 0,
                sin, cos, 0,
                0, 0, 1);
        }

        /// <summary>
        /// 矩阵加法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FMatrix3x3 operator +(FMatrix3x3 left, FMatrix3x3 right)
        {
            return new FMatrix3x3(
                left.m00 + right.m00, left.m01 + right.m01, left.m02 + right.m02,
                left.m10 + right.m10, left.m11 + right.m11, left.m12 + right.m12,
                left.m20 + right.m20, left.m21 + right.m21, left.m22 + right.m22);
        }

        /// <summary>
        /// 矩阵减法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FMatrix3x3 operator -(FMatrix3x3 left, FMatrix3x3 right)
        {
            return new FMatrix3x3(
                left.m00 - right.m00, left.m01 - right.m01, left.m02 - right.m02,
                left.m10 - right.m10, left.m11 - right.m11, left.m12 - right.m12,
                left.m20 - right.m20, left.m21 - right.m21, left.m22 - right.m22);
        }

        /// <summary>
        /// 矩阵乘法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FMatrix3x3 operator *(FMatrix3x3 left, FMatrix3x3 right)
        {
            return new FMatrix3x3(
                (left.m00 * right.m00) + (left.m01 * right.m10) + (left.m02 * right.m20),
                (left.m00 * right.m01) + (left.m01 * right.m11) + (left.m02 * right.m21),
                (left.m00 * right.m02) + (left.m01 * right.m12) + (left.m02 * right.m22),
                (left.m10 * right.m00) + (left.m11 * right.m10) + (left.m12 * right.m20),
                (left.m10 * right.m01) + (left.m11 * right.m11) + (left.m12 * right.m21),
                (left.m10 * right.m02) + (left.m11 * right.m12) + (left.m12 * right.m22),
                (left.m20 * right.m00) + (left.m21 * right.m10) + (left.m22 * right.m20),
                (left.m20 * right.m01) + (left.m21 * right.m11) + (left.m22 * right.m21),
                (left.m20 * right.m02) + (left.m21 * right.m12) + (left.m22 * right.m22));
        }

        /// <summary>
        /// 矩阵乘法
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static FVector3 operator *(FMatrix3x3 matrix, FVector3 vector)
        {
            return matrix.MultiplyVector(vector);
        }

        /// <summary>
        /// 矩阵乘法
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FMatrix3x3 operator *(FMatrix3x3 matrix, FFloat value)
        {
            return new FMatrix3x3(
                matrix.m00 * value, matrix.m01 * value, matrix.m02 * value,
                matrix.m10 * value, matrix.m11 * value, matrix.m12 * value,
                matrix.m20 * value, matrix.m21 * value, matrix.m22 * value);
        }

        /// <summary>
        /// 矩阵乘法
        /// </summary>
        /// <param name="value"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static FMatrix3x3 operator *(FFloat value, FMatrix3x3 matrix)
        {
            return matrix * value;
        }

        /// <summary>
        /// 判断两个矩阵是否近似相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool Approximately(FMatrix3x3 left, FMatrix3x3 right, FFloat tolerance)
        {
            return
                FMath.Abs(left.m00 - right.m00) <= tolerance &&
                FMath.Abs(left.m01 - right.m01) <= tolerance &&
                FMath.Abs(left.m02 - right.m02) <= tolerance &&
                FMath.Abs(left.m10 - right.m10) <= tolerance &&
                FMath.Abs(left.m11 - right.m11) <= tolerance &&
                FMath.Abs(left.m12 - right.m12) <= tolerance &&
                FMath.Abs(left.m20 - right.m20) <= tolerance &&
                FMath.Abs(left.m21 - right.m21) <= tolerance &&
                FMath.Abs(left.m22 - right.m22) <= tolerance;
        }

        /// <summary>
        /// 判断两个矩阵是否数值相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(FMatrix3x3 left, FMatrix3x3 right)
        {
            return
                left.m00 == right.m00 && left.m01 == right.m01 && left.m02 == right.m02 &&
                left.m10 == right.m10 && left.m11 == right.m11 && left.m12 == right.m12 &&
                left.m20 == right.m20 && left.m21 == right.m21 && left.m22 == right.m22;
        }

        /// <summary>
        /// 判断两个矩阵是否数值不等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(FMatrix3x3 left, FMatrix3x3 right)
        {
            return !(left == right);
        }

        /// <summary>
        /// 判断两个矩阵是否为同一个
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is FMatrix3x3 matrix && this == matrix;
        }

        /// <summary>
        /// 矩阵的哈希值
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return m00.GetHashCode() ^ m01.GetHashCode() ^ m02.GetHashCode() ^
                m10.GetHashCode() ^ m11.GetHashCode() ^ m12.GetHashCode() ^
                m20.GetHashCode() ^ m21.GetHashCode() ^ m22.GetHashCode();
        }

        /// <summary>
        /// 矩阵转换为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"(({m00},{m01},{m02}),({m10},{m11},{m12}),({m20},{m21},{m22}))";
        }
    }
}
