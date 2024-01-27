using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{

    //[FMODUnity.EventRef]
    //FMODUnity.EventReference;
    public string atmos = "event:/Atmos/Atmos";
    //public string ambient = "event:/Ambient/Ambient";
    //public string run = "event:/Running/Run";
    //public string breaks = "event:/Breaks/Breaks";


    public Transform player;
    float playerDistance;

    FMOD.Studio.EventInstance AtmosEv;
    //FMOD.Studio.EventInstance AmbientEv;
    //FMOD.Studio.EventInstance RunEv;
    //FMOD.Studio.EventInstance BreaksEv;

    // Start is called before the first frame update
    void Start()
    {
       
        
        //FoleyEv.EventReference = FMODUnity.RuntimeManager.PathToEventReference(foley);
        AtmosEv = FMODUnity.RuntimeManager.CreateInstance(atmos);
        //AmbientEv = FMODUnity.RuntimeManager.CreateInstance(ambient);
        //RunEv = FMODUnity.RuntimeManager.CreateInstance(run);
        //BreaksEv = FMODUnity.RuntimeManager.CreateInstance(breaks);

        AtmosEv.start();
        //AmbientEv.start();
        //RunEv.start();
        //BreaksEv.start();
    }

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void RestartEvents()
    {
        //Debug.Log("Release");
        AtmosEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        AtmosEv.release();
        //AmbientEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        //RunEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        //BreaksEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        //FMODUnity.RuntimeManager.PlayOneShot("event:/Ending");

    }


    // Update is called once per frame
    void Update()
    {
        playerDistance = Vector3.Distance(player.position, this.transform.position);
        playerDistance = playerDistance / 100;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("WavesLvl", playerDistance);
    }
}
