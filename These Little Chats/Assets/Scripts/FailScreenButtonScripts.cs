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

    public void RetryGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Triggers the fail screen from the Yarn file when the player has caused someone to leave
    /// </summary>
    [YarnCommand("activateFailScreen")]
    public void ActivateFailScreen()
    {
        SceneManager.LoadScene("FailScreen");
    }
}
