using Godot;
using System;

public partial class Explosion : Node2D
{
	public float Radius = 100.0f; // 爆炸半徑
	public float Force = 500.0f; // 爆炸推力
	public float Duration = 0.5f; // 爆炸持續時間（秒）

	private Timer _timer;

	public override void _Ready()
	{
		// 初始化計時器
		_timer = new Timer();
		_timer.WaitTime = Duration;
		_timer.OneShot = true;
		_timer.Connect("timeout", new Callable(this, nameof(OnExplosionTimeout)));
		AddChild(_timer);
		
		_timer.Start();
		
		
		// 執行爆炸效果
		ApplyExplosionForce();
	}

	public override void _Draw()
	{
		// 繪製爆炸範圍（用於 Debug）
		DrawCircle(Vector2.Zero, Radius, new Color(1, 0, 0, 0.5f)); // 半透明紅色圓形
	}

	private void ApplyExplosionForce()
	{
		// 搜索場景中的剛體並應用推力
		var allRNodes = GetTree().GetNodesInGroup("RigidBodies");
		foreach (Node node in allRNodes)
		{
			if (node is RigidBody2D rigidBody)
			{
				Vector2 direction = (rigidBody.GlobalPosition - GlobalPosition).Normalized();
				float distance = (rigidBody.GlobalPosition - GlobalPosition).Length();

				if (distance <= Radius)
				{
					float forceMultiplier = 1.0f - (distance / Radius); // 力量隨距離遞減
					rigidBody.ApplyImpulse(direction * Force * forceMultiplier);
					GD.Print(direction * Force * forceMultiplier);
				}
			}
		}
	}

	private void OnExplosionTimeout()
	{
		QueueFree(); // 自動銷毀節點
	}
}
