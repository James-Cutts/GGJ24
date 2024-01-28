using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{

    public string music = "event:/Music/Music";

    FMOD.Studio.EventInstance MusicEv;

    void Start()
    {
        MusicEv = FMODUnity.RuntimeManager.CreateInstance(music);
        MusicEv.start();
    }

    public void Load(string LevelName)
    {
        MusicEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SceneManager.LoadScene(LevelName);   
    }
}
