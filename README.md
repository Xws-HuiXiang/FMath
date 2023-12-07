# FMath 定点数数学库
定点数实现逻辑为扩大指定的倍数（默认为1024），然后使用对应的整数计算。

注意：**浮点数转换为定点浮点数会有精度损失**。

目标框架为`netstandard2.0`。

对 unity 引擎做了适配。

使用位运算加速计算。

*注：库中所有方法、字段、属性等均有自动注释*

#### 示例：

``` c#
using FixedMath;

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
        Console.WriteLine($"Sqrt({v8}) -> {FMath.Sqrt(v8)}");
```
