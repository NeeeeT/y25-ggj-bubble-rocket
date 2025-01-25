public static class BubbleConfig
{
    public const float SizeScaleBase = 50.0f; // 用於碰撞形狀比例計算的基準值
    public const int MaxBubbleCount = 30; // 泡泡數量上限
    public const float CollisionCheckDuration = 10.0f; // seconds
    public const int CollisionSplitThreshold = 2;
    public const float MinRandomVelocity = -500; // 隨機初始速度
    public const float MaxRandomVelocity = 500;

    // 成長相關
    public const float SizeGrowthFactor = 1.0f; // 每等級大小增長因子

    // 浮力相關
    public const float BuoyancyCoefficient = 0.05f; // 浮力係數 k
    public const float MaxBuoyancy = 0.5f; // 最大浮力限制

    // 質量相關
    public const float DensityFactor = 0.1f; // 質量密度係數
}