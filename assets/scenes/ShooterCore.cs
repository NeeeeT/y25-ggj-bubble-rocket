using Godot;
using System;
using System.Security.Principal;

public partial class ShooterCore : Node2D
{
	private BubbleShooter _bubbleShooter;
	public int BubbleTypeIndex { get; private set; } = 0;
	private const int bubbleTypeCount = 2;

	[Export] public AudioStreamPlayer2D bubbleSwitch;
	public void ShootByIndex( Vector2 startPosition, Vector2 position, string index)
	{
		_bubbleShooter.ShootBubble(startPosition, startPosition + position, index);
	}
	
	public void SwtichBubbleUp(){
		BubbleTypeIndex = Math.Abs(BubbleTypeIndex + 1) % bubbleTypeCount;
		bubbleSwitch.Play();
	}

	public void SwtichBubbleDown(){
		BubbleTypeIndex = Math.Abs(BubbleTypeIndex - 1) % bubbleTypeCount;
		bubbleSwitch.Play();
	}

	public void ShootBySwitch( Vector2 startPosition, Vector2 position)
	{
		// _bubbleShooter.ShootBubble(startPosition, startPosition + position, index);
		string type = BubbleTypeIndex switch{
			1 => "Fire",
			_ => "Normal",
		};
		GD.Print($"Shoot :{type}");
		_bubbleShooter.ShootBubble(startPosition, startPosition + position, type);
	}

	// Called when the node enters the scene tree for the first time.
	public void TEST( Vector2 startPosition, Vector2 position)
	{
		// _bubbleShooter.ShootBubble(startPosition, position);
		_bubbleShooter.ShootBubble(startPosition, startPosition + position, "Normal");
	}
	
	public void TEST_Fire( Vector2 startPosition, Vector2 position)
	{
		_bubbleShooter.ShootBubble(startPosition, startPosition + position, "Fire");
	}
	
	public void TEST_Fusion( Vector2 startPosition, Vector2 position)
	{
		_bubbleShooter.ShootBubble(startPosition, startPosition + position,"Fusion");
	}
	
	public void TEST_Tape( Vector2 startPosition, Vector2 position)
	{
		_bubbleShooter.ShootBubble(startPosition, startPosition + position,"Tape");
	}
	
	public void TEST_Death( Vector2 startPosition, Vector2 position)
	{
		_bubbleShooter.ShootBubble(startPosition, startPosition + position, "Death");
	}

	public override void _Draw()
	{
		
		//DrawCircle(Vector2.Zero, 121, new Color(0.3f, 0.3f, 0, 0.5f)); // 半透明紅色圓形
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
