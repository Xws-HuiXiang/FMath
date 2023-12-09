using System;
using System.Linq;

namespace FixedMath
{
    /// <summary>
    /// 定点数使用的数学运算
    /// </summary>
    public static class FMath
    {
        /// <summary>
        /// π
        /// </summary>
        public readonly static FFloat PI = new FFloat(3.1415926535);
        /// <summary>
        /// π对应的角度值
        /// </summary>
        public const int PIAngle = 360;
        /// <summary>
        /// 2π
        /// </summary>
        public readonly static FFloat PI2 = 2 * FMath.PI;
        /// <summary>
        /// 2π对应的角度值
        /// </summary>
        public const int PI2Angle = 360;
        /// <summary>
        /// π/2
        /// </summary>
        public readonly static FFloat HalfPI = FMath.PI / 2;
        /// <summary>
        /// π/2对应的角度值
        /// </summary>
        public readonly static FFloat HalfPIAngle = 90;
        /// <summary>
        /// 自然对数基数 e
        /// </summary>
        public readonly static FFloat E = new FFloat(2.7182818284);
        /// <summary>
        /// 弧度转角度的常量：180/π
        /// </summary>
        public readonly static FFloat Rad2Deg = 180 / FMath.PI;
        /// <summary>
        /// 角度转弧度的常量：π/180
        /// </summary>
        public readonly static FFloat Deg2Rad = FMath.PI / 180;

        /// <summary>
        /// 返回指定数字的平方根
        /// </summary>
        /// <param name="value">需要开方的值</param>
        /// <param name="interatorCount">迭代次数</param>
        /// <returns></returns>
        public static FFloat Sqrt(FFloat value, int interatorCount = 8)
        {
            if (value == FFloat.Zero) return 0;
            if (value < 0) throw new ArgumentException("尝试对负数开平方");

            //使用牛顿迭代法计算平方根
            FFloat result = value;
            FFloat history;
            int count = 0;
            do
            {
                history = result;
                //注：右移一位的结果为“值除以2”
                result = (result + value / result) >> 1;

                ++count;
            } while (result != history && count < interatorCount);

            return result;
        }

        /// <summary>
        /// 计算 x 的 y 次方
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static FFloat Pow(FFloat x, int y)
        {
            //任何一个数都可以表示为2^n的和，所以循环n次就可以变成循环n的二进制位数次
            if (x == FFloat.Zero) return 1;
            long b = y;
            if(b < 0)
            {
                x = 1 / x;
                b = -b;
            }
            FFloat res = 1;
            while (b != 0)
            {
                if ((b & 1) == 1)
                    res *= x;
                x *= x;
                b >>= 1;
            }

            return res;
        }

        /// <summary>
        /// 返回指定数字在使用指定底时的对数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="newBase"></param>
        /// <returns></returns>
        public static FFloat Log(FFloat value, FFloat newBase)
        {
            if (value <= FFloat.Zero) throw new ArgumentException("负数与零无对数");
            if (value == FFloat.One) return FFloat.Zero;

            //先换底，换成（以e为底value的对数 除以 以e为底newBase的对数）
            FFloat v1 = LogE(value);
            FFloat v2 = LogE(newBase);
            //分别计算自然对数求结果

            return v1 / v2;
        }

        /// <summary>
        /// 返回指定数字在使用 2 为底数时的对数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FFloat Log2(FFloat value)
        {
            return FMath.Log(value, 2);
        }

        /// <summary>
        /// 返回指定数字在使用 10 为底数时的对数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FFloat Log10(FFloat value)
        {
            return FMath.Log(value, 10);
        }

        /// <summary>
        /// 返回指定数字在使用 e 为底数时的对数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FFloat LogE(FFloat value)
        {
            return LogE(value, 8);
        }

        /// <summary>
        /// 返回指定数字在使用 e 为底数时的对数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="expandCount">多项式展开次数</param>
        /// <returns></returns>
        public static FFloat LogE(FFloat value, int expandCount)
        {
            if (value <= FFloat.Zero) throw new ArgumentException("负数与零无对数");
            if (value == FFloat.One) return FFloat.Zero;

            FFloat res = FFloat.Zero;
            if(value > 1)
            {
                //当x>1时，自然对数的泰勒展开： ln(1+x) = x - (x^2)/2 + (x^3)/3 - (x^4)/4 + ... + (-1)^(n-1)*(x^n)/n + ...
                FFloat x = value - 1;
                for (int i = 1; i <= expandCount; i++)
                {
                    FFloat v = (FMath.Pow(-1, (i - 1))) * FMath.Pow(x, i) / i;

                    res += v;
                }
            }
            else
            {
                res = FFloat.Zero;
            }

            return res;
        }

        /// <summary>
        /// 向上取整
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FFloat Ceiling(FFloat value)
        {
            return (value.Int + 1);
        }

