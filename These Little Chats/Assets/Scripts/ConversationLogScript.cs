using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;   //Let's us talk to UI stuff.

public class ConversationLogScript : MonoBehaviour {

    public GameObject primaryDialogueSource;
    public GameObject secondaryDialogueSource;
    public GameObject primaryDialogueText;
    public GameObject secondaryDialogueText;
    public GameObject primaryCharacterNameText;
    public GameObject secondaryCharacterNameText;
    public GameObject primarySpeechBubble;
    public GameObject secondarySpeechBubble;

    string lineToAdd;
    string previousLineToAdd;
    GameObject previousDialogueSource;

    //COMMENTED OUT TO SHOW THAT THE FUNCTIONALITY FOR THESE DOESN'T WORK YET
    //string primarySpeechBubbleLineToAddWithCharacterName;
    //string secondarySpeechBubbleLineToAddWithCharacterName;

    Queue<string> linesInLog = new Queue <string>();

    public void AddToConversationLog (GameObject dialogueSource) {

        GameObject dialogueTextForThisLine = null;
        GameObject nameTextForThisLine = null;
        GameObject speechBubbleForThisLine = null;

        if (dialogueSource == primaryDialogueSource)
        {
            dialogueTextForThisLine = primaryDialogueText;
            nameTextForThisLine = primaryCharacterNameText;
            speechBubbleForThisLine = primarySpeechBubble;
        } else if (dialogueSource == secondaryDialogueSource)
        {
            dialogueTextForThisLine = secondaryDialogueText;
            nameTextForThisLine = secondaryCharacterNameText;
            speechBubbleForThisLine = secondarySpeechBubble;
        }

        lineToAdd = dialogueTextForThisLine.GetComponent<Text>().text;

        if(lineToAdd != previousLineToAdd
            //&& previousDialogueSource != dialogueSource
            && lineToAdd != " "
            && lineToAdd.Contains("Displayed Text Goes Here") == false)
        {
            string lineToAddWithCharacterName = "\n<b><color=#" + ColorUtility.ToHtmlStringRGB(speechBubbleForThisLine.GetComponent<Image>().color) + ">" + nameTextForThisLine.GetComponent<Text>().text + ":</color></b> " + lineToAdd;
            this.GetComponent<Text>().text = this.GetComponent<Text>().text + lineToAddWithCharacterName;

            previousLineToAdd = lineToAdd;
            previousDialogueSource = dialogueSource;

            linesInLog.Enqueue(lineToAddWithCharacterName);

            if(linesInLog.Count > 10)
            {
                string oldestLineWithCharacterName = linesInLog.Dequeue();
                string currentConversationLogText = this.GetComponent<Text>().text;
                currentConversationLogText = currentConversationLogText.Replace(oldestLineWithCharacterName, "");
                this.GetComponent<Text>().text = currentConversationLogText;
            }

            /*
            if (dialogueSource == primaryDialogueSource)
            {
                primarySpeechBubbleLineToAddWithCharacterName = lineToAddWithCharcterName;
            } else if(dialogueSource == secondaryDialogueSource)
            {
                secondarySpeechBubbleLineToAddWithCharacterName = lineToAddWithCharcterName;
            }
            */
        }
    }

    /*
    /// <summary>
    /// Cued whenever the player presses a key to continue dialogue
    /// </summary>
    public void LoadToConversationLog()
    {
        linesInLog.Enqueue(primarySpeechBubbleLineToAddWithCharacterName);

        if(linesInLog.Count > 10)
        {
            string oldestLineWithCharacterName = linesInLog.Dequeue();
            string currentConversationLogText = this.GetComponent<Text>().text;
            currentConversationLogText = currentConversationLogText.Replace(oldestLineWithCharacterName, "");
            this.GetComponent<Text>().text = currentConversationLogText;

        }

        linesInLog.Enqueue(secondarySpeechBubbleLineToAddWithCharacterName);

        //(A lazy, gross, inefficient way of doing this, but screw it)
        if(linesInLog.Count > 10)
        {
            string oldestLineWithCharacterName = linesInLog.Dequeue();
            string currentConversationLogText = this.GetComponent<Text>().text;
            currentConversationLogText = currentConversationLogText.Replace(oldestLineWithCharacterName, "");
            this.GetComponent<Text>().text = currentConversationLogText;

        }
    }
    */
}
