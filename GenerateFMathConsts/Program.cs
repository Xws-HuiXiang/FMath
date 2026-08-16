using System.Globalization;
using System.Text;

namespace GenerateFMathConsts
{
    internal static class Program
    {
        /// <summary>
        /// 默认缩放倍率
        /// </summary>
        private const long DefaultMultiplier = 65536;
        /// <summary>
        /// CORDIC 算法迭代次数
        /// </summary>
        private const int CordicIterationCount = 17;

        private static int Main(string[] args)
        {
            if (args.Length == 0 || IsHelp(args[0]))
            {
                PrintUsage();
                return args.Length == 0 ? 1 : 0;
            }

            if (!TryParseArgs(args, out long multiplier, out string? outputPath, out string error))
            {
                Console.Error.WriteLine(error);
                PrintUsage();
                return 1;
            }

            outputPath ??= FindDefaultOutputPath();
            string content = GenerateContent(multiplier);
            string? directory = Path.GetDirectoryName(outputPath);

            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText(outputPath, content, new UTF8Encoding(true));
            Console.WriteLine($"已生成 FMath.Consts.cs 文件");
            Console.WriteLine($"缩放倍率: {multiplier.ToString(CultureInfo.InvariantCulture)}");
            Console.WriteLine($"输出目录: {Path.GetFullPath(outputPath)}");
            WaitForKeyPress();

            return 0;
        }

        /// <summary>
        /// 等待按下任意键继续
        /// </summary>
        /// <param name="message"></param>
        public static void WaitForKeyPress(string message = "按任意键继续...")
        {
            Console.Write(message);
            Console.ReadKey(true);  // true 表示不显示按下的键
            Console.WriteLine();    // 换行，保证后续输出从新行开始
        }

        /// <summary>
        /// 解析输入参数
        /// </summary>
        /// <param name="args">参数源</param>
        /// <param name="multiplier">缩放倍率</param>
        /// <param name="outputPath">文件输出路径</param>
        /// <param name="error">错误信息</param>
        /// <returns></returns>
        private static bool TryParseArgs(string[] args, out long multiplier, out string? outputPath, out string error)
        {
            multiplier = 0;
            outputPath = null;
            error = string.Empty;

            string? multiplierText = null;

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];

                if (arg == "-m" || arg == "--multiplier")
                {
                    if (++i >= args.Length)
                    {
                        error = "缺少缩放倍率参数。";
                        return false;
                    }

                    multiplierText = args[i];
                }
                else if (arg == "-o" || arg == "--output")
                {
                    if (++i >= args.Length)
                    {
                        error = "缺少输出文件路径。";
                        return false;
                    }

                    outputPath = args[i];
                }
                else if (multiplierText == null)
                {
                    multiplierText = arg;
                }
                else if (outputPath == null)
                {
                    outputPath = arg;
                }
                else
                {
                    error = $"无法识别参数：{arg}";
                    return false;
                }
            }

