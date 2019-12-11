using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManageIntro : MonoBehaviour
{
    public void IntroBack()
    {
        SceneManager.LoadScene("firstScene");
    }
}
