using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;   //Let's us talk to UI stuff.
using Yarn.Unity;   //Lets us talk to Yarn stuff, I believe.

/// <summary>
/// Contains all of the functions that can be called from a Yarn sheet to alter character values,
/// change portraits, etc.
/// Attach this script to each character gameObject in the scene (The gameObjects called "Lance,"
/// "Allison," etc).
/// </summary>
public class YarnCommands : MonoBehaviour {

    public VariableStorageBehaviour variableManager;

    //Note to Self: gameplayVariablesManager is kind of poorly named and confusing, because of the
    //naming of VariableStorageBehaviour. Should probably fix.
    public GameObject gameplayVariablesManager;
    public GameObject characterPortraitGameObject;
    public GameObject nameText;
    public GameObject portraitBackground;

    public Sprite neutral;
    public Sprite happy;
    public Sprite sad;
    public Sprite angry;

    public GameObject convosRemainingText;

    public GameObject valueDisplay;
    public GameObject valueIcon;

    //Character variables handled directly in this script. Set initial values in the inspector!
    public int value;
    public int infoSharingValueThreshold;
    public string willShareInfoYarnBool;
    public int veryPleasedValueThreshold;
    public string veryPleasedYarnBool;
    public int somewhatPleasedValueThreshold;
    public string somewhatPleasedYarnBool;
    public int neutralValueThreshold;
    public string neutralYarnBool;
    public int somewhatAngryValueThreshold;
    public string somewhatAngryYarnBool;
    public int veryAngryValueThreshold;
    public string veryAngryYarnBool;

    // Use this for initialization
    void Start () {
        nameText.GetComponent<Text>().text = null;
        portraitBackground.gameObject.SetActive(false);

        convosRemainingText.GetComponent<Text>().text = gameplayVariablesManager.GetComponent<GameplayVariablesManager>().conversationsRemaining.ToString() + " TALKS LEFT";

        valueDisplay.gameObject.SetActive(false);

        variableManager.SetValue(neutralYarnBool, new Yarn.Value(true));
    }

    /// <summary>
    /// Activates this character's portrait display, and switches in their neutral portrait.
    /// </summary>
    [YarnCommand("activateNeutralPortrait")]
    public void ActivateNeutralPortrait()
    {
        characterPortraitGameObject.GetComponent<Image>().sprite = neutral;
        characterPortraitGameObject.GetComponent<Image>().color = Color.white;

        nameText.GetComponent<Text>().text = this.gameObject.name;
        portraitBackground.gameObject.SetActive(true);
    }

    /// <summary>
    /// Activates this character's portrait display, and switches in their happy portrait.
    /// </summary>
    [YarnCommand("activateHappyPortrait")]
    public void ActivateHappyPortrait()
    {
        characterPortraitGameObject.GetComponent<Image>().sprite = happy;
        characterPortraitGameObject.GetComponent<Image>().color = Color.white;

        nameText.GetComponent<Text>().text = this.gameObject.name;
        portraitBackground.gameObject.SetActive(true);
    }

    /// <summary>
    /// Activates this character's portrait display, and switches in their sad portrait.
    /// </summary>
    [YarnCommand("activateSadPortrait")]
    public void ActivateSadPortrait()
    {
        characterPortraitGameObject.GetComponent<Image>().sprite = sad;
        characterPortraitGameObject.GetComponent<Image>().color = Color.white;

        nameText.GetComponent<Text>().text = this.gameObject.name;
        portraitBackground.gameObject.SetActive(true);
    }

    /// <summary>
    /// Activates this character's portrait display, and switches in their angry portrait.
    /// </summary>
    [YarnCommand("activateAngryPortrait")]
    public void ActivateAngryPortrait()
    {
        characterPortraitGameObject.GetComponent<Image>().sprite = angry;
        characterPortraitGameObject.GetComponent<Image>().color = Color.white;

        nameText.GetComponent<Text>().text = this.gameObject.name;
        portraitBackground.gameObject.SetActive(true);
    }

    /// <summary>
    /// Clears and deactivates all portraits.
    /// </summary>
    [YarnCommand("clearPortrait")]
    public void ClearPortrait()
    {
        characterPortraitGameObject.GetComponent<Image>().sprite = null;
        characterPortraitGameObject.GetComponent<Image>().color = new Vector4 (1, 1, 1, 0);

        nameText.GetComponent<Text>().text = null;
        portraitBackground.gameObject.SetActive(false);
    }

    /// <summary>
    /// Causes the UI text for the number of conversations left to tick down one conversation.
    /// </summary>
    [YarnCommand("passTime")]
    public void passTime()
    {
        gameplayVariablesManager.GetComponent<GameplayVariablesManager>().conversationsRemaining--;
        convosRemainingText.GetComponent<Text>().text = gameplayVariablesManager.GetComponent<GameplayVariablesManager>().conversationsRemaining.ToString() + " TALKS LEFT";
    }

