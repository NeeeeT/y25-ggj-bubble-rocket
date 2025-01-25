using System.Collections.Generic;

public interface IElement
{
    string Name { get; } // 元素名稱，例如火焰、毒素
    void ApplyEffect(Bubble bubble); // 對泡泡產生的效果
}

// 火焰元素實作
public class FireElement : IElement
{
    public string Name => "Fire";

    public void ApplyEffect(Bubble bubble)
    {
        // 火焰效果：增加等級或特定屬性
        bubble.Level += 1; // 增加泡泡等級
    }
}

// 毒素元素實作
public class PoisonElement : IElement
{
    public string Name => "Poison";

    public void ApplyEffect(Bubble bubble)
    {
        // 毒素效果：減少等級或特定屬性
        bubble.Level = bubble.Level > 1 ? bubble.Level - 1 : 1; // 等級最低為 1
    }
}

// 元素管理類
public class ElementManager
{
    private readonly List<IElement> _elements = new List<IElement>();

    // 添加元素
    public void AddElement(IElement element)
    {
        _elements.Add(element);
    }

    // 移除元素
    public void RemoveElement(IElement element)
    {
        _elements.Remove(element);
    }

    // 應用所有元素效果
    public void ApplyEffects(Bubble bubble)
    {
        foreach (var element in _elements)
        {
            element.ApplyEffect(bubble);
        }
    }
}