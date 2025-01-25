using Godot;
using System;

public partial class Explosion : Node2D
{
    [Export] public float Radius = 100.0f; // 爆炸半徑
    [Export] public float Force = 5000.0f; // 爆炸推力
    [Export] public float Duration = 0.5f; // 爆炸持續時間（秒）

    private Timer _timer;

    public override void _Ready()
    {
        // 初始化計時器
        _timer = new Timer();
        _timer.WaitTime = Duration;
        _timer.OneShot = true;
        _timer.Connect("timeout", new Callable(this, nameof(OnExplosionTimeout)));
        AddChild(_timer);
        
        _timer.Start();

        // 執行爆炸效果
        ApplyExplosionForce();
    }
    public override void _Draw()
    {
        // 繪製爆炸範圍（用於 Debug）
        DrawCircle(Vector2.Zero, Radius, new Color(1, 0, 0, 0.5f)); // 半透明紅色圓形
    }

    private void ApplyExplosionForce()
    {
        // 獲取所有在爆炸範圍內的物體
        var area2D = new Area2D();
        var collisionShape = new CollisionShape2D
        {
            Shape = new CircleShape2D { Radius = Radius }
        };
        area2D.AddChild(collisionShape);
        AddChild(area2D);

        area2D.Monitoring = true;
        area2D.Monitorable = true;

        // 搜索範圍內的剛體並應用推力
        foreach (var body in area2D.GetOverlappingBodies())
        {
            if (body is RigidBody2D rigidBody)
            {
                Vector2 direction = (rigidBody.GlobalPosition - GlobalPosition).Normalized();
                rigidBody.ApplyImpulse(Vector2.Zero, direction * Force);
            }
        }

        // 清理範圍節點
        RemoveChild(area2D);
        area2D.QueueFree();
    }

    private void OnExplosionTimeout()
    {
        QueueFree(); // 自動銷毀節點
    }

    public static void TriggerExplosion(Node parent, Vector2 position, float radius, float force, float duration)
    {
        var explosion = new Explosion
        {
            Position = position,
            Radius = radius,
            Force = force,
            Duration = duration
        };

        parent.AddChild(explosion);
    }
}