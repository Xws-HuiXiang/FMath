using FixedMath.BaseType;

namespace FixedMath
{
    /// <summary>
    /// 定点数使用的数学运算
    /// </summary>
    public static partial class FMath
    {
        /// <summary>
        /// 极小值
        /// </summary>
        public readonly static FFloat Epsilon = FFloat.FromRaw(1);
        /// <summary>
        /// π
        /// </summary>
        public readonly static FFloat PI = FFloat.FromRaw(205887);
        /// <summary>
        /// π对应的角度值
        /// </summary>
        public const int PIAngle = 180;
        /// <summary>
        /// 2π
        /// </summary>
        public readonly static FFloat PI2 = FFloat.FromRaw(411775);
        /// <summary>
        /// 2π对应的角度值
        /// </summary>
        public const int PI2Angle = 360;
        /// <summary>
        /// π/2
        /// </summary>
        public readonly static FFloat HalfPI = FFloat.FromRaw(102944);
        /// <summary>
        /// π/2对应的角度值
        /// </summary>
        public readonly static FFloat HalfPIAngle = 90;
        /// <summary>
        /// 自然对数基数 e
        /// </summary>
        public readonly static FFloat E = FFloat.FromRaw(178145);
        /// <summary>
        /// 弧度转角度的常量：180/π
        /// </summary>
        public readonly static FFloat Rad2Deg = FFloat.FromRaw(3754936);
        /// <summary>
        /// 角度转弧度的常量：π/180
        /// </summary>
        public readonly static FFloat Deg2Rad = FFloat.FromRaw(1144);
        /// <summary>
        /// 四元数球面插值退化为线性插值的点乘阈值
        /// </summary>
        public readonly static FFloat QuaternionSlerpLinearThreshold = FFloat.FromRaw(65503);

        private readonly static FFloat Ln2 = FFloat.FromRaw(45426);
        private readonly static FFloat CordicK = FFloat.FromRaw(39797);
        private readonly static FFloat[] CordicAtanTable =
        [
            FFloat.FromRaw(51472),
            FFloat.FromRaw(30386),
            FFloat.FromRaw(16055),
            FFloat.FromRaw(8150),
            FFloat.FromRaw(4091),
            FFloat.FromRaw(2047),
            FFloat.FromRaw(1024),
            FFloat.FromRaw(512),
            FFloat.FromRaw(256),
            FFloat.FromRaw(128),
            FFloat.FromRaw(64),
            FFloat.FromRaw(32),
            FFloat.FromRaw(16),
            FFloat.FromRaw(8),
            FFloat.FromRaw(4),
            FFloat.FromRaw(2),
            FFloat.FromRaw(1)
        ];
    }
}
