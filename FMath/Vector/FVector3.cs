using System;
using System.Collections.Generic;
using System.Text;
#if UNITY
using UnityEngine;
#endif

namespace FMath.Vector
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
