using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class asd : MonoBehaviour
{
    public enum MonsterState { idle, trace, attack, die };
    public MonsterState monsterState = MonsterState.idle;
    private Transform monsterTr;
    private Transform playerTr;
    private Transform towerTr;
    private UnityEngine.AI.NavMeshAgent nvAgent;
    private Animator animator;
    public bool traceFlag = false;
    public float playerTraceDist = 7.0f;
    public float towerTraceDist = 14.0f;
    public float attackDist = 2.0f;
    private bool isDie = false;

    private int hp = 100;


    // Start is called before the first frame update
    void Start()
    {
        monsterTr = this.gameObject.GetComponent<Transform>();
        playerTr = GameObject.Find("StopPoint_C").GetComponent<Transform>();
        towerTr = GameObject.Find("StopPoint1_C").GetComponent<Transform>();


        nvAgent = this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = this.gameObject.GetComponent<Animator>();

        //StartCoroutine(this.CheckMonsterState());
        //StartCoroutine(this.MonsterAction());

    }

    // Update is called once per frame
    void Update()
    {
        

        float monsterTowerDist = Vector3.Distance(monsterTr.position, towerTr.position);
        float playerMonsterDist = Vector3.Distance(playerTr.position, monsterTr.position);
        float playerTowerDist = Vector3.Distance(playerTr.position, towerTr.position);

        if (playerTowerDist <= playerTraceDist && playerMonsterDist < monsterTowerDist)
        {
            nvAgent.destination = playerTr.position;
            transform.LookAt(playerTr);
            nvAgent.isStopped = false;
            traceFlag = true;
            Debug.Log("playerTr");

        }
        else if (playerTraceDist< playerMonsterDist && playerMonsterDist <= towerTraceDist)
        {
            nvAgent.destination = towerTr.position;
            transform.LookAt(towerTr);
            nvAgent.isStopped = false;
            traceFlag = true;
            Debug.Log("towerTr"+ playerTowerDist);
        }

        else
        {
            nvAgent.isStopped = true;
            Debug.Log(playerTowerDist);
        }

    }

    IEnumerator CheckMonsterState()
    {
        while(!isDie)
        {
            yield return new WaitForSeconds(0.2f);//슬립사용.yield return null.
            float monsterTowerDist = Vector3.Distance(monsterTr.position, towerTr.position);
            float playerMonsterDist = Vector3.Distance(playerTr.position, monsterTr.position);
            float playerTowerDist = Vector3.Distance(playerTr.position, towerTr.position);

            if (playerTowerDist > attackDist)
            {
                monsterState = MonsterState.trace;
                Debug.Log("trace");
            }
            //else if (playerTowerDist <= attackDist && !FindObjectOfType<GameManager>().isGameOver)
            //{
            //    monsterState = MonsterState.attack;
            //}
            else if(playerTowerDist <= attackDist){
                monsterState = MonsterState.attack;
                Debug.Log("attack");
            }
            else
            {
                //nvAgent.isStopped = true;
                Debug.Log("idle");
            }

        }
    }

    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (monsterState)
            {
                //case MonsterState.idle:
                //    nvAgent.isStopped = true;
                //    animator.SetBool("Walk", false);
                //    break;
                case MonsterState.trace:
                    nvAgent.isStopped = false;
                    animator.SetBool("Walk", true);
                    animator.SetBool("Shoot", false);
                    break;
                case MonsterState.attack:
                    animator.SetBool("Shoot", true);
                    break;
            }
            yield return null;
        }
    }

    public void GetDamage(float amount)
    {
        hp -= (int)(amount);
        animator.SetBool("Get Hit", true);

        if (hp <= 0)
        {
            MonsterDie();
        }

    }

    void MonsterDie()
    {
        if (isDie == true) return;//return

        StopAllCoroutines();
        isDie = true;
        monsterState = MonsterState.die;
        nvAgent.isStopped = true;
        animator.SetBool("Die", true);

        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;
        foreach(Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = false;
        }
        Destroy(this.gameObject);
    }
}
