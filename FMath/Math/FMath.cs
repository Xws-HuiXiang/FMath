using System;
using FixedMath.BaseType;

namespace FixedMath
{
    /// <summary>
    /// 定点数使用的数学运算
    /// </summary>
    public static partial class FMath
    {
        /// <summary>
        /// 返回指定数字的方根
        /// <para>使用整数二分法开方</para>
        /// </summary>
        /// <param name="value">需要开方的值</param>
        /// <returns></returns>
        public static FFloat Sqrt(FFloat value)
        {
            if (value == FFloat.Zero) return 0;
            if (value < 0) throw new ArgumentException("尝试对负数开方");

            FInt128 target = FInt128.FromInt64(value.RawValue) << FFloat.BitMoveCount;
            ulong low = 0;
            ulong high = 1;

            while (CompareSquare(high, target) <= 0)
                high <<= 1;

            ulong result = 0;
            high--;

            while (low <= high)
            {
                ulong mid = low + ((high - low) >> 1);
                int compare = CompareSquare(mid, target);

                if (compare <= 0)
                {
                    result = mid;
                    low = mid + 1;
                }
                else
                {
                    high = mid - 1;
                }
            }

            return FFloat.FromRaw((long)result);
        }

        /// <summary>
        /// value的平方值与target的值做比较
        /// <para>如果left小于right，则返回-1；若left大于right，则返回1；否则返回0</para>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private static int CompareSquare(ulong value, FInt128 target)
        {
            FInt128 square = FInt128.MultiplyUnsigned(value, value);

            return FInt128.CompareUnsigned(square, target);
        }

