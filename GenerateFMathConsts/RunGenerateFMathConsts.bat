echo off

::modify file encoding to UTF-8
chcp 65001

title 生成FMath库中使用的常量文件内容

dotnet run --project GenerateFMathConsts.csproj -- -m 65536 -o ../FMath/Math/FMath.Consts.cs