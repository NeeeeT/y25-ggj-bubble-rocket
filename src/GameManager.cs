using System.Collections.Generic;
using Godot;

public partial class GameManager : Node
{
	[Export] public PackedScene[] levels;
	[Export] public int currentLevelId = 0;

	[Export] public TitleView titleView;


	public override void _Ready()
	{
		// register ui event
		titleView.startBtn.Pressed += StartNewGame;
		titleView.exitBtn.Pressed += ExitGame;


	}

	public override void _Process(double delta)
	{
		base._Process(delta);
	}

	public void StartNewGame()
	{
		GD.Print("StartNewGame");
		// reset player

		// reset current level
		currentLevelId = 0;
	}

	public void StartNextLevel()
	{
		// reset player

		// reset current level
		currentLevelId++;

	}

	public void ReturnToTitle()
	{

	}

	public void ExitGame(){
		GetTree().Quit();
	}
}
