using System.Collections.Generic;
using Godot;
using LifeAtomGameDemo;

public interface IElement
{
    string Name { get; } // 元素名稱，例如火焰、毒素
    void ApplyEffect(IBubble me, Bubble targetBubble, Node parent, Vector2 midpoint); // 對泡泡產生的效果
}

// 一般元素實作
public class NormalElement : IElement
{
    public string Name => "Normal";

    public void ApplyEffect(IBubble me, Bubble bubble, Node parent, Vector2 midpoint)
    {
        // 一般效果：減少速度
        //bubble.Level = bubble.Level > 1 ? bubble.Level - 1 : 1; // 等級最低為 1
    }
}

// 火焰元素實作
public class FireElement : IElement
{
    public string Name => "Fire";

    public void ApplyEffect(IBubble me, Bubble bubble, Node parent, Vector2 midpoint)
    {
        // 火焰效果：增加等級或特定屬性
        bubble.Level += 1; // 增加泡泡等級
        GD.Print("作用Fire");
        
        var explosion = new Explosion
        {
            Position = midpoint
        };

        parent.AddChild(explosion);
        
    }
}

// 融合元素實作
public class FusionElement : IElement
{
    public string Name => "Fusion";

    public void ApplyEffect(IBubble me, Bubble bubble, Node parent, Vector2 midpoint)
    {
        // 融合效果：
        var tempScale = (me.Size + bubble.Size) / 1.5f;
        bubble.Die();
        me.Die();

        parent.GetNode<BubbleManager>("BubbleManager").CreateBubble(midpoint, tempScale);
    }
}


// 黏合元素實作
public class TapeElement : IElement
{
    public string Name => "TapeE";

    public void ApplyEffect(IBubble me, Bubble bubble, Node parent, Vector2 midpoint)
    {
        // 黏合效果：位置貼合+延遲性同生共死
        // todo
        // 互相加速度
        me.TapeEffect(bubble, midpoint);
        
        me.RevengeTarget = bubble;
        bubble.RevengeTarget = me;
    }
}

// 障礙元素
public class DeathElement : IElement
{
    public string Name => "Death";
    
    public void ApplyEffect(IBubble me, Bubble bubble, Node parent, Vector2 midpoint)
    {
        // 障礙元素效果：即死
        bubble.Die();
    }
    
}