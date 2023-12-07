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
        public readonly static FFloat PI = new FFloat(Math.PI);
        /// <summary>
        /// π对应的角度值
        /// </summary>
        public const int PIAngle = 360;
        /// <summary>
        /// 2π
        /// </summary>
        public readonly static FFloat PI2 = new FFloat(2 * Math.PI);
        /// <summary>
        /// 2π对应的角度值
        /// </summary>
        public const int PI2Angle = 360;
        /// <summary>
        /// 自然对数基数 e
        /// </summary>
        public readonly static FFloat E = new FFloat(Math.E);
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
        /// 正弦函数
        /// </summary>
        /// <param name="radian">弧度值</param>
        /// <returns></returns>
        public static FFloat Sin(FFloat radian)
        {
            //处理弧度值在0~2PI范围内
            FFloat value = radian / PI2;
            radian -= PI2 * value.Int;
            //处理负值
            if (radian < 0)
                radian += PI2;
            //将 radian 重映射到[0 ~ TotalCount]作为数组索引
            FFloat index = (radian / PI2) * FMathTable.SinTable.Length;
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
            //需要优先处理，因为定点数计算数字越大误差越大
            if (angle > 0)
            {
                while (angle >= FMath.PI2Angle)
                    angle -= FMath.PI2Angle;
            }
            else if (angle < 0)
            {
                while (angle <= FMath.PI2Angle)
                    angle += FMath.PI2Angle;
            }

            return Sin(angle * FMath.Deg2Rad);
        }

        /// <summary>
        /// 反余弦函数
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static FFloat Acos(FFloat angle)
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
    }
}
