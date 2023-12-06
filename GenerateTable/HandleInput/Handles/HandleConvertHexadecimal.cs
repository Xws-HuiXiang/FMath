using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateTable
{
    public class HandleConvertHexadecimal
    {
        public static void OnEnterState()
        {
            Console.WriteLine("是否将值转换为十六进制表示（true 或 false，默认为 true）");
        }

        public static bool Handle(InputState inputState, string? content)
        {
            if (string.IsNullOrEmpty(content))
            {
                Program.OutputOptions.ConvertHexadecimal = true;
            }
            else
            {
                if (bool.TryParse(content, out bool convertHexadecimal))
                {
                    Program.OutputOptions.ConvertHexadecimal = convertHexadecimal;
                }
                else
                {
                    Console.WriteLine("请输入‘true’或‘false’");

                    return false;
                }
            }

            return true;
        }
    }
}
