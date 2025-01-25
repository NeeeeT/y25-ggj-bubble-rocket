using Godot;
using System;
using LifeAtomGameDemo;

public partial class BubbleShooter : Node2D
{
	[Export] public PackedScene BubbleScene; // 泡泡場景
	[Export] public NodePath BubbleManagerPath; // 泡泡管理器節點的路徑

	private BubbleManager _bubbleManager;

	public override void _Ready()
	{
		if (BubbleScene == null)
			throw new NullReferenceException("BubbleScene cannot be null");

		_bubbleManager = GetNode<BubbleManager>(BubbleManagerPath);
		if (_bubbleManager == null)
			throw new NullReferenceException("BubbleManager cannot be null");
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
		{
			ShootBubble(mouseEvent.Position);
		}
	}

	private void ShootBubble(Vector2 targetPosition)
	{
		// 生成泡泡
		var bubble = (Bubble)BubbleScene.Instantiate();

		// 設定泡泡初始位置和方向
		bubble.Position = new Vector2(0, 0); // 初始位置為 (0,0)
		var direction = (targetPosition - bubble.Position).Normalized();

		// 設置泡泡的速度
		bubble.LinearVelocity = direction * 200; // 射向點擊位置，速度為 200

		// 將泡泡加入到管理器中
		_bubbleManager.AddChild(bubble);
	}
}
