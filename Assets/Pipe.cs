using UnityEngine;
using System.Collections;

public class Pipe : MonoBehaviour
{
    GameObject refObj2;
    bool flag2;


    void Start()
    {
        refObj2 = GameObject.Find("unitychan");
        flag2 = true;
    }

    void Update()
    {
        Debug.Log(transform.position.x);
        unitychan c2 = refObj2.GetComponent<unitychan>();

        if (c2.transform.position.x >= unitychan.setPos.x && flag2 == true)
        {
            transform.position = new Vector3(39.5f, 5f, 0f);

            flag2 = false;
            Debug.Log("tekito");
        }

    }
}
