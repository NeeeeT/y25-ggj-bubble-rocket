using Godot;

public partial class GameManager : Node
{
	[Export] public int MaxBubbleCount = BubbleConfig.MaxBubbleCount; // 最大泡泡數量
	//public static int CurrentBubbleCount = 0; // 當前泡泡數量
}
