﻿using System;
using System.Collections.Generic;
using System.Text;
#if UNITY
using UnityEngine;
#endif

namespace FixedMath.Vector
{
    /// <summary>
    /// 定点数使用的三维向量
    /// </summary>
    public struct FVector3
    {
        #region 常用向量
        /// <summary>
        /// 向量（0, 0, 0）
        /// </summary>
        public static FVector3 Zero { get { return new FVector3(0, 0, 0); } }
        /// <summary>
        /// 向量（1, 1, 1）
        /// </summary>
        public static FVector3 One { get { return new FVector3(1, 1, 1); } }
        /// <summary>
        /// 向量（0, 0, 1）
        /// </summary>
        public static FVector3 Forward { get { return new FVector3(0, 0, 1); } }
        /// <summary>
        /// 向量（0, 0, -1）
        /// </summary>
        public static FVector3 Back { get { return new FVector3(0, 0, -1); } }
        /// <summary>
        /// 向量（-1, 0, 0）
        /// </summary>
        public static FVector3 Left { get { return new FVector3(-1, 0, 0); } }
        /// <summary>
        /// 向量（1, 0, 0）
        /// </summary>
        public static FVector3 Right { get { return new FVector3(1, 0, 0); } }
        /// <summary>
        /// 向量（0, 1, 0）
        /// </summary>
        public static FVector3 Up { get { return new FVector3(0, 1, 0); } }
        /// <summary>
        /// 向量（0, -1, 0）
        /// </summary>
        public static FVector3 Down { get { return new FVector3(0, -1, 0); } }
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
        /// 使用定点数构造定点向量
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public FVector3(FFloat x, FFloat y, FFloat z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

#if UNITY
        /// <summary>
        /// 使用 unity 的 vector3 类型构造定点向量
        /// </summary>
        /// <param name="vector3"></param>
        public FVector3(Vector3 vector3)
        {
            this.x = new FFloat(vector3.x);
            this.y = new FFloat(vector3.y);
            this.z = new FFloat(vector3.z);
        }

        /// <summary>
        /// 返回 unity 的向量对象
        /// </summary>
        public Vector3 Vector3
        {
            get
            {
                return new Vector3(x.Float, y.Float, z.Float);
            }
        }
#endif

        /// <summary>
        /// 获取放大后的值数组
        /// </summary>
        /// <returns></returns>
        public long[] ConvertLongArray()
        {
            return new long[] { x.ScaledValue, y.ScaledValue, z.ScaledValue };
        }

        /// <summary>
        /// 向量长度的平方
        /// </summary>
        public FFloat sqrMagnitude { get { return x * x + y * y + z * z; } }

        /// <summary>
        /// 计算向量长度的平方
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static FFloat SqrMagnitude(FVector3 vector)
        {
            return vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
        }

        /// <summary>
        /// 向量长度
        /// </summary>
        public FFloat Magnitude { get { return FMath.Sqrt(this.sqrMagnitude); } }
        #region 运算符重载
        /// <summary>
        /// 向量加法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FVector3 operator +(FVector3 left, FVector3 right)
        {
            FFloat x = left.x + right.x;
            FFloat y = left.y + right.y;
            FFloat z = left.z + right.z;

            return new FVector3(x, y, z);
        }

        /// <summary>
        /// 向量减法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FVector3 operator -(FVector3 left, FVector3 right)
        {
            FFloat x = left.x - right.x;
            FFloat y = left.y - right.y;
            FFloat z = left.z - right.z;

            return new FVector3(x, y, z);
        }

        /// <summary>
        /// 向量乘法
        /// </summary>
        /// <param name="value"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public static FVector3 operator *(FVector3 left, FFloat value)
        {
            FFloat x = left.x * value;
            FFloat y = left.y * value;
            FFloat z = left.z * value;

            return new FVector3(x, y, z);
        }

        /// <summary>
        /// 向量乘法
        /// </summary>
        /// <param name="value"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public static FVector3 operator *(FFloat value, FVector3 left)
        {
            FFloat x = value * left.x;
            FFloat y = value * left.y;
            FFloat z = value * left.z;

            return new FVector3(x, y, z);
        }

        /// <summary>
        /// 向量除法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FVector3 operator /(FVector3 left, FFloat value)
        {
            FFloat x = left.x / value;
            FFloat y = left.y / value;
            FFloat z = left.z / value;

            return new FVector3(x, y, z);
        }

        /// <summary>
        /// 向量值取反
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static FVector3 operator -(FVector3 vector)
        {
            FFloat x = -vector.x;
            FFloat y = -vector.y;
            FFloat z = -vector.z;

            return new FVector3(x, y, z);
        }

        /// <summary>
        /// 判断向量相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(FVector3 left, FVector3 right)
        {
            return left.x == right.x && left.y == right.y && left.z == right.z;
        }

        /// <summary>
        /// 判断向量不等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(FVector3 left, FVector3 right)
        {
            return left.x != right.x || left.y != right.y || left.z != right.z;
        }
        #endregion

        /// <summary>
        /// 判断对象是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj is FVector3 v)
            {
                return v.x == x && v.y == y && v.z == z;
            }

            return false;
        }

        /// <summary>
        /// 返回这个对象的 HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
        }

        /// <summary>
        /// 返回对象的 x、y 和 z 轴值的字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"({x},{y},{z})";
        }
    }
}
