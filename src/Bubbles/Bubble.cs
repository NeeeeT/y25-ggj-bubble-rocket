using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using LifeAtomGameDemo;
using LifeAtomGameDemo.Elements;

public partial class Bubble : RigidBody2D, IBubble
{
	// 實現介面屬性
	[Export] public float Size { get; set; } = 50.0f; // 初始大小
	[Export] public float Weight { get; set; } = 1.0f; // 初始重量
	[Export] public int Level { get; set; } = 1; // 初始等級
	[Export] public float CooldownTime { get; set; } = 2.0f; // 冷卻時間
	[Export] public float LevelGrowthRate { get; set; } = 1f; // 每秒等級增長速度

	public ElementManager ElementManager { get; set; } = new ElementManager();

	public event Action<Bubble> OnBubbleDestroyed; // 泡泡刪除事件
	public bool canBeSplit => Level > 1;

	// 碰撞紀錄
	private List<float> collisionTimestamps = new List<float>();
	private float lastSplitTime = 0f;
	private float elapsedTime_forLevel = 0.0f; // 記錄經過的時間
	private float currentTime = 0f;
	private float BirthTime = 0f;

	private Label _levelLabel; // 用於顯示等級的文字節點
	private Vector2? _acceleration;


	public Color _Modulate
	{
		set => this.Modulate = value;
	}

	public float VelocityFactor { get; set; } = 1f;

	public override void _Ready()
	{
		BirthTime = Time.GetTicksMsec()/1000;
		_acceleration = null;
		ElementManager.init(this);
		
		// 初始化泡泡大小
		var sprite = GetNode<Sprite2D>("Sprite2D");
		sprite.Scale = new Vector2(Size / sprite.Texture.GetWidth(), Size / sprite.Texture.GetHeight());

		// 碰撞形狀比例基於 BubbleConfig.SizeScaleBase
		var collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		collisionShape.Scale = new Vector2(Size / BubbleConfig.SizeScaleBase, Size / BubbleConfig.SizeScaleBase);

		// 設定 Area2D 的碰撞形狀大小
		var areaShape = GetNode<Area2D>("Area2D").GetNode<CollisionShape2D>("CollisionShape2D");
		areaShape.Scale = new Vector2(Size / BubbleConfig.SizeScaleBase, Size / BubbleConfig.SizeScaleBase);

		// 初始化文字節點
		_levelLabel = new Label
		{
			Text = Level.ToString(),
			Modulate = Colors.Black
		};
		AddChild(_levelLabel);
		UpdateLabelPosition();
	}

	public override void _Process(double delta)
	{
		currentTime = Time.GetTicksMsec() / 1000.0f;
		
		// 移除寂寞死的設定時間區間外的碰撞記錄
		collisionTimestamps.RemoveAll(timestamp => currentTime - timestamp > BubbleConfig.CollisionCheckDuration);

		CheckLifeRules();

		// 更新等級
		elapsedTime_forLevel += (float)delta;
		if (elapsedTime_forLevel >= 1.0f) // 每秒更新一次等級
		{
			Level += (int)(elapsedTime_forLevel * LevelGrowthRate);
			elapsedTime_forLevel = 0.0f;
			UpdateSize(); // 更新大小
			UpdateLabel(); // 更新文字
		}
		
		if(Level>=BubbleConfig.MaxLivableLevel)
			Die();

		if(VelocityFactor != 1.0f)
			this.LinearVelocity *= VelocityFactor;
		//this.ApplyAcceleration(delta);
	}

	private void UpdateLabel()
	{
		_levelLabel.Text = Level.ToString();
		UpdateLabelPosition();
	}

	private void UpdateLabelPosition()
	{
		_levelLabel.Position = new Vector2(-_levelLabel.GetMinimumSize().X / 2, -Size / 2 - 10); // 文字在泡泡上方
	}

	public void Split()
	{
		// 通知管理器進行分裂
		lastSplitTime = currentTime;
		var manager = (BubbleManager)GetParent();
		manager.SplitBubble(this);
		GD.Print("Split:" + manager.CurrentBubbleCount);
	}

	public void Die()
	{
		
		OnBubbleDestroyed?.Invoke(this); // 通知管理器
	}

	
	public void HandleCollision(IBubble other)
	{
		float currentTime = Time.GetTicksMsec() / 1000.0f;

		// 清理超過檢查時間的碰撞時間戳
		collisionTimestamps.RemoveAll(timestamp => currentTime - timestamp > BubbleConfig.CollisionCheckDuration);

		// 添加當前碰撞時間戳
		collisionTimestamps.Add(currentTime);
		
		// 檢查分裂條件
		if (collisionTimestamps.Count >= BubbleConfig.CollisionSplitThreshold &&
			currentTime - lastSplitTime >= CooldownTime)
		{
			Split();
		}
		
		GD.Print("pon");
		other.ElementManager.ApplyEffects(other, this, GetTree().Root);
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

		// 設定 Area2D 的碰撞形狀大小
		var areaShape = GetNode<Area2D>("Area2D").GetNode<CollisionShape2D>("CollisionShape2D");
		areaShape.Scale = new Vector2(Size / BubbleConfig.SizeScaleBase, Size / BubbleConfig.SizeScaleBase);

		// 更新文字位置
		UpdateLabelPosition();
	}

	public void TapeEffect(Bubble bubble, Vector2 midpoint)
	{
		this._acceleration = midpoint;
		bubble._acceleration = midpoint;
	}

	private void ApplyAcceleration(double delta)
	{
		// 加速度相關
		if (this._acceleration == null)
			return;

		// 計算指向目標的方向
		Vector2 direction = ((Vector2)this._acceleration - GlobalPosition).Normalized();

		// 更新速度（限制最大速度）
		this.LinearVelocity += (_acceleration * (float)delta)?? Vector2.Zero;
		if (this.LinearVelocity.Length() > BubbleConfig.MaxAccelerationSpeedAllowed)
		{
			this.LinearVelocity  = this.LinearVelocity .Normalized() * BubbleConfig.MaxAccelerationSpeedAllowed;
		}

		// 更新位置
		Position += this.LinearVelocity * (float)delta;
	}
	
	private void CheckLifeRules()
	{
		GD.Print("碰撞次數: "+collisionTimestamps.Count);
		GD.Print("空窗期: "+(currentTime-Math.Max(BirthTime, collisionTimestamps.LastOrDefault())));
		if (collisionTimestamps.Count == 0 && 
			currentTime-Math.Max(BirthTime, collisionTimestamps.LastOrDefault()) > BubbleConfig.CollisionCheckDuration)
		{
			CallDeferred(nameof(Die));
		}
	}
	
	private void _on_Area_body_entered(Node body)
	{
		if (body == this)
		{
			GD.Print("無效碰撞: self");
			return;
		}
		
		if (body is IBubble bubble)
		{
			HandleCollision(bubble);
		}
		else
		{
			GD.Print("無效碰撞: not IBubble");
		}
	}
}
