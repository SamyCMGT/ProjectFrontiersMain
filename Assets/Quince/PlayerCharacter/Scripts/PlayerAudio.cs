using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource dashAudio;

    public void playDashAudio()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            dashAudio.Play();
            Debug.Log("audio");
        }
    }

    public void Update()
    {
        playDashAudio();
    }
}
