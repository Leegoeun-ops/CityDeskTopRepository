using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtrl3_C : MonoBehaviour
{
    private Transform monsterTr;
    private Transform playerTr;
    private Transform towerTr;
    private UnityEngine.AI.NavMeshAgent nvAgent;
    private Animator animator;

    public float traceDist = 7.0f;
    public float attackDist = 2.0f;
    private bool isDie = false;

    private int hp = 100;
    void Start()
    {
        monsterTr = this.gameObject.GetComponent<Transform>();
        playerTr = GameObject.Find("StopPoint_C").GetComponent<Transform>();
        towerTr = GameObject.Find("StopPoint3_C").GetComponent<Transform>();


        nvAgent = this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(towerTr.position, monsterTr.position);
        float dist2 = Vector3.Distance(playerTr.position, monsterTr.position);
        
        if (dist2 <= traceDist)
        {
            nvAgent.destination = playerTr.position;
            transform.LookAt(playerTr);

        }
        else
        {
            nvAgent.destination = towerTr.position;
            
            
        }
    }

    public void GetDamage(float amount)
    {
        hp -= (int)(amount);
        //animator.SetTrigger("IsHit");

        if (hp <= 0)
        {
            MonsterDie();
        }

    }

    void MonsterDie()
    {
        if (isDie == true) return;//return

        //StopAllCoroutines();
        isDie = true;
        //monsterState = MonsterState.die;
        nvAgent.isStopped = true;
        //animator.SetTrigger("IsDie");

        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;
        foreach (Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = false;
        }
        Destroy(this.gameObject);
    }
}
