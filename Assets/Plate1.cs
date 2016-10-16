using UnityEngine;
using System.Collections;

public class Plate1 : MonoBehaviour
{
    public static Vector3 setPos;
    GameObject refObj1;


    void Start()
    {
        setPos = new Vector3(6.5f, 0.25f, 0);
        refObj1 = GameObject.Find("unitychan");
        
    }

    void Update()
    {
        Debug.Log(transform.position.x);
        unitychan c1 = refObj1.GetComponent<unitychan>();


            if (c1.transform.position.x >= unitychan.setPos.x && Input.GetKey(KeyCode.DownArrow)
            && transform.position.z > setPos.z)
            {
                transform.position += new Vector3(0f, 0f, -1f * Time.deltaTime);
            
            }

    }
}
