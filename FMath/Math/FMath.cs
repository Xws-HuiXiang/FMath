using System;
using System.Collections.Generic;
using System.Text;

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
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FFloat Asin(FFloat value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 反余弦函数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FFloat Acos(FFloat value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 反正切函数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FFloat Atan(FFloat value)
        {
            throw new NotImplementedException();
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
