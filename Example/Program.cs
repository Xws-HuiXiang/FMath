using FixedMath;

namespace Example
{
    public class Program
    {
        static void Main(string[] args)
        {
            //构造定点数
            Console.WriteLine("---------- 构造定点数 ----------");
            FFloat v1 = new FFloat(2.5);
            FFloat v7 = new FFloat(5.6f);
            FFloat v2 = new FFloat(4);
            FFloat v3 = new FFloat(3);
            //显示转换
            FFloat v4 = (FFloat)0.3f;
            FFloat v5 = (FFloat)0.7;
            //隐式转换
            FFloat v6 = 6;
            FFloat v8 = 16;
            //定点数运算
            Console.WriteLine("---------- 定点数运算 ----------");
            Console.WriteLine($"-({v1}) = {-v1}");
            Console.WriteLine($"{v1} + {v2} = {v1 + v2}");
            Console.WriteLine($"{v1} - {v2} = {v1 - v2}");
            Console.WriteLine($"{v1} * {v2} = {v1 * v2}");
            Console.WriteLine($"{v1} / {v2} = {v1 / v2}");
            Console.WriteLine($"{v6} % {v2} = {v6 % v2}");
            //定点数比较
            Console.WriteLine("---------- 定点数比较 ----------");
            Console.WriteLine($"{v1} < {v2} ? {v1 < v2}");
            Console.WriteLine($"{v1} > {v2} ? {v1 > v2}");
            Console.WriteLine($"{v1} <= {v2} ? {v1 <= v2}");
            Console.WriteLine($"{v1} >= {v2} ? {v1 >= v2}");
            Console.WriteLine($"{v1} == {v2} ? {v1 == v2}");
            Console.WriteLine($"{v1} != {v2} ? {v1 != v2}");
            //定点数数学库
            Console.WriteLine("---------- 定点数数学库 ----------");
            Console.WriteLine($"π -> {FMath.PI}");
            Console.WriteLine($"2π -> {FMath.PI2}");
            Console.WriteLine($"e -> {FMath.E}");
            Console.WriteLine($"Deg2Rad -> {FMath.Deg2Rad}");
            Console.WriteLine($"Rad2Deg -> {FMath.Rad2Deg}");
            FFloat eulerAngle1 = 30;
            FFloat rad1 = (FFloat)1.57075;
            FFloat rad2 = (FFloat)32.415926;
            FFloat eulerAngle2 = 60;
            FFloat eulerAngle3 = 90;
            FFloat eulerAngle4 = 360 + 30;
            FFloat eulerAngle5 = -360 - 60;
            FFloat eulerAngle6 = 720 + 30;
            Console.WriteLine($"SinAngle({eulerAngle1}) -> {FMath.SinAngle(eulerAngle1)} >>> 标准数学库计算结果：{Math.Sin(eulerAngle1.Double * (Math.PI / 180))}");
            Console.WriteLine($"SinAngle({eulerAngle2}) -> {FMath.SinAngle(eulerAngle2)} >>> 标准数学库计算结果：{Math.Sin(eulerAngle2.Double * (Math.PI / 180))}");
            Console.WriteLine($"SinAngle({eulerAngle5}) -> {FMath.SinAngle(eulerAngle5)} >>> 标准数学库计算结果：{Math.Sin(eulerAngle5.Double * (Math.PI / 180))}");
            Console.WriteLine($"Sin({rad1}) -> {FMath.Sin(rad1)} >>> 标准数学库计算结果：{Math.Sin(rad1.Double)}");
            Console.WriteLine($"Sin({rad2}) -> {FMath.Sin(rad2)} >>> 标准数学库计算结果：{Math.Sin(rad2.Double)}");
            Console.WriteLine($"CosAngle({eulerAngle3}) -> {FMath.CosAngle(eulerAngle3)} >>> 标准数学库计算结果：{Math.Cos(eulerAngle3.Double * (Math.PI / 180))}");
            Console.WriteLine($"CosAngle({eulerAngle5}) -> {FMath.CosAngle(eulerAngle5)} >>> 标准数学库计算结果：{Math.Cos(eulerAngle5.Double * (Math.PI / 180))}");
            Console.WriteLine($"CosAngle({eulerAngle6}) -> {FMath.CosAngle(eulerAngle6)} >>> 标准数学库计算结果：{Math.Cos(eulerAngle6.Double * (Math.PI / 180))}");
            Console.WriteLine($"Cos({rad1}) -> {FMath.Cos(rad1)} >>> 标准数学库计算结果：{Math.Cos(rad1.Double)}");
            Console.WriteLine($"Cos({rad2}) -> {FMath.Cos(rad2)} >>> 标准数学库计算结果：{Math.Cos(rad2.Double)}");
            Console.WriteLine($"TanAngle({eulerAngle1}) -> {FMath.TanAngle(eulerAngle1)} >>> 标准数学库计算结果：{Math.Tan(eulerAngle1.Double * (Math.PI / 180))}");
            Console.WriteLine($"TanAngle({eulerAngle2}) -> {FMath.TanAngle(eulerAngle2)} >>> 标准数学库计算结果：{Math.Tan(eulerAngle2.Double * (Math.PI / 180))}");
            Console.WriteLine($"TanAngle({eulerAngle4}) -> {FMath.TanAngle(eulerAngle4)} >>> 标准数学库计算结果：{Math.Tan(eulerAngle4.Double * (Math.PI / 180))}");
            Console.WriteLine($"Tan({rad1}) -> {FMath.Tan(rad1)} >>> 标准数学库计算结果：{Math.Tan(rad1.Double)}");
            Console.WriteLine($"Tan({rad2}) -> {FMath.Tan(rad2)} >>> 标准数学库计算结果：{Math.Tan(rad2.Double)}");
            FFloat v9 = new FFloat(0.1);
            FFloat v10 = new FFloat(-0.3);
            FFloat v11 = new FFloat(0.8);
            Console.WriteLine($"Asin({v9}) -> {FMath.Asin(v9)} >>> 标准数学库计算结果：{Math.Asin(v9.Double)}");
            Console.WriteLine($"Asin({v10}) -> {FMath.Asin(v10)} >>> 标准数学库计算结果：{Math.Asin(v10.Double)}");
            Console.WriteLine($"Asin({v11}) -> {FMath.Asin(v11)} >>> 标准数学库计算结果：{Math.Asin(v11.Double)}");
            Console.WriteLine($"Acos({v9}) -> {FMath.Acos(v9)} >>> 标准数学库计算结果：{Math.Acos(v9.Double)}");
            Console.WriteLine($"Acos({v10}) -> {FMath.Acos(v10)} >>> 标准数学库计算结果：{Math.Acos(v10.Double)}");
            Console.WriteLine($"Acos({v11}) -> {FMath.Acos(v11)} >>> 标准数学库计算结果：{Math.Acos(v11.Double)}");
            Console.WriteLine($"Atan({v9}) -> {FMath.Atan(v9)} >>> 标准数学库计算结果：{Math.Atan(v9.Double)}");
            Console.WriteLine($"Atan({v10}) -> {FMath.Atan(v10)} >>> 标准数学库计算结果：{Math.Atan(v10.Double)}");
            Console.WriteLine($"Atan({v11}) -> {FMath.Atan(v11)} >>> 标准数学库计算结果：{Math.Atan(v11.Double)}");
            Console.WriteLine($"Sqrt({v8}) -> {FMath.Sqrt(v8)}");
            Console.WriteLine($"Pow({v3},{v2}) -> {FMath.Pow(v3, v2.Int)}");
            FFloat v12 = -4;
            Console.WriteLine($"Pow({v12},{v3}) -> {FMath.Pow(v12, v3.Int)}");
            Console.WriteLine($"Abs({v10}) -> {FMath.Abs(v10)}");
            Console.WriteLine($"Abs({-11}) -> {FMath.Abs(-11)}");
            FFloat v13 = 4;
            FFloat v14 = 16;
            Console.WriteLine($"LogE({v14}) -> {FMath.LogE(v14)} >>> 标准数学库计算结果：{Math.Log(v14.Double, Math.E)}");
            Console.WriteLine($"Log{v13}({v14}) -> {FMath.Log(v13, v14)} >>> 标准数学库计算结果：{Math.Log(v13.Double, v14.Double)}");
            Console.WriteLine($"Log10({v14}) -> {FMath.Log10(v14)} >>> 标准数学库计算结果：{Math.Log(v14.Double, 10)}");
            Console.WriteLine($"Log2({v14}) -> {FMath.Log2(v14)} >>> 标准数学库计算结果：{Math.Log(v14.Double, 2)}");
            //定点数向量
            Console.WriteLine("---------- 定点数向量 ----------");
            //定点数矩阵
            Console.WriteLine("---------- 定点数矩阵 ----------");

            Console.ReadLine();
        }
    }
}
