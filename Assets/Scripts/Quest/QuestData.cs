using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "ScriptableObjects/QuestData", order = 2)]
public class QuestData : ScriptableObject
{
    public int targetAmount = 0;
    public int LocationIndex = 0;
    public string targetName = "";
    public string displayTargetName = "";
}