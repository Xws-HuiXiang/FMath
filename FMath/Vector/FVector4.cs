using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace FixedMath.Vector
{
    /// <summary>
    /// 定点数四维向量
    /// </summary>
    public struct FVector4
    {
        #region 常用向量
        /// <summary>
        /// 向量（0, 0）
        /// </summary>
        public static FVector4 Zero { get { return new FVector4(0, 0, 0, 0); } }
        /// <summary>
        /// 向量（1, 1）
        /// </summary>
        public static FVector4 One { get { return new FVector4(1, 1, 1, 1); } }
        #endregion

        /// <summary>
        /// 向量 x 轴的值
        /// </summary>
        public FFloat x;
        /// <summary>
        /// 向量 y 轴的值
        /// </summary>
        public FFloat y;
        /// <summary>
        /// 向量 z 轴的值
        /// </summary>
        public FFloat z;
        /// <summary>
        /// 向量 w 轴的值
        /// </summary>
        public FFloat w;

        /// <summary>
        /// 构建定点数四维向量
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        public FVector4(FFloat x, FFloat y, FFloat z, FFloat w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        /// <summary>
        /// 获取放大后的值数组
        /// </summary>
        /// <returns></returns>
        public long[] ConvertLongArray()
        {
            return new long[] { x.ScaledValue, y.ScaledValue, z.ScaledValue, w.ScaledValue };
        }

        /// <summary>
        /// 向量长度的平方
        /// </summary>
        public FFloat sqrMagnitude { get { return x * x + y * y + z * z + w * w; } }

        /// <summary>
        /// 计算向量长度的平方
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static FFloat SqrMagnitude(FVector4 vector)
        {
            return vector.x * vector.x + vector.y * vector.y + vector.z * vector.z + vector.w * vector.w;
        }

        /// <summary>
        /// 向量长度
        /// </summary>
        public FFloat Magnitude { get { return FMath.Sqrt(Dot(this, this)); } }

        /// <summary>
        /// 返回当前向量的单位向量
        /// </summary>
        public FVector4 normalized
        {
            get
            {
                if (this.Magnitude > 0)
                {
                    FFloat rate = FFloat.One / this.Magnitude;

                    return new FVector4(x * rate, y * rate, z * rate, w * rate);
                }
                else
                {
                    return FVector4.Zero;
                }
            }
        }

        /// <summary>
        /// 将当前向量转换为单位向量
        /// </summary>
        public void Normalize()
        {
            if (this.Magnitude > 0)
            {
                FFloat rate = FFloat.One / this.Magnitude;

                x *= rate;
                y *= rate;
            }
        }

        /// <summary>
        /// 计算指定向量的单位向量
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static FVector4 Normalize(FVector4 vector)
        {
            if (vector.Magnitude > 0)
            {
                FFloat rate = FFloat.One / vector.Magnitude;

                return new FVector4(vector.x * rate, vector.y * rate, vector.z * rate, vector.w * rate);
            }
            else
            {
                return FVector4.Zero;
            }
        }

        /// <summary>
        /// 向量点乘
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static FFloat Dot(FVector4 a, FVector4 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }
    }
}
