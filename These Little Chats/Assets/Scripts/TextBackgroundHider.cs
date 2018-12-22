using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Checks to see if the dialogue text is hidden or not. If it is hidden, this object will turn its image invisible.
public class TextBackgroundHider : MonoBehaviour {

    public GameObject dialogueText;

	// Use this for initialization
	void Start () {
        //dialogueText = GameObject.FindGameObjectWithTag("Dialogue Text");
    }
	
	// Update is called once per frame
	void Update () {

        if (dialogueText == null)
        {
            this.gameObject.SetActive(false);
        }
	}
}
