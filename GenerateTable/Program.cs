using GenerateTable.Output;

namespace GenerateTable
{
    public class Program
    {
        static void Main(string[] args)
        {
            HandleInput.Init();

            Console.WriteLine("欢迎使用三角函数值生成程序");
            Console.WriteLine("任何情况下，输入“quit”将退出程序。输入“back”将返回上一步");

            string? input = Console.ReadLine();
            while (!HandleInput.Handle(input))
            {
                input = Console.ReadLine();
            }

            Console.ReadLine();
        }

        private static OutputOptions outputOptions = new OutputOptions();
        /// <summary>
        /// 输出选项
        /// </summary>
        public static OutputOptions OutputOptions { get { return outputOptions; } }

        private static InputState nowInputState;
        /// <summary>
        /// 当前的输入状态
        /// </summary>
        public static InputState NowInputState
        {
            get { return nowInputState; }
            set
            {
                nowInputState = value;
                switch (value)
                {
                    case InputState.None:
                        break;
                    case InputState.SelectTrigonometricFunction:
                        HandleSelectTrigonometricFunction.OnEnterState();
                        break;
                    case InputState.SelectSliceAmount:
                        HandleSelectSliceAmount.OnEnterState();
                        break;
                    case InputState.Magnification:
                        HandleMagnification.OnEnterState();
                        break;
                    case InputState.ConvertHexadecimal:
                        HandleConvertHexadecimal.OnEnterState();
                        break;
                    case InputState.Restart:
                        HandleRestart.OnEnterState();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
