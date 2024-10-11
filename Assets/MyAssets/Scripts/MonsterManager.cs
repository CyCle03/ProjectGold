using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

[System.Serializable]
public class MonsterLevel
{
    public GameObject[] monsters;
}

public class MonsterManager : MonoBehaviour
{
    public GameObject[] m_RegenPoints;
    public MonsterLevel[] s_Monster;
    public MonsterLevel[] m_Prefabs;

    public int[] spawnRange;
    public int[] spawnLimit;
    public float[] regenTimes;
    
    float[] currentRegenTimes;
    int[] spawnCnt;
    int[] mobs;
    bool[] canSpawnMonster;

    private void Awake()
    {
        var m_levelType = m_RegenPoints.Length;
        mobs = new int[m_levelType];
        currentRegenTimes = new float[m_levelType];
        spawnCnt = new int[m_levelType];
        canSpawnMonster = new bool[m_levelType];

        for (int i = 0; i < m_levelType; i++)
        {
            mobs[i] = m_Prefabs[i].monsters.Length;
            spawnCnt[i] = 0;
            s_Monster[i].monsters = new GameObject[spawnLimit[i]];

            for (int j = 0; j < spawnLimit[i]; j++)
            {
                GameObject instMob = m_Prefabs[i].monsters[Random.Range(0, mobs[i])];
                Vector3 spawnPoint = m_RegenPoints[i].transform.position;
                float pointX = Random.Range(spawnPoint.x - spawnRange[i], spawnPoint.x + spawnRange[i]);
                float pointZ = Random.Range(spawnPoint.z - spawnRange[i], spawnPoint.z + spawnRange[i]);
                Vector3 instPos = new Vector3(pointX, 0, pointZ);
                s_Monster[i].monsters[j] = Instantiate(instMob, instPos, Quaternion.identity);
                MonsterMove mMove = s_Monster[i].monsters[j].GetComponent<MonsterMove>();
                mMove.m_level = i;
                mMove.indexOfPool = j;
                spawnCnt[i]++;
            }
            currentRegenTimes[i] = regenTimes[i];
            canSpawnMonster[i] = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawnMonster[0])
        {
            if (currentRegenTimes[0] <= 0)
            { SpawnMonster(0); }
            else { currentRegenTimes[0] -= Time.deltaTime; }
        }
        if (canSpawnMonster[1])
        {
            if (currentRegenTimes[1] <= 0)
            { SpawnMonster(1); }
            else { currentRegenTimes[1] -= Time.deltaTime; }
        }
        if (canSpawnMonster[2])
        {
            if (currentRegenTimes[2] <= 0)
            { SpawnMonster(2); }
            else { currentRegenTimes[2] -= Time.deltaTime; }
        }
    }

    public void SpawnMonster(int _level)
    {
        GameObject instMob = m_Prefabs[_level].monsters[Random.Range(0, mobs[_level])];
        Vector3 spawnPoint = m_RegenPoints[_level].transform.position;
        float pointX = Random.Range(spawnPoint.x - spawnRange[_level], spawnPoint.x + spawnRange[_level]);
        float pointZ = Random.Range(spawnPoint.z - spawnRange[_level], spawnPoint.z + spawnRange[_level]);
        Vector3 instPos = new Vector3(pointX, 0, pointZ);

        for (int i = 0; i < spawnLimit[_level]; i++)
        {
            if (s_Monster[_level].monsters[i] == null)
            {
                GameObject tempSpawnObj = Instantiate(instMob, instPos, Quaternion.identity);
                s_Monster[_level].monsters[i] = tempSpawnObj;
                MonsterMove mMove = s_Monster[_level].monsters[i].GetComponent<MonsterMove>();
                mMove.m_level = _level;
                mMove.indexOfPool = i;
                spawnCnt[_level]++;
                currentRegenTimes[_level] = regenTimes[_level];
                if (spawnCnt[_level] < spawnLimit[_level])
                {
                    canSpawnMonster[_level] = true;
                }
                break;
            }
        }
    }

    public void DespawnMonster(int _level, int _index)
    {
        s_Monster[_level].monsters[_index] = null;
        spawnCnt[_level]--;
        canSpawnMonster[_level] = true;
    }
}
