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
			// 轉換滑鼠點擊位置到 Node2D 的本地座標
			Vector2 targetPosition = GetGlobalMousePosition();
			
			ShootBubble(targetPosition);
			

			GD.Print("射擊"+mouseEvent.Position);
		}
	}

	private void ShootBubble(Vector2 targetPosition)
	{
		// 生成泡泡
		var bubble =  _bubbleManager.CreateBubble(new Vector2(0, 0), BubbleConfig.SizeScaleBase) ;
		if (bubble == null)
		{
			GD.Print("泡泡射出失敗");
			return;
		}
		
		var direction = (targetPosition - bubble.Position).Normalized();

		// 設置泡泡的速度
		bubble.LinearVelocity = direction * 200; // 射向點擊位置，速度為 200

		bubble.ElementManager.init(bubble);
		bubble.ElementManager.AddElement(new NormalElement());
	}
}
