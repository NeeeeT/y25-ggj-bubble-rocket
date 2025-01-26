using Godot;
using System;

public partial class ResultView : Control
{
	[Export] Label winTitle;
	[Export] Label loseTitle;
	[Export] public Button returnTitleBtn;
	[Export] public Button nextLevelBtn;
	[Export] public Button retryLevelBtn;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ShowWinView()
	{
		Visible = true;
		winTitle.Visible = true;
		loseTitle.Visible = false;
		returnTitleBtn.Visible = true;
		nextLevelBtn.Visible = true;
		retryLevelBtn.Visible = false;
	}

	public void ShowFinalWinView()
	{
		Visible = true;
		winTitle.Visible = true;
		loseTitle.Visible = false;
		returnTitleBtn.Visible = true;
		nextLevelBtn.Visible = false;
		retryLevelBtn.Visible = false;
	}

	public void ShowLoseView()
	{
		Visible = true;
		winTitle.Visible = false;
		loseTitle.Visible = true;
		returnTitleBtn.Visible = true;
		nextLevelBtn.Visible = false;
		retryLevelBtn.Visible = true;
	}

	public void HideView(){
		Visible = false;
	}
}
