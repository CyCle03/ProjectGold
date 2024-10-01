using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MonsterMove : MonoBehaviour
{
    public Monster myMonster;
    public float monsterSight = 30;
    public MonsterObject monsterObj;
    public Player player;
    public Slider hpSlider;
    public GameObject groundItem;

    //MonsterManager mManager;
    CharacterController cc;
    NavMeshAgent monsterNav;
    //Transform targetTransform;
    Transform playerTransform;
    Vector3 originPos;
    ItemObject dropItem;

    float atkCdw = 0;

    enum MonsterState
    {
        Idle,
        Move,
        Attack,
        Die,
        Damaged,
        Return
    }

    MonsterState m_State = MonsterState.Idle;

    // Start is called before the first frame update
    void Start()
    {
        //uManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        myMonster = new Monster(monsterObj);
        dropItem = myMonster.dropItem;
        monsterNav = gameObject.GetComponent<NavMeshAgent>();
        cc = gameObject.GetComponent<CharacterController>();
        player = GameObject.Find("Player").GetComponent<Player>();
        hpSlider.value = (float)myMonster.curruntHP / (float)myMonster.maxHP;
        playerTransform = player.GetComponentInParent<Transform>();
        originPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_State)
        {
            case MonsterState.Idle:
                IdleMonster();
                break;
            case MonsterState.Move:
                MoveMonster();
                break;
            case MonsterState.Attack:
                AttackMonster();
                break;
            case MonsterState.Return:
                ReturnMonster();
                break;
            case MonsterState.Damaged:
                DamagedMonster();
                break;
            case MonsterState.Die:
                break;
        }
    }

    public void GetDamaged(float hitPower)
    {
        if (m_State == MonsterState.Damaged || m_State == MonsterState.Die)
        {
            return;
        }
        print(myMonster.mName + " Hit");
        myMonster.curruntHP -= hitPower;
        UpdateHPbar();
        if (myMonster.curruntHP <= 0)
        {
            print(myMonster.mName + " State: AnyState -> Die");
            m_State = MonsterState.Die;
            DieMonster();
        }
        else if (m_State == MonsterState.Idle)
        {
            print(myMonster.mName + " State: Idle -> Damaged");
            m_State = MonsterState.Damaged;
        }
    }

    void IdleMonster()
    {
        if (monsterObj.type == MonsterType.Aggressive || monsterObj.type == MonsterType.Boss)
        {
            if (Vector3.Distance(transform.position, playerTransform.position) < monsterSight)
            {
                m_State = MonsterState.Move;
                print(myMonster.mName + " State: Idle -> Move");
            }
        }
    }

    void MoveMonster()
    {
        if (playerTransform != null)
        {
            playerTransform = player.GetComponentInParent<Transform>();
            if (Vector3.Distance(transform.position, originPos) > monsterSight)
            {
                m_State = MonsterState.Return;
                print(myMonster.mName + " State: Move -> Return");
            }
            else if (Vector3.Distance(transform.position, playerTransform.position) > myMonster.attackRange)
            {
                Vector3 dir = (playerTransform.position - transform.position).normalized;
                cc.Move(dir * myMonster.moveSpeed * Time.deltaTime);
            }
            else
            {
                m_State = MonsterState.Attack;
                print(myMonster.mName + " State: Move -> Attack");
                atkCdw = 0;
            }
        }
        else
        {
            m_State = MonsterState.Idle;
            print(myMonster.mName + " State: Move -> Idle");
        }
        
    }

/*    IEnumerator MoveToTarget()
    {
        //targetTransform = SearchTarget();
        atkCdw = 0;
        if (playerTransform != null)
        {
            if (Vector3.Distance(transform.position, playerTransform.position) > myMonster.attackRange)
            {
                monsterNav.isStopped = true;
                monsterNav.ResetPath();

                monsterNav.stoppingDistance = myMonster.attackRange;
                monsterNav.destination = playerTransform.position;
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
        yield return null;
    }*/

    void AttackMonster()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) < myMonster.attackRange)
        {
            atkCdw -= Time.deltaTime;
            if (atkCdw <= 0)
            {
                print(myMonster.mName + " Attack");
                player.GetDamaged(myMonster.attackPower);
                atkCdw = 1 / myMonster.attackSpeed;
            }
        }
        else
        {
            m_State = MonsterState.Move;
            print(myMonster.mName + " State: Attack -> Move");
            atkCdw = 1 / myMonster.attackSpeed;
        }
    }

    void ReturnMonster()
    {
        if (myMonster.curruntHP < myMonster.maxHP)
        {
            myMonster.curruntHP += myMonster.curruntHP * Time.deltaTime;
            UpdateHPbar();
        }
        else
        {
            myMonster.curruntHP = myMonster.maxHP;
            UpdateHPbar();
        }
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            cc.Move(dir * myMonster.moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = originPos;
            myMonster.curruntHP = myMonster.maxHP;
            UpdateHPbar();
            m_State = MonsterState.Idle;
            print(myMonster.mName + " State: Return -> Idle");
        }
    }

    void DamagedMonster()
    {
        StartCoroutine(DelayedMonster());
    }

    IEnumerator DelayedMonster()
    {
        yield return new WaitForSeconds(0.5f);
        m_State = MonsterState.Move;
    }

    void DieMonster()
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

        if (dropItem != null)
        {
            groundItem.GetComponent<GroundItem>().item = dropItem;
            GameObject droped = Instantiate(groundItem);
            droped.transform.position = transform.position;
        }
        print("¼Ò¸ê");
        Destroy(gameObject);
    }

    public void UpdateHPbar()
    {
        hpSlider.value = (float)myMonster.curruntHP / (float)myMonster.maxHP;
    }

/*    public Transform SearchTarget()
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
    }*/
}
