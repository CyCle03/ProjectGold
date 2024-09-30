using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Neutral Monster", menuName = "Inventory System/Monsters/NeutralMonster")]

public class NeutralMonster : MonsterObject
{
    private void Awake()
    {
        type = MonsterType.Neutral;
    }
}
