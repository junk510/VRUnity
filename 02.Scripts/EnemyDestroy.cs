using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroy : MonoBehaviour
{
    private EnemyHp death;
    void Start()
    {
        death = GameObject.Find("Enemy").GetComponent<EnemyHp>();
    }

    void Update()
    {
        if(death.HPBar.value <= 0)
        {
            Destroy(gameObject);
        }
    }
}
