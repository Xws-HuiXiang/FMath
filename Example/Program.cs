using FixedMath;

namespace Example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //构造定点数
            FFloat v1 = new FFloat(2.5);
            FFloat v2 = new FFloat(4);
            FFloat v3 = new FFloat(3);
            //显示转换
            FFloat v4 = (FFloat)0.3f;
            FFloat v5 = (FFloat)0.7;
            //隐式转换
            FFloat v6 = 6;
            //定点数运算
            Console.WriteLine($"-({v1}) = {-v1}");
            Console.WriteLine($"{v1} + {v2} = {v1 + v2}");
            Console.WriteLine($"{v1} - {v2} = {v1 - v2}");
            Console.WriteLine($"{v1} * {v2} = {v1 * v2}");
            Console.WriteLine($"{v1} / {v2} = {v1 / v2}");
            //定点数比较
            Console.WriteLine($"{v1} < {v2} ? {v1 < v2}");
            Console.WriteLine($"{v1} > {v2} ? {v1 > v2}");
            Console.WriteLine($"{v1} <= {v2} ? {v1 <= v2}");
            Console.WriteLine($"{v1} >= {v2} ? {v1 >= v2}");
            Console.WriteLine($"{v1} == {v2} ? {v1 == v2}");
            Console.WriteLine($"{v1} != {v2} ? {v1 != v2}");
            //定点数向量
            //数学库
            Console.WriteLine($"3 Sqrt -> {FMath.Sqrt(3)}");

            Console.ReadLine();
        }
    }
}