        /// <summary>
        /// 向下取整
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FFloat Floor(FFloat value)
        {
            return value.Int;
        }

        /// <summary>
        /// 返回给定数字中最大的值
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static FFloat Max(params FFloat[] values)
        {
            if(values == null || values.Length == 0)
                return FFloat.Zero;

            FFloat res = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] > res)
                    res = values[i];
            }

            return res;
        }

        /// <summary>
        /// 返回给定数字中最小的值
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static FFloat Min(params FFloat[] values)
        {
            if (values == null || values.Length == 0)
                return FFloat.Zero;

            FFloat res = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] < res)
                    res = values[i];
            }

            return res;
        }

        /// <summary>
        /// 返回数字的整数部分
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FFloat Truncate(FFloat value)
        {
            return value.Int;
        }

        /// <summary>
        /// 返回指定数字的绝对值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FFloat Abs(FFloat value)
        {
            if (value < 0)
                return -value;

            return value;
        }

        /// <summary>
        /// 返回指定数字的绝对值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FFloat Abs(int value)
        {
            int mask = value >> 31;

            return ((value ^ mask) - mask);
        }

        /// <summary>
        /// 正弦函数
        /// </summary>
        /// <param name="radian">弧度值</param>
        /// <returns></returns>
        public static FFloat Sin(FFloat radian)
        {
            //处理弧度值在0~2PI范围内
            FFloat value = radian / PI2;
            if(radian > 0)
                radian -= PI2 * value.Int;
            else if(radian < 0)
                radian += PI2 * value.Int;
            //处理负值
            if (radian < 0)
                radian += PI2;
            //将 radian 重映射到[0 ~ TotalCount]作为数组索引
            int nMax = FMathTable.SinTable.Length;
            int nMin = 0;
            FFloat oMax = FMath.PI2;
            FFloat oMin = 0;
            FFloat index = ((nMax - nMin) / (oMax - oMin)) * (radian - oMin) + nMin;
            //钳制索引在数组范围内
            index = FMath.Clamp(index, 0, FMathTable.SinTable.Length - 1);
            FFloat v = FMathTable.SinTable[index.RoundToInt];

            v /= FMathTable.SCALE;

            return v;
        }

        /// <summary>
        /// 正弦函数
        /// </summary>
        /// <param name="angle">角度值</param>
        /// <returns></returns>
        public static FFloat SinAngle(FFloat angle)
        {
            //处理角度在0~360度之间
            angle = ClampEulerAngle360(angle);

            return Sin(angle * FMath.Deg2Rad);
        }

        /// <summary>
        /// 余弦函数
        /// </summary>
        /// <param name="radian">弧度值</param>
        /// <returns></returns>
        public static FFloat Cos(FFloat radian)
        {
            //处理弧度值在0~2PI范围内
            FFloat value = radian / PI2;
            if (radian > 0)
                radian -= PI2 * value.Int;
            else if (radian < 0)
                radian += PI2 * value.Int;
            //处理负值
            if (radian < 0)
                radian += PI2;
            //将 radian 重映射到[0 ~ TotalCount]作为数组索引
            int nMax = FMathTable.CosTable.Length;
            int nMin = 0;
            FFloat oMax = FMath.PI2;
            FFloat oMin = 0;
            FFloat index = ((nMax - nMin) / (oMax - oMin)) * (radian - oMin) + nMin;
            //钳制索引在数组范围内
            index = FMath.Clamp(index, 0, FMathTable.CosTable.Length - 1);
            FFloat v = FMathTable.CosTable[index.RoundToInt];

            v /= FMathTable.SCALE;

            return v;
        }

        /// <summary>
        /// 余弦函数
        /// </summary>
        /// <param name="angle">角度值</param>
        /// <returns></returns>
        public static FFloat CosAngle(FFloat angle)
        {
            //处理角度在0~360度之间
            angle = ClampEulerAngle360(angle);

            return Cos(angle * FMath.Deg2Rad);
        }

        /// <summary>
        /// 正切函数
        /// <para>注意当弧度值接近极限值（例如 0.5π、1.5π等），因为结果为查表所得所以结果不一定准确，甚至会出现正负号与期望值不同的问题</para>
        /// </summary>
        /// <param name="radian">弧度值</param>
        /// <returns></returns>
        public static FFloat Tan(FFloat radian)
        {
            //处理弧度值在(-PI/2~PI/2)范围内
            FFloat value = radian / PI;
            if (radian < -HalfPI)
                radian += PI * value.Int;
            else if(radian > HalfPI)
                radian -= PI * value.Int;
            //将 radian 重映射到[0 ~ TotalCount]作为数组索引
            int nMax = FMathTable.TanTable.Length;
            int nMin = 0;
            FFloat oMax = FMath.HalfPI;
            FFloat oMin = -FMath.HalfPI;
            FFloat index = ((nMax - nMin) / (oMax - oMin)) * (radian - oMin) + nMin;
            //钳制索引在数组范围内
            index = FMath.Clamp(index, 0, FMathTable.TanTable.Length - 1);
            FFloat v = FMathTable.TanTable[index.RoundToInt];

            v /= FMathTable.SCALE;

            return v;
        }

        /// <summary>
        /// 正切函数
        /// </summary>
        /// <param name="angle">角度值</param>
        /// <para>注意当角度值接近极限值（例如 90°、270°等），因为结果为查表所得所以结果不一定准确，甚至会出现正负号与期望值不同的问题</para>
        /// <returns></returns>
        public static FFloat TanAngle(FFloat angle)
        {
            //处理角度在-180~180度之间
            angle = ClampEulerAngle90(angle);

            return Tan(angle * FMath.Deg2Rad);
        }

        /// <summary>
        /// 反正弦函数
        /// <para>注意：当输入值大于1时，将返回表中最后一个值；当输入值小于-1时，将返回表中第一个值</para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FFloat Asin(FFloat value)
        {
            if (value >= 1)
                return FMathTable.AsinTable.Last();
            if (value <= -1)
                return FMathTable.AsinTable.First();

            int halfLength = FMathTable.AsinTable.Length / 2;
            FFloat rate = (value * halfLength) + halfLength;
            rate = Clamp(rate, FFloat.Zero, FMathTable.AsinTable.Length - 1);

            FFloat res = FMathTable.AsinTable[rate.Int];
            res /= FMathTable.SCALE;

            return res;
        }

        /// <summary>
        /// 反余弦函数
        /// <para>注意：当输入值大于1时，将返回表中最后一个值；当输入值小于-1时，将返回表中第一个值</para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FFloat Acos(FFloat value)
        {
            if (value >= 1)
                return FMathTable.AcosTable.Last();
            if (value <= -1)
                return FMathTable.AcosTable.First();

            int halfLength = FMathTable.AcosTable.Length / 2;
            FFloat rate = (value * halfLength) + halfLength;
            rate = Clamp(rate, FFloat.Zero, FMathTable.AcosTable.Length - 1);

            FFloat res = FMathTable.AcosTable[rate.Int];
            res /= FMathTable.SCALE;

            return res;
        }

        /// <summary>
        /// 反正切函数
        /// <para>由于定义域为整个实数域，所以无法使用查表的方式获取结果</para>
        /// <para>函数将使用反正切函数的泰勒展开式计算结果</para>
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static FFloat Atan(FFloat value)
        {
            return Atan(value, 8);
        }

        /// <summary>
        /// 反正切函数
        /// <para>由于定义域为整个实数域，所以无法使用查表的方式获取结果</para>
        /// <para>函数将使用反正切函数的泰勒展开式计算结果</para>
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="expandCount">多项式展开次数</param>
        /// <returns></returns>
        public static FFloat Atan(FFloat value, int expandCount)
        {
            FFloat res = 0;
            for (int i = 1; i <= expandCount; i++)
            {
                int k = ((2 * i) - 1);
                FFloat m = (FFloat)1 / k;
                FFloat v = FMath.Pow(value, k);

                if (i % 2 == 0)
                {
                    //多项式减
                    res -= (m * v);
                }
                else
                {
                    //多项式加
                    res += (m * v);
                }
            }

            return res;
        }

        /// <summary>
        /// 将输入值钳制在指定范围内
        /// </summary>
        /// <param name="input"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static FFloat Clamp(FFloat input, FFloat min, FFloat max)
        {
            if (input < min)
                return min;
            if (input > max)
                return max;

            return input;
        }

        /// <summary>
        /// 以 2π 为周期，钳制输入角度到（0~360）度之间
        /// </summary>
        /// <param name="angle">欧拉角</param>
        /// <returns></returns>
        public static FFloat ClampEulerAngle360(FFloat angle)
        {
            if (angle > 0)
            {
                while (angle >= FMath.PI2Angle)
                    angle -= FMath.PI2Angle;
            }
            else if (angle < 0)
            {
                while (angle < 0)
                    angle += FMath.PI2Angle;
            }

            return angle;
        }

        /// <summary>
        /// 以 π 为周期，钳制输入角度到（-90~90）度之间
        /// </summary>
        /// <param name="angle">欧拉角</param>
        /// <returns></returns>
        public static FFloat ClampEulerAngle90(FFloat angle)
        {
            if (angle > 0)
            {
                while (angle >= FMath.HalfPIAngle)
                    angle -= FMath.HalfPIAngle;
            }
            else if (angle < 0)
            {
                while (angle < 0)
                    angle += FMath.HalfPIAngle;
            }

            return angle;
        }
    }
}