            if (!long.TryParse(multiplierText, NumberStyles.Integer, CultureInfo.InvariantCulture, out multiplier) || multiplier <= 0)
            {
                error = "缩放倍率必须是大于 0 的整数。";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 生成文件具体内容
        /// </summary>
        /// <param name="multiplier"></param>
        /// <returns></returns>
        private static string GenerateContent(long multiplier)
        {
            long epsilonRaw = 1;
            long piRaw = ToRaw(Math.PI, multiplier);
            long pi2Raw = ToRaw(Math.PI * 2.0, multiplier);
            long halfPiRaw = ToRaw(Math.PI / 2.0, multiplier);
            long eRaw = ToRaw(Math.E, multiplier);
            long rad2DegRaw = ToRaw(180.0 / Math.PI, multiplier);
            long deg2RadRaw = ToRaw(Math.PI / 180.0, multiplier);
            long quaternionSlerpLinearThresholdRaw = ToRaw(0.9995, multiplier);
            long ln2Raw = ToRaw(Math.Log(2.0), multiplier);
            long cordicKRaw = ToRaw(GetCordicK(), multiplier);
            long[] cordicAtanTable = GetCordicAtanTable(multiplier);

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("using FixedMath.BaseType;");
            builder.AppendLine();
            builder.AppendLine("namespace FixedMath");
            builder.AppendLine("{");
            builder.AppendLine("    /// <summary>");
            builder.AppendLine("    /// 定点数使用的数学运算");
            builder.AppendLine("    /// </summary>");
            builder.AppendLine("    public static partial class FMath");
            builder.AppendLine("    {");
            builder.AppendLine("        /// <summary>");
            builder.AppendLine("        /// 极小值");
            builder.AppendLine("        /// </summary>");
            builder.AppendLine($"        public readonly static FFloat Epsilon = FFloat.FromRaw({epsilonRaw.ToString(CultureInfo.InvariantCulture)});");
            builder.AppendLine("        /// <summary>");
            builder.AppendLine("        /// π");
            builder.AppendLine("        /// </summary>");
            builder.AppendLine($"        public readonly static FFloat PI = FFloat.FromRaw({piRaw.ToString(CultureInfo.InvariantCulture)});");
            builder.AppendLine("        /// <summary>");
            builder.AppendLine("        /// π对应的角度值");
            builder.AppendLine("        /// </summary>");
            builder.AppendLine("        public const int PIAngle = 180;");
            builder.AppendLine("        /// <summary>");
            builder.AppendLine("        /// 2π");
            builder.AppendLine("        /// </summary>");
            builder.AppendLine($"        public readonly static FFloat PI2 = FFloat.FromRaw({pi2Raw.ToString(CultureInfo.InvariantCulture)});");
            builder.AppendLine("        /// <summary>");
            builder.AppendLine("        /// 2π对应的角度值");
            builder.AppendLine("        /// </summary>");
            builder.AppendLine("        public const int PI2Angle = 360;");
            builder.AppendLine("        /// <summary>");
            builder.AppendLine("        /// π/2");
            builder.AppendLine("        /// </summary>");
            builder.AppendLine($"        public readonly static FFloat HalfPI = FFloat.FromRaw({halfPiRaw.ToString(CultureInfo.InvariantCulture)});");
            builder.AppendLine("        /// <summary>");
            builder.AppendLine("        /// π/2对应的角度值");
            builder.AppendLine("        /// </summary>");
            builder.AppendLine("        public readonly static FFloat HalfPIAngle = 90;");
            builder.AppendLine("        /// <summary>");
            builder.AppendLine("        /// 自然对数基数 e");
            builder.AppendLine("        /// </summary>");
            builder.AppendLine($"        public readonly static FFloat E = FFloat.FromRaw({eRaw.ToString(CultureInfo.InvariantCulture)});");
            builder.AppendLine("        /// <summary>");
            builder.AppendLine("        /// 弧度转角度的常量：180/π");
            builder.AppendLine("        /// </summary>");
            builder.AppendLine($"        public readonly static FFloat Rad2Deg = FFloat.FromRaw({rad2DegRaw.ToString(CultureInfo.InvariantCulture)});");
            builder.AppendLine("        /// <summary>");
            builder.AppendLine("        /// 角度转弧度的常量：π/180");
            builder.AppendLine("        /// </summary>");
            builder.AppendLine($"        public readonly static FFloat Deg2Rad = FFloat.FromRaw({deg2RadRaw.ToString(CultureInfo.InvariantCulture)});");
            builder.AppendLine("        /// <summary>");
            builder.AppendLine("        /// 四元数球面插值退化为线性插值的点乘阈值");
            builder.AppendLine("        /// </summary>");
            builder.AppendLine($"        public readonly static FFloat QuaternionSlerpLinearThreshold = FFloat.FromRaw({quaternionSlerpLinearThresholdRaw.ToString(CultureInfo.InvariantCulture)});");
            builder.AppendLine();
            builder.AppendLine($"        private readonly static FFloat Ln2 = FFloat.FromRaw({ln2Raw.ToString(CultureInfo.InvariantCulture)});");
            builder.AppendLine($"        private readonly static FFloat CordicK = FFloat.FromRaw({cordicKRaw.ToString(CultureInfo.InvariantCulture)});");
            builder.AppendLine("        private readonly static FFloat[] CordicAtanTable =");
            builder.AppendLine("        [");

            for (int i = 0; i < cordicAtanTable.Length; i++)
            {
                string suffix = i == cordicAtanTable.Length - 1 ? string.Empty : ",";
                builder.AppendLine($"            FFloat.FromRaw({cordicAtanTable[i].ToString(CultureInfo.InvariantCulture)}){suffix}");
            }

            builder.AppendLine("        ];");
            builder.AppendLine("    }");
            builder.AppendLine("}");

            return builder.ToString();
        }

        private static long ToRaw(double value, long multiplier)
        {
            return checked((long)Math.Round(value * multiplier, MidpointRounding.AwayFromZero));
        }

        private static long ScaleRaw(long rawValue, long multiplier)
        {
            long scaled = checked((long)Math.Round(rawValue * (double)multiplier / DefaultMultiplier, MidpointRounding.AwayFromZero));

            return scaled == 0 && rawValue != 0 ? 1 : scaled;
        }

        private static double GetCordicK()
        {
            double result = 1.0;

            for (int i = 0; i < CordicIterationCount; i++)
                result *= 1.0 / Math.Sqrt(1.0 + Math.Pow(2.0, -2 * i));

            return result;
        }

        private static long[] GetCordicAtanTable(long multiplier)
        {
            long[] table = new long[CordicIterationCount];

            for (int i = 0; i < table.Length; i++)
                table[i] = ToRaw(Math.Atan(Math.Pow(2.0, -i)), multiplier);

            return table;
        }

        /// <summary>
        /// 查找默认输出路径
        /// </summary>
        /// <returns></returns>
        private static string FindDefaultOutputPath()
        {
            foreach (string root in EnumerateSearchRoots())
            {
                string mathDirectory = Path.Combine(root, "FMath", "Math");
                if (Directory.Exists(mathDirectory))
                    return Path.Combine(mathDirectory, "FMath.Consts.cs");
            }

            return Path.Combine(Environment.CurrentDirectory, "FMath", "Math", "FMath.Consts.cs");
        }

        /// <summary>
        /// 枚举所有需要搜索的文件夹
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<string> EnumerateSearchRoots()
        {
            foreach (string start in new[] { Environment.CurrentDirectory, AppContext.BaseDirectory })
            {
                DirectoryInfo? directory = new DirectoryInfo(start);

                while (directory != null)
                {
                    yield return directory.FullName;
                    directory = directory.Parent;
                }
            }
        }

        private static bool IsHelp(string arg)
        {
            return arg == "-h" || arg == "--help" || arg == "/?";
        }

        /// <summary>
        /// 打印使用指南
        /// </summary>
        private static void PrintUsage()
        {
            Console.WriteLine("本工具用于生成FMath库中使用的常量文件内容");
            Console.WriteLine("使用方式:");
            Console.WriteLine("  GenerateFMathConsts <multiplier> [outputPath]");
            Console.WriteLine("  GenerateFMathConsts -m <multiplier> -o <outputPath>");
            Console.WriteLine("  GenerateFMathConsts --multiplier <multiplier> --output <outputPath>");
            Console.WriteLine("  缩放倍率为实际缩放值，如缩放65536倍（也就是2^16），则需要在缩放倍率的参数写入 65536");
            Console.WriteLine();
            Console.WriteLine("示例:");
            Console.WriteLine("  dotnet run --project GenerateFMathConsts -- 65536");
        }
    }
}
