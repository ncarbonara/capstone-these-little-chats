using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;   //Let's us talk to UI stuff.
using Yarn.Unity;   //Lets us talk to Yarn stuff.

/// <summary>
/// Contains all Yarn commands associated with SFX that are called from a Yarn file. This script is
/// attached to both the EnvironmentSoundFX and VocalBarkSoundFX gameObjects
/// </summary>
public class SoundFXYarnCommands : MonoBehaviour {

    //SOUND EFFECTS (used only in EnvironmentSoundFX)
    public AudioClip doorKnock;
    public AudioClip placeDMScreen;
    public AudioClip doorOpenClose;
    public AudioClip sitDown;
    public AudioClip d20Roll;

    //VOCAL BARKS (used only in VocalBarkSoundFX)
    public AudioClip lanceSampleBark;
    public AudioClip allisonSampleBark;
    public AudioClip rubySampleBark;
    public AudioClip franklinSampleBark;

    [YarnCommand("triggerSound")]
    public void TriggerSound(string sound)
    {
        if(sound == "doorKnock") //SOUND EFFECTS
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
        else if(sound == "d20Roll")
        {
            this.GetComponent<AudioSource>().clip = d20Roll;
            this.GetComponent<AudioSource>().Play();
        } else if (sound == "lanceSampleBark")  //VOCAL BARKS
        {
            this.GetComponent<AudioSource>().clip = lanceSampleBark;
            this.GetComponent<AudioSource>().Play();
        } else if(sound == "allisonSampleBark")
        {
            this.GetComponent<AudioSource>().clip = allisonSampleBark;
            this.GetComponent<AudioSource>().Play();
        } else if(sound == "rubySampleBark")
        {
            this.GetComponent<AudioSource>().clip = rubySampleBark;
            this.GetComponent<AudioSource>().Play();
        } else if(sound == "franklinSampleBark")
        {
            this.GetComponent<AudioSource>().clip = franklinSampleBark;
            this.GetComponent<AudioSource>().Play();
        }
    }
}
