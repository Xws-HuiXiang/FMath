#!/bin/bash

# 设置终端标题
echo -en "\033]0;生成FMath库中使用的常量文件内容\007"

dotnet run --project GenerateFMathConsts.csproj -- -m 65536 -o ../FMath/Math/FMath.Consts.cs