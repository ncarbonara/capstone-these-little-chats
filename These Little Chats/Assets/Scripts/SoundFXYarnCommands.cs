using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;   //Let's us talk to UI stuff.
using Yarn.Unity;   //Lets us talk to Yarn stuff.

/// <summary>
/// Contains all Yarn commands associated with SFX that are called from a Yarn file
/// </summary>
public class SoundFXYarnCommands : MonoBehaviour {

    public AudioClip doorKnock;
    public AudioClip placeDMScreen;
    public AudioClip doorOpenClose;
    public AudioClip sitDown;

    [YarnCommand("triggerSound")]
    public void TriggerSound(string sound)
    {
        if(sound == "doorKnock")
        {
            this.GetComponent<AudioSource>().clip = doorKnock;
            this.GetComponent<AudioSource>().Play();
        }
        else if(sound == "placeDMScreen")
        {
            this.GetComponent<AudioSource>().clip = placeDMScreen;
            this.GetComponent<AudioSource>().Play();
        }
        else if(sound == "doorOpenClose")
        {
            this.GetComponent<AudioSource>().clip = doorOpenClose;
            this.GetComponent<AudioSource>().Play();
        }
        else if(sound == "sitDown")
        {
            this.GetComponent<AudioSource>().clip = sitDown;
            this.GetComponent<AudioSource>().Play();
        }
    }
}
