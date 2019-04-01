using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;   //Let's us talk to UI stuff.

public class ConversationLogScript : MonoBehaviour {

    public GameObject dialogueText;

    // Update is called once per frame
    void Update () {
        string lineToAdd = dialogueText.GetComponent<Text>().text;
        this.GetComponent<Text>().text = this.GetComponent<Text>().text + lineToAdd;
    }
}
