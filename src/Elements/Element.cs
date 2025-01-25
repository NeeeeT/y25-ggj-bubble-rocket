using System.Collections.Generic;
using Godot;
using LifeAtomGameDemo;

public interface IElement
{
    string Name { get; } // 元素名稱，例如火焰、毒素
    void ApplyEffect(Bubble bubble, Node parent, Vector2 midpoint); // 對泡泡產生的效果
}

// 火焰元素實作
public class FireElement : IElement
{
    public string Name => "Fire";

    public void ApplyEffect(Bubble bubble, Node parent, Vector2 midpoint)
    {
        // 火焰效果：增加等級或特定屬性
        bubble.Level += 1; // 增加泡泡等級
        GD.Print("作用Fire");
        
        var explosion = new Explosion
        {
            Position = midpoint,
            Radius = 100,
            Force = 500,
            Duration = 0.5f
        };

        parent.AddChild(explosion);
        
    }
}

// 毒素元素實作
public class PoisonElement : IElement
{
    public string Name => "Poison";

    public void ApplyEffect(Bubble bubble, Node parent, Vector2 midpoint)
    {
        // 毒素效果：減少等級或特定屬性
        bubble.Level = bubble.Level > 1 ? bubble.Level - 1 : 1; // 等級最低為 1
    }
}