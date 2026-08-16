using FixedMath;
using System.Globalization;

namespace Example
{
    public class Program
    {
        private const double FFloatTolerance = 0.00002;
        private const double MathTolerance = 0.001;

        private static int totalCount;
        private static int passedCount;
        private static int failedCount;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            RunFFloatTests();
            RunFMathTests();
            RunFVector2Tests();
            RunFVector3Tests();
            RunFVector4Tests();
            RunFMatrixTests();
            RunFQuaternionTests();

            Console.WriteLine();
            Console.WriteLine("========== 测试汇总 ==========");
            Console.WriteLine($"总计：{totalCount}，通过：{passedCount}，失败：{failedCount}");

            if (failedCount > 0)
                Environment.ExitCode = 1;
        }

        private static void RunFFloatTests()
        {
            //构造定点数
            Section("构造定点数");
            FFloat fromInt = new FFloat(2);
            FFloat fromFloat = new FFloat(2.5f);
            FFloat fromDouble = new FFloat(2.75);
            FFloat explicitFloat = (FFloat)0.3f;
            FFloat explicitDouble = (FFloat)0.7;
            FFloat implicitInt = 6;
            FFloat fromRaw = FFloat.FromRaw(FFloat.MULTIPLER_FACTOR + (FFloat.MULTIPLER_FACTOR / 2));
            FFloat fromRawOverload = FFloat.FromRaw(3, false);

            CheckFFloat("new FFloat(int)", fromInt, 2);
            CheckFFloat("new FFloat(float)", fromFloat, 2.5);
            CheckFFloat("new FFloat(double)", fromDouble, 2.75);
            CheckFFloat("(FFloat)float", explicitFloat, 0.3, FFloatTolerance * 2);
            CheckFFloat("(FFloat)double", explicitDouble, 0.7, FFloatTolerance * 2);
            CheckFFloat("implicit int", implicitInt, 6);
            CheckFFloat("FromRaw(raw)", fromRaw, 1.5);
            CheckFFloat("FromRaw(value, false)", fromRawOverload, 3);

            Console.WriteLine("---------- 定点数常量 ----------");
            CheckFFloat("FFloat.Zero", FFloat.Zero, 0);
            CheckFFloat("FFloat.One", FFloat.One, 1);
            CheckLong("FFloat.BitMoveCount", FFloat.BitMoveCount, 16);
            CheckLong("FFloat.MULTIPLER_FACTOR", FFloat.MULTIPLER_FACTOR, 65536);

            Section("定点数属性和转换");
            FFloat value = new FFloat(-2.75);
            CheckLong("RawValue", value.RawValue, -180224);
            CheckDouble("Float", value.Float, -2.75, FFloatTolerance);
            CheckDouble("Double", value.Double, -2.75, FFloatTolerance);
            CheckInt("Int", value.Int, -2);
            CheckInt("FloorToInt", value.FloorToInt, -3);
            CheckInt("RoundToInt 2.5", new FFloat(2.5).RoundToInt, 2);
            CheckInt("RoundToInt 3.5", new FFloat(3.5).RoundToInt, 4);
            CheckInt("RoundToInt -2.5", new FFloat(-2.5).RoundToInt, -2);
            CheckDouble("explicit float", (float)new FFloat(1.25), 1.25, FFloatTolerance);
            CheckDouble("explicit double", (double)new FFloat(1.25), 1.25, FFloatTolerance);
            CheckInt("explicit int", (int)new FFloat(1.99), 1);
            CheckString("ToString()", new FFloat(1.5).ToString(), "1.5");
            CheckString("ToString(decimalPlaces)", new FFloat(1.23456).ToString(2), "1.23");
            CheckString("DumpInt()", new FFloat(3.8).DumpInt(), "3");
            CheckString("DumpFloat()", new FFloat(3.5).DumpFloat(), "3.5");
            CheckString("DumpDouble()", new FFloat(3.5).DumpDouble(), "3.5");

            //定点数运算
            Section("定点数运算");
            FFloat left = new FFloat(2.5);
            FFloat right = new FFloat(4);
            CheckFFloat("unary -", -left, -2.5);
            CheckFFloat("+", left + right, 6.5);
            CheckFFloat("-", left - right, -1.5);
            CheckFFloat("*", left * right, 10);
            CheckFFloat("/", left / right, 0.625);
            CheckFFloat("%", new FFloat(6) % right, 2);
            CheckFFloat(">>", new FFloat(8) >> 1, 4);
            CheckFFloat("<<", new FFloat(2) << 2, 8);
            CheckFFloat("large multiply", new FFloat(1000000) * new FFloat(1000000), 1000000000000, 0.5);
            CheckFFloat("large divide", (new FFloat(1000000) * new FFloat(1000000)) / new FFloat(1000000), 1000000, FFloatTolerance);
            CheckThrows<DivideByZeroException>("division by zero", () => _ = left / FFloat.Zero);
            CheckThrows<DivideByZeroException>("mod by zero", () => _ = left % FFloat.Zero);

            //定点数比较
            Section("定点数比较");
            CheckBool("<", left < right, true);
            CheckBool(">", left > right, false);
            CheckBool("<=", left <= right, true);
            CheckBool(">=", left >= right, false);
            CheckBool("==", left == new FFloat(2.5), true);
            CheckBool("!=", left != right, true);
            CheckBool("Equals same", left.Equals(new FFloat(2.5)), true);
            CheckBool("Equals null", left.Equals(null), false);
            CheckBool("GetHashCode same", left.GetHashCode() == new FFloat(2.5).GetHashCode(), true);
        }

