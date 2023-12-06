using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateTable
{
    /// <summary>
    /// 处理选择三角函数
    /// </summary>
    public class HandleSelectTrigonometricFunction
    {
        public static void OnEnterState()
        {
            Console.WriteLine("从以下列表中选择要生成的三角函数（输入序号或函数名称）：");
            Console.WriteLine("1. Sin（正弦函数）");
            Console.WriteLine("2. Cos（余弦函数）");
            Console.WriteLine("3. Tan（正切函数）");
        }

        public static bool Handle(InputState inputState, string? content)
        {
            if (string.IsNullOrEmpty(content)) return false;

            if (content.Equals("sin", StringComparison.OrdinalIgnoreCase) || content.Equals("1"))
            {
                Program.OutputOptions.GenerateType = GenerateType.Sin;
                //定义域为（0 ~ 2PI）
                Program.OutputOptions.DefineDomainMin = 0;
                Program.OutputOptions.DefineDomainMax = 2 * Math.PI;
            }
            else if (content.Equals("cos", StringComparison.OrdinalIgnoreCase) || content.Equals("2"))
            {
                Program.OutputOptions.GenerateType = GenerateType.Cos;
                //定义域为（0 ~ 2PI）
                Program.OutputOptions.DefineDomainMin = 0;
                Program.OutputOptions.DefineDomainMax = 2 * Math.PI;
            }
            else if (content.Equals("tan", StringComparison.OrdinalIgnoreCase) || content.Equals("3"))
            {
                Program.OutputOptions.GenerateType = GenerateType.Tan;
                //定义域为（-0.5PI ~ 0.5PI）
                Program.OutputOptions.DefineDomainMin = -0.5 * Math.PI;
                Program.OutputOptions.DefineDomainMax = 0.5 * Math.PI;
            }
            else
            {
                Console.WriteLine($"未知的索引：{content}，请重新输入");

                return false;
            }

            return true;
        }
    }
}
