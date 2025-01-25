using Godot;
using System;
using LifeAtomGameDemo;

public partial class MainScene : Node2D
{
	[Export] public PackedScene BubbleScene;
	[Export] public int BubbleCount = 10; // 初始泡泡數量

	private BubbleManager _bubbleManager;
	public override void _Ready()
	{
		if (BubbleScene == null)
			throw new NullReferenceException("BubbleScene cannot be null");

		_bubbleManager= GetNode<BubbleManager>("BubbleManager");
		if(_bubbleManager == null)
			throw new NullReferenceException("BubbleManager cannot be null");
		
		// 隨機生成泡泡
		var random = new RandomNumberGenerator();
		random.Randomize();

		for (int i = 1; i <= Math.Min(BubbleCount, BubbleConfig.MaxBubbleCount) ; i++)
		{
			var bubble = (RigidBody2D)BubbleScene.Instantiate();
			_bubbleManager.AddChild(bubble);

			// 隨機位置與方向
			bubble.Position = new Vector2(
				random.RandfRange(BubbleConfig.MinRandomVelocity, BubbleConfig.MaxRandomVelocity),
				random.RandfRange(BubbleConfig.MinRandomVelocity, BubbleConfig.MaxRandomVelocity));
			bubble.LinearVelocity = new Vector2(
				random.RandfRange(BubbleConfig.MinRandomVelocity, BubbleConfig.MaxRandomVelocity),
				random.RandfRange(BubbleConfig.MinRandomVelocity, BubbleConfig.MaxRandomVelocity));
		}
	}
}
