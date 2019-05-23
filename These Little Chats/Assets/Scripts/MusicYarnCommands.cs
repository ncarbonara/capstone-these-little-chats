using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;   //Let's us talk to UI stuff.
using Yarn.Unity;   //Lets us talk to Yarn stuff.

/// <summary>
/// Contains all Yarn commands associated with music that are called from a Yarn file
/// </summary>
public class MusicYarnCommands : MonoBehaviour {

    bool fadeOutSound;
    bool fadeInSound;
    float defaultAudioVolume;

    //MAJOR MUSIC TRACKS (CuedMusicStings gameObject only)
    public AudioClip musicTrackOne;
    public AudioClip musicTrackTwo;
    public AudioClip musicTrackThree;
    public AudioClip musicTrackFour;
    public AudioClip musicTrackFive;

    //TENSION LEVEL AUDIO LAYERS (CuedMusicTensionLayers gameObject only)
    public AudioClip lowTensionLayer;
    public AudioClip midTensionLayer;
    public AudioClip highTensionLayer;

    void Start()
    {
        fadeInSound = false;
        fadeOutSound = false;

        //First checks what the developer has set the desired volume to, saves this, then sets the
        //starting volume to 0.
        defaultAudioVolume = this.GetComponent<AudioSource>().volume;
        this.GetComponent<AudioSource>().volume = 0f;
    }

    void Update()
    {

        //Triggered when it's time for an audio track to fade out
        if(fadeInSound == true)
        {
            this.GetComponent<AudioSource>().volume += Time.deltaTime;

            if(this.GetComponent<AudioSource>().volume >= defaultAudioVolume)
            {
                this.GetComponent<AudioSource>().volume = defaultAudioVolume;
                fadeInSound = false;
            }
        }

        //Triggered when it's time for an audio track to fade out
        if (fadeOutSound == true)
        {
            this.GetComponent<AudioSource>().volume -= Time.deltaTime / 2f;

            if (this.GetComponent<AudioSource>().volume <= 0f)
            {
                this.GetComponent<AudioSource>().Stop();
                //this.GetComponent<AudioSource>().volume = defaultAudioVolume;
                fadeOutSound = false;
            }
        }
    }

    [YarnCommand("triggerSound")]
    public void TriggerSound(string sound)
    {
        //Used for cuing up basic music tracks
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
        else if(sound == "musicTrackFive")
        {
            this.GetComponent<AudioSource>().clip = musicTrackFive;
            this.GetComponent<AudioSource>().Play();
        }

        //Used for switching tension layers of a piece. This needs to work a little differently
        //than just switching music tracks overall, because the loops need to continue at the same
        //time placement that the previous track had been playing at
        if (sound == "lowTensionLayer")
        {
            float currentPlaybackPosition = this.GetComponent<AudioSource>().time;
            this.GetComponent<AudioSource>().clip = lowTensionLayer;
            this.GetComponent<AudioSource>().Play();
            this.GetComponent<AudioSource>().time = currentPlaybackPosition;
        } else if(sound == "midTensionLayer")
        {
            float currentPlaybackPosition = this.GetComponent<AudioSource>().time;
            this.GetComponent<AudioSource>().clip = midTensionLayer;
            this.GetComponent<AudioSource>().Play();
            this.GetComponent<AudioSource>().time = currentPlaybackPosition;
        } else if(sound == "highTensionLayer")
        {
            float currentPlaybackPosition = this.GetComponent<AudioSource>().time;
            this.GetComponent<AudioSource>().clip = highTensionLayer;
            this.GetComponent<AudioSource>().Play();
            this.GetComponent<AudioSource>().time = currentPlaybackPosition;
        }

        fadeInSound = true;
    }

    /// <summary>
    /// Tells the audio source to gradually fade out the sound
    /// </summary>
    [YarnCommand("fadeOutSound")]
    public void FadeOutSound()
    {
        fadeOutSound = true;
    }

    /// <summary>
    /// Tells the audio source to stop playing music altogether.
    /// </summary>
    [YarnCommand("stopSound")]
    public void StopSound()
    {
        this.GetComponent<AudioSource>().Stop();
    }
}
