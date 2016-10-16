using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class buttonScript : MonoBehaviour
{

    public void ButtonPush()
    {
        SceneManager.LoadScene("GameOver");
        Debug.Log("Button Push !!");
    }
}