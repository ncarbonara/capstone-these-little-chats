using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;   //Let's us talk to UI stuff.
using Yarn.Unity;   //Lets us talk to Yarn stuff.
using Yarn.Unity.Example;

/// <summary>
/// Contains all Yarn commands associated with SFX that are called from a Yarn file. This script is
/// attached to both the EnvironmentSoundFX and VocalBarkSoundFX gameObjects
/// </summary>
public class SoundFXYarnCommands : MonoBehaviour {

    public GameObject primaryDialogueGameObject;
    public GameObject secondaryDialogueGameObject;
    public GameObject tertiaryDialogueGameObject;
    public GameObject quaternaryDialogueGameObject;

    //SOUND EFFECTS (used only in EnvironmentSoundFX)
    public AudioClip doorKnock;
    public AudioClip doorOpenClose;
    public AudioClip sitDown;
    public AudioClip d20Roll;

    //VOCAL BARKS (used only in VocalBarkSoundFX)
    public AudioClip sampleLaugh;
    public AudioClip sampleGreeting;
    public AudioClip sampleThinking;
    public AudioClip sampleUgh;
    public AudioClip brynnIntroHey;
    public AudioClip rubyIntroHey;
    public AudioClip franklinUgh;
    public AudioClip allisonSeriously;
    public AudioClip lanceNegativeComeOn;

    List<AudioClip> soundList = new List<AudioClip>(); 

    void Update()
    {
        if(this.GetComponent<AudioSource>().isPlaying == true)
        {
            primaryDialogueGameObject.GetComponent<ModifiedExampleDialogueUI>().soundEffectIsPlaying = true;
            secondaryDialogueGameObject.GetComponent<ModifiedExampleDialogueUI>().soundEffectIsPlaying = true;
            tertiaryDialogueGameObject.GetComponent<ModifiedExampleDialogueUI>().soundEffectIsPlaying = true;
            quaternaryDialogueGameObject.GetComponent<ModifiedExampleDialogueUI>().soundEffectIsPlaying = true;
        } else {
            primaryDialogueGameObject.GetComponent<ModifiedExampleDialogueUI>().soundEffectIsPlaying = false;
            secondaryDialogueGameObject.GetComponent<ModifiedExampleDialogueUI>().soundEffectIsPlaying = false;
            tertiaryDialogueGameObject.GetComponent<ModifiedExampleDialogueUI>().soundEffectIsPlaying = false;
            quaternaryDialogueGameObject.GetComponent<ModifiedExampleDialogueUI>().soundEffectIsPlaying = false;
        }
    }

    [YarnCommand("triggerSound")]
    public void TriggerSound(string sound)
    {
        if(sound == "doorKnock")                                    //SOUND EFFECTS
        {
            this.GetComponent<AudioSource>().clip = doorKnock;
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
        } else if (sound == "sampleLaugh")                          //VOCAL BARKS
        {
            this.GetComponent<AudioSource>().clip = sampleLaugh;
            this.GetComponent<AudioSource>().Play();
        } else if(sound == "sampleGreeting")
        {
            this.GetComponent<AudioSource>().clip = sampleGreeting;
            this.GetComponent<AudioSource>().Play();
        } else if(sound == "sampleThinking")
        {
            this.GetComponent<AudioSource>().clip = sampleThinking;
            this.GetComponent<AudioSource>().Play();
        } else if(sound == "sampleUgh")
        {
            this.GetComponent<AudioSource>().clip = sampleUgh;
            this.GetComponent<AudioSource>().Play();
        } else if(sound == "brynnIntroHey")
        {
            this.GetComponent<AudioSource>().clip = brynnIntroHey;
            this.GetComponent<AudioSource>().Play();
        } else if(sound == "rubyIntroHey")
        {
            this.GetComponent<AudioSource>().clip = rubyIntroHey;
            this.GetComponent<AudioSource>().Play();
        } else if(sound == "franklinUgh")
        {
            this.GetComponent<AudioSource>().clip = franklinUgh;
            this.GetComponent<AudioSource>().Play();
        } else if(sound == "allisonSeriously")
        {
            this.GetComponent<AudioSource>().clip = allisonSeriously;
            this.GetComponent<AudioSource>().Play();
        } else if(sound == "lanceNegativeComeOn")
        {
            this.GetComponent<AudioSource>().clip = lanceNegativeComeOn;
            this.GetComponent<AudioSource>().Play();
        }
    }
}
