namespace LifeAtomGameDemo;

using Godot;
using System.Collections.Generic;

public partial class BubbleManager : Node
{
	[Export] public PackedScene BubbleScene { get; set; } // 泡泡場景
	[Export] public PackedScene FireBubbleScene { get; set; } // 泡泡場景
	[Export] public int MaxBubbleCount { get; set; } = BubbleConfig.MaxBubbleCount; // 最大泡泡數量

	private int _currentBubbleCount => _bubbles.Count;
	public int CurrentBubbleCount => _currentBubbleCount; // 當前泡泡數量
	public bool CanSplite { get; set; } = false;

	private List<Bubble> _bubbles = new List<Bubble>();

	public override void _Process(double delta)
	{
		//GD.Print("泡泡num: "+ _currentBubbleCount);
	}

	// 生成泡泡
	public Bubble CreateBubble(Vector2 position, float size, IElement element=null)
	{
		if (_currentBubbleCount >= MaxBubbleCount)
		{
			GD.Print("Cannot create more bubbles, limit reached.");
			return null;
		}

		var newBubble = (Bubble)BubbleScene.Instantiate();
		newBubble.Position = position;
		newBubble.Size = size;
		newBubble.Weight = size * BubbleConfig.DensityFactor;

		AddChild(newBubble); // 添加到場景樹
		_bubbles.Add(newBubble);

		// 連接刪除信號
		newBubble.OnBubbleDestroyed += HandleBubbleDestroyed;

		if (element != null)
		{
			newBubble.ElementManager.init(newBubble);
			newBubble.ElementManager.AddElement(element);
		}
		
		return newBubble;
	}

		public Bubble CreateFireBubble(Vector2 position, float size, IElement element=null)
	{
		if (_currentBubbleCount >= MaxBubbleCount)
		{
			GD.Print("Cannot create more bubbles, limit reached.");
			return null;
		}

		var newBubble = (Bubble)FireBubbleScene.Instantiate();
		newBubble.Position = position;
		newBubble.Size = size;
		newBubble.Weight = size * BubbleConfig.DensityFactor;

		AddChild(newBubble); // 添加到場景樹
		_bubbles.Add(newBubble);

		// 連接刪除信號
		newBubble.OnBubbleDestroyed += HandleBubbleDestroyed;

		if (element != null)
		{
			newBubble.ElementManager.init(newBubble);
			newBubble.ElementManager.AddElement(element);
		}
		
		return newBubble;
	}

	// 泡泡分裂
	public void SplitBubble(Bubble bubble)
	{
		if (!bubble.canBeSplit)
		{
			GD.Print("泡泡等級不足以分裂");
			return;
		}
		
		if (_currentBubbleCount + 2 > MaxBubbleCount) {
			GD.Print("Cannot split, bubble limit reached.");
			return;
		}

		// 創建兩個子泡泡
		for (int i = 0; i < 2; i++)
		{
			var newBubble = CreateBubble(bubble.Position, bubble.Size);

			// 延遲設置子泡泡屬性
			CallDeferred(nameof(InitializeBubble), newBubble, bubble, i);
		}

		// 延遲刪除原泡泡
		_bubbles.Remove(bubble);
		bubble.Die();
	}

	private void InitializeBubble(Bubble newBubble, Bubble parentBubble,int i)
	{
		newBubble.Position = parentBubble.Position + new Vector2((i == 0 ? -1 : 1) * parentBubble.Size / 2, 0);
		newBubble.Size = parentBubble.Size / 2;
		newBubble.Weight = parentBubble.Weight / 2;

		// 設置初始速度
		newBubble.LinearVelocity = new Vector2(
			(i == 0 ? -1 : 1) * BubbleConfig.MaxRandomVelocity,
			GD.Randf() * BubbleConfig.MaxRandomVelocity - BubbleConfig.MinRandomVelocity);
	}

	// 處理泡泡刪除
	private void HandleBubbleDestroyed(Bubble bubble)
	{
		_bubbles.Remove(bubble);
		bubble.QueueFree();
	}

	// 清除所有泡泡
	public void ClearAllBubbles()
	{
		foreach (var bubble in _bubbles)
		{
			bubble.QueueFree();
		}
		_bubbles.Clear();
	}
}
