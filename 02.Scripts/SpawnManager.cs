using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviourPun
{
    private bool enableSpawn = true;
    private GameObject Enemy;    //Prefab을 받을 enemy
    private int MonsterCount = 0;
    private int MonsterMaxCount = 3;
    bool tryd = false;

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Enemy = Resources.Load("Enemy", typeof(GameObject)) as GameObject;
            InvokeRepeating("SpawnEnemy", 3, 1);
            //3초후 부터, SpawnEnemy함수를 2초마다 반복해서 실행 시킵니다.
        }
    }

    void SpawnEnemy()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            float randomX = Random.Range(-0.5f, 0.5f); //적이 나타날 X좌표를 랜덤으로 생성해 줍니다.

            if (enableSpawn)
            {
                GameObject enemy = PhotonNetwork.Instantiate(Enemy.name, new Vector3(randomX, 1.1f, 0f), Quaternion.identity);
                //랜덤한 위치와, 화면 제일 위에서 Enemy를 하나 생성해줍니다.
                MonsterCount++;
                if (MonsterCount == MonsterMaxCount)
                {
                    enableSpawn = false;    //monstercount가 maxcount랑 같아지면 더이상 스폰되지 않도록 설정
                }
            }
        }
    }
}
