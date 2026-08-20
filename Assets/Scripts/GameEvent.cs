using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvent
{
    public static Action<GameObject, GameObject, float> Attack; //(attacker, target, damage)
    public static Action<float> PlayerHeal; 
    public static Action<GameObject, float, float> HealthChanged; //(target, currentHealth, maxHealth)
}
