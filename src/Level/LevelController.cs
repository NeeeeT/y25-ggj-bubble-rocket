using Godot;
using System;

public partial class LevelController : Node
{
	GameManager gm;
	[Export] public float LevelTime = 60;
	double countdown = 60;
	bool isStart = false;
	int bubbleShootTypeTemp = 0;
	[Export] Control gameUI;
	[Export] Label timeText;
	[Export] TextureRect normalBubbleIcon;
	[Export] TextureRect fireBubbleIcon;
	[Export] ResultView resultView;

	[Export] AudioStreamPlayer2D endSound;

	bool isBGM2 = false;
	bool isBGM3 = false;

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
			timeText.Text = Math.Floor(countdown).ToString();
			if (countdown <= 0){
				GD.Print("Time's up");
				SetLose();
			}
			if (countdown < LevelTime / 3f){
				if (!isBGM3) gm.TriggerChangeBGM(2);
			}
			else if (countdown < LevelTime / 3f * 2f){
				if (!isBGM2) gm.TriggerChangeBGM(1);
			}

		}
		UpdateBubbleTypeDisplay();
	}

	private void UpdateBubbleTypeDisplay(){
		if (Input.IsActionJustPressed("SwitchBubbleUp")){
			bubbleShootTypeTemp = Math.Abs(bubbleShootTypeTemp + 1) % 2;
		}
		if (Input.IsActionJustPressed("SwitchBubbleDown")){
			bubbleShootTypeTemp = Math.Abs(bubbleShootTypeTemp - 1) % 2;
		}
		normalBubbleIcon.Visible = bubbleShootTypeTemp == 0;
		fireBubbleIcon.Visible = bubbleShootTypeTemp == 1;
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
		gameUI.Visible = true;
		countdown = LevelTime;
		isStart = true;
	}

	public void SetWin()
	{
		gameUI.Visible = false;
		if (!isStart) return; 
		isStart = false;
		GD.Print("Win!");
		
		// gm.StartNextLevel();
		// show win panel
		if (gm.IsFinalLevel){
			resultView.ShowFinalWinView();
		}
		else{
			resultView.ShowWinView();
		}

		endSound.Play();
	}

	public void SetLose()
	{
		gameUI.Visible = false;
		if (!isStart) return; 
		isStart = false;
		GD.Print("Lose!");
		// show lose panel
		resultView.ShowLoseView();

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
