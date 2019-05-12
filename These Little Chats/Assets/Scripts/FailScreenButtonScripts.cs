using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;  //Let's us interact with scenes
using Yarn.Unity;   //Lets us talk to Yarn stuff

/// <summary>
/// This script is attached to the buttons in the fail screen, allowing players to either retry or
/// quit, and to the SceneManager object in the main scene, so the ActivateFailScreen function can
/// be called as needed
/// </summary>
public class FailScreenButtonScripts : MonoBehaviour {

    public GameObject primaryDialogue;
    public string desiredRestartNode;
    public GameObject failScreenObjects;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void RetryGame()
    {
        primaryDialogue.GetComponent<DialogueRunner>().startNode = desiredRestartNode;
        primaryDialogue.GetComponent<DialogueRunner>().StartDialogue();
        failScreenObjects.SetActive(false);

        //SceneManager.LoadScene("MainScene");

        //This is all code that was meant to start the scene from partway through rather than at the
        //very beginning when the player restarts the game. It doesn't work yet, unfortunately.

        /*
        GameObject dialogue = GameObject.Find("Dialogue");
        GameObject brynnPose = GameObject.Find("Brynn Pose");
        GameObject lancePose = GameObject.Find("Lance Pose");
        GameObject allisonPose = GameObject.Find("Allison Pose");
        GameObject franklinPose = GameObject.Find("Franklin Pose");
        GameObject rubyPose = GameObject.Find("Ruby Pose");

        dialogue.GetComponent<DialogueRunner>().startNode = "MajorChoice1";

        lancePose.SetActive(true);
        allisonPose.SetActive(true);
        franklinPose.SetActive(true);
        rubyPose.SetActive(true);
        */

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    /*
    /// <summary>
    /// Triggers the fail screen from the Yarn file when the player has caused someone to leave
    /// </summary>
    [YarnCommand("activateFailScreen")]
    public void ActivateFailScreen()
    {
        SceneManager.LoadScene("FailScreen");
    }
    */
}
