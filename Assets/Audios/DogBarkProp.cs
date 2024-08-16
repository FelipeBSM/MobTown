using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBarkProp : MonoBehaviour
{
    // Start is called before the first frame update

    bool playitOnce = true;
    // Update is called once per frame
    void Update()
    {
        float percentageChance = 0.6f;
        if (Random.value < percentageChance && playitOnce ==true)
        {
            this.gameObject.GetComponent<AudioSource>().Play();
            playitOnce = false;
            StartCoroutine(BackNPlay());
        }
    }
    IEnumerator BackNPlay()
    {
        float rand = Random.Range(3.5f, 15.5f);
        yield return new WaitForSeconds(rand);
        playitOnce = true;
    }
}
