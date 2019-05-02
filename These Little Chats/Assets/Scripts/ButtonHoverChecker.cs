using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to see if the player's mouse is hovering over this button. Used to determine if a click
/// should continue dialogue or not.
/// </summary>
public class ButtonHoverChecker : MonoBehaviour {

    public bool mouseHoveringOverButton;

    private void Start()
    {
        mouseHoveringOverButton = false;
        Physics.queriesHitTriggers = true;
    }

    private void OnMouseOver()
    {
        mouseHoveringOverButton = true;
        Debug.Log("Mouse Hover: " + mouseHoveringOverButton);
    }

    private void OnMouseExit()
    {
        mouseHoveringOverButton = false;
    }
}
