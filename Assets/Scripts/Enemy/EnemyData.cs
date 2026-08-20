using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeEnemy
{
    Normal,
    TreeMonster,
    Boss
}
[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    public TypeEnemy type;
    public float maxHealth;
    public float moveSpeed;
    public float damage;
}