        private static void RunFMathTests()
        {
            //定点数数学库
            Section("定点数数学库");
            CheckFFloat("PI", FMath.PI, Math.PI, FFloatTolerance);
            CheckInt("PIAngle", FMath.PIAngle, 180);
            CheckFFloat("PI2", FMath.PI2, Math.PI * 2, FFloatTolerance * 2);
            CheckInt("PI2Angle", FMath.PI2Angle, 360);
            CheckFFloat("HalfPI", FMath.HalfPI, Math.PI / 2, FFloatTolerance);
            CheckFFloat("HalfPIAngle", FMath.HalfPIAngle, 90);
            CheckFFloat("E", FMath.E, Math.E, FFloatTolerance);
            CheckFFloat("Rad2Deg", FMath.Rad2Deg, 180 / Math.PI, 0.001);
            CheckFFloat("Deg2Rad", FMath.Deg2Rad, Math.PI / 180, FFloatTolerance);

            CheckFFloat("Sqrt", FMath.Sqrt(16), 4);
            CheckFFloat("Sqrt small value", FMath.Sqrt(new FFloat(0.0001)), 0.01, MathTolerance);
            CheckThrows<ArgumentException>("Sqrt negative", () => _ = FMath.Sqrt(-1));
            CheckFFloat("Pow positive", FMath.Pow(3, 4), 81);
            CheckFFloat("Pow negative base", FMath.Pow(-4, 3), -64);
            CheckFFloat("Pow zero", FMath.Pow(9, 0), 1);
            CheckFFloat("Pow negative exponent", FMath.Pow(2, -3), 0.125);
            CheckThrows<DivideByZeroException>("Pow zero negative exponent", () => _ = FMath.Pow(0, -1));

            FFloat logValue = 16;
            CheckFFloat("LogE", FMath.LogE(logValue), Math.Log(logValue.Double), 0.001);
            CheckFFloat("LogE expand", FMath.LogE(logValue, 24), Math.Log(logValue.Double), 0.001);
            CheckFFloat("Log2", FMath.Log2(logValue), Math.Log(logValue.Double, 2), 0.001);
            CheckFFloat("Log10", FMath.Log10(logValue), Math.Log10(logValue.Double), 0.001);
            CheckFFloat("Log base 4", FMath.Log(logValue, 4), Math.Log(logValue.Double, 4), 0.001);
            CheckThrows<ArgumentException>("Log negative", () => _ = FMath.LogE(-1));
            CheckThrows<ArgumentException>("Log base 1", () => _ = FMath.Log(2, 1));

            CheckFFloat("Ceiling positive", FMath.Ceiling(new FFloat(1.2)), 2);
            CheckFFloat("Ceiling negative", FMath.Ceiling(new FFloat(-1.2)), -1);
            CheckFFloat("Floor positive", FMath.Floor(new FFloat(1.8)), 1);
            CheckFFloat("Floor negative", FMath.Floor(new FFloat(-1.2)), -2);
            CheckFFloat("Max two", FMath.Max(2, 5), 5);
            CheckFFloat("Max three", FMath.Max(2, 5, 3), 5);
            CheckFFloat("Max params", FMath.Max(1, 7, 3), 7);
            CheckFFloat("Max empty", FMath.Max(Array.Empty<FFloat>()), 0);
            CheckFFloat("Min two", FMath.Min(2, 5), 2);
            CheckFFloat("Min three", FMath.Min(2, 5, -3), -3);
            CheckFFloat("Min params", FMath.Min(1, -7, 3), -7);
            CheckFFloat("Min empty", FMath.Min(Array.Empty<FFloat>()), 0);
            CheckFFloat("Truncate positive", FMath.Truncate(new FFloat(1.8)), 1);
            CheckFFloat("Truncate negative", FMath.Truncate(new FFloat(-1.8)), -1);
            CheckFFloat("Abs FFloat", FMath.Abs(new FFloat(-1.8)), 1.8);
            CheckInt("Abs int", FMath.Abs(-11), 11);
            CheckLong("AbsToLong int.MinValue", FMath.AbsToLong(int.MinValue), 2147483648L);
            CheckFFloat("AbsToFFloat", FMath.AbsToFFloat(-11), 11);
            CheckFFloat("AbsToFFloat int.MinValue", FMath.AbsToFFloat(int.MinValue), 2147483648.0, 0.5);

            FFloat rad1 = (FFloat)1.57075;
            FFloat rad2 = (FFloat)32.415926;
            CheckFFloat("Sin", FMath.Sin(rad1), Math.Sin(rad1.Double), MathTolerance);
            CheckFFloat("Sin large radian", FMath.Sin(rad2), Math.Sin(rad2.Double), MathTolerance);
            CheckFFloat("SinAngle", FMath.SinAngle(60), Math.Sin(60 * Math.PI / 180), MathTolerance);
            CheckFFloat("Cos", FMath.Cos(rad1), Math.Cos(rad1.Double), MathTolerance);
            CheckFFloat("Cos large radian", FMath.Cos(rad2), Math.Cos(rad2.Double), MathTolerance);
            CheckFFloat("CosAngle", FMath.CosAngle(750), Math.Cos(750 * Math.PI / 180), MathTolerance);
            CheckFFloat("Tan", FMath.Tan(rad2), Math.Tan(rad2.Double), MathTolerance);
            CheckFFloat("TanAngle", FMath.TanAngle(390), Math.Tan(390 * Math.PI / 180), MathTolerance);
            CheckThrows<DivideByZeroException>("TanAngle 90", () => _ = FMath.TanAngle(90));

            FFloat asinValue = new FFloat(0.8);
            CheckFFloat("Asin", FMath.Asin(asinValue), Math.Asin(asinValue.Double), MathTolerance);
            CheckFFloat("Asin clamp high", FMath.Asin(2), Math.PI / 2, MathTolerance);
            CheckFFloat("Asin clamp low", FMath.Asin(-2), -Math.PI / 2, MathTolerance);
            CheckFFloat("Acos", FMath.Acos(asinValue), Math.Acos(asinValue.Double), MathTolerance);
            CheckFFloat("Acos clamp high", FMath.Acos(2), 0, MathTolerance);
            CheckFFloat("Acos clamp low", FMath.Acos(-2), Math.PI, MathTolerance);
            CheckFFloat("Atan", FMath.Atan(asinValue), Math.Atan(asinValue.Double), MathTolerance);
            CheckFFloat("Atan expand overload", FMath.Atan(asinValue, 24), Math.Atan(asinValue.Double), MathTolerance);
            FFloat atanExpandValue = new FFloat(0.5);
            CheckBool("Atan expand count affects result", FMath.Atan(atanExpandValue, 2) != FMath.Atan(atanExpandValue, 12), true);
            CheckThrows<ArgumentException>("Atan invalid expand count", () => _ = FMath.Atan(asinValue, 0));
            CheckFFloat("Atan2", FMath.Atan2(1, -1), Math.Atan2(1, -1), MathTolerance);

            FMath.SinCos(60 * FMath.Deg2Rad, out FFloat sin, out FFloat cos);
            CheckFFloat("SinCos sin", sin, Math.Sin(60 * FMath.Deg2Rad.Double), MathTolerance);
            CheckFFloat("SinCos cos", cos, Math.Cos(60 * FMath.Deg2Rad.Double), MathTolerance);
            CheckFFloat("Clamp low", FMath.Clamp(-2, -1, 1), -1);
            CheckFFloat("Clamp mid", FMath.Clamp(new FFloat(0.5), -1, 1), 0.5);
            CheckFFloat("Clamp high", FMath.Clamp(2, -1, 1), 1);
            CheckFFloat("Normalize360 positive", FMath.Normalize360(750), 30);
            CheckFFloat("Normalize360 negative", FMath.Normalize360(-420), 300);
            CheckFFloat("NormalizeAngle90 positive", FMath.NormalizeAngle90(120), -60);
            CheckFFloat("NormalizeAngle90 negative", FMath.NormalizeAngle90(-120), 60);
        }

