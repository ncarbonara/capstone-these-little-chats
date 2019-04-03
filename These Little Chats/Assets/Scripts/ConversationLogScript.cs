using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;   //Let's us talk to UI stuff.

public class ConversationLogScript : MonoBehaviour {

    public GameObject dialogueText;
    public GameObject characterNameText;

    string lineToAdd;
    string previousLineToAdd;

    Queue<string> linesInLog = new Queue <string>();

    public void AddToConversationLog () {
        lineToAdd = dialogueText.GetComponent<Text>().text;

        if(lineToAdd != previousLineToAdd
            && lineToAdd != " "
            && lineToAdd.Contains("Displayed Text Goes Here") == false)
        {
            Debug.Log("Line to Add: " + lineToAdd + " / Previous Line to Add: " + previousLineToAdd);
            string lineToAddWithCharcterName = "\n" + characterNameText.GetComponent<Text>().text + ": " + lineToAdd;
            this.GetComponent<Text>().text = this.GetComponent<Text>().text + lineToAddWithCharcterName;
            previousLineToAdd = lineToAdd;
            linesInLog.Enqueue(lineToAddWithCharcterName);
        }

        if(linesInLog.Count > 10)
        {
            Debug.Log("Many lines!");
            string oldestLineWithCharacterName = linesInLog.Dequeue();
            string currentConversationLogText = this.GetComponent<Text>().text;
            currentConversationLogText = currentConversationLogText.Replace(oldestLineWithCharacterName, "");
            this.GetComponent<Text>().text = currentConversationLogText;

        }



    }
}
