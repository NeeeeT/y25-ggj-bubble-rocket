using Godot;
using System;

public partial class LevelController : Node
{
	GameManager gm;
	[Export] public float LevelTime = 60;
	double countdown = 60;
	bool isStart = false;
	[Export] Label timeText;
	[Export] ResultView resultView;

	[Export] AudioStreamPlayer2D bgm;
	[Export] AudioStreamPlayer2D endSound;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gm = (GameManager)GetParent();
		// timeText = (Label)GetNode("TimeText");
		resultView.returnTitleBtn.Pressed += ReturnTitle;
		resultView.nextLevelBtn.Pressed += StartNextLevel;
		resultView.retryLevelBtn.Pressed += RetryLevel;

		StartLevel();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (isStart)
		{
			countdown -= delta;
			timeText.Text = countdown.ToString();
			if (countdown <= 0){
				GD.Print("Time's up");
				SetLose();
			}
		}
	}

	private void ReturnTitle(){
		GD.Print("ReturnTitle");
		HideLevelUI();
		gm.ReturnToTitle();
	}

	private void StartNextLevel(){
		GD.Print("StartNextLevel");
		HideLevelUI();
		gm.StartNextLevel();
	}

	private void RetryLevel(){
		GD.Print("RetryLevel");
		HideLevelUI();
		gm.RetryLevel();
	}

	public void StartLevel()
	{
		countdown = LevelTime;
		isStart = true;
	}

	public void SetWin()
	{
		isStart = false;
		GD.Print("Win!");
		
		// gm.StartNextLevel();
		// show win panel
		resultView.ShowWinView();

		bgm.Stop();
		endSound.Play();
	}

	public void SetLose()
	{
		isStart = false;
		GD.Print("Lose!");
		// show lose panel
		resultView.ShowLoseView();

		bgm.Stop();
		endSound.Play();
	}

	public void HitDanger()
	{
		SetLose();
	}

	public void HitGoal()
	{
		SetWin();
	}

	public void HideLevelUI(){
		timeText.Visible = false;
		resultView.HideView();
	}
}
