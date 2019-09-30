using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraButtonFunctions : MonoBehaviour {

    public GameObject conversationLog;
    bool conversationLogIsOpen;

    public GameObject creditsPage;
    public GameObject title;
    public GameObject startButton;
    public GameObject creditsButton;
    public GameObject quitButton;
    bool creditsPageIsOpen;

    void Start()
    {
        conversationLogIsOpen = false;
        creditsPageIsOpen = false;
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

    public void OpenAndCloseCreditsPage()
    {
        if(creditsPageIsOpen == false)
        {
            title.gameObject.SetActive(false);
            startButton.gameObject.SetActive(false);
            creditsButton.gameObject.SetActive(false);
            quitButton.gameObject.SetActive(false);

            creditsPage.gameObject.SetActive(true);
            creditsPageIsOpen = true;
        }
        else if(creditsPageIsOpen == true)
        {
            title.gameObject.SetActive(true);
            startButton.gameObject.SetActive(true);
            creditsButton.gameObject.SetActive(true);
            quitButton.gameObject.SetActive(true);

            creditsPage.gameObject.SetActive(false);
            creditsPageIsOpen = false;
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
