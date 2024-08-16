using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassagerSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] passagers;
    public GameObject[] spawns;
    public List<GameObject> passagersInGameList = new List<GameObject>();
 
    public void SpawnOnePassager()
    {
        int random = Random.Range(0, passagers.Length);
        int randomSpawns = Random.Range(0, spawns.Length);
        GameObject obj = Instantiate(passagers[random],
            spawns[randomSpawns].transform.position,Quaternion.identity);
        passagersInGameList.Add(obj);
        
        Debug.LogError(obj.name);

    }
}
