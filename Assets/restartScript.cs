using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class restartScript : MonoBehaviour
{

    public void ButtonPush()
    {
        SceneManager.LoadScene("StartScene");
        Debug.Log("Button Push !!");
    }
}