using Godot;
using System;

public partial class TitleView : Control
{
	[Export] public Button startBtn;
	[Export] public Button exitBtn;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// startBtn.Connect();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