        /// <summary>
        /// 计算 x 的 y 次方
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        /// <exception cref="DivideByZeroException"></exception>
        public static FFloat Pow(FFloat x, int y)
        {
            //任何一个数都可以表示为2^n的和，所以循环n次就可以变成循环n的二进制位数次
            if (x == FFloat.Zero)
            {
                if (y == 0)
                    return FFloat.One;
                if (y > 0)
                    return FFloat.Zero;

                throw new DivideByZeroException();
            }
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
            if (newBase <= 0)
                throw new ArgumentException("对数换底时的新底值必须大于0");
            if (newBase == 1)
                throw new ArgumentException("对数换底时的新底值必须不为1");

            //先换底，换成（以e为底value的对数 除以 以e为底newBase的对数）
            //分别计算自然对数求结果
            FFloat v1 = LogE(value);
            FFloat v2 = LogE(newBase);

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
        /// <para>注意：数字越大，结果越不精确。若需要大数字的相对精确的结果，请使用重载函数‘LogE(FFloat value, int expandCount)’并指定‘expandCount’参数为适宜的大小</para>
        /// <para>默认多项式展开次数为16</para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FFloat LogE(FFloat value)
        {
            return LogE(value, 16);
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
            if (expandCount <= 0) throw new ArgumentException("自然对数的多项式展开次数必须大于0", nameof(expandCount));

            FFloat normalized = value;
            FFloat exponent = FFloat.Zero;

            while (normalized > 2)
            {
                normalized >>= 1;
                exponent += 1;
            }

            while (normalized < new FFloat(0.5))
            {
                normalized <<= 1;
                exponent -= 1;
            }

            //自然对数的泰勒展开式：ln(x) = ln((1+y)/(1-y))=2y((1/1*y^0) + (1/3*y^2) + (1/5*y^4) + (1/7*y^6) + ...)
            //其中，y=(x-1)/(x+1)
            FFloat y = (normalized - 1) / (normalized + 1);
            FFloat y2 = y * y;
            FFloat sum = 0;
            FFloat term = 1;
            for(int i = 1; i <= expandCount; i++)
            {
                FFloat v = new FFloat(1) / ((2 * i) - 1);
                sum += v * term;
                term *= y2;
            }

            FFloat res = (2 * y * sum) + (exponent * Ln2);

            return res;
        }

        /// <summary>
        /// 向上取整
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FFloat Ceiling(FFloat value)
        {
            long raw = value.RawValue;

            long integer = raw >> FFloat.BitMoveCount;

            if ((raw & (FFloat.MULTIPLER_FACTOR - 1)) != 0)
                integer++;

            return FFloat.FromRaw(integer << FFloat.BitMoveCount, true);
        }

        /// <summary>
        /// 向下取整
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FFloat Floor(FFloat value)
        {
            long raw = value.RawValue;
            long integerRaw = (raw >> FFloat.BitMoveCount) << FFloat.BitMoveCount;

            return FFloat.FromRaw(integerRaw, true);
        }

        /// <summary>
        /// 返回给定数字中最大的值
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FFloat Max(FFloat left, FFloat right)
        {
            return left > right ? left : right;
        }

        /// <summary>
        /// 返回给定数字中最大的值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static FFloat Max(FFloat a, FFloat b, FFloat c)
        {
            return Max(Max(a, b), c);
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
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FFloat Min(FFloat left, FFloat right)
        {
            return left < right ? left : right;
        }

        /// <summary>
        /// 返回给定数字中最小的值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static FFloat Min(FFloat a, FFloat b, FFloat c)
        {
            return Min(Min(a, b), c);
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
            return new FFloat(value.Int);
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
        public static int Abs(int value)
        {
            int mask = value >> 31;

            return ((value ^ mask) - mask);
        }

        /// <summary>
        /// 返回指定数字的绝对值，结果为long类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long AbsToLong(int value)
        {
            if (value == int.MinValue)
                return 2147483648L;

            return Abs(value);
        }

        /// <summary>
        /// 返回指定数字的绝对值，结果为定点数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FFloat AbsToFFloat(int value)
        {
            return FFloat.FromRaw(AbsToLong(value) << FFloat.BitMoveCount);
        }

        /// <summary>
        /// 弧度标准化，用于将任意给定的弧度值限制在 (-π, π] 区间内
        /// <para>区间选择(-π, π]是考虑与C#的数学库对齐</para>
        /// </summary>
        /// <param name="radian">弧度值</param>
        /// <returns></returns>
        private static FFloat NormalizeRadiansSigned(FFloat radian)
        {
            radian %= PI2;

            if (radian > PI)
                radian -= PI2;
            else if (radian <= -PI)
                radian += PI2;

            return radian;
        }

        /// <summary>
        /// CORDIC 算法实现。用于计算给定弧度 radians 的正弦和余弦值
        /// </summary>
        /// <param name="radians">弧度值</param>
        /// <param name="sin">改弧度值对应的sin值</param>
        /// <param name="cos">改弧度值对应的cos值</param>
        private static void CordicRotate(FFloat radians, out FFloat sin, out FFloat cos)
        {
            FFloat sign = FFloat.One;
            //先将弧度归一化
            radians = NormalizeRadiansSigned(radians);

            if (radians > HalfPI)
            {
                radians -= PI;
                sign = -1;
            }
            else if (radians < -HalfPI)
            {
                radians += PI;
                sign = -1;
            }

            FFloat x = CordicK;
            FFloat y = FFloat.Zero;
            FFloat z = radians;

            for (int i = 0; i < CordicAtanTable.Length; i++)
            {
                FFloat xShift = x >> i;
                FFloat yShift = y >> i;

                if (z >= 0)
                {
                    x -= yShift;
                    y += xShift;
                    z -= CordicAtanTable[i];
                }
                else
                {
                    x += yShift;
                    y -= xShift;
                    z += CordicAtanTable[i];
                }
            }

            sin = y * sign;
            cos = x * sign;
        }

        /// <summary>
        /// CORDIC ATan2 算法实现。用于计算二维向量 (x, y) 与 X 轴正方向的夹角（即方位角），结果以弧度表示，范围在 [-π, π] 之间
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        private static FFloat CordicAtan2(FFloat y, FFloat x)
        {
            if (x == 0)
            {
                if (y > 0)
                    return HalfPI;
                if (y < 0)
                    return -HalfPI;

                return FFloat.Zero;
            }

            FFloat angle = FFloat.Zero;
            FFloat xValue = x;
            FFloat yValue = y;

            if (xValue < 0)
            {
                angle = yValue >= 0 ? PI : -PI;
                xValue = -xValue;
                yValue = -yValue;
            }

            for (int i = 0; i < CordicAtanTable.Length; i++)
            {
                if (yValue == 0)
                    break;

                FFloat xShift = xValue >> i;
                FFloat yShift = yValue >> i;

                if (yValue >= 0)
                {
                    xValue += yShift;
                    yValue -= xShift;
                    angle += CordicAtanTable[i];
                }
                else
                {
                    xValue -= yShift;
                    yValue += xShift;
                    angle -= CordicAtanTable[i];
                }
            }

            return angle;
        }

        /// <summary>
        /// 正弦函数
        /// </summary>
        /// <param name="radian">弧度值</param>
        /// <returns></returns>
        public static FFloat Sin(FFloat radian)
        {
            SinCos(radian, out FFloat sin, out _);

            return sin;
        }

        /// <summary>
        /// 正弦函数
        /// </summary>
        /// <param name="angle">角度值</param>
        /// <returns></returns>
        public static FFloat SinAngle(FFloat angle)
        {
            //处理角度在0~360度之间
            angle = Normalize360(angle);

            return Sin(angle * Deg2Rad);
        }

        /// <summary>
        /// 余弦函数
        /// </summary>
        /// <param name="radian">弧度值</param>
        /// <returns></returns>
        public static FFloat Cos(FFloat radian)
        {
            SinCos(radian, out _, out FFloat cos);

            return cos;
        }

        /// <summary>
        /// 余弦函数
        /// </summary>
        /// <param name="angle">角度值</param>
        /// <returns></returns>
        public static FFloat CosAngle(FFloat angle)
        {
            //处理角度在0~360度之间
            angle = Normalize360(angle);

            return Cos(angle * Deg2Rad);
        }

        /// <summary>
        /// 正切函数
        /// </summary>
        /// <param name="radian">弧度值</param>
        /// <returns></returns>
        public static FFloat Tan(FFloat radian)
        {
            SinCos(radian, out FFloat sin, out FFloat cos);

            //防止接近0导致结果过大。这两种方式都可以
            /*
            if (Abs(cos).RawValue <= 8)
                throw new DivideByZeroException();
            */
            if (cos == FFloat.Zero) throw new DivideByZeroException();

            return sin / cos;
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
            angle = NormalizeAngle90(angle);

            if (angle == 90 || angle == -90)
                throw new DivideByZeroException();

            return Tan(angle * Deg2Rad);
        }

        /// <summary>
        /// 反正弦函数，结果为弧度
        /// <para>注意：当输入值大于1时，将返回表中最后一个值；当输入值小于-1时，将返回表中第一个值</para>
        /// </summary>
        /// <param name="value">输入范围-1~1</param>
        /// <returns></returns>
        public static FFloat Asin(FFloat value)
        {
            if (value >= 1)
                return HalfPI;
            if (value <= -1)
                return -HalfPI;

            FFloat inside = 1 - (value * value);
            if (inside < 0)
                inside = 0;

            return Atan2(value, Sqrt(inside));
        }

        /// <summary>
        /// 反余弦函数，结果为弧度
        /// <para>注意：当输入值大于1时，将返回表中最后一个值；当输入值小于-1时，将返回表中第一个值</para>
        /// </summary>
        /// <param name="value">输入范围-1~1</param>
        /// <returns></returns>
        public static FFloat Acos(FFloat value)
        {
            if (value >= 1)
                return FFloat.Zero;
            if (value <= -1)
                return PI;

            FFloat inside = 1 - (value * value);
            if (inside < 0)
                inside = 0;

            return Atan2(Sqrt(inside), value);
        }

        /// <summary>
        /// 反正切函数，结果为弧度
        /// <para>使用CORDIC算法获取计算结果</para>
        /// </summary>
        /// <param name="value">任意实数</param>
        /// <returns></returns>
        public static FFloat Atan(FFloat value)
        {
            return Atan2(value, FFloat.One);
        }

        /// <summary>
        /// 反正切函数，结果为弧度
        /// <para>函数将使用反正切函数的泰勒展开式计算结果。这种计算方式比较昂贵，但精度比较好</para>
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="expandCount">多项式展开次数</param>
        /// <returns></returns>
        public static FFloat Atan(FFloat value, int expandCount)
        {
            if (expandCount <= 0)
                throw new ArgumentException("反正切函数的多项式展开次数不能小于等于0");

            if (value == 0)
                return FFloat.Zero;
            if (value > 1)
                return HalfPI - Atan(FFloat.One / value, expandCount);
            if (value < -1)
                return -HalfPI - Atan(FFloat.One / value, expandCount);

            FFloat half = FFloat.FromRaw(FFloat.MULTIPLER_FACTOR / 2);
            if (Abs(value) > half)
            {
                FFloat reduced = value / (1 + Sqrt(1 + (value * value)));

                return 2 * Atan(reduced, expandCount);
            }

            FFloat result = FFloat.Zero;
            FFloat term = value;
            FFloat value2 = value * value;

            for (int i = 0; i < expandCount; i++)
            {
                FFloat item = term / ((2 * i) + 1);

                if ((i & 1) == 0)
                    result += item;
                else
                    result -= item;

                term *= value2;
            }

            return result;
        }

        /// <summary>
        /// 同时计算正弦和余弦
        /// </summary>
        /// <param name="radians">弧度值</param>
        /// <param name="sin"></param>
        /// <param name="cos"></param>
        public static void SinCos(FFloat radians, out FFloat sin, out FFloat cos)
        {
            CordicRotate(radians, out sin, out cos);
        }

        /// <summary>
        /// 反正切函数
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static FFloat Atan2(FFloat y, FFloat x)
        {
            return CordicAtan2(y, x);
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
        public static FFloat Normalize360(FFloat angle)
        {
            angle %= 360;

            if (angle < 0)
                angle += 360;

            return angle;
        }

        /// <summary>
        /// 以 π 为周期，钳制输入角度到（-90~90）度之间
        /// </summary>
        /// <param name="angle">欧拉角</param>
        /// <returns></returns>
        public static FFloat NormalizeAngle90(FFloat angle)
        {
            angle %= 180;

            if (angle > 90)
                angle -= 180;
            else if (angle < -90)
                angle += 180;

            return angle;
        }
    }
}
