using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicalInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cameraObj;
    public HudManager hud;
    void Start()
    {
        cameraObj.SetActive(false);
    }

    // Update is called once per frame
    public void InitializeMechanical(GameObject trig,GameObject player,bool F)
    {
        cameraObj.SetActive(true);
        player.GetComponent<Car>().SetCanMove(false);
        

        player.transform.position = trig.transform.position;
        hud.EnableMechanic(trig,cameraObj,player,F);
    }
    

}
