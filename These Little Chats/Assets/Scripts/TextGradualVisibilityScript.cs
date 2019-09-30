using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

/// <summary>
/// Used as a center-formatted-text-friendly alternative to the default YarnSpinner text printing
/// system. Intended to help text be more easily read as it's appearing onscreen, even when the
/// text is center-formatted.
/// </summary>
public class TextGradualVisibilityScript : MonoBehaviour
{
    string text;
    int index;
    public bool resetTextColor;
    public bool allTextShown;

    // Use this for initialization
    void Start()
    {
        allTextShown = false;
        resetTextColor = false;
    }

    // Update is called once per frame
    private void Update()
    {
        //insert color end one character further
        text = this.GetComponent<Text>().text;

        if(text.Contains("<color=#ffffffff>") == false)
        {
            text = text.Insert(0, "<color=#ffffffff>");
        }

        if(text.Contains("</color>"))
        {
            index = text.IndexOf("</color>");
            text = text.Remove(index, 8);
        } else
        {
            index = 17;
        }

        //Debug.Log("Index is currently: " + index);

        text = text.Insert(index + 1, "</color>");

        if(index + 1 > text.Length)
        {
            allTextShown = true;
        }

        this.GetComponent<Text>().text = text;
    }

    public void ResetTextColor()
    {
        allTextShown = false;

        index = text.IndexOf("</color>");

        text = this.GetComponent<Text>().text;
        text = text.Insert(0, "<color=#ffffffff>");
        this.GetComponent<Text>().text = text;

        index = 0;
    }
}
