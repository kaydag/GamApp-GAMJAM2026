using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvent
{
    public static Action<GameObject, GameObject, float> Attack; //(attacker, target, damage)
    public static Action<float> PlayerHeal; 
    // thêm biến bool để phân biệt hồi máu hay nhận damage, player bị hurt hay enemy hurt
    public static Action<GameObject, float, float, bool, bool> HealthChanged; //(target, currentHealth, maxHealth)
    public static Action<BaseEnemy> EnemyDie;
}
