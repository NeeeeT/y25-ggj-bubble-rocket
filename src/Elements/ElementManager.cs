using System.Collections.Generic;
using Godot;

namespace LifeAtomGameDemo.Elements;

// 元素管理類
public class ElementManager
{
    private IBubble m_bubble = null;
    private readonly List<IElement> _elements = new List<IElement>();

    public void init(IBubble bubble)
    {
        m_bubble = bubble;
    }
    
    // 添加元素
    public void AddElement(IElement element)
    {
        _elements.Add(element);
        if(element is FireElement)
            m_bubble._Modulate = new Color(1, 0, 0); // RGB 為 (1, 0, 0)，即紅色
    }

    // 移除元素
    public void RemoveElement(IElement element)
    {
        _elements.Remove(element);
    }

    // 應用所有元素效果
    public void ApplyEffects(Bubble targetBubble,Node parent)
    {        
        // 計算兩個泡泡位置的中點
        var midpoint = (m_bubble.Position + targetBubble.Position) / 2;

        foreach (var element in _elements)
        {
            element.ApplyEffect(targetBubble, parent, midpoint);
        }
    }
}
