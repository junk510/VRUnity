using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public Slider HPBar;
    public GameObject HeadUPPosition;

    private void Start()
    {
        //HPBar.value = HP;
        //HP = HPBAR 100으로 설정
    }

    void Update()
    {
        HPBar.transform.position = HeadUPPosition.transform.position;
        //HPBar를 적 위에다가 이미지 표시되게 설정
        if (HPBar.value <= 0)
        {
            Destroy(gameObject);
            return;
        }
    }


    public void DamageUpdate()
    {
        HPBar.value -= 1;
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Enemy"))   //플레이어의 검에 닿으면 HPVar가 10씩 달도록 설정
        {
            DamageUpdate();
        }
    }
}
