using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHp : MonoBehaviour
{
    public Slider HPBar;
    public GameObject HeadUPPosition;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        //HPBar.value = HP;
        //HP = HPBAR 100으로 설정
    }

    void Update()
    {
        HPBar.transform.position = HeadUPPosition.transform.position;
        //HPBar를 적 위에다가 이미지 표시되게 설정

        if(HPBar.value <= 0)    //HPBar가 0이면 적이 파괴되도록 설정
        {
            StartCoroutine(EnemyDeadRou());
        }
    }

    IEnumerator EnemyDeadRou()
    {
        anim.SetTrigger("die");
        yield return new WaitForSeconds(2.8f);
        Destroy(gameObject);
    }

    public void DamageUpdate()
    {
        HPBar.value -= 20;
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Player"))   //플레이어의 검에 닿으면 HPVar가 10씩 달도록 설정
        {
            DamageUpdate();
        }
    }

}
