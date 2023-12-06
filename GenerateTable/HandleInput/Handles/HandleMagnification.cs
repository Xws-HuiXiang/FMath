using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateTable
{
    public class HandleMagnification
    {
        public static void OnEnterState()
        {
            Console.WriteLine("请输入放大倍数（不输入则使用默认的100000）：");
        }

        public static bool Handle(InputState inputState, string? content)
        {
            if (string.IsNullOrEmpty(content))
            {
                Program.OutputOptions.Magnification = 100000;
            }
            else
            {
                if (int.TryParse(content, out int value))
                {
                    if (value <= 0)
                    {
                        Console.WriteLine("请输入正整数");

                        return false;
                    }
                    else
                    {
                        Program.OutputOptions.Magnification = value;
                    }
                }
                else
                {
                    Console.WriteLine("请输入一个整数");

                    return false;
                }
            }

            return true;
        }
    }
}
