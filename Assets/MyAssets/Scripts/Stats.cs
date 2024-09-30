using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{
    public string unitName;
    public int unitID;
    public float maxHP;
    public float curruntHP;
    public float attackPower;
    public float moveSpeed;
    public float attackSpeed;
    public float attackRange;
    public GameObject unitObj;

    public Stats(string unitName, int unitID, float maxHP, float attackPower, float moveSpeed, float attackSpeed, float attackRange)
    {
        this.unitName = unitName;
        this.unitID = unitID;
        this.maxHP = maxHP;
        this.curruntHP = maxHP;
        this.attackPower = attackPower;
        this.moveSpeed = moveSpeed;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
    }

    // 유닛의 상태 출력
    public void DisplayStats()
    {
        Debug.Log($"Name: {unitName}\nID: {unitID}\nHealth: {maxHP}\nAttack Power: {attackPower}\nSpeed: {moveSpeed}\nAttack Speed: {attackSpeed}\nAttack Range: {attackRange}\n");
    }
}

