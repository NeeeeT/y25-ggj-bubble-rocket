using System;
using Godot;
using LifeAtomGameDemo.Elements;

namespace LifeAtomGameDemo;

public interface IBubble
{
	// 屬性
	float Size { get; set; } // 泡泡大小
	float Weight { get; set; } // 泡泡重量
	int Level { get; set; } // 泡泡等級
	float CooldownTime { get; set; } // 冷卻時間
	float LevelGrowthRate { get; set; } // 等級成長速率 (每秒)
	

	ElementManager ElementManager { get; set; }
	Vector2 Position { get; set; }
	Color _Modulate { set; }
	float VelocityFactor { get; set; }

	// 方法
	void Split(); // 分裂泡泡
	void Die(); // 刪除泡泡
	void HandleCollision(IBubble other); // 處理碰撞
	void UpdateSize(); // 根據等級調整大小
	void TapeEffect(Bubble bubble, Vector2 midpoint);
}
