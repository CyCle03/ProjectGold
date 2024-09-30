using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Peaceful Monster", menuName = "Inventory System/Monsters/PeacefulMonster")]

public class PeacefulMonster : MonsterObject
{
    private void Awake()
    {
        type = MonsterType.Peaceful;
    }
}
