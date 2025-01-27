using Godot;
using System;

public partial class Explosion : Node2D
{
	public float Radius = 100.0f; // 爆炸半徑
	public float Force = BubbleConfig.ExplosionPow; // 爆炸推力
	public float Duration = 0.5f; // 爆炸持續時間（秒）

	private Timer _timer;

	[Export] public AudioStreamPlayer2D explosion;

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
		// PackedScene explosionScene = GetParent().GetNode("BubbleRocketFinal").GetNode<GameManager>("GameManager").explosionScene;
		// Explosion ex = explosionScene.Instantiate() as Explosion;
		// AddChild(ex);
		// GD.Print("get ex? " + ex == null);
		// explosion.Play();
		CreateSound();

	}

	private void CreateSound()
	{
		// 1. 動態創建 AudioStreamPlayer2D 節點
		var audioPlayer = new AudioStreamPlayer2D();

		// 2. 動態加載音訊檔案
		Random random = new Random();
		int index = random.Next(1, 4);
		var audioStream = ResourceLoader.Load<AudioStream>($"res://assets/audios/explode/Explode{index}.wav");
		if (audioStream == null)
		{
			GD.PrintErr("音訊資源加載失敗！");
			return;
		}

		// 3. 將音訊資源分配給 AudioStreamPlayer2D
		audioPlayer.Stream = audioStream;

		// 4. 將 AudioStreamPlayer2D 添加到當前場景樹
		AddChild(audioPlayer);

		// 5. 播放音訊
		audioPlayer.Play();
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
					rigidBody.ApplyImpulse(direction * Force);
					GD.Print(direction * Force);
				}
			}
		}
	}

	private void OnExplosionTimeout()
	{
		QueueFree(); // 自動銷毀節點
	}
}
