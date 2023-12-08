using GenerateTable.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateTable
{
    /// <summary>
    /// 处理输入
    /// </summary>
    public class HandleInput
    {
        /// <summary>
        /// 处理输入的字典。返回值代表是否能进入下个状态
        /// </summary>
        private static Dictionary<InputState, Func<InputState, string?, bool>?> handleActionDict = new Dictionary<InputState, Func<InputState, string?, bool>?>();

        /// <summary>
        /// 初始化所有处理器
        /// </summary>
        public static void Init()
        {
            Program.NowInputState = InputState.SelectTrigonometricFunction;

            RegisterHandleAction(InputState.SelectTrigonometricFunction, HandleSelectTrigonometricFunction.Handle);
            RegisterHandleAction(InputState.SelectSliceAmount, HandleSelectSliceAmount.Handle);
            RegisterHandleAction(InputState.Magnification, HandleMagnification.Handle);
            RegisterHandleAction(InputState.ConvertHexadecimal, HandleConvertHexadecimal.Handle);
            RegisterHandleAction(InputState.Restart, HandleRestart.Handle);
        }

        /// <summary>
        /// 处理输入
        /// </summary>
        /// <param name="content"></param>
        /// <returns>是否退出程序</returns>
        public static bool Handle(string? content)
        {
            //如果输入了 quit 则退出程序
            if(content != null && content.Equals("quit", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            //如果输入了 back 则返回上一步
            if(content != null && content.Equals("back", StringComparison.OrdinalIgnoreCase))
            {
                BackInput();

                return false;
            }

            if(handleActionDict.TryGetValue(Program.NowInputState, out Func<InputState, string?, bool>? callback))
            {
                bool? result = callback?.Invoke(Program.NowInputState, content);
                if(result != null)
                {
                    if (result.Value)
                    {
                        //循环输入状态
                        switch (Program.NowInputState)
                        {
                            case InputState.SelectTrigonometricFunction:
                                Program.NowInputState = InputState.SelectSliceAmount;
                                break;
                            case InputState.SelectSliceAmount:
                                Program.NowInputState = InputState.Magnification;
                                break;
                            case InputState.Magnification:
                                Program.NowInputState = InputState.ConvertHexadecimal;
                                break;
                            case InputState.ConvertHexadecimal:
                                Program.NowInputState = InputState.Restart;
                                break;
                            case InputState.Restart:
                                Program.NowInputState = InputState.SelectTrigonometricFunction;
                                break;
                            default:
                                break;
                        }

                        //如果选择完最后一项，则准备输出内容
                        if(Program.NowInputState == InputState.Restart)
                        {
                            Console.WriteLine("准备生成表格内容");
                            OutputTable.Output();
                            Console.WriteLine($"{Program.OutputOptions.GenerateType}函数值生成完成，定义域为({Program.OutputOptions.DefineDomainMin},{Program.OutputOptions.DefineDomainMax})。拆分为{Program.OutputOptions.SliceAmount}段，放大{Program.OutputOptions.Magnification}倍。是否转换为十六进制：{(Program.OutputOptions.ConvertHexadecimal ? "是" : "否")}");
                            Console.WriteLine("生成的内容已拷贝到剪切板。再次按下回车键将重新生成");
                        }
                    }
                }
            }


            return false;
        }

        /// <summary>
        /// 返回上一步输入
        /// </summary>
        public static void BackInput()
        {
            switch (Program.NowInputState)
            {
                case InputState.SelectTrigonometricFunction:
                    break;
                case InputState.SelectSliceAmount:
                    Program.NowInputState = InputState.SelectTrigonometricFunction;
                    break;
                case InputState.Magnification:
                    Program.NowInputState = InputState.SelectSliceAmount;
                    break;
                case InputState.ConvertHexadecimal:
                    Program.NowInputState = InputState.Magnification;
                    break;
                case InputState.Restart:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 注册输入处理函数
        /// </summary>
        /// <param name="inputState"></param>
        /// <param name="callback"></param>
        public static void RegisterHandleAction(InputState inputState, Func<InputState, string?, bool> callback)
        {
            if (callback == null)
            {
                Console.WriteLine($"注册输入处理函数失败，输入类型：{inputState}的处理函数不能为空");

                return;
            }

            if (!handleActionDict.ContainsKey(inputState))
            {
                handleActionDict.Add(inputState, callback);
            }
            else
            {
                Console.WriteLine($"注册输入处理函数失败，不能重复注册类型：{inputState}");
            }
        }
    }
}
