using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Yarn.Unity.Example;   //Let's us talk to ModifiedExampleDialogueUI.cs stuff

/// <summary>
/// Used to see if the player's mouse is hovering over this button. Used to determine if a click
/// should continue dialogue or not.
/// </summary>
public class ButtonHoverChecker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool mouseHoveringOverButton;

    public GameObject primaryDialogue;
    public GameObject secondaryDialogue;
    public GameObject tertiaryDialogue;
    public GameObject quaternaryDialogue;

    private void Start()
    {
        mouseHoveringOverButton = false;
        Physics.queriesHitTriggers = true;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        mouseHoveringOverButton = true;

        primaryDialogue.GetComponent<ModifiedExampleDialogueUI>().conversationLogButtonIsBeingHoveredOver = true;
        secondaryDialogue.GetComponent<ModifiedExampleDialogueUI>().conversationLogButtonIsBeingHoveredOver = true;
        tertiaryDialogue.GetComponent<ModifiedExampleDialogueUI>().conversationLogButtonIsBeingHoveredOver = true;
        quaternaryDialogue.GetComponent<ModifiedExampleDialogueUI>().conversationLogButtonIsBeingHoveredOver = true;

        Debug.Log("Mouse Hover: " + mouseHoveringOverButton);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        mouseHoveringOverButton = false;

        primaryDialogue.GetComponent<ModifiedExampleDialogueUI>().conversationLogButtonIsBeingHoveredOver = false;
        secondaryDialogue.GetComponent<ModifiedExampleDialogueUI>().conversationLogButtonIsBeingHoveredOver = false;
        tertiaryDialogue.GetComponent<ModifiedExampleDialogueUI>().conversationLogButtonIsBeingHoveredOver = false;
        quaternaryDialogue.GetComponent<ModifiedExampleDialogueUI>().conversationLogButtonIsBeingHoveredOver = false;
    }
}
