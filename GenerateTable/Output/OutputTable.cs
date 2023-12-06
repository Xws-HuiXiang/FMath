using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateTable.Output
{
    public class OutputTable
    {
        /// <summary>
        /// 根据选择的项输出内容
        /// </summary>
        public static void Output()
        {
            string[] outputArray = new string[Program.OutputOptions.SliceAmount];

            //根据区域计算每一份的大小
            double totalSize = Program.OutputOptions.DefineDomainMax - Program.OutputOptions.DefineDomainMin;
            double unitSize = totalSize / Program.OutputOptions.SliceAmount;
            //循环计算结果
            bool convertHexadecimal = Program.OutputOptions.ConvertHexadecimal;
            GenerateType generateType = Program.OutputOptions.GenerateType;
            int scale = Program.OutputOptions.Magnification;
            double min = Program.OutputOptions.DefineDomainMin;
            for (int i = 0; i < Program.OutputOptions.SliceAmount; i++)
            {
                //计算输入值
                double x = min + i * unitSize;
                //计算结果值
                double value = Calculation(generateType, x);
                //将结果值进行缩放
                value *= scale;
                //只取整数部分作为输出结果
                int intValue = (int)Math.Round(value);
                //格式转换
                string strValue = convertHexadecimal ? "0x" + intValue.ToString("X") : intValue.ToString();

                outputArray[i] = strValue;
            }

            //拼接数据
            StringBuilder builder = new StringBuilder();
            for(int i = 0; i <  outputArray.Length; i++)
            {
                builder.Append(outputArray[i]);
                if (i < outputArray.Length - 1)
                {
                    builder.Append(',');
                    builder.Append('\t');
                }

                //一行20个
                if ((i + 1) % 20 == 0)
                    builder.Append("\r\n");
            }
            string content = builder.ToString();

            //结果预览
            Console.WriteLine(content);

            //写入剪切板
            Clipborad.SetText(content);
        }

        /// <summary>
        /// 根据生成类型计算输入的结果
        /// </summary>
        /// <param name="generateType"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static double Calculation(GenerateType generateType, double x)
        {
            switch (generateType)
            {
                case GenerateType.Sin:
                    return Math.Sin(x);
                case GenerateType.Cos:
                    return Math.Cos(x);
                case GenerateType.Tan:
                    return Math.Tan(x);
                default:
                    break;
            }

            throw new Exception($"未处理的函数类型：{generateType}，无法计算结果");
        }
    }
}
