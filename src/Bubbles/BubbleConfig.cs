public static class BubbleConfig
{
    public static float SizeScaleBase = 50.0f; // 用於碰撞形狀比例計算的基準值
    public static int MaxBubbleCount = 100; // 泡泡數量上限
    
    public static  int MaxLivableLevel = 3; // 泡泡等級超過則撐破
    
    public static float CollisionCheckDuration = 8.0f; // 泡泡寂寞死前的容許時長
    public static int CollisionSplitThreshold = 6; // 分裂的碰撞次數條件
    
    public static float MinRandomVelocity = -100; // 隨機初始速度
    public static float MaxRandomVelocity = 100;

    // 成長相關
    public static float SizeGrowthFactor = 50.2f; // 每等級大小增長因子

    // 浮力相關
    public static float BuoyancyCoefficient = 0.05f; // 浮力係數 k
    public static float MaxBuoyancy = 0.5f; // 最大浮力限制

    // 質量相關
    public static float DensityFactor = 0f; // 質量密度係數
    public static  float MaxAccelerationSpeedAllowed = 20f;
    public static  float RevengeTimeNeed = 0.4f;
    public static float ExplosionPow = 50.0f;
}