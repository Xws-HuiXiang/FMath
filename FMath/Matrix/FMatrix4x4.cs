using System;

namespace FixedMath
{
    /// <summary>
    /// 定点数四阶矩阵
    /// </summary>
    public struct FMatrix4x4
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
        /// 矩阵30位置的值
        /// </summary>
        public FFloat m30;
        /// <summary>
        /// 矩阵31位置的值
        /// </summary>
        public FFloat m31;
        /// <summary>
        /// 矩阵32位置的值
        /// </summary>
        public FFloat m32;
        /// <summary>
        /// 矩阵33位置的值
        /// </summary>
        public FFloat m33;

        /// <summary>
        /// 构建四阶矩阵
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
        /// <param name="m30"></param>
        /// <param name="m31"></param>
        /// <param name="m32"></param>
        /// <param name="m33"></param>
        public FMatrix4x4(
            FFloat m00, FFloat m01, FFloat m02, FFloat m03,
            FFloat m10, FFloat m11, FFloat m12, FFloat m13,
            FFloat m20, FFloat m21, FFloat m22, FFloat m23,
            FFloat m30, FFloat m31, FFloat m32, FFloat m33)
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
            this.m30 = m30;
            this.m31 = m31;
            this.m32 = m32;
            this.m33 = m33;
        }

        /// <summary>
        /// 四阶矩阵，所有值均为0
        /// </summary>
        public static FMatrix4x4 Zero { get { return new FMatrix4x4(); } }
        /// <summary>
        /// 四阶矩阵的单位矩阵
        /// </summary>
        public static FMatrix4x4 Identity
        {
            get
            {
                return new FMatrix4x4(
                    1, 0, 0, 0,
                    0, 1, 0, 0,
                    0, 0, 1, 0,
                    0, 0, 0, 1);
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
                    case 12: return m30;
                    case 13: return m31;
                    case 14: return m32;
                    case 15: return m33;
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
                    case 12: m30 = value; break;
                    case 13: m31 = value; break;
                    case 14: m32 = value; break;
                    case 15: m33 = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// 转置矩阵
        /// </summary>
        public FMatrix4x4 Transposed
        {
            get
            {
                return new FMatrix4x4(
                    m00, m10, m20, m30,
                    m01, m11, m21, m31,
                    m02, m12, m22, m32,
                    m03, m13, m23, m33);
            }
        }

        /// <summary>
        /// 行列式
        /// </summary>
        public FFloat Determinant
        {
            get
            {
                FFloat det0 = Det3(m11, m12, m13, m21, m22, m23, m31, m32, m33);
                FFloat det1 = Det3(m10, m12, m13, m20, m22, m23, m30, m32, m33);
                FFloat det2 = Det3(m10, m11, m13, m20, m21, m23, m30, m31, m33);
                FFloat det3 = Det3(m10, m11, m12, m20, m21, m22, m30, m31, m32);

                return (m00 * det0) - (m01 * det1) + (m02 * det2) - (m03 * det3);
            }
        }

        /// <summary>
        /// 计算矩阵的转置矩阵
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static FMatrix4x4 Transpose(FMatrix4x4 matrix)
        {
            return matrix.Transposed;
        }

        /// <summary>
        /// 逆矩阵
        /// </summary>
        public readonly FMatrix4x4 Inversed => Inverse(this);

        /// <summary>
        /// 计算矩阵的逆矩阵
        /// <para>使用高斯消元算法求逆，在定点数的环境下由于误差累计可能导致误差比较明显</para>
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static FMatrix4x4 Inverse(FMatrix4x4 matrix)
        {
            FFloat[,] a = new FFloat[4, 8];

            for (int row = 0; row < 4; row++)
            {
                for (int column = 0; column < 4; column++)
                    a[row, column] = matrix[row, column];

                a[row, row + 4] = FFloat.One;
            }

            for (int pivot = 0; pivot < 4; pivot++)
            {
                int pivotRow = pivot;
                FFloat pivotAbs = FMath.Abs(a[pivotRow, pivot]);

                for (int row = pivot + 1; row < 4; row++)
                {
                    FFloat rowAbs = FMath.Abs(a[row, pivot]);
                    if (rowAbs > pivotAbs)
                    {
                        pivotAbs = rowAbs;
                        pivotRow = row;
                    }
                }

                if (pivotAbs <= FMath.Epsilon)
                    throw new InvalidOperationException("矩阵不可逆");

                if (pivotRow != pivot)
                {
                    for (int column = 0; column < 8; column++)
                    {
                        FFloat temp = a[pivot, column];
                        a[pivot, column] = a[pivotRow, column];
                        a[pivotRow, column] = temp;
                    }
                }

                FFloat pivotValue = a[pivot, pivot];
                for (int column = 0; column < 8; column++)
                    a[pivot, column] /= pivotValue;

                for (int row = 0; row < 4; row++)
                {
                    if (row == pivot)
                        continue;

                    FFloat factor = a[row, pivot];
                    if (factor == 0)
                        continue;

                    for (int column = 0; column < 8; column++)
                        a[row, column] -= factor * a[pivot, column];
                }
            }

            return new FMatrix4x4(
                a[0, 4], a[0, 5], a[0, 6], a[0, 7],
                a[1, 4], a[1, 5], a[1, 6], a[1, 7],
                a[2, 4], a[2, 5], a[2, 6], a[2, 7],
                a[3, 4], a[3, 5], a[3, 6], a[3, 7]);
        }

        /// <summary>
        /// 计算仿射矩阵的逆矩阵
        /// <para>如果矩阵最后一行不是(0,0,0,1)，则会抛出异常</para>
        /// </summary>
        /// <returns></returns>
        public FMatrix4x4 InverseAffine()
        {
            return InverseAffine(this);
        }

        /// <summary>
        /// 计算仿射矩阵的逆矩阵
        /// <para>如果矩阵最后一行不是(0,0,0,1)，则会抛出异常</para>
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static FMatrix4x4 InverseAffine(FMatrix4x4 matrix)
        {
            if (matrix.m30 != 0 || matrix.m31 != 0 || matrix.m32 != 0 || matrix.m33 != FFloat.One)
                throw new InvalidOperationException("FMatrix4x4矩阵不是仿射矩阵，无法使用仿射矩阵的逆矩阵函数。请改为使用Inverse()方法求逆矩阵");

            return FMatrix3x4.Inverse(matrix.ToMatrix3x4()).ToMatrix4x4();
        }

        /// <summary>
        /// 通用齐次向量乘法
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public FVector4 Multiply(FVector4 vector)
        {
            return new FVector4(
                (m00 * vector.x) + (m01 * vector.y) + (m02 * vector.z) + (m03 * vector.w),
                (m10 * vector.x) + (m11 * vector.y) + (m12 * vector.z) + (m13 * vector.w),
                (m20 * vector.x) + (m21 * vector.y) + (m22 * vector.z) + (m23 * vector.w),
                (m30 * vector.x) + (m31 * vector.y) + (m32 * vector.z) + (m33 * vector.w));
        }

        /// <summary>
        /// 矩阵乘点坐标
        /// <para>处理完整的4x4矩阵，会计算w分量</para>
        /// <para>也就是用一个完整的4x4矩阵变换一个点</para>
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        /// <exception cref="DivideByZeroException"></exception>
        public FVector3 MultiplyPoint(FVector3 point)
        {
            FFloat x = (m00 * point.x) + (m01 * point.y) + (m02 * point.z) + m03;
            FFloat y = (m10 * point.x) + (m11 * point.y) + (m12 * point.z) + m13;
            FFloat z = (m20 * point.x) + (m21 * point.y) + (m22 * point.z) + m23;
            FFloat w = (m30 * point.x) + (m31 * point.y) + (m32 * point.z) + m33;

            if (FMath.Abs(w) <= FMath.Epsilon)
                throw new DivideByZeroException();

            if (w != 1)
                return new FVector3(x / w, y / w, z / w);

            return new FVector3(x, y, z);
        }

        /// <summary>
        /// 4x4矩阵与3x4矩阵相乘
        /// <para>只处理3x4仿射矩阵，不处理齐次坐标的透视除法</para>
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public FVector3 MultiplyPoint3x4(FVector3 point)
        {
            return new FVector3(
                (m00 * point.x) + (m01 * point.y) + (m02 * point.z) + m03,
                (m10 * point.x) + (m11 * point.y) + (m12 * point.z) + m13,
                (m20 * point.x) + (m21 * point.y) + (m22 * point.z) + m23);
        }

        /// <summary>
        /// 矩阵与向量方向相乘
        /// <para>不考虑平移的方向变换</para>
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public FVector3 MultiplyDirection(FVector3 direction)
        {
            return new FVector3(
                (m00 * direction.x) + (m01 * direction.y) + (m02 * direction.z),
                (m10 * direction.x) + (m11 * direction.y) + (m12 * direction.z),
                (m20 * direction.x) + (m21 * direction.y) + (m22 * direction.z));
        }

        /// <summary>
        /// 变换法线
        /// <para>法线需要使用矩阵左上角3x3部分的逆转置进行变换，才能正确处理非均匀缩放</para>
        /// </summary>
        /// <param name="normal"></param>
        /// <returns></returns>
        public FVector3 TransformNormal(FVector3 normal)
        {
            return new FMatrix3x3(
                m00, m01, m02,
                m10, m11, m12,
                m20, m21, m22).TransformNormal(normal);
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
                case 3: return new FVector4(m30, m31, m32, m33);
                default: throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// 获取指定列
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public FVector4 GetColumn(int column)
        {
            switch (column)
            {
                case 0: return new FVector4(m00, m10, m20, m30);
                case 1: return new FVector4(m01, m11, m21, m31);
                case 2: return new FVector4(m02, m12, m22, m32);
                case 3: return new FVector4(m03, m13, m23, m33);
                default: throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// 转为3x4仿射矩阵
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public FMatrix3x4 ToMatrix3x4()
        {
            if (m30 != 0 || m31 != 0 || m32 != 0 || m33 != 1)
                throw new InvalidOperationException("该矩阵不是仿射矩阵");

            return new FMatrix3x4(
                m00, m01, m02, m03,
                m10, m11, m12, m13,
                m20, m21, m22, m23);
        }

        /// <summary>
        /// 构建变换矩阵
        /// </summary>
        /// <param name="translation"></param>
        /// <returns></returns>
        public static FMatrix4x4 Translate(FVector3 translation)
        {
            return FMatrix3x4.Translate(translation).ToMatrix4x4();
        }

        /// <summary>
        /// 构建缩放矩阵
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static FMatrix4x4 Scale(FVector3 scale)
        {
            return FMatrix3x4.Scale(scale).ToMatrix4x4();
        }

        /// <summary>
        /// 构建绕x轴旋转的矩阵
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static FMatrix4x4 RotateX(FFloat radians)
        {
            return FMatrix3x4.RotateX(radians).ToMatrix4x4();
        }

        /// <summary>
        /// 构建绕y轴旋转的矩阵
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static FMatrix4x4 RotateY(FFloat radians)
        {
            return FMatrix3x4.RotateY(radians).ToMatrix4x4();
        }

        /// <summary>
        /// 构建绕z轴旋转的矩阵
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static FMatrix4x4 RotateZ(FFloat radians)
        {
            return FMatrix3x4.RotateZ(radians).ToMatrix4x4();
        }

        /// <summary>
        /// 构建平移、旋转和缩放的矩阵
        /// </summary>
        /// <param name="translation"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static FMatrix4x4 TRS(FVector3 translation, FMatrix3x3 rotation, FVector3 scale)
        {
            return FMatrix3x4.TRS(translation, rotation, scale).ToMatrix4x4();
        }

        /// <summary>
        /// 构建右手坐标系的View矩阵
        /// </summary>
        /// <param name="eye">观察点</param>
        /// <param name="target">观察目标</param>
        /// <param name="up">向上方向</param>
        /// <returns></returns>
        public static FMatrix4x4 LookAt(FVector3 eye, FVector3 target, FVector3 up)
        {
            FVector3 zAxis = FVector3.Normalize(eye - target);
            if (zAxis.sqrMagnitude == 0)
                throw new ArgumentException("观察点不能与观察目标重合", nameof(target));

            FVector3 xAxis = FVector3.Normalize(FVector3.Cross(up, zAxis));
            if (xAxis.sqrMagnitude == 0)
                throw new ArgumentException("up方向不能与观察方向平行", nameof(up));

            FVector3 yAxis = FVector3.Cross(zAxis, xAxis);

            return new FMatrix4x4(
                xAxis.x, xAxis.y, xAxis.z, -FVector3.Dot(xAxis, eye),
                yAxis.x, yAxis.y, yAxis.z, -FVector3.Dot(yAxis, eye),
                zAxis.x, zAxis.y, zAxis.z, -FVector3.Dot(zAxis, eye),
                0, 0, 0, 1);
        }

        /// <summary>
        /// 构建右手坐标系、OpenGL NDC 深度范围 [-1, 1] 的透视投影矩阵
        /// </summary>
        /// <param name="fovY">纵向视野角，单位为弧度</param>
        /// <param name="aspect">宽高比</param>
        /// <param name="near">近裁剪面距离</param>
        /// <param name="far">远裁剪面距离</param>
        /// <returns></returns>
        public static FMatrix4x4 Perspective(FFloat fovY, FFloat aspect, FFloat near, FFloat far)
        {
            if (fovY <= 0 || fovY >= FMath.PI)
                throw new ArgumentOutOfRangeException(nameof(fovY));
            if (aspect <= 0)
                throw new ArgumentOutOfRangeException(nameof(aspect));
            if (near <= 0)
                throw new ArgumentOutOfRangeException(nameof(near));
            if (far <= near)
                throw new ArgumentOutOfRangeException(nameof(far));

            FFloat f = FFloat.One / FMath.Tan(fovY / 2);
            FFloat range = near - far;

            return new FMatrix4x4(
                f / aspect, 0, 0, 0,
                0, f, 0, 0,
                0, 0, (far + near) / range, (2 * far * near) / range,
                0, 0, -1, 0);
        }

        /// <summary>
        /// 构建右手坐标系、OpenGL NDC 深度范围 [-1, 1] 的正交投影矩阵
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <param name="top"></param>
        /// <param name="near"></param>
        /// <param name="far"></param>
        /// <returns></returns>
        public static FMatrix4x4 Orthographic(FFloat left, FFloat right, FFloat bottom, FFloat top, FFloat near, FFloat far)
        {
            if (FMath.Abs(right - left) <= FMath.Epsilon)
                throw new ArgumentException("左右平面不能相同");
            if (FMath.Abs(top - bottom) <= FMath.Epsilon)
                throw new ArgumentException("上下平面不能相同");
            if (FMath.Abs(far - near) <= FMath.Epsilon)
                throw new ArgumentException("远近平面不能相同");

            FFloat width = right - left;
            FFloat height = top - bottom;
            FFloat depth = far - near;

            return new FMatrix4x4(
                2 / width, 0, 0, -(right + left) / width,
                0, 2 / height, 0, -(top + bottom) / height,
                0, 0, -2 / depth, -(far + near) / depth,
                0, 0, 0, 1);
        }

        /// <summary>
        /// 构建右手坐标系的正交投影矩阵
        /// </summary>
        /// <param name="width">视口宽度</param>
        /// <param name="height">视口高度</param>
        /// <param name="near">近裁剪面距离</param>
        /// <param name="far">远裁剪面距离</param>
        /// <returns></returns>
        public static FMatrix4x4 Orthographic(FFloat width, FFloat height, FFloat near, FFloat far)
        {
            FFloat halfWidth = width / 2;
            FFloat halfHeight = height / 2;

            return Orthographic(-halfWidth, halfWidth, -halfHeight, halfHeight, near, far);
        }

        /// <summary>
        /// 构建右手坐标系的正交投影矩阵
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <param name="top"></param>
        /// <param name="near"></param>
        /// <param name="far"></param>
        /// <returns></returns>
        public static FMatrix4x4 Ortho(FFloat left, FFloat right, FFloat bottom, FFloat top, FFloat near, FFloat far)
        {
            return Orthographic(left, right, bottom, top, near, far);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <param name="b3"></param>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <param name="c3"></param>
        /// <returns></returns>
        private static FFloat Det3(
            FFloat a1, FFloat a2, FFloat a3,
            FFloat b1, FFloat b2, FFloat b3,
            FFloat c1, FFloat c2, FFloat c3)
        {
            return a1 * ((b2 * c3) - (b3 * c2)) - a2 * ((b1 * c3) - (b3 * c1)) + a3 * ((b1 * c2) - (b2 * c1));
        }

        /// <summary>
        /// 矩阵加法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FMatrix4x4 operator +(FMatrix4x4 left, FMatrix4x4 right)
        {
            return new FMatrix4x4(
                left.m00 + right.m00, left.m01 + right.m01, left.m02 + right.m02, left.m03 + right.m03,
                left.m10 + right.m10, left.m11 + right.m11, left.m12 + right.m12, left.m13 + right.m13,
                left.m20 + right.m20, left.m21 + right.m21, left.m22 + right.m22, left.m23 + right.m23,
                left.m30 + right.m30, left.m31 + right.m31, left.m32 + right.m32, left.m33 + right.m33);
        }

        /// <summary>
        /// 矩阵减法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FMatrix4x4 operator -(FMatrix4x4 left, FMatrix4x4 right)
        {
            return new FMatrix4x4(
                left.m00 - right.m00, left.m01 - right.m01, left.m02 - right.m02, left.m03 - right.m03,
                left.m10 - right.m10, left.m11 - right.m11, left.m12 - right.m12, left.m13 - right.m13,
                left.m20 - right.m20, left.m21 - right.m21, left.m22 - right.m22, left.m23 - right.m23,
                left.m30 - right.m30, left.m31 - right.m31, left.m32 - right.m32, left.m33 - right.m33);
        }

        /// <summary>
        /// 矩阵乘法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FMatrix4x4 operator *(FMatrix4x4 left, FMatrix4x4 right)
        {
            return new FMatrix4x4(
                (left.m00 * right.m00) + (left.m01 * right.m10) + (left.m02 * right.m20) + (left.m03 * right.m30),
                (left.m00 * right.m01) + (left.m01 * right.m11) + (left.m02 * right.m21) + (left.m03 * right.m31),
                (left.m00 * right.m02) + (left.m01 * right.m12) + (left.m02 * right.m22) + (left.m03 * right.m32),
                (left.m00 * right.m03) + (left.m01 * right.m13) + (left.m02 * right.m23) + (left.m03 * right.m33),
                (left.m10 * right.m00) + (left.m11 * right.m10) + (left.m12 * right.m20) + (left.m13 * right.m30),
                (left.m10 * right.m01) + (left.m11 * right.m11) + (left.m12 * right.m21) + (left.m13 * right.m31),
                (left.m10 * right.m02) + (left.m11 * right.m12) + (left.m12 * right.m22) + (left.m13 * right.m32),
                (left.m10 * right.m03) + (left.m11 * right.m13) + (left.m12 * right.m23) + (left.m13 * right.m33),
                (left.m20 * right.m00) + (left.m21 * right.m10) + (left.m22 * right.m20) + (left.m23 * right.m30),
                (left.m20 * right.m01) + (left.m21 * right.m11) + (left.m22 * right.m21) + (left.m23 * right.m31),
                (left.m20 * right.m02) + (left.m21 * right.m12) + (left.m22 * right.m22) + (left.m23 * right.m32),
                (left.m20 * right.m03) + (left.m21 * right.m13) + (left.m22 * right.m23) + (left.m23 * right.m33),
                (left.m30 * right.m00) + (left.m31 * right.m10) + (left.m32 * right.m20) + (left.m33 * right.m30),
                (left.m30 * right.m01) + (left.m31 * right.m11) + (left.m32 * right.m21) + (left.m33 * right.m31),
                (left.m30 * right.m02) + (left.m31 * right.m12) + (left.m32 * right.m22) + (left.m33 * right.m32),
                (left.m30 * right.m03) + (left.m31 * right.m13) + (left.m32 * right.m23) + (left.m33 * right.m33));
        }

        /// <summary>
        /// 矩阵乘法
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static FVector4 operator *(FMatrix4x4 matrix, FVector4 vector)
        {
            return matrix.Multiply(vector);
        }

        /// <summary>
        /// 矩阵乘法
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static FVector3 operator *(FMatrix4x4 matrix, FVector3 point)
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
        public static bool Approximately(FMatrix4x4 left, FMatrix4x4 right, FFloat tolerance)
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
                FMath.Abs(left.m23 - right.m23) <= tolerance &&
                FMath.Abs(left.m30 - right.m30) <= tolerance &&
                FMath.Abs(left.m31 - right.m31) <= tolerance &&
                FMath.Abs(left.m32 - right.m32) <= tolerance &&
                FMath.Abs(left.m33 - right.m33) <= tolerance;
        }

        /// <summary>
        /// 判断矩阵是否相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(FMatrix4x4 left, FMatrix4x4 right)
        {
            return
                left.m00 == right.m00 && left.m01 == right.m01 && left.m02 == right.m02 && left.m03 == right.m03 &&
                left.m10 == right.m10 && left.m11 == right.m11 && left.m12 == right.m12 && left.m13 == right.m13 &&
                left.m20 == right.m20 && left.m21 == right.m21 && left.m22 == right.m22 && left.m23 == right.m23 &&
                left.m30 == right.m30 && left.m31 == right.m31 && left.m32 == right.m32 && left.m33 == right.m33;
        }

        /// <summary>
        /// 判断矩阵是否不等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(FMatrix4x4 left, FMatrix4x4 right)
        {
            return !(left == right);
        }

        /// <summary>
        /// 判断矩阵是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is FMatrix4x4 matrix && this == matrix;
        }

        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return m00.GetHashCode() ^ m01.GetHashCode() ^ m02.GetHashCode() ^ m03.GetHashCode() ^
                m10.GetHashCode() ^ m11.GetHashCode() ^ m12.GetHashCode() ^ m13.GetHashCode() ^
                m20.GetHashCode() ^ m21.GetHashCode() ^ m22.GetHashCode() ^ m23.GetHashCode() ^
                m30.GetHashCode() ^ m31.GetHashCode() ^ m32.GetHashCode() ^ m33.GetHashCode();
        }

        /// <summary>
        /// 转为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"(({m00},{m01},{m02},{m03}),({m10},{m11},{m12},{m13}),({m20},{m21},{m22},{m23}),({m30},{m31},{m32},{m33}))";
        }
    }
}
