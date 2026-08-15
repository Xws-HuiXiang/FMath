# FMath 定点数数学库

实现了大部分基础数学函数；实现向量相关运算；实现矩阵相关运算（正在开发）、四元数相关运算（正在开发）。

定点数实现逻辑为扩大指定的倍数（默认为65536），然后使用对应的整数计算。

注意：**浮点数转换为定点浮点数会有精度损失**。

目标框架为`netstandard2.0`。

使用位运算加速计算。

*注：库中所有方法、字段、属性等均有自动注释*

**自行修改缩放倍数的方法：在`FFloat`类中，找到`BitMoveCount`常量，修改后重新编译即可。**

#### 定点数构造方式

直接构造，且支持隐式转换和显示转换

```C#
FFloat fromInt = new FFloat(2);
FFloat fromFloat = new FFloat(2.5f);
FFloat explicitDouble = (FFloat)0.7;
FFloat implicitInt = 6;
```

#### 定点数数学库 API

主要的类为`FMath`，其中方法名称与`Math`类类似

```C#
FMath.Sqrt(16);
FMath.E;
FMath.LogE(logValue);
FMath.Ceiling(new FFloat(1.2));
FMath.Max(2, 5);
Math.Sin(rad1.Double);
FMath.Acos(asinValue);
```

~~支持定点数向量`FVector2`、`FVector3`和`FVector4`；支持定点数四元数`FQuaternion`；支持定点数矩阵`FMatrix`。~~
