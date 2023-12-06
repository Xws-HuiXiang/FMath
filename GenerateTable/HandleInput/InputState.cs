using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateTable
{
    /// <summary>
    /// 当前处于选择哪项的状态
    /// </summary>
    public enum InputState
    {
        None,
        /// <summary>
        /// 选择三角函数
        /// </summary>
        SelectTrigonometricFunction,
        /// <summary>
        /// 选择切片数量
        /// </summary>
        SelectSliceAmount,
        /// <summary>
        /// 输入放大倍数
        /// </summary>
        Magnification,
        /// <summary>
        /// 是否转换为十六进制
        /// </summary>
        ConvertHexadecimal,
        /// <summary>
        /// 是否重新生成
        /// </summary>
        Restart
    }
}
