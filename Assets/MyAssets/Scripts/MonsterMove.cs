using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMove : MonoBehaviour
{
    public Monster myMonster;
    public float monsterSight = 100;
    public MonsterObject monsterObj;
    public Player player;

    //MonsterManager mManager;
    CharacterController cc;
    NavMeshAgent monsterNav;
    Transform targetTransform;

    float atkCdw = 0;

    enum MonsterState
    {
        Idle,
        Move,
        Attack,
        Die,
        Ready
    }

    MonsterState m_State = MonsterState.Idle;

    // Start is called before the first frame update
    void Start()
    {
        //uManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        myMonster = new Monster(monsterObj);
        monsterNav = gameObject.GetComponent<NavMeshAgent>();
        cc = gameObject.GetComponent<CharacterController>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_State)
        {
            case MonsterState.Idle:
                break;
            case MonsterState.Move:
                MoveMonster();
                break;
            case MonsterState.Attack:
                AttackMonster();
                break;
            case MonsterState.Die:
                break;
        }
        if (atkCdw > 0)
        {
            atkCdw -= Time.deltaTime;
        }

    }

    public void GetDamaged(float hitPower)
    {
        print(myMonster.mName + " Hit");
        myMonster.curruntHP -= hitPower;
        if (m_State == MonsterState.Die)
        {
            return;
        }
        else if (myMonster.curruntHP <= 0)
        {
            m_State = MonsterState.Die;
            DieMonster();
        }
        else if (m_State == MonsterState.Idle)
        {
            m_State = MonsterState.Move;
        }
        StartCoroutine(Hitted());
    }

    IEnumerator Hitted()
    {
        yield return new WaitForSeconds(1.0f);
    }

    public void MoveMonster()
    {
        if (targetTransform == null)
        {
            StartCoroutine(MoveToTarget());
        }
    }

    IEnumerator MoveToTarget()
    {
        yield return null;
        targetTransform = SearchTarget();
        if (targetTransform != null)
        {
            if (Vector3.Distance(transform.position, targetTransform.position) > myMonster.attackRange)
            {
                monsterNav.isStopped = true;
                monsterNav.ResetPath();

                monsterNav.stoppingDistance = myMonster.attackRange;
                monsterNav.destination = targetTransform.position;
            }
            else
            {
                m_State = MonsterState.Attack;
            }
        }
        else
        {
            m_State = MonsterState.Idle;
        }
    }

    public void AttackMonster()
    {
        if (Vector3.Distance(transform.position, targetTransform.position) < myMonster.attackRange)
        {
            if (atkCdw <= 0)
            {
                print(myMonster.mName + " Attack");
                targetTransform.GetComponentInParent<Player>().GetDamaged(myMonster.attackPower);
                atkCdw = 1 / myMonster.attackSpeed;
            }
            else
            {
                m_State = MonsterState.Move;
            }
        }
    }

    public void DieMonster()
    {
        print(myMonster.mName + " Die");
        StopAllCoroutines();

        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        cc.enabled = false;

        player.UpdateEXP(myMonster.mExp);

        yield return new WaitForSeconds(1f);
        print("¼Ò¸ê");
        Destroy(gameObject);
    }

    public Transform SearchTarget()
    {
        targetTransform = null;
        float targetDistance = monsterSight;//Mathf.Infinity;
        int targetLayer = 10;

        Collider[] cols = Physics.OverlapSphere(transform.position, monsterSight, 1 << targetLayer);

        foreach (Collider col in cols)
        {
            float distance = Vector3.Distance(transform.position, col.transform.position);
            if (distance < targetDistance)
            {
                targetTransform = col.transform;
                targetDistance = distance;
            }
        }
        return targetTransform;
    }
}
