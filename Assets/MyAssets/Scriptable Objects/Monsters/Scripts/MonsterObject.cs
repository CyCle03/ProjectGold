using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    Peaceful,
    Aggressive,
    Passive,
    Neutral,
    Boss
}

[CreateAssetMenu(fileName = "New Monster", menuName = "Inventory System/Monsters/Monster")]
public class MonsterObject : ScriptableObject
{

    public GameObject monsterObj;
    public MonsterType type;
    [TextArea(15, 20)]
    public string description;

    public Monster data = new Monster();

    public Monster CreateMonster()
    {
        Monster newMonster = new Monster(this);
        return newMonster;
    }

    // 유닛의 상태 출력
    
}

[System.Serializable]
public class Monster
{
    public string mName;
    public int mId = -1;
    public float maxHP;
    public float curruntHP;
    public float attackPower;
    public float moveSpeed;
    public float attackSpeed;
    public float attackRange;
    public int mExp;

    public Monster()
    {
        mName = "";
        mId = -1;
    }

    public Monster(MonsterObject monster)
    {
        mName = monster.name;
        mId = monster.data.mId;
        maxHP = monster.data.maxHP;
        curruntHP = maxHP;
        attackPower = monster.data.attackPower;
        moveSpeed = monster.data.moveSpeed;
        attackSpeed = monster.data.attackSpeed;
        attackRange = monster.data.attackRange;
        mExp = monster.data.mExp;
    }

    public void DisplayStats()
    {
        Debug.Log($"Name: {mName}\nID: {mId}\nHealth: {maxHP}\nAttack Power: {attackPower}\nSpeed: {moveSpeed}\nAttack Speed: {attackSpeed}\nAttack Range: {attackRange}\nExp: {mExp}");
    }
}