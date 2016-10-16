using UnityEngine;
using System.Collections;

public class unitychan : MonoBehaviour
{

    public Vector3 position;
    public static Vector3 setPos;
    public static Vector3 setPos2;
    GameObject refObj2;


    void Start()
    {
        position = transform.position;
        setPos = new Vector3(5.5f, 0.5f, 0);
        setPos2 = new Vector3(18f, 1.2f, 1.2f);
        refObj2 = GameObject.Find("Plate1");
    }


    void Update()
    {
    }
}
