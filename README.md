[中文](./README.zh-CN.md) | English

# FMath Fixed-Point Mathematics Library

FMath implements most fundamental mathematical functions using fixed-point arithmetic, as well as fixed-point operations for vectors, matrices, and quaternions.

The fixed-point implementation works by scaling values by a specified factor (65536 by default) and then performing the corresponding calculations using integers.

Rotation-related calculations use a **right-handed coordinate system**.

> **Note:** Converting floating-point values to fixed-point values may result in a loss of precision.

Bitwise operations are used to accelerate calculations.

**All methods, fields, properties, and other members in the library are automatically documented.**

## Changing the Fixed-Point Scale Factor

To change the scale factor manually:

1. In the `FFloat` class, locate the `BitMoveCount` constant and change it to the desired value.

2. Open the `GenerateFMathConsts/RunGenerateFMathConsts.bat` batch file (or the `GenerateFMathConsts/RunGenerateFMathConsts.sh` shell script on macOS), and change the numeric parameter following `-m` to the desired scale factor. Make sure it matches the scale factor used by FMath.
   
   For example, if `BitMoveCount` is `16` (`2^16`), the scale factor is `65536`, so change the parameter to: `-m 65536`. Save the file and run the script.

3. Rebuild the FMath library.

4. Note: Because the library uses bitwise operations, the scale factor must be a power of 2.

#### Fixed-Point Construction

Fixed-point values can be constructed directly, with support for both implicit and explicit conversions.

```C#
FFloat fromInt = new FFloat(2);
FFloat fromFloat = new FFloat(2.5f);
FFloat explicitDouble = (FFloat)0.7;
FFloat implicitInt = 6;
```

#### Fixed-Point Mathematics API

The main class is `FMath`, whose method names are similar to those in the standard `Math` class.

```C#
FMath.Sqrt(16);
FMath.E;
FMath.LogE(logValue);
FMath.Ceiling(new FFloat(1.2));
FMath.Max(2, 5);
Math.Sin(rad1.Double);
FMath.Acos(asinValue);
```