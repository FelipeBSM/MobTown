using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimsController : MonoBehaviour
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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activate1 = !activate1;
            activate2 = false;
            activate3 = false;
            anim.SetBool("isAnim01", activate1);
            anim.SetBool("isAnim02", activate2);
            anim.SetBool("isAnim03", activate3);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            activate2 = !activate2;
            activate1 = false;
            activate3 = false;
            anim.SetBool("isAnim02", activate2);
            anim.SetBool("isAnim01", activate1);
            anim.SetBool("isAnim03", activate3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            activate3 = !activate3;
            activate2 = false;
            activate1 = false;
            anim.SetBool("isAnim03", activate3);
            anim.SetBool("isAnim01", activate1);
            anim.SetBool("isAnim02", activate2);
        }
    }
   
}