    /// <summary>
    /// Reveals the character's value to the player via an addition to the UI.
    /// </summary>
    [YarnCommand("revealValue")]
    public void RevealValue()
    {
        valueDisplay.gameObject.SetActive(true);
    }

    /// <summary>
    /// Increases the character's value.
    /// </summary>
    [YarnCommand("increaseValue")]
    public void IncreaseValue()
    {
        ChangeValue(true);
    }

    /// <summary>
    /// Decreases the character's value.
    /// </summary>
    [YarnCommand("decreaseValue")]
    public void DecreaseValue()
    {
        ChangeValue(false);
    }

    /// <summary>
    /// Handles all the changes for the character's value, positive or negative.
    /// </summary>
    void ChangeValue(bool isPositiveChange)
    {
        if(isPositiveChange == true)
        {
            value++;
        }
        else if(isPositiveChange == false)
        {
            value--;
        }

        Debug.Log(this.gameObject.name + " Value: " + value.ToString());

        /*
        //If character value is high/low enough, change a Yarn variable of the correct name.
        if(value == infoSharingValueThreshold)
        {
            variableManager.SetValue(willShareInfoYarnBool, new Yarn.Value(true));
            valueIcon.GetComponent<Image>().color = Color.blue;
            Debug.Log(this.gameObject.name + " will share info.");
        }
        */

        //Changes a variable in the Yarn sheet that activates "very pleased" text for this 
        //character
        if(value == veryPleasedValueThreshold)
        {
            variableManager.SetValue(veryPleasedYarnBool, new Yarn.Value(true));
            variableManager.SetValue(somewhatPleasedYarnBool, new Yarn.Value(false));
            variableManager.SetValue(neutralYarnBool, new Yarn.Value(false));
            variableManager.SetValue(somewhatAngryYarnBool, new Yarn.Value(false));
            variableManager.SetValue(veryAngryYarnBool, new Yarn.Value(false));
            valueIcon.GetComponent<Image>().color = new Color(0, 1, 0, 1);
            Debug.Log(this.gameObject.name + " is very pleased.");
        }

        //Changes a variable in the Yarn sheet that activates "somewhat pleased" text for this
        //character
        if(value == somewhatPleasedValueThreshold)
        {
            variableManager.SetValue(veryPleasedYarnBool, new Yarn.Value(false));
            variableManager.SetValue(somewhatPleasedYarnBool, new Yarn.Value(true));
            variableManager.SetValue(neutralYarnBool, new Yarn.Value(false));
            variableManager.SetValue(somewhatAngryYarnBool, new Yarn.Value(false));
            variableManager.SetValue(veryAngryYarnBool, new Yarn.Value(false));
            valueIcon.GetComponent<Image>().color = new Color(0, 0.5f, 0, 1);
            Debug.Log(this.gameObject.name + " is somewhat pleased.");
        }

        //Changes a variable in the Yarn sheet that activates "neutral" text for this character
        if(value == neutralValueThreshold)
        {
            variableManager.SetValue(veryPleasedYarnBool, new Yarn.Value(false));
            variableManager.SetValue(somewhatPleasedYarnBool, new Yarn.Value(false));
            variableManager.SetValue(neutralYarnBool, new Yarn.Value(true));
            variableManager.SetValue(somewhatAngryYarnBool, new Yarn.Value(false));
            variableManager.SetValue(veryAngryYarnBool, new Yarn.Value(false));
            valueIcon.GetComponent<Image>().color = Color.white;
            Debug.Log(this.gameObject.name + " is feeling neutral.");
        }

        //Changes a variable in the Yarn sheet that activates "somewhat angry" text for this 
        //character
        if(value == somewhatAngryValueThreshold)
        {
            variableManager.SetValue(veryPleasedYarnBool, new Yarn.Value(false));
            variableManager.SetValue(somewhatPleasedYarnBool, new Yarn.Value(false));
            variableManager.SetValue(neutralYarnBool, new Yarn.Value(false));
            variableManager.SetValue(somewhatAngryYarnBool, new Yarn.Value(true));
            variableManager.SetValue(veryAngryYarnBool, new Yarn.Value(false));
            valueIcon.GetComponent<Image>().color = new Color(0.5f, 0, 0, 1);
            Debug.Log(this.gameObject.name + " is somewhat angry.");
        }

        //Changes a variable in the Yarn sheet that activates "very angry" text for this character
        if(value == veryAngryValueThreshold)
        {
            variableManager.SetValue(veryPleasedYarnBool, new Yarn.Value(false));
            variableManager.SetValue(somewhatPleasedYarnBool, new Yarn.Value(false));
            variableManager.SetValue(neutralYarnBool, new Yarn.Value(false));
            variableManager.SetValue(somewhatAngryYarnBool, new Yarn.Value(false));
            variableManager.SetValue(veryAngryYarnBool, new Yarn.Value(true));
            valueIcon.GetComponent<Image>().color = new Color(1, 0, 0, 1);
            Debug.Log(this.gameObject.name + " is very angry.");
        }
    }
}
