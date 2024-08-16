using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyModelScript : MonoBehaviour
{
    private Animator anim;
    private bool activate1, activate2, activate3;
    // Start is called before the first frame update
    void Start()
    {
        activate1 = false;
        activate2 = false;
        activate3 = false;
        anim = GetComponent<Animator>();
    }


    void Update()
    {
      
        HandleInputs();

    }
    private void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            
            activate1 = !activate1;
            activate2 = false;
            activate3 = false;
            anim.SetBool("press4", activate1);
            anim.SetBool("press5", activate2);
            anim.SetBool("press6", activate3);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            activate2 = !activate2;
            activate1 = false;
            activate3 = false;
            anim.SetBool("press5", activate2);
            anim.SetBool("press4", activate1);
            anim.SetBool("press6", activate3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            activate3 = !activate3;
            activate2 = false;
            activate1 = false;
            anim.SetBool("press6", activate3);
            anim.SetBool("press4", activate1);
            anim.SetBool("press5", activate2);
        }
    }

}
