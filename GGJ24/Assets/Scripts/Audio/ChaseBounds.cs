using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBounds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
        GameObject[] npcsFemale = GameObject.FindGameObjectsWithTag("NPCFemale");

        foreach (var npc in npcs)
        {
            if (Vector3.Distance(transform.position, npc.transform.position) < 30)
            {

                
            }
            else
            {

                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("ChaseState", 0); //idle

            }
        }
        foreach (var npcFemale in npcsFemale)
        {
            if (Vector3.Distance(transform.position, npcFemale.transform.position) < 30)
            {

                
            }
            else
            {
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("ChaseState", 0); //idle

            }
        }


    }

    IEnumerator MusicTimer()
    {
        
        yield return new WaitForSeconds(3);
        Destroy(gameObject);

    }
}
