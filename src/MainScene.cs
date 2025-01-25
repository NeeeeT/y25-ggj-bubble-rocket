using Godot;
using System;
using LifeAtomGameDemo;

public partial class MainScene : Node2D
{
	[Export] public PackedScene BubbleScene;
	[Export] public int BubbleCount = 12; // 初始泡泡數量
	[Export] public float SpawnInterval = 0.05f; // 每次生成泡泡的間隔時間（秒）

	private BubbleManager _bubbleManager;
	private int _spawnedBubbleCount = 0; // 已生成的泡泡數量
	private Timer _spawnTimer; // 用於控制生成的計時器

	public override void _Ready()
	{
		if (BubbleScene == null)
			throw new NullReferenceException("BubbleScene cannot be null");

		_bubbleManager = GetNode<BubbleManager>("BubbleManager");
		if (_bubbleManager == null)
			throw new NullReferenceException("BubbleManager cannot be null");

		// 初始化計時器
		_spawnTimer = new Timer();
		_spawnTimer.WaitTime = SpawnInterval;
		_spawnTimer.OneShot = false;
		_spawnTimer.Connect("timeout", new Callable(this, nameof(SpawnBubble)));
		AddChild(_spawnTimer);

		_spawnTimer.Start(); // 開始生成泡泡的計時
	}

	private void SpawnBubble()
	{
		if (_spawnedBubbleCount >= Math.Min(BubbleCount, BubbleConfig.MaxBubbleCount))
		{
			_spawnTimer.Stop(); // 停止計時器
			_bubbleManager.CanSplite = true;
			return;
		}

		var random = new RandomNumberGenerator();
		random.Randomize();

		// 生成泡泡

		// 設置隨機位置與方向
		var pos = new Vector2(
			random.RandfRange(BubbleConfig.MinRandomVelocity, BubbleConfig.MaxRandomVelocity),
			random.RandfRange(BubbleConfig.MinRandomVelocity, BubbleConfig.MaxRandomVelocity));

		var bubble = _bubbleManager.CreateBubble(pos, BubbleConfig.SizeScaleBase);
		
		bubble.LinearVelocity = new Vector2(
			random.RandfRange(BubbleConfig.MinRandomVelocity, BubbleConfig.MaxRandomVelocity),
			random.RandfRange(BubbleConfig.MinRandomVelocity, BubbleConfig.MaxRandomVelocity));

		
		_bubbleManager.AddChild(bubble);
		
		_spawnedBubbleCount++; // 更新已生成泡泡數量
	}
}
