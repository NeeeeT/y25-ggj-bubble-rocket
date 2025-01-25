using System;
using Godot;
using System.Collections.Generic;
using LifeAtomGameDemo;

public partial class Bubble : RigidBody2D, IBubble
{
	// 實現介面屬性
	[Export] public float Size { get; set; } = 50.0f; // 初始大小
	[Export] public float Weight { get; set; } = 1.0f; // 初始重量
	[Export] public int Level { get; set; } = 1; // 初始等級
	[Export] public float CooldownTime { get; set; } = 2.0f; // 冷卻時間
	[Export] public float LevelGrowthRate { get; set; } = 0.1f; // 每秒等級增長速度
	
	public event Action<Bubble> OnBubbleDestroyed; // 泡泡刪除事件
	
	private List<float> collisionTimestamps = new List<float>();
	private float lastSplitTime = -1.0f;
	private float elapsedTime = 0.0f; // 記錄經過的時間
	private float currentTime = 0f;
	public override void _Ready()
	{
		// 初始化泡泡大小
		var sprite = GetNode<Sprite2D>("Sprite2D");
		sprite.Scale = new Vector2(Size / sprite.Texture.GetWidth(), Size / sprite.Texture.GetHeight());

		// 碰撞形狀比例基於 BubbleConfig.SizeScaleBase
		var collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		collisionShape.Scale = new Vector2(Size / BubbleConfig.SizeScaleBase, Size / BubbleConfig.SizeScaleBase);

		// 設定 Area2D 的碰撞形狀大小
		var areaShape = GetNode<Area2D>("Area2D").GetNode<CollisionShape2D>("CollisionShape2D");
		areaShape.Scale = new Vector2(Size / BubbleConfig.SizeScaleBase, Size / BubbleConfig.SizeScaleBase);

		//GetNode<Area2D>("Area2D").Connect("body_entered", new Callable(this, nameof(_on_Area_body_entered)));
	}

	public override void _Process(double delta)
	{
		currentTime = Time.GetTicksMsec() / 1000.0f;
		collisionTimestamps.RemoveAll(timestamp => currentTime - timestamp > BubbleConfig.CollisionCheckDuration);

		CheckLifeRules();

		// 更新等級
		elapsedTime += (float)delta;
		if (elapsedTime >= 1.0f) // 每秒更新一次等級
		{
			Level += (int)(elapsedTime * LevelGrowthRate);
			elapsedTime = 0.0f;
			UpdateSize(); // 更新大小
		}
		
		// 計算並應用浮力
		//ApplyBuoyancy();
	}
	private void ApplyBuoyancy()
	{
		// 計算浮力
		float volume = MathF.Pow(Size, 3);
		float buoyancyForce = BubbleConfig.BuoyancyCoefficient * volume - Weight;
		buoyancyForce = Math.Clamp(buoyancyForce, -BubbleConfig.MaxBuoyancy, BubbleConfig.MaxBuoyancy);

		// 延遲設置線性速度
		SetDeferred("linear_velocity", new Vector2(LinearVelocity.X, LinearVelocity.Y - buoyancyForce));
	}
	private void CheckLifeRules()
	{
		if (collisionTimestamps.Count == 0 && currentTime > BubbleConfig.CollisionCheckDuration)
		{
			CallDeferred(nameof(Die));
		}
	}

	public void Split()
	{
		// 通知管理器進行分裂
		var manager = (BubbleManager)GetParent();
		manager.SplitBubble(this);
		GD.Print("Split:"+manager.CurrentBubbleCount);
	}

	public void Die()
	{
		//GD.Print("Bubble died.");
		OnBubbleDestroyed?.Invoke(this); // 通知管理器
		QueueFree();

	}
	
	public void HandleCollision(IBubble other)
	{
		float currentTime = Time.GetTicksMsec() / 1000.0f;

		// 清理超過檢查時間的碰撞時間戳
		collisionTimestamps.RemoveAll(timestamp => currentTime - timestamp > BubbleConfig.CollisionCheckDuration);

		// 添加當前碰撞時間戳
		collisionTimestamps.Add(currentTime);

		// 檢查分裂條件
		bool a = collisionTimestamps.Count >= BubbleConfig.CollisionSplitThreshold;
		bool b = currentTime - collisionTimestamps[collisionTimestamps.Count - BubbleConfig.CollisionSplitThreshold] <=
		         BubbleConfig.CollisionCheckDuration;
		bool c = !(currentTime - lastSplitTime < CooldownTime);
		
		// GD.Print("a: "+a);
		// GD.Print("b: "+b);
		// GD.Print("c: "+c);
		
		if (collisionTimestamps.Count >= BubbleConfig.CollisionSplitThreshold &&
		    currentTime - collisionTimestamps[collisionTimestamps.Count - BubbleConfig.CollisionSplitThreshold] <=
		    BubbleConfig.CollisionCheckDuration &&
		    !(currentTime - lastSplitTime < CooldownTime))
		{
			Split();
		}
	}
	
	public void UpdateSize()
	{
		// 計算新大小（基於等級）
		Size = Size + BubbleConfig.SizeGrowthFactor * Level;

		// 計算新體積和質量
		Weight = Size * BubbleConfig.DensityFactor; // 質量公式

		// 更新 Sprite 和碰撞形狀的縮放
		var sprite = GetNode<Sprite2D>("Sprite2D");
		sprite.Scale = new Vector2(Size / BubbleConfig.SizeScaleBase, Size / BubbleConfig.SizeScaleBase);

		var collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		collisionShape.Scale = new Vector2(Size / BubbleConfig.SizeScaleBase, Size / BubbleConfig.SizeScaleBase);
	}


	private void _on_Area_body_entered(Node body)
	{
		if (body is IBubble bubble)
		{
			HandleCollision(bubble);
		}
	}
}