        private static void RunFVector2Tests()
        {
            //定点数向量
            Section("FVector2");
            CheckVector2("Zero", FVector2.Zero, 0, 0);
            CheckVector2("One", FVector2.One, 1, 1);
            CheckVector2("Left", FVector2.Left, -1, 0);
            CheckVector2("Right", FVector2.Right, 1, 0);
            CheckVector2("Up", FVector2.Up, 0, 1);
            CheckVector2("Down", FVector2.Down, 0, -1);

            FVector2 v = new FVector2(3, 4);
            CheckLongArray("ConvertLongArray", v.ConvertLongArray(), 3 * FFloat.MULTIPLER_FACTOR, 4 * FFloat.MULTIPLER_FACTOR);
            CheckFFloat("sqrMagnitude", v.sqrMagnitude, 25);
            CheckFFloat("SqrMagnitude", FVector2.SqrMagnitude(v), 25);
            CheckFFloat("Magnitude", v.magnitude, 5);
            CheckFFloat("GetMagnitude", FVector2.Magnitude(v), 5);
            CheckVector2("normalized", v.Normalized, 0.6, 0.8, MathTolerance);
            FVector2 mutable = v;
            mutable.Normalize();
            CheckVector2("Normalize()", mutable, 0.6, 0.8, MathTolerance);
            CheckVector2("Normalize static", FVector2.Normalize(v), 0.6, 0.8, MathTolerance);
            CheckVector2("Normalize zero", FVector2.Normalize(FVector2.Zero), 0, 0);
            CheckFFloat("Dot", FVector2.Dot(v, new FVector2(2, -1)), 2);
            CheckFFloat("CrossValue", FVector2.CrossValue(FVector2.Right, FVector2.Up), 1);
            CheckFFloat("Angle", FVector2.Angle(FVector2.Right, FVector2.Up), Math.PI / 2, MathTolerance);
            CheckFFloat("Angle zero", FVector2.Angle(FVector2.Zero, FVector2.Up), 0);
            CheckFFloat("SignedAngle", FVector2.SignedAngle(FVector2.Right, FVector2.Up), Math.PI / 2, MathTolerance);
            CheckFFloat("SignedAngle negative", FVector2.SignedAngle(FVector2.Up, FVector2.Right), -Math.PI / 2, MathTolerance);
            CheckFFloat("Distance", FVector2.Distance(FVector2.Zero, v), 5);
            CheckFFloat("SqrDistance", FVector2.SqrDistance(FVector2.Zero, v), 25);
            CheckVector2("Add static", FVector2.Add(new FVector2(1, 2), new FVector2(3, 4)), 4, 6);
            CheckVector2("Subtract static", FVector2.Subtract(new FVector2(1, 2), new FVector2(3, 4)), -2, -2);
            CheckVector2("Multiply static", FVector2.Multiply(new FVector2(1, 2), 3), 3, 6);
            CheckVector2("Divide static", FVector2.Divide(new FVector2(3, 6), 3), 1, 2);
            CheckVector2("Lerp", FVector2.Lerp(FVector2.Zero, new FVector2(10, 20), new FFloat(0.25)), 2.5, 5);
            CheckVector2("Lerp clamp", FVector2.Lerp(FVector2.Zero, new FVector2(10, 20), 2), 10, 20);
            CheckVector2("LerpUnclamped", FVector2.LerpUnclamped(FVector2.Zero, new FVector2(10, 20), 2), 20, 40);
            CheckVector2("MoveTowards", FVector2.MoveTowards(FVector2.Zero, v, 2), 1.2, 1.6, MathTolerance);
            CheckVector2("MoveTowards target", FVector2.MoveTowards(FVector2.Zero, v, 5), 3, 4);
            CheckVector2("Scale", FVector2.Scale(new FVector2(2, 3), new FVector2(4, 5)), 8, 15);
            CheckVector2("Max", FVector2.Max(new FVector2(2, 7), new FVector2(4, 5)), 4, 7);
            CheckVector2("Min", FVector2.Min(new FVector2(2, 7), new FVector2(4, 5)), 2, 5);
            CheckVector2("ClampMagnitude", FVector2.ClampMagnitude(v, 2), 1.2, 1.6, MathTolerance);
            CheckVector2("Project", FVector2.Project(v, FVector2.Right), 3, 0);
            CheckVector2("Project zero", FVector2.Project(v, FVector2.Zero), 0, 0);
            CheckVector2("Reflect", FVector2.Reflect(new FVector2(1, -1), FVector2.Up), 1, 1);
            CheckVector2("Perpendicular", FVector2.Perpendicular(new FVector2(2, 3)), -3, 2);
            CheckVector2("+", new FVector2(1, 2) + new FVector2(3, 4), 4, 6);
            CheckVector2("-", new FVector2(1, 2) - new FVector2(3, 4), -2, -2);
            CheckVector2("vector * scalar", new FVector2(1, 2) * 3, 3, 6);
            CheckVector2("scalar * vector", 3 * new FVector2(1, 2), 3, 6);
            CheckVector2("/", new FVector2(3, 6) / 3, 1, 2);
            CheckVector2("unary -", -new FVector2(1, -2), -1, 2);
            CheckBool("==", new FVector2(1, 2) == new FVector2(1, 2), true);
            CheckBool("!=", new FVector2(1, 2) != new FVector2(1, 3), true);
            CheckBool("Equals same", new FVector2(1, 2).Equals(new FVector2(1, 2)), true);
            CheckBool("Equals null", new FVector2(1, 2).Equals(null), false);
            CheckBool("GetHashCode same", new FVector2(1, 2).GetHashCode() == new FVector2(1, 2).GetHashCode(), true);
            CheckString("ToString", new FVector2(1, 2).ToString(), "(1,2)");
        }

        private static void RunFVector3Tests()
        {
            Section("FVector3");
            CheckVector3("Zero", FVector3.Zero, 0, 0, 0);
            CheckVector3("One", FVector3.One, 1, 1, 1);
            CheckVector3("Forward", FVector3.Forward, 0, 0, 1);
            CheckVector3("Back", FVector3.Back, 0, 0, -1);
            CheckVector3("Left", FVector3.Left, -1, 0, 0);
            CheckVector3("Right", FVector3.Right, 1, 0, 0);
            CheckVector3("Up", FVector3.Up, 0, 1, 0);
            CheckVector3("Down", FVector3.Down, 0, -1, 0);

            FVector3 v = new FVector3(2, 3, 6);
            CheckLongArray("ConvertLongArray", v.ConvertLongArray(), 2 * FFloat.MULTIPLER_FACTOR, 3 * FFloat.MULTIPLER_FACTOR, 6 * FFloat.MULTIPLER_FACTOR);
            CheckFFloat("sqrMagnitude", v.sqrMagnitude, 49);
            CheckFFloat("SqrMagnitude", FVector3.SqrMagnitude(v), 49);
            CheckFFloat("Magnitude", v.magnitude, 7);
            CheckFFloat("GetMagnitude", FVector3.Magnitude(v), 7);
            CheckVector3("normalized", v.Normalized, 2.0 / 7, 3.0 / 7, 6.0 / 7, MathTolerance);
            FVector3 mutable = v;
            mutable.Normalize();
            CheckVector3("Normalize()", mutable, 2.0 / 7, 3.0 / 7, 6.0 / 7, MathTolerance);
            CheckVector3("Normalize static", FVector3.Normalize(v), 2.0 / 7, 3.0 / 7, 6.0 / 7, MathTolerance);
            CheckVector3("Normalize zero", FVector3.Normalize(FVector3.Zero), 0, 0, 0);
            CheckFFloat("Dot", FVector3.Dot(v, new FVector3(1, -2, 3)), 14);
            CheckVector3("Cross", FVector3.Cross(FVector3.Right, FVector3.Up), 0, 0, 1);
            CheckFFloat("Angle", FVector3.Angle(FVector3.Right, FVector3.Up), Math.PI / 2, MathTolerance);
            CheckFFloat("Angle zero", FVector3.Angle(FVector3.Zero, FVector3.Up), 0);
            CheckFFloat("SignedAngle", FVector3.SignedAngle(FVector3.Right, FVector3.Up, FVector3.Forward), Math.PI / 2, MathTolerance);
            CheckFFloat("SignedAngle negative", FVector3.SignedAngle(FVector3.Up, FVector3.Right, FVector3.Forward), -Math.PI / 2, MathTolerance);
            CheckFFloat("Distance", FVector3.Distance(FVector3.Zero, v), 7);
            CheckFFloat("SqrDistance", FVector3.SqrDistance(FVector3.Zero, v), 49);
            CheckVector3("Add static", FVector3.Add(new FVector3(1, 2, 3), new FVector3(3, 4, 5)), 4, 6, 8);
            CheckVector3("Subtract static", FVector3.Subtract(new FVector3(1, 2, 3), new FVector3(3, 4, 5)), -2, -2, -2);
            CheckVector3("Multiply static", FVector3.Multiply(new FVector3(1, 2, 3), 3), 3, 6, 9);
            CheckVector3("Divide static", FVector3.Divide(new FVector3(3, 6, 9), 3), 1, 2, 3);
            CheckVector3("Lerp", FVector3.Lerp(FVector3.Zero, new FVector3(10, 20, 30), new FFloat(0.25)), 2.5, 5, 7.5);
            CheckVector3("Lerp clamp", FVector3.Lerp(FVector3.Zero, new FVector3(10, 20, 30), 2), 10, 20, 30);
            CheckVector3("LerpUnclamped", FVector3.LerpUnclamped(FVector3.Zero, new FVector3(10, 20, 30), 2), 20, 40, 60);
            CheckVector3("MoveTowards", FVector3.MoveTowards(FVector3.Zero, v, 2), 4.0 / 7, 6.0 / 7, 12.0 / 7, MathTolerance);
            CheckVector3("MoveTowards target", FVector3.MoveTowards(FVector3.Zero, v, 7), 2, 3, 6);
            CheckVector3("Scale", FVector3.Scale(new FVector3(2, 3, 4), new FVector3(4, 5, 6)), 8, 15, 24);
            CheckVector3("Max", FVector3.Max(new FVector3(2, 7, 1), new FVector3(4, 5, 3)), 4, 7, 3);
            CheckVector3("Min", FVector3.Min(new FVector3(2, 7, 1), new FVector3(4, 5, 3)), 2, 5, 1);
            CheckVector3("ClampMagnitude", FVector3.ClampMagnitude(v, 2), 4.0 / 7, 6.0 / 7, 12.0 / 7, MathTolerance);
            CheckVector3("Project", FVector3.Project(v, FVector3.Up), 0, 3, 0);
            CheckVector3("Project zero", FVector3.Project(v, FVector3.Zero), 0, 0, 0);
            CheckVector3("ProjectOnPlane", FVector3.ProjectOnPlane(v, FVector3.Up), 2, 0, 6);
            CheckVector3("Reflect", FVector3.Reflect(new FVector3(1, -1, 0), FVector3.Up), 1, 1, 0);
            CheckVector3("+", new FVector3(1, 2, 3) + new FVector3(3, 4, 5), 4, 6, 8);
            CheckVector3("-", new FVector3(1, 2, 3) - new FVector3(3, 4, 5), -2, -2, -2);
            CheckVector3("vector * scalar", new FVector3(1, 2, 3) * 3, 3, 6, 9);
            CheckVector3("scalar * vector", 3 * new FVector3(1, 2, 3), 3, 6, 9);
            CheckVector3("/", new FVector3(3, 6, 9) / 3, 1, 2, 3);
            CheckVector3("unary -", -new FVector3(1, -2, 3), -1, 2, -3);
            CheckBool("==", new FVector3(1, 2, 3) == new FVector3(1, 2, 3), true);
            CheckBool("!=", new FVector3(1, 2, 3) != new FVector3(1, 2, 4), true);
            CheckBool("Equals same", new FVector3(1, 2, 3).Equals(new FVector3(1, 2, 3)), true);
            CheckBool("Equals null", new FVector3(1, 2, 3).Equals(null), false);
            CheckBool("GetHashCode same", new FVector3(1, 2, 3).GetHashCode() == new FVector3(1, 2, 3).GetHashCode(), true);
            CheckString("ToString", new FVector3(1, 2, 3).ToString(), "(1,2,3)");
        }

        private static void RunFVector4Tests()
        {
            Section("FVector4");
            CheckVector4("Zero", FVector4.Zero, 0, 0, 0, 0);
            CheckVector4("One", FVector4.One, 1, 1, 1, 1);

            FVector4 v = new FVector4(1, 2, 3, 4);
            CheckLongArray("ConvertLongArray", v.ConvertLongArray(), 1 * FFloat.MULTIPLER_FACTOR, 2 * FFloat.MULTIPLER_FACTOR, 3 * FFloat.MULTIPLER_FACTOR, 4 * FFloat.MULTIPLER_FACTOR);
            CheckFFloat("sqrMagnitude", v.sqrMagnitude, 30);
            CheckFFloat("SqrMagnitude", FVector4.SqrMagnitude(v), 30);
            CheckFFloat("Magnitude", v.magnitude, Math.Sqrt(30), MathTolerance);
            CheckFFloat("GetMagnitude", FVector4.Magnitude(v), Math.Sqrt(30), MathTolerance);
            CheckVector4("normalized", v.normalized, 1 / Math.Sqrt(30), 2 / Math.Sqrt(30), 3 / Math.Sqrt(30), 4 / Math.Sqrt(30), MathTolerance);
            FVector4 mutable = v;
            mutable.Normalize();
            CheckVector4("Normalize()", mutable, 1 / Math.Sqrt(30), 2 / Math.Sqrt(30), 3 / Math.Sqrt(30), 4 / Math.Sqrt(30), MathTolerance);
            CheckVector4("Normalize static", FVector4.Normalize(v), 1 / Math.Sqrt(30), 2 / Math.Sqrt(30), 3 / Math.Sqrt(30), 4 / Math.Sqrt(30), MathTolerance);
            CheckVector4("Normalize zero", FVector4.Normalize(FVector4.Zero), 0, 0, 0, 0);
            CheckFFloat("Dot", FVector4.Dot(v, new FVector4(2, 3, 4, 5)), 40);
            CheckVector4("Cross", FVector4.Cross(new FVector4(1, 0, 0, 0), new FVector4(0, 1, 0, 0), new FVector4(0, 0, 1, 0)), 0, 0, 0, -1);
            CheckFFloat("Angle", FVector4.Angle(new FVector4(1, 0, 0, 0), new FVector4(0, 1, 0, 0)), Math.PI / 2, MathTolerance);
            CheckFFloat("Angle zero", FVector4.Angle(FVector4.Zero, FVector4.One), 0);
            CheckFFloat("Distance", FVector4.Distance(FVector4.Zero, v), Math.Sqrt(30), MathTolerance);
            CheckFFloat("SqrDistance", FVector4.SqrDistance(FVector4.Zero, v), 30);
            CheckVector4("Add static", FVector4.Add(new FVector4(1, 2, 3, 4), new FVector4(3, 4, 5, 6)), 4, 6, 8, 10);
            CheckVector4("Subtract static", FVector4.Subtract(new FVector4(1, 2, 3, 4), new FVector4(3, 4, 5, 6)), -2, -2, -2, -2);
            CheckVector4("Multiply static", FVector4.Multiply(new FVector4(1, 2, 3, 4), 3), 3, 6, 9, 12);
            CheckVector4("Divide static", FVector4.Divide(new FVector4(3, 6, 9, 12), 3), 1, 2, 3, 4);
            CheckVector4("Lerp", FVector4.Lerp(FVector4.Zero, new FVector4(10, 20, 30, 40), new FFloat(0.25)), 2.5, 5, 7.5, 10);
            CheckVector4("Lerp clamp", FVector4.Lerp(FVector4.Zero, new FVector4(10, 20, 30, 40), 2), 10, 20, 30, 40);
            CheckVector4("LerpUnclamped", FVector4.LerpUnclamped(FVector4.Zero, new FVector4(10, 20, 30, 40), 2), 20, 40, 60, 80);
            CheckVector4("MoveTowards", FVector4.MoveTowards(FVector4.Zero, new FVector4(0, 0, 3, 4), 2), 0, 0, 1.2, 1.6, MathTolerance);
            CheckVector4("MoveTowards target", FVector4.MoveTowards(FVector4.Zero, new FVector4(0, 0, 3, 4), 5), 0, 0, 3, 4);
            CheckVector4("Scale", FVector4.Scale(new FVector4(2, 3, 4, 5), new FVector4(4, 5, 6, 7)), 8, 15, 24, 35);
            CheckVector4("Max", FVector4.Max(new FVector4(2, 7, 1, 9), new FVector4(4, 5, 3, 6)), 4, 7, 3, 9);
            CheckVector4("Min", FVector4.Min(new FVector4(2, 7, 1, 9), new FVector4(4, 5, 3, 6)), 2, 5, 1, 6);
            CheckVector4("ClampMagnitude", FVector4.ClampMagnitude(new FVector4(0, 0, 3, 4), 2), 0, 0, 1.2, 1.6, MathTolerance);
            CheckVector4("Project", FVector4.Project(v, new FVector4(0, 0, 1, 0)), 0, 0, 3, 0);
            CheckVector4("Project zero", FVector4.Project(v, FVector4.Zero), 0, 0, 0, 0);
            CheckVector4("Reflect", FVector4.Reflect(new FVector4(1, -1, 0, 0), new FVector4(0, 1, 0, 0)), 1, 1, 0, 0);
            CheckVector4("+", new FVector4(1, 2, 3, 4) + new FVector4(3, 4, 5, 6), 4, 6, 8, 10);
            CheckVector4("-", new FVector4(1, 2, 3, 4) - new FVector4(3, 4, 5, 6), -2, -2, -2, -2);
            CheckVector4("vector * scalar", new FVector4(1, 2, 3, 4) * 3, 3, 6, 9, 12);
            CheckVector4("scalar * vector", 3 * new FVector4(1, 2, 3, 4), 3, 6, 9, 12);
            CheckVector4("/", new FVector4(3, 6, 9, 12) / 3, 1, 2, 3, 4);
            CheckVector4("unary -", -new FVector4(1, -2, 3, -4), -1, 2, -3, 4);
            CheckBool("==", new FVector4(1, 2, 3, 4) == new FVector4(1, 2, 3, 4), true);
            CheckBool("!=", new FVector4(1, 2, 3, 4) != new FVector4(1, 2, 3, 5), true);
            CheckBool("Equals same", new FVector4(1, 2, 3, 4).Equals(new FVector4(1, 2, 3, 4)), true);
            CheckBool("Equals null", new FVector4(1, 2, 3, 4).Equals(null), false);
            CheckBool("GetHashCode same", new FVector4(1, 2, 3, 4).GetHashCode() == new FVector4(1, 2, 3, 4).GetHashCode(), true);
            CheckString("ToString", new FVector4(1, 2, 3, 4).ToString(), "(1,2,3,4)");
        }

