using Godot;
using System;

public partial class ShooterCore : Node2D
{
	private BubbleShooter _bubbleShooter;
	
	// Called when the node enters the scene tree for the first time.
	public void TEST(Vector2 position)
	{
		GD.Print("成功呼叫C#腳本: "+position);
		_bubbleShooter.ShootBubble(position);
	}

	public override void _Draw()
	{
		
		DrawCircle(Vector2.Zero, 121, new Color(0.3f, 0.3f, 0, 0.5f)); // 半透明紅色圓形
	}

	public override void _Ready()
	{ 
		_bubbleShooter = GetNode<BubbleShooter>("BubbleShooter");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
