using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Passive Monster", menuName = "Inventory System/Monsters/PassiveMonster")]

public class PassiveiveMonster : MonsterObject
{
    private void Awake()
    {
        type = MonsterType.Passive;
    }
}
