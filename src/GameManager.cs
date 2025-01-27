using System.Collections.Generic;
using Godot;

public partial class GameManager : Node
{
	[Export] public PackedScene[] levels;
	[Export] public int currentLevelId = 0;
	public bool IsFinalLevel => currentLevelId + 1 == levels.Length;
	[Export] public TitleView titleView;

	private LevelController currentLevelController = null;
	[Export] public PackedScene explosionScene;
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
		LoadLevel(currentLevelId);

		titleView.Visible = false;
	}

	public void StartNextLevel()
	{
		GD.Print("StartNextLevel");
		// reset player
		// reset current level
		if (currentLevelId == levels.Length){
			GD.Print("Reach end level");
			return;
		} 
		currentLevelId++;
		LoadLevel(currentLevelId);
	}

	public void RetryLevel()
	{
		GD.Print("RetryLevel");
		LoadLevel(currentLevelId);
	}

	private void SetBGMVolume(double val1, double val2, double val3){
		MusicPlayer musicPlayer = (MusicPlayer) GetNode("/root/MusicPlayer");
		double[] vals = new double[] { val1, val2, val3 };
		musicPlayer.SetSections(vals);
	}

	private void LoadLevel(int levelIndex)
	{
		
		// LevelController levelInstance = levels[levelIndex].Instantiate();
		// currentLevelController = levelInstance;
		SetBGMVolume(1.0, 0, 0);
		if (currentLevelController != null){
			currentLevelController.QueueFree();
		}

		var levelInstance = (LevelController)levels[levelIndex].Instantiate();
		AddChild(levelInstance);
		currentLevelController = levelInstance;
		// Node levelRoot = levelInstance.GetNode("LevelRoot");
		// if (levelRoot is LevelController)
		// {
		//     currentLevelController = levelRoot as LevelController;
		//     GD.Print($"Find level [{levelIndex}] controller");
		// }
	}

	public void TriggerChangeBGM(int index)
	{
		switch(index){
			case 0: SetBGMVolume(1.0, 0, 0); break;
			case 1: SetBGMVolume(0, 1.0, 0); break;
			case 2: SetBGMVolume(0, 0, 1.0); break;
		}
	}

	public void ReturnToTitle()
	{
		titleView.Visible = true;
	}

	public void ExitGame(){
		GetTree().Quit();
	}
}
