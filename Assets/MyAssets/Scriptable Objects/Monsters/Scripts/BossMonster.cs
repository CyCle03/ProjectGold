using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Boss Monster", menuName = "Inventory System/Monsters/BossMonster")]

public class BossMonster : MonsterObject
{
    private void Awake()
    {
        type = MonsterType.Boss;
    }
}