using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

//플레이어 캐릭터를 조작하기 위한 사용자 입력을 감지
//감지된 입력값을 다른 컴포넌트가 사용할 수 있도록 제공
public class PlayerMovement : MonoBehaviourPun, Idamageable
{
    private float moveSpeed = 5.0f;
    private float rotSpeed = 3.0f;

    private Animator anim;

    bool isHit = false; //bool 값 설정

    private AudioSource AudioPlayer;
    public AudioClip WalkSound; //걷는 사운드
    public AudioClip SwordSound; //소드 사운드


    void Start()
    {
        anim = GetComponent<Animator>();
        AudioPlayer = GetComponent<AudioSource>();
        if (photonView.IsMine)
        {
            Transform camera = this.transform.Find("Main Camera");
            camera.gameObject.SetActive(true);
        }
    }

    // 매 프레임 사용자 입력을 감지
    void FixedUpdate()
    {
        //로컬 플레이어가 아닌 경우 입력을 받지 않음
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            Debug.Log("실패");
            return;
        }
        //게임오버 상태에서는 사용자 입력을 감지하지 않음
        /*
         * 아직 안만듬
         */
            Debug.Log("성공");
            MoveCtrl(); //움직이게 컨트롤
            RotCtrl();  //카메라 화면 돌아가게 컨트롤
            OnHit();    //플레이어가 때리는걸 컨트롤
            OnBlock();  //플레이어가 막는걸 컨트롤
    }
    //playerstate = 0 : Idle 1: walk 2: attack 3: block ->animator
    void MoveCtrl()
    {
        if (!isHit)
        {
            AudioPlayer.clip = WalkSound; //오디오 플레이어 소스에 걷는소리 클립을 연결
            if (Input.GetKey(KeyCode.W))
            {
                this.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                //플레이어가 앞으로 이동
                anim.SetInteger("PlayerState", 1);
            }
            if (Input.GetKey(KeyCode.S))
            {
                this.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
                //플레이어가 뒤로 이동
                anim.SetInteger("PlayerState", 1);
            }
            if (Input.GetKey(KeyCode.A))
            {
                this.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
                anim.SetInteger("PlayerState", 1);
            }
            if (Input.GetKey(KeyCode.D))
            {
                this.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                anim.SetInteger("PlayerState", 1);
            }
            PlayCtrl();
        }
    }

    void PlayCtrl()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S))
        {
            AudioPlayer.Play();
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.S))
        {
            anim.SetInteger("PlayerState", 0);
            AudioPlayer.Stop();
        }
    }

    void RotCtrl()
    {
        float rotY = Input.GetAxis("Mouse X") * rotSpeed;

        this.transform.localRotation *= Quaternion.Euler(0, rotY, 0);
        //마우스 X축 좌우로 rotY 기준으로 회전 할 수 있게 설정
    }

    private void OnHit() //플레이어 일반공격
    {
        if (Input.GetMouseButtonDown(0)) //왼쪽 마우스 클릭을 누르면 공격하게 끔 설정
        {
            isHit = true;   //공격하는 중간에 움직일 수 없게
            AudioPlayer.Stop(); //걷는 소리 스톱
            AudioPlayer.clip = SwordSound;  //walkSound를 swordsound로 바꿔서 설정
            StartCoroutine(OnHitRoutine());
        }
    }

    IEnumerator OnHitRoutine() //공격 쓰레드 추가
    {
        anim.SetInteger("PlayerState", 2);
        yield return new WaitForSeconds(0.4f);
        AudioPlayer.Play();
        yield return new WaitForSeconds(0.6f);  //오디오 소리가 칼 휘두를때 맞춰서 소리낼수 있도록 설정
        AudioPlayer.Stop();
        anim.SetInteger("PlayerState", 0);
        isHit = false;  //공격이 끝나고 움직일 수 있게 설정
    }

    private void OnBlock()
    {
        if (Input.GetMouseButtonDown(1)) //왼쪽 마우스 클릭을 누르면 
        {
            isHit = true;   //방어 할때 움직일 수 없도록 설정
            anim.SetInteger("PlayerState", 3);
        }
        if (Input.GetMouseButtonUp(1))
        {
            anim.SetInteger("PlayerState", 0);
            isHit = false;  //방어가 끝나고 움직일 수 있게 설정
        }
    }

}
