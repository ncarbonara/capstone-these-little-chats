using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraButtonFunctions : MonoBehaviour {

    public GameObject conversationLog;
    bool conversationLogIsOpen;

    void Start()
    {
        conversationLogIsOpen = false;
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Opens and closes the conversation log
    /// </summary>
    public void OpenAndCloseConversationLog()
    {

        if (conversationLogIsOpen == false)
        {
            conversationLog.gameObject.SetActive(true);
            conversationLogIsOpen = true;
        } else if(conversationLogIsOpen == true)
        {
            conversationLog.gameObject.SetActive(false);
            conversationLogIsOpen = false;
        }
    }

    /// <summary>
    /// Causes audio to be played whenever the player presses an option button.
    /// </summary>
    public void PlayOptionButtonAudio()
    {
        this.GetComponent<AudioSource>().Play();
    }
}
