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
    }
}
