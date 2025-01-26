public static class BubbleConfig
{
    public const float SizeScaleBase = 50.0f; // 用於碰撞形狀比例計算的基準值
    public const int MaxBubbleCount = 16; // 泡泡數量上限
    
    public static int MaxLivableLevel = 15; // 泡泡等級超過則撐破
    
    public const float CollisionCheckDuration = 6.0f; // 泡泡寂寞死前的容許時長
    public const int CollisionSplitThreshold = 3; // 分裂的碰撞次數條件
    
    public const float MinRandomVelocity = -500; // 隨機初始速度
    public const float MaxRandomVelocity = 500;

    // 成長相關
    public const float SizeGrowthFactor = 1f; // 每等級大小增長因子

    // 浮力相關
    public const float BuoyancyCoefficient = 0.05f; // 浮力係數 k
    public const float MaxBuoyancy = 0.5f; // 最大浮力限制

    // 質量相關
    public const float DensityFactor = 0.1f; // 質量密度係數
    public static float MaxAccelerationSpeedAllowed = 20f;
    public static float RevengeTimeNeed = 0.4f;
}