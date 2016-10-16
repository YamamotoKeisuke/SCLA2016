using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class UnityChanAnimatorcontroller : MonoBehaviour
{

    private Animator animator;
    private int doWalkId;

    GameObject refObj3;
    GameObject refObj4;


    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        doWalkId = Animator.StringToHash("Do Walk");

        refObj3 = GameObject.Find("unitychan");
        refObj4 = GameObject.Find("Plate1");

        
    }

    // Update is called once per frame
    void Update()
    {
        unitychan c3 = refObj3.GetComponent<unitychan>();
        Plate1 c4 = refObj4.GetComponent<Plate1>();

        animator.SetBool(doWalkId, true);
        Debug.Log("true");

        if(c3.transform.position.x < unitychan.setPos.x)
        {
            c3.transform.position += new Vector3(6f * Time.deltaTime, 0f, 0f);
        }

        else if(c3.transform.position.x >= unitychan.setPos.x && c4.transform.position.z > 0)
        {
            animator.SetBool(doWalkId, false);
        }

        else if (c4.transform.position.z <= 0 && c3.transform.position.x < 18f)
        {
            animator.SetBool(doWalkId, true);
            Debug.Log("true");
            c3.transform.position += new Vector3(6f * Time.deltaTime, 0f, 0f);

        }

        else if(c3.transform.position.x >= 18f)
        {
            
            animator.SetBool(doWalkId, false);
            Debug.Log("false");
        }


    }
}
