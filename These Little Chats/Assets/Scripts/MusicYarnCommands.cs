using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;   //Let's us talk to UI stuff.
using Yarn.Unity;   //Lets us talk to Yarn stuff.

/// <summary>
/// Contains all Yarn commands associated with music that are called from a Yarn file
/// </summary>
public class MusicYarnCommands : MonoBehaviour {

    public AudioClip musicTrackOne;
    public AudioClip musicTrackTwo;
    public AudioClip musicTrackThree;
    public AudioClip musicTrackFour;

    [YarnCommand("triggerSound")]
    public void TriggerSound(string sound)
    {
        if(sound == "musicTrackOne")
        {
            this.GetComponent<AudioSource>().clip = musicTrackOne;
            this.GetComponent<AudioSource>().Play();
        }
        else if(sound == "musicTrackTwo")
        {
            this.GetComponent<AudioSource>().clip = musicTrackTwo;
            this.GetComponent<AudioSource>().Play();
        }
        else if(sound == "musicTrackThree")
        {
            this.GetComponent<AudioSource>().clip = musicTrackThree;
            this.GetComponent<AudioSource>().Play();
        }
        else if(sound == "musicTrackFour")
        {
            this.GetComponent<AudioSource>().clip = musicTrackFour;
            this.GetComponent<AudioSource>().Play();
        }
    }
}
