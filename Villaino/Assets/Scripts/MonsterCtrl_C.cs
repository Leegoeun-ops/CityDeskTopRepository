using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Timers;

public class MonsterCtrl_C : MonoBehaviour
{
    public enum MonsterState { idle, trace, attack, die };
    public MonsterState monsterState = MonsterState.idle;
    private Transform monsterTr;
    private Transform playerTr;
    private Transform towerTr;
    private UnityEngine.AI.NavMeshAgent nvAgent;
    private Animator animator;

    public float playerTraceDist = 15.0f;
    public float towerTraceDist = 20.0f;
    public float attackDist = 2.0f;
    private bool isDie = false;

    private int hp = 100;
    private int currentTime;


    void Start()
    {
        currentTime = 0;
        monsterTr = this.gameObject.GetComponent<Transform>();
        playerTr = GameObject.Find("StopPoint_C").GetComponent<Transform>();
        towerTr = GameObject.Find("Tower Mage").GetComponent<Transform>();

        nvAgent = this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = this.gameObject.GetComponent<Animator>();

        StartCoroutine(this.CheckMonsterState());
        StartCoroutine(this.MonsterAction());

    }

    void Update()
    {

        

        float monsterTowerDist = Vector3.Distance(monsterTr.position, towerTr.position);
        //float playerMonsterDist = Vector3.Distance(playerTr.position, monsterTr.position);
        //float playerTowerDist = Vector3.Distance(playerTr.position, towerTr.position);

        //if (playerMonsterDist <= towerTraceDist && playerMonsterDist <= monsterTowerDist)
        //{
        //    nvAgent.destination = playerTr.position;
        //    transform.LookAt(playerTr);


        //}

        //else
        //{
        //    nvAgent.destination = towerTr.position;
        //    transform.LookAt(towerTr);
        //}

        nvAgent.destination = towerTr.position;
        transform.LookAt(towerTr);
    }

    IEnumerator CheckMonsterState()
    {
        while(!isDie)
        {
            yield return new WaitForSeconds(0.2f);//슬립사용.yield return null.
            float monsterTowerDist = Vector3.Distance(monsterTr.position, towerTr.position);
            float playerMonsterDist = Vector3.Distance(playerTr.position, monsterTr.position);
            float playerTowerDist = Vector3.Distance(playerTr.position, towerTr.position);

            if (playerMonsterDist > attackDist && monsterTowerDist > attackDist)///playerMonsterDist <= towerTraceDist && playerMonsterDist <= monsterTowerDist
            {
                monsterState = MonsterState.trace;
                Debug.Log("trace"+ monsterTowerDist);
            }
            //else if (playerTowerDist <= attackDist && !FindObjectOfType<GameManager>().isGameOver)
            //{
            //    monsterState = MonsterState.attack;
            //}
            //else if(monsterTowerDist <= attackDist || playerMonsterDist <= attackDist)
            //{
            //    monsterState = MonsterState.attack;
            //    Debug.Log("attack");
            //}
            
            else
            {
                //monsterState = MonsterState.idle;
                monsterState = MonsterState.attack;
                Debug.Log("attack");
            }

        }
    }

    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (monsterState)
            {
                case MonsterState.idle:
                    nvAgent.isStopped = true;
                    animator.SetBool("Walk", false);
                    break;
                case MonsterState.trace:
                    nvAgent.isStopped = false;
                    animator.SetBool("Walk", true);
                    animator.SetBool("Shoot", false);

                    break;
                case MonsterState.attack:
                    nvAgent.isStopped = true;
                    //animator.SetBool("Walk", false);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PUNCH")
        {
            animator.SetTrigger("New Trigger");
            Debug.Log("punch");
        }
        else if (other.tag == "MONSTER")
        {
            animator.SetBool("Die", true);
            StartCoroutine(MyEvent());
            
        }

        IEnumerator MyEvent()
        {
            while (true)
            {
                yield return new WaitForSeconds(3f); // wait half a second
                Destroy(this.gameObject);                                 // do things
            }
        }
    }

}
