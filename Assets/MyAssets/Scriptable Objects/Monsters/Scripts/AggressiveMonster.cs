using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Aggressive Monster", menuName = "Inventory System/Monsters/AggressiveMonster")]

public class AggressiveMonster : MonsterObject
{
    private void Awake()
    {
        type = MonsterType.Aggressive;
    }
}