        private static void RunFMatrixTests()
        {
            Section("FMatrix3x3");
            FMatrix3x3 scale3 = FMatrix3x3.Scale(new FVector3(2, 3, 4));
            FMatrix3x3 normalScale3 = FMatrix3x3.Scale(new FVector3(2, 1, 1));
            CheckVector3("3x3 identity vector", FMatrix3x3.Identity * new FVector3(1, 2, 3), 1, 2, 3);
            CheckVector3("3x3 scale vector", scale3 * new FVector3(1, 2, 3), 2, 6, 12);
            CheckFFloat("3x3 determinant", scale3.Determinant, 24);
            CheckVector3("3x3 inverse", FMatrix3x3.Inverse(scale3) * new FVector3(2, 6, 12), 1, 2, 3, MathTolerance);
            CheckVector3("3x3 rotate z", FMatrix3x3.RotateZ(FMath.HalfPI) * FVector3.Right, 0, 1, 0, MathTolerance);
            CheckVector3("3x3 transform normal non uniform scale", normalScale3.TransformNormal(new FVector3(1, 1, 0)), 1 / Math.Sqrt(5), 2 / Math.Sqrt(5), 0, MathTolerance);
            CheckFFloat("3x3 transpose", scale3.Transposed.m11, 3);
            CheckFFloat("3x3 indexer", scale3[2, 2], 4);
            CheckVector3("3x3 get row", scale3.GetRow(1), 0, 3, 0);
            CheckVector3("3x3 get column", scale3.GetColumn(2), 0, 0, 4);
            CheckBool("3x3 approximately", FMatrix3x3.Approximately(scale3, scale3 + (FMatrix3x3.Identity * FMath.Epsilon), FMath.Epsilon), true);
            CheckBool("3x3 equality", FMatrix3x3.Identity == FMatrix3x3.Identity, true);
            CheckThrows<InvalidOperationException>("3x3 inverse singular", () => _ = FMatrix3x3.Inverse(FMatrix3x3.Zero));

            Section("FMatrix3x4");
            FMatrix3x4 translate3x4 = FMatrix3x4.Translate(new FVector3(1, 2, 3));
            FMatrix3x4 affineScale = FMatrix3x4.Scale(new FVector3(2, 3, 4));
            FMatrix3x4 affine = translate3x4 * affineScale;
            CheckVector3("3x4 transform point", affine.MultiplyPoint(new FVector3(1, 1, 1)), 3, 5, 7);
            CheckVector3("3x4 transform vector", affine.MultiplyVector(new FVector3(1, 1, 1)), 2, 3, 4);
            CheckVector3("3x4 operator point", affine * new FVector3(1, 1, 1), 3, 5, 7);
            CheckVector3("3x4 inverse", affine.Inversed.MultiplyPoint(new FVector3(3, 5, 7)), 1, 1, 1, MathTolerance);
            CheckVector3("3x4 trs", FMatrix3x4.TRS(new FVector3(1, 2, 3), FMatrix3x3.Identity, new FVector3(2, 3, 4)).MultiplyPoint(new FVector3(1, 1, 1)), 3, 5, 7);
            CheckVector3("3x4 rotate z", FMatrix3x4.RotateZ(FMath.HalfPI).MultiplyVector(FVector3.Right), 0, 1, 0, MathTolerance);
            CheckVector3("3x4 transform normal non uniform scale", FMatrix3x4.Scale(new FVector3(2, 1, 1)).TransformNormal(new FVector3(1, 1, 0)), 1 / Math.Sqrt(5), 2 / Math.Sqrt(5), 0, MathTolerance);
            CheckFFloat("3x4 indexer", affine[1, 3], 2);
            CheckVector4("3x4 get row", affine.GetRow(0), 2, 0, 0, 1);
            CheckVector3("3x4 get column", affine.GetColumn(3), 1, 2, 3);
            CheckBool("3x4 approximately", FMatrix3x4.Approximately(affine, affine + new FMatrix3x4(0, 0, 0, FMath.Epsilon, 0, 0, 0, 0, 0, 0, 0, 0), FMath.Epsilon), true);
            CheckBool("3x4 equality", FMatrix3x4.Identity == FMatrix3x4.Identity, true);

            Section("FMatrix4x4");
            FMatrix4x4 matrix4 = affine.ToMatrix4x4();
            FMatrix4x4 viewIdentity = FMatrix4x4.LookAt(FVector3.Zero, FVector3.Back, FVector3.Up);
            FMatrix4x4 viewTranslated = FMatrix4x4.LookAt(new FVector3(0, 0, 10), FVector3.Zero, FVector3.Up);
            FMatrix4x4 perspective = FMatrix4x4.Perspective(FMath.HalfPI, 1, 1, 101);
            FMatrix4x4 orthographic = FMatrix4x4.Orthographic(-1, 1, -2, 2, 1, 101);
            CheckVector3("4x4 multiply point", matrix4.MultiplyPoint(new FVector3(1, 1, 1)), 3, 5, 7);
            CheckVector3("4x4 multiply point 3x4", matrix4.MultiplyPoint3x4(new FVector3(1, 1, 1)), 3, 5, 7);
            CheckVector3("4x4 multiply direction", matrix4.MultiplyDirection(new FVector3(1, 1, 1)), 2, 3, 4);
            CheckVector4("4x4 multiply vector4", matrix4 * new FVector4(1, 1, 1, 1), 3, 5, 7, 1);
            CheckVector3("4x4 operator point", matrix4 * new FVector3(1, 1, 1), 3, 5, 7);
            CheckFFloat("4x4 determinant", matrix4.Determinant, 24);
            CheckVector3("4x4 inverse", FMatrix4x4.Inverse(matrix4).MultiplyPoint(new FVector3(3, 5, 7)), 1, 1, 1, MathTolerance);
            CheckVector3("4x4 inverse affine", matrix4.InverseAffine().MultiplyPoint(new FVector3(3, 5, 7)), 1, 1, 1, MathTolerance);
            CheckVector3("4x4 trs", FMatrix4x4.TRS(new FVector3(1, 2, 3), FMatrix3x3.Identity, new FVector3(2, 3, 4)).MultiplyPoint(new FVector3(1, 1, 1)), 3, 5, 7);
            CheckVector3("4x4 transform normal non uniform scale", FMatrix4x4.Scale(new FVector3(2, 1, 1)).TransformNormal(new FVector3(1, 1, 0)), 1 / Math.Sqrt(5), 2 / Math.Sqrt(5), 0, MathTolerance);
            CheckFFloat("4x4 transpose", matrix4.Transposed.m30, 1);
            CheckFFloat("4x4 indexer", matrix4[2, 3], 3);
            CheckVector4("4x4 get row", matrix4.GetRow(0), 2, 0, 0, 1);
            CheckVector4("4x4 get column", matrix4.GetColumn(3), 1, 2, 3, 1);
            CheckBool("4x4 lookat identity", FMatrix4x4.Approximately(viewIdentity, FMatrix4x4.Identity, new FFloat(0.001)), true);
            CheckVector3("4x4 lookat point", viewTranslated.MultiplyPoint(FVector3.Zero), 0, 0, -10, MathTolerance);
            CheckFFloat("4x4 perspective m00", perspective.m00, 1, MathTolerance);
            CheckFFloat("4x4 perspective m22", perspective.m22, -1.02, MathTolerance);
            CheckFFloat("4x4 perspective m23", perspective.m23, -2.02, MathTolerance);
            CheckFFloat("4x4 perspective m32", perspective.m32, -1);
            CheckFFloat("4x4 orthographic m00", orthographic.m00, 1);
            CheckFFloat("4x4 orthographic m11", orthographic.m11, 0.5);
            CheckFFloat("4x4 orthographic m22", orthographic.m22, -0.02, MathTolerance);
            CheckFFloat("4x4 orthographic m23", orthographic.m23, -1.02, MathTolerance);
            CheckBool("4x4 ortho alias", FMatrix4x4.Approximately(orthographic, FMatrix4x4.Ortho(-1, 1, -2, 2, 1, 101), FMath.Epsilon), true);
            CheckBool("4x4 approximately", FMatrix4x4.Approximately(matrix4, matrix4 + new FMatrix4x4(0, 0, 0, FMath.Epsilon, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0), FMath.Epsilon), true);
            CheckBool("4x4 equality", FMatrix4x4.Identity == FMatrix4x4.Identity, true);
            CheckThrows<InvalidOperationException>("4x4 inverse singular", () => _ = FMatrix4x4.Inverse(FMatrix4x4.Zero));
            CheckThrows<InvalidOperationException>("4x4 inverse affine non affine", () => _ = perspective.InverseAffine());
            CheckThrows<ArgumentException>("4x4 lookat same point", () => _ = FMatrix4x4.LookAt(FVector3.Zero, FVector3.Zero, FVector3.Up));
            CheckThrows<ArgumentException>("4x4 lookat invalid up", () => _ = FMatrix4x4.LookAt(FVector3.Zero, FVector3.Back, FVector3.Back));
            CheckThrows<ArgumentOutOfRangeException>("4x4 perspective invalid fov", () => _ = FMatrix4x4.Perspective(FMath.PI, 1, 1, 10));
            CheckThrows<ArgumentOutOfRangeException>("4x4 perspective invalid aspect", () => _ = FMatrix4x4.Perspective(FMath.HalfPI, 0, 1, 10));
            CheckThrows<ArgumentException>("4x4 orthographic invalid width", () => _ = FMatrix4x4.Orthographic(1, 1, -1, 1, 1, 10));

            Section("FMatrix");
            FMatrix genericA = new FMatrix(2, 2, 1, 2, 3, 4);
            FMatrix genericB = new FMatrix(2, 2, 5, 6, 7, 8);
            FMatrix genericAdd = genericA + genericB;
            FMatrix genericMul = genericA * genericB;
            FMatrix genericNear = new FMatrix(2, 2, FFloat.One + FMath.Epsilon, 2, 3, 4);
            CheckFFloat("generic add", genericAdd[1, 1], 12);
            CheckFFloat("generic multiply 00", genericMul[0, 0], 19);
            CheckFFloat("generic multiply 11", genericMul[1, 1], 50);
            CheckFFloat("generic transpose", genericA.Transposed()[1, 0], 2);
            CheckFFloat("generic identity", (FMatrix.Identity(2) * genericA)[1, 1], 4);
            CheckFFloat("generic get row", genericA.GetRow(1)[0], 3);
            CheckFFloat("generic get column", genericA.GetColumn(1)[0], 2);
            CheckBool("generic approximately", FMatrix.Approximately(genericA, genericNear, FMath.Epsilon), true);
            CheckThrows<ArgumentException>("generic size mismatch", () => _ = genericA * new FMatrix(3, 1));
        }

        private static void RunFQuaternionTests()
        {
            Section("FQuaternion");
            FQuaternion quaternion = new FQuaternion(1, 2, 3, 4);
            CheckString("Constructor callable", quaternion.GetType().Name, nameof(FQuaternion));
        }

        private static void Section(string title)
        {
            Console.WriteLine();
            Console.WriteLine($"---------- {title} ----------");
        }

        private static void CheckFFloat(string name, FFloat actual, double expected, double tolerance = FFloatTolerance)
        {
            CheckDouble(name, actual.Double, expected, tolerance, actual.ToString(), expected.ToString(CultureInfo.InvariantCulture));
        }

        private static void CheckDouble(string name, double actual, double expected, double tolerance, string? actualText = null, string? expectedText = null)
        {
            bool pass = Math.Abs(actual - expected) <= tolerance;
            string extra = $"误差={Math.Abs(actual - expected):0.########}, 容差={tolerance}";
            Check(name, pass, actualText ?? actual.ToString(CultureInfo.InvariantCulture), expectedText ?? expected.ToString(CultureInfo.InvariantCulture), extra);
        }

        private static void CheckInt(string name, int actual, int expected)
        {
            Check(name, actual == expected, actual.ToString(), expected.ToString());
        }

        private static void CheckLong(string name, long actual, long expected)
        {
            Check(name, actual == expected, actual.ToString(), expected.ToString());
        }

