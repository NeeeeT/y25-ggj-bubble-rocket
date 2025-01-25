using System.Collections.Generic;

namespace LifeAtomGameDemo.Elements;

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
