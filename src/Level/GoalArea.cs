using Godot;
using System;

public partial class GoalArea : Area2D
{
	[Export] public LevelController controller;

	public override void _Ready()
	{
		// 連接 body_entered 信號
		Connect("body_entered", new Callable(this, "_OnBodyEntered"));
	}

	private void _OnBodyEntered(Node body)
	{
		if (body.Name == "RocketBody") // 檢查是否是玩家
		{
			GD.Print("Player entered the area!");
			controller.HitGoal();
		}
	}
}
