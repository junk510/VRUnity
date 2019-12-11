using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;   //유니티용 포톤 컴포넌트
using Photon.Realtime;  //포톤 서비스 관련 라이브러리
using UnityEngine.UI;

public class ButtonManagefirst : MonoBehaviourPunCallbacks  //monobehaviour를 확장한 클래스
{
    private string gameVersion = "1";
    public Text connectionInfoText;
    public Button startButton;

    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        //접속에 필요한 정보(게임 버전) 설정
        PhotonNetwork.ConnectUsingSettings();
        //설정한 정보로 마스터 서버 접속 시도
        startButton.interactable = false;
        //룸 접속 버튼 잠시 비활성화
        connectionInfoText.text = "Battle Royal RPG 서버에 접속중.....";
        //접속 시도 중임을 텍스트로 표시
    }

    public override void OnConnectedToMaster() //접속 성공
    {
        //룸 접속 버튼 활성화
        startButton.interactable = true;
        //접속 정보 표시
        connectionInfoText.text = "온라인 : Battle Royal RPG 서버와 연결됨";
    }

    public override void OnDisconnected(DisconnectCause cause) //접속 실패
    {
        //룸 접속 버튼 비활성화
        startButton.interactable = false;
        //접속 정보 표시
        connectionInfoText.text = "오프라인 : Battle Royal RPG 서버와 연결되지 않음\n접속 재시도 중...";

        //마스터 서버로의 재접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect() //Game Start 버튼을 클릭했을때 실행할 메서드
    {
        //중복 접속 시도를 막기 위해 접속 버튼 잠시 비활성화
        startButton.interactable = false;

        //마스터 서버에 접속 중이라면
        if(PhotonNetwork.IsConnected)
        {
            //룸 접속 실행
            connectionInfoText.text = "룸에 접속...";
            PhotonNetwork.JoinRandomRoom();
        }

        else
        {
            //마스터 서버에 접속 중이 아니라면 마스터 서버에 접속 시도
            connectionInfoText.text = "오프라인 : Battle Royal RPG 서버와 연결되지 않음\n 접속 재시도 중...";
            //마스터 서버로의 재접속 시도
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)   //랜덤 룸 접속에 실패한 경우 자동 실행
    {
        //접속 상태 표시
        connectionInfoText.text = "빈 방이 없음, 새로운 방 생성...";
        //최대 4명을 수용 가능한 빈 방 생성
        PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = 4});
    }

    public override void OnJoinedRoom() //룸 참가에 성공한 경우 자동 실행
    {
        //접속 상태 표시
        connectionInfoText.text = "방 참가 성공";
        //모든 룸 참가자가 PlayerEnemyUIPart 씬을 로드하게 함
        PhotonNetwork.LoadLevel("PlayerEnemyUIPart");
        //SceneManager.LoadScene()은 이전 씬의 모든 게임 오브젝트를 삭제하고 다음씬을 로드하므로
        //로비 씬의 네트워크 정보가 유지되지 않는다. -> 다른 사람의 캐릭터가 보이지 않는다.
    }

    //public void Intro()
    //{
    //    SceneManager.LoadScene("introScene");
    //}

    //public void Help()
    //{
    //    SceneManager.LoadScene("helpScene");
    //}
}
