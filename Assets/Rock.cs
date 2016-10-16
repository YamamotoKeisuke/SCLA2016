using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour
{
    public static Vector3 setPos;
    GameObject refObj2;


    void Start()
    {
        refObj2 = GameObject.Find("unitychan");
        setPos = new Vector3(19.5f, 4f, 0);
    }

    void Update()
    {
        Debug.Log(transform.position.x);
        unitychan c2 = refObj2.GetComponent<unitychan>();

        if (c2.transform.position.x >= unitychan.setPos2.x && Input.GetKey(KeyCode.UpArrow) 
            && transform.position.y < setPos.y)
        {
                transform.position += new Vector3(0f,1f * Time.deltaTime, 0f);

        }
    }
}
    