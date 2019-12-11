using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class EnemyMovement : MonoBehaviourPun
{
    private int HP = 100;
    //몬스터 HP 저장하는 변수
    private float MoveSpeed = 4.0f;
    //몬스터 이동 속도를 저장하는 변수
    private float DistanceToPlayer;
    //몬스터와 플레이어 사이의 거리를 저장할 변수
    private Transform target;
    private GameObject[] targets;
    //private Transform targetstr;
    private PhotonView photonView;
    private Player gameObject;
    private Animator anim;
    private bool a = false;
    int num;
    int ran;
    private EnemyHp enm;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            A();
            //targets = GameObject.FindGameObjectWithTag("Player").transform;
            //Player한테 따라오도록 타겟지정
            anim = GetComponent<Animator>();
            enm = GetComponent<EnemyHp>();
        }
    }
    private void A()
    {

        targets = GameObject.FindGameObjectsWithTag("Player");
        num = targets.Length;

        if (num >= 2)
        {
            num = num - 1;
            ran = Random.Range(0, num);
        }
        else if (num == 1)
        {
            num = 0;
            ran = num;
        }
        target = targets[ran].GetComponent<Transform>();
        a = true;
        Invoke("A", 6f);
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if(enm.HPBar.value <= 0)
        {
            transform.position = transform.position;
            return;
        }

        if (a == true)
            DistanceToPlayer = Vector3.Distance(transform.position, target.position);
        //적과 플레이어 사이의 거리
        if (DistanceToPlayer > 2.5f) //거리가 2.5f보다 클때 적이 플레이어를 향해 움직이도록
        {
            Move();
        }
        else //거리가 2.5f 이하일때 적이 플레이어를 공격하도록 설정
        {
            Attack();
        }
    }

    void Move()
    {
        transform.LookAt(target.position);
        //적이 플레이어를 바라본다
        transform.position += transform.forward * MoveSpeed * Time.deltaTime;
        //적이 MoveSpeed의 값에 따라 플레이어를 향해 이동하도록 설정
        anim.SetInteger("EnemyState", 1);
        //EnemyState 1애니메이션 동작 -> Walk
    }

    void Attack()
    {
        anim.SetInteger("EnemyState", 2);
        //EnemyState 2애니메이션 동작 -> Attack
        if (DistanceToPlayer > 2.5f) //플레이어가 거리를 벌릴 때 거리가 2.5f보다 클때 적이 플레이어를 향해 움직이도록
        {
            anim.SetInteger("EnemyState", 1);  //EnemyState 1애니메이션 동작 -> Walk
        }
    }
}
