using Godot;
using System;
using LifeAtomGameDemo;

public partial class BubbleShooter : Node2D
{
    [Export] public PackedScene BubbleScene; // 泡泡場景
    [Export] public NodePath BubbleManagerPath; // 泡泡管理器節點的路徑

    private BubbleManager _bubbleManager;

    public override void _Ready()
    {
        if (BubbleScene == null)
            throw new NullReferenceException("BubbleScene cannot be null");

        _bubbleManager = GetNode<BubbleManager>(BubbleManagerPath);
        if (_bubbleManager == null)
            throw new NullReferenceException("BubbleManager cannot be null");
    }

    public override void _Input(InputEvent @event)
    {
        // if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left &&
        //     mouseEvent.Pressed)
        // {
        //     // 轉換滑鼠點擊位置到 Node2D 的本地座標
        //     Vector2 targetPosition = GetGlobalMousePosition();

        //     ShootBubble(Vector2.Zero, targetPosition);


        //     GD.Print("射擊" + mouseEvent.Position);
        // }
    }

    public void ShootBubble(Vector2 StartPosition,Vector2 targetPosition, String element = "")
    {
        // 生成泡泡
        var bubble = _bubbleManager.CreateBubble(StartPosition, BubbleConfig.SizeScaleBase);
        //GD.Print("泡泡出現位置: "+ Position);
        if (bubble == null)
        {
            GD.Print("泡泡射出失敗");
            return;
        }

        var direction = (targetPosition -StartPosition).Normalized();
        GD.Print("目標位置: "+ targetPosition);
        GD.Print("起點位置: "+ StartPosition);
        
        // 設置泡泡的速度
        bubble.LinearVelocity = direction * 200; // 射向點擊位置，速度為 200

        bubble.ElementManager.init(bubble);

        switch (element)
        {
            case "Normal":
            {
                bubble.ElementManager.AddElement(new NormalElement()); // 賦予屬性: 一般 
                break;
            }
            case "Fire":
            {
                bubble.ElementManager.AddElement(new FireElement()); // 賦予屬性: 火
                break;
            }
            case "Fusion":
            {
                bubble.ElementManager.AddElement(new FusionElement()); // 賦予屬性: 融合
                break;
            }
            case "Tape":
            {
                bubble.ElementManager.AddElement(new TapeElement()); // 賦予屬性: 黏合
                break;
            }
            case "Death":
            {
                bubble.ElementManager.AddElement(new DeathElement()); // 賦予屬性: 障礙物
                break;
            }
            case "1":
            {
                bubble.ElementManager.AddElement(new NormalElement()); 
                break;
            }
            case "2":
            {
                bubble.ElementManager.AddElement(new FireElement()); 
                bubble.ElementManager.AddElement(new NormalElement());  
                break;
            }
            case "3":
            {
                bubble.ElementManager.AddElement(new FireElement());  
                bubble.ElementManager.AddElement(new TapeElement());
                break;
            }

            default:
            {
                break;
            }
        }
    }
}