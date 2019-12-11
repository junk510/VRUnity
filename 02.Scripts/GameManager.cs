using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    private GameObject playerPrefab;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    void Start()
    {
        playerPrefab = Resources.Load("Player", typeof(GameObject)) as GameObject;
        // 생성할 랜덤 위치 지정
        Vector3 randomSpawnPos = Random.insideUnitSphere * 5f;
        // 위치 값은 0.3으로 변경
        randomSpawnPos.y = 0.3f;
        //네트워크상의 모든 클라이언트에서 생성 실행
        //해당 게임 오브젝트의 주도권은 생성 메서드를 직접 실행한 클라이언트에 있음
        PhotonNetwork.Instantiate(playerPrefab.name, randomSpawnPos, Quaternion.identity);
    }

    void Update()
    {
        //키보드 입력을 감지하고 룸을 나가게 함
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    //룸을 나갈 때 자동 실행되는 메서드
    public override void OnLeftRoom()
    {
        //룸을 나가면 로비 씬으로 돌아감.
        SceneManager.LoadScene("firstScene");
    }
}