        private static void CheckBool(string name, bool actual, bool expected)
        {
            Check(name, actual == expected, actual.ToString(), expected.ToString());
        }

        private static void CheckString(string name, string actual, string expected)
        {
            Check(name, actual == expected, actual, expected);
        }

        private static void CheckVector2(string name, FVector2 actual, double expectedX, double expectedY, double tolerance = FFloatTolerance)
        {
            bool pass = Close(actual.x.Double, expectedX, tolerance) && Close(actual.y.Double, expectedY, tolerance);
            Check(name, pass, actual.ToString(), $"({expectedX},{expectedY})", $"容差={tolerance}");
        }

        private static void CheckVector3(string name, FVector3 actual, double expectedX, double expectedY, double expectedZ, double tolerance = FFloatTolerance)
        {
            bool pass = Close(actual.x.Double, expectedX, tolerance) && Close(actual.y.Double, expectedY, tolerance) && Close(actual.z.Double, expectedZ, tolerance);
            Check(name, pass, actual.ToString(), $"({expectedX},{expectedY},{expectedZ})", $"容差={tolerance}");
        }

        private static void CheckVector4(string name, FVector4 actual, double expectedX, double expectedY, double expectedZ, double expectedW, double tolerance = FFloatTolerance)
        {
            bool pass =
                Close(actual.x.Double, expectedX, tolerance) &&
                Close(actual.y.Double, expectedY, tolerance) &&
                Close(actual.z.Double, expectedZ, tolerance) &&
                Close(actual.w.Double, expectedW, tolerance);

            Check(name, pass, $"({actual.x},{actual.y},{actual.z},{actual.w})", $"({expectedX},{expectedY},{expectedZ},{expectedW})", $"容差={tolerance}");
        }

        private static void CheckLongArray(string name, long[] actual, params long[] expected)
        {
            bool pass = actual.Length == expected.Length;
            if (pass)
            {
                for (int i = 0; i < actual.Length; i++)
                {
                    if (actual[i] != expected[i])
                    {
                        pass = false;
                        break;
                    }
                }
            }

            Check(name, pass, $"[{string.Join(",", actual)}]", $"[{string.Join(",", expected)}]");
        }

        private static void CheckThrows<TException>(string name, Action action)
            where TException : Exception
        {
            totalCount++;
            try
            {
                action();
                failedCount++;
                Console.WriteLine($"[FAIL] {name} | 未抛出异常 | 期望：{typeof(TException).Name}");
            }
            catch (TException)
            {
                passedCount++;
                Console.WriteLine($"[PASS] {name} | 抛出：{typeof(TException).Name}");
            }
            catch (Exception ex)
            {
                failedCount++;
                Console.WriteLine($"[FAIL] {name} | 抛出：{ex.GetType().Name} | 期望：{typeof(TException).Name}");
            }
        }

        private static void Check(string name, bool pass, string actual, string expected, string extra = "")
        {
            totalCount++;
            if (pass)
                passedCount++;
            else
                failedCount++;

            string suffix = string.IsNullOrEmpty(extra) ? string.Empty : $" | {extra}";
            Console.WriteLine($"[{(pass ? "PASS" : "FAIL")}] {name} | 实际：{actual} | 期望：{expected}{suffix}");
        }

        private static bool Close(double actual, double expected, double tolerance)
        {
            return Math.Abs(actual - expected) <= tolerance;
        }
    }
}
