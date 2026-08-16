using System;

namespace FixedMath
{
    /// <summary>
    /// 定点数通用矩阵
    /// </summary>
    public sealed class FMatrix
    {
        private readonly FFloat[] values;

        /// <summary>
        /// 矩阵行数
        /// </summary>
        public int RowCount { get; private set; }
        /// <summary>
        /// 矩阵列数
        /// </summary>
        public int ColumnCount { get; private set; }

        /// <summary>
        /// 构建矩阵
        /// </summary>
        /// <param name="rowCount">行数</param>
        /// <param name="columnCount">列数</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public FMatrix(int rowCount, int columnCount)
        {
            if (rowCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(rowCount));
            if (columnCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(columnCount));

            RowCount = rowCount;
            ColumnCount = columnCount;
            values = new FFloat[rowCount * columnCount];
        }

        /// <summary>
        /// 构建矩阵
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="columnCount"></param>
        /// <param name="values"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public FMatrix(int rowCount, int columnCount, params FFloat[] values)
            : this(rowCount, columnCount)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));
            if (values.Length != this.values.Length)
                throw new ArgumentException("输入值数量与矩阵尺寸不匹配", nameof(values));

            Array.Copy(values, this.values, values.Length);
        }

        /// <summary>
        /// 根据索引获取对应位置的值
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public FFloat this[int row, int column]
        {
            get
            {
                CheckIndex(row, column);
                return values[(row * ColumnCount) + column];
            }
            set
            {
                CheckIndex(row, column);
                values[(row * ColumnCount) + column] = value;
            }
        }

        /// <summary>
        /// 返回所有行和所有列都是0的矩阵
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="columnCount"></param>
        /// <returns></returns>
        public static FMatrix Zero(int rowCount, int columnCount)
        {
            return new FMatrix(rowCount, columnCount);
        }

        /// <summary>
        /// 返回单位矩阵
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static FMatrix Identity(int size)
        {
            FMatrix matrix = new FMatrix(size, size);
            for (int i = 0; i < size; i++)
                matrix[i, i] = FFloat.One;

            return matrix;
        }

        /// <summary>
        /// 克隆当前矩阵
        /// </summary>
        /// <returns></returns>
        public FMatrix Clone()
        {
            return new FMatrix(RowCount, ColumnCount, ToArray());
        }

        /// <summary>
        /// 将矩阵当前的所有值转换为数组
        /// </summary>
        /// <returns></returns>
        public FFloat[] ToArray()
        {
            FFloat[] copy = new FFloat[values.Length];
            Array.Copy(values, copy, values.Length);

            return copy;
        }

        /// <summary>
        /// 计算该矩阵的转置矩阵
        /// </summary>
        /// <returns></returns>
        public FMatrix Transposed()
        {
            FMatrix result = new FMatrix(ColumnCount, RowCount);

            for (int row = 0; row < RowCount; row++)
            {
                for (int column = 0; column < ColumnCount; column++)
                    result[column, row] = this[row, column];
            }

            return result;
        }

        /// <summary>
        /// 计算指定矩阵的转置矩阵
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static FMatrix Transpose(FMatrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));

            return matrix.Transposed();
        }

        /// <summary>
        /// 矩阵加法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FMatrix operator +(FMatrix left, FMatrix right)
        {
            CheckSameSize(left, right);

            FMatrix result = new FMatrix(left.RowCount, left.ColumnCount);
            for (int i = 0; i < left.values.Length; i++)
                result.values[i] = left.values[i] + right.values[i];

            return result;
        }

        /// <summary>
        /// 矩阵减法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FMatrix operator -(FMatrix left, FMatrix right)
        {
            CheckSameSize(left, right);

            FMatrix result = new FMatrix(left.RowCount, left.ColumnCount);
            for (int i = 0; i < left.values.Length; i++)
                result.values[i] = left.values[i] - right.values[i];

            return result;
        }

        /// <summary>
        /// 矩阵乘法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static FMatrix operator *(FMatrix left, FMatrix right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));
            if (left.ColumnCount != right.RowCount)
                throw new ArgumentException("矩阵尺寸不匹配");

            FMatrix result = new FMatrix(left.RowCount, right.ColumnCount);

            for (int row = 0; row < result.RowCount; row++)
            {
                for (int column = 0; column < result.ColumnCount; column++)
                {
                    FFloat sum = FFloat.Zero;
                    for (int i = 0; i < left.ColumnCount; i++)
                        sum += left[row, i] * right[i, column];

                    result[row, column] = sum;
                }
            }

            return result;
        }

        /// <summary>
        /// 矩阵乘法
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static FMatrix operator *(FMatrix matrix, FFloat value)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));

            FMatrix result = new FMatrix(matrix.RowCount, matrix.ColumnCount);
            for (int i = 0; i < matrix.values.Length; i++)
                result.values[i] = matrix.values[i] * value;

            return result;
        }

        /// <summary>
        /// 矩阵乘法
        /// </summary>
        /// <param name="value"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static FMatrix operator *(FFloat value, FMatrix matrix)
        {
            return matrix * value;
        }

        /// <summary>
        /// 检查参与加减运算的两个矩阵的行列数量是否匹配
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        private static void CheckSameSize(FMatrix left, FMatrix right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));
            if (left.RowCount != right.RowCount || left.ColumnCount != right.ColumnCount)
                throw new ArgumentException("矩阵尺寸不匹配");
        }

        /// <summary>
        /// 索引范围检查
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        private void CheckIndex(int row, int column)
        {
            if (row < 0 || row >= RowCount)
                throw new IndexOutOfRangeException();
            if (column < 0 || column >= ColumnCount)
                throw new IndexOutOfRangeException();
        }
    }
}
