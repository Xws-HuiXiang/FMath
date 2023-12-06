using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateTable.Output
{
    /// <summary>
    /// 输出选项
    /// </summary>
    public class OutputOptions
    {
        private GenerateType generateType;
        /// <summary>
        /// 选择的三角函数
        /// </summary>
        public GenerateType GenerateType
        {
            get { return generateType; }
            set { generateType = value; }
        }
        private double defineDomainMin;
        private double defineDomainMax;
        /// <summary>
        /// 定义域最小值
        /// </summary>
        public double DefineDomainMin
        {
            get { return defineDomainMin; }
            set { defineDomainMin = value; }
        }
        /// <summary>
        /// 定义域最大值
        /// </summary>
        public double DefineDomainMax
        {
            get { return defineDomainMax; }
            set { defineDomainMax = value; }
        }
        private int sliceAmount;
        /// <summary>
        /// 将定义域划分为多少段
        /// </summary>
        public int SliceAmount
        {
            get { return sliceAmount; }
            set { sliceAmount = value; }
        }
        private int magnification;
        /// <summary>
        /// 放大倍数
        /// </summary>
        public int Magnification
        {
            get { return magnification; }
            set { magnification = value; }
        }
        private bool convertHexadecimal;
        /// <summary>
        /// 是否将值转换为十六进制表示
        /// </summary>
        public bool ConvertHexadecimal
        {
            get { return convertHexadecimal; }
            set { convertHexadecimal = value; }
        }
    }
}
