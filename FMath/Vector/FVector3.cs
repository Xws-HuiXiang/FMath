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

        public FFloat x;
        public FFloat y;
        public FFloat z;

        public FVector3(FFloat x, FFloat y, FFloat z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

#if UNITY
        public FVector3(Vector3 vector3)
        {
            this.x = new FFloat(vector3.x);
            this.y = new FFloat(vector3.y);
            this.z = new FFloat(vector3.z);
        }

        /// <summary>
        /// 转换为对应的Unity中的向量对象
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

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj is FVector3 v)
            {
                return v.x == x && v.y == y && v.z == z;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
        }

        public override string ToString()
        {
            return $"({x},{y},{z})";
        }
    }
}
