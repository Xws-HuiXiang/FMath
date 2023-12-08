using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateTable
{
    /// <summary>
    /// 生成的三角函数类型
    /// </summary>
    public enum GenerateType
    {
        /// <summary>
        /// 正弦函数
        /// </summary>
        Sin = 1,
        /// <summary>
        /// 余弦函数
        /// </summary>
        Cos = 2,
        /// <summary>
        /// 正切函数
        /// </summary>
        Tan = 3,
        /// <summary>
        /// 反正弦函数
        /// </summary>
        Asin,
        /// <summary>
        /// 反余弦函数
        /// </summary>
        Acos
    }
}
