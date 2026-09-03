中文 | [English](./READMD.md)

# FMath 定点数数学库

实现大部分基础数学函数按定点数运算；实现向量相关定点数运算；实现矩阵相关定点数运算、四元数相关定点数运算。

定点数实现逻辑为扩大指定的倍数（默认为65536），然后使用对应的整数计算。

旋转相关计算中，使用右手坐标系。

> 注意：**浮点数转换为定点浮点数会有精度损失**。

使用位运算加速计算。

**库中所有方法、字段、属性等均有自动注释。**

**自行修改缩放倍数的方法：**

1. 在`FFloat`类中，找到`BitMoveCount`常量，修改为需要的数值。

2. 打开`GenerateFMathConsts/RunGenerateFMathConsts.bat`批处理文件（Mac系统为`GenerateFMathConsts/RunGenerateFMathConsts.sh`shell脚本文件），修改`-m`后的数字参数为放大倍数（注意与FMath中的缩放倍率保持一致），比如BitMoveCount为16（2^16），则需要修改为`-m 65536`，保存后运行。

3. 重新编译FMath库。

4. 注意，由于库中使用了位运算，所以扩大倍数必须为2的幂次方。

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
