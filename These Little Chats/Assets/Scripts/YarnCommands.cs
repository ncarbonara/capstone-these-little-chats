using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;   //Let's us talk to UI stuff.
using Yarn.Unity;   //Lets us talk to Yarn stuff, I think.

/// <summary>
/// Attach this script to each character gameObject in the scene.
/// </summary>
public class YarnCommands : MonoBehaviour {

    public VariableStorageBehaviour variableManager;

    //gameplayVariablesManager is kind of poorly named and confusing, see above
    public GameObject gameplayVariablesManager;
    public GameObject characterPortraitGameObject;
    public GameObject nameText;
    public GameObject portraitBackground;

    public Sprite characterNeutral;
    public Sprite characterHappy;
    public Sprite characterSad;
    public Sprite characterAngry;

    public GameObject convosRemainingText;

    public GameObject lanceValueDisplay;
    public GameObject lanceValueIcon;

    public GameObject allisonValueDisplay;
    public GameObject allisonValueIcon;

    public GameObject franklinValueDisplay;
    public GameObject franklinValueIcon;

    public GameObject rubyValueDisplay;
    public GameObject rubyValueIcon;


    // Use this for initialization
    void Start () {
        nameText.GetComponent<Text>().text = null;
        portraitBackground.gameObject.SetActive(false);

        convosRemainingText.GetComponent<Text>().text = gameplayVariablesManager.GetComponent<GameplayVariablesManager>().conversationsRemaining.ToString() + " TALKS LEFT";

        lanceValueDisplay.gameObject.SetActive(false);
        allisonValueDisplay.gameObject.SetActive(false);
        franklinValueDisplay.gameObject.SetActive(false);
        rubyValueDisplay.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

	}

    /*
    /// <summary>
    /// (Looks like this is something for Steve/Dana/Xander)
    /// </summary>
    /// <param name="increaseOrDecrease"></param>
    [YarnCommand("changeTrustValue")]
    public void ChangeTrustValue(string increaseOrDecrease)
    {

        if (increaseOrDecrease == "increase")
        {
            if (this.gameObject.name == "Steve")
            {
                gameplayVariablesManager.GetComponent<GameplayVariablesManager>().steveTrust++;
                Debug.Log("Steve's trust in you has increased!");
                Debug.Log("Steve Trust: " + gameplayVariablesManager.GetComponent<GameplayVariablesManager>().steveTrust.ToString());
            } else if(this.gameObject.name == "Dana")
            {
                gameplayVariablesManager.GetComponent<GameplayVariablesManager>().danaTrust++;
                Debug.Log("Dana's trust in you has increased!");
                Debug.Log("Dana Trust: " + gameplayVariablesManager.GetComponent<GameplayVariablesManager>().danaTrust.ToString());
            } else if(this.gameObject.name == "Xander")
            {
                gameplayVariablesManager.GetComponent<GameplayVariablesManager>().xanderTrust++;
                Debug.Log("Xander's trust in you has increased!");
                Debug.Log("Xander Trust: " + gameplayVariablesManager.GetComponent<GameplayVariablesManager>().xanderTrust.ToString());
            }

        } else if (increaseOrDecrease == "decrease")
        {
            if(this.gameObject.name == "Steve")
            {
                gameplayVariablesManager.GetComponent<GameplayVariablesManager>().steveTrust--;
                Debug.Log("Steve's trust in you has decreased!");
                Debug.Log("Steve Trust: " + gameplayVariablesManager.GetComponent<GameplayVariablesManager>().steveTrust.ToString());
            }
            else if(this.gameObject.name == "Dana")
            {
                gameplayVariablesManager.GetComponent<GameplayVariablesManager>().danaTrust--;
                Debug.Log("Dana's trust in you has decreased!");
                Debug.Log("Dana Trust: " + gameplayVariablesManager.GetComponent<GameplayVariablesManager>().danaTrust.ToString());
            }
            else if(this.gameObject.name == "Xander")
            {
                gameplayVariablesManager.GetComponent<GameplayVariablesManager>().xanderTrust--;
                Debug.Log("Xander's trust in you has decreased!");
                Debug.Log("Xander Trust: " + gameplayVariablesManager.GetComponent<GameplayVariablesManager>().xanderTrust.ToString());
            }
        }
    }

    /// <summary>
    /// An old function used for a character I made up for a prototype.
    /// </summary>
    [YarnCommand("incrementKeithTrust")]
    public void IncrementKeithTrust()
    {
        gameplayVariablesManager.GetComponent<GameplayVariablesManager>().keithTrustLevel++;
        Debug.Log("Keith Trust: " + gameplayVariablesManager.GetComponent<GameplayVariablesManager>().keithTrustLevel.ToString());

        //If Keith trust is high/low enough, change the yarn variable, perhaps based on an exposed unity variable we can play with.
        if(gameplayVariablesManager.GetComponent<GameplayVariablesManager>().keithTrustLevel >= 2)
        {
            variableManager.SetValue("$trust_Keith", new Yarn.Value(true));
        }
    }
    */

    [YarnCommand("activateNeutralPortrait")]
    public void ActivateNeutralPortrait()
    {
        characterPortraitGameObject.GetComponent<Image>().sprite = characterNeutral;
        characterPortraitGameObject.GetComponent<Image>().color = Color.white;

        nameText.GetComponent<Text>().text = this.gameObject.name;
        portraitBackground.gameObject.SetActive(true);
    }

    [YarnCommand("activateHappyPortrait")]
    public void ActivateHappyPortrait()
    {
        characterPortraitGameObject.GetComponent<Image>().sprite = characterHappy;
        characterPortraitGameObject.GetComponent<Image>().color = Color.white;

        nameText.GetComponent<Text>().text = this.gameObject.name;
        portraitBackground.gameObject.SetActive(true);
    }

    [YarnCommand("activateSadPortrait")]
    public void ActivateSadPortrait()
    {
        characterPortraitGameObject.GetComponent<Image>().sprite = characterSad;
        characterPortraitGameObject.GetComponent<Image>().color = Color.white;

        nameText.GetComponent<Text>().text = this.gameObject.name;
        portraitBackground.gameObject.SetActive(true);
    }

    [YarnCommand("activateAngryPortrait")]
    public void ActivateAngryPortrait()
    {
        characterPortraitGameObject.GetComponent<Image>().sprite = characterAngry;
        characterPortraitGameObject.GetComponent<Image>().color = Color.white;

        nameText.GetComponent<Text>().text = this.gameObject.name;
        portraitBackground.gameObject.SetActive(true);
    }

    [YarnCommand("clearPortrait")]
    public void ClearPortrait()
    {
        characterPortraitGameObject.GetComponent<Image>().sprite = null;
        characterPortraitGameObject.GetComponent<Image>().color = new Vector4 (1, 1, 1, 0);

        nameText.GetComponent<Text>().text = null;
        portraitBackground.gameObject.SetActive(false);
    }

    [YarnCommand("passTime")]
    public void passTime()
    {
        gameplayVariablesManager.GetComponent<GameplayVariablesManager>().conversationsRemaining--;
        convosRemainingText.GetComponent<Text>().text = gameplayVariablesManager.GetComponent<GameplayVariablesManager>().conversationsRemaining.ToString() + " TALKS LEFT";

    }

    //Lance Values
    /// <summary>
    /// Reveals Lance's value to the player via an addition to the UI.
    /// </summary>
    [YarnCommand("revealLanceValue")]
    public void RevealLanceValue()
    {
        lanceValueDisplay.gameObject.SetActive(true);
    }

    /// <summary>
    /// Increases Lance's value.
    /// </summary>
    [YarnCommand("increaseLanceValue")]
    public void IncreaseLanceValue()
    {
        ChangeLanceValue(true);
    }

    /// <summary>
    /// Decreases Lance's value.
    /// </summary>
    [YarnCommand("decreaseLanceValue")]
    public void DecreaseLanceValue()
    {
        ChangeLanceValue(false);
    }

    /// <summary>
    /// Handles all the changes for Lance's value, positive or negative.
    /// </summary>
    void ChangeLanceValue(bool isPositiveChange)
    {
        if (isPositiveChange == true)
        {
            gameplayVariablesManager.GetComponent<GameplayVariablesManager>().lanceValue++;
        } else if (isPositiveChange == false)
        {
            gameplayVariablesManager.GetComponent<GameplayVariablesManager>().lanceValue--;
        }

        Debug.Log("Lance Value: " + gameplayVariablesManager.GetComponent<GameplayVariablesManager>().lanceValue.ToString());

        //If Lance value is high/low enough, change the yarn variable.
        if(gameplayVariablesManager.GetComponent<GameplayVariablesManager>().lanceValue >= gameplayVariablesManager.GetComponent<GameplayVariablesManager>().lanceValueThreshhold)
        {
            variableManager.SetValue("$lance_will_share_info", new Yarn.Value(true));
            lanceValueIcon.GetComponent<Image>().color = Color.green;
        }
    }

    //Allison Values
    /// <summary>
    /// Reveals Allison's value to the player via an addition to the UI.
    /// </summary>
    [YarnCommand("revealAllisonValue")]
    public void RevealAllisonValue()
    {
        allisonValueDisplay.gameObject.SetActive(true);
    }

    /// <summary>
    /// Increases Allison's value.
    /// </summary>
    [YarnCommand("increaseAllisonValue")]
    public void IncreaseAllisonValue()
    {
        ChangeAllisonValue(true);
    }

    /// <summary>
    /// Decreases Allison's value.
    /// </summary>
    [YarnCommand("decreaseAllisonValue")]
    public void DecreaseAllisonValue()
    {
        ChangeAllisonValue(false);
    }

    /// <summary>
    /// Handles all the changes for Allison's value, positive or negative.
    /// </summary>
    void ChangeAllisonValue(bool isPositiveChange)
    {
        if(isPositiveChange == true)
        {
            gameplayVariablesManager.GetComponent<GameplayVariablesManager>().allisonValue++;
        }
        else if(isPositiveChange == false)
        {
            gameplayVariablesManager.GetComponent<GameplayVariablesManager>().allisonValue--;
        }

        Debug.Log("Allison Value: " + gameplayVariablesManager.GetComponent<GameplayVariablesManager>().allisonValue.ToString());

        //If Allison confidence is high/low enough, change the yarn variable.
        if(gameplayVariablesManager.GetComponent<GameplayVariablesManager>().allisonValue >= gameplayVariablesManager.GetComponent<GameplayVariablesManager>().allisonValueThreshhold)
        {
            variableManager.SetValue("$allison_will_share_info", new Yarn.Value(true));
            allisonValueIcon.GetComponent<Image>().color = Color.green;
        }
    }

    //Franklin Values
    /// <summary>
    /// Reveals to the player what Franklin's value is via an addition to the UI.
    /// </summary>
    [YarnCommand("revealFranklinValue")]
    public void RevealFranklinValue()
    {
        franklinValueDisplay.gameObject.SetActive(true);
    }

    /// <summary>
    /// Changes Franklin's value standing in a positive direction.
    /// </summary>
    [YarnCommand("increaseFranklinValue")]
    public void IncreaseFranklinValue()
    {
        ChangeFranklinValue(true);
    }

    /// <summary>
    /// Changes Franklin's value standing in a negative direction.
    /// </summary>
    [YarnCommand("decreaseFranklinValue")]
    public void DecreaseFranklinValue()
    {
        ChangeFranklinValue(false);
    }

    /// <summary>
    /// Handles all the changes for Franklin's value, positive or negative.
    /// </summary>
    void ChangeFranklinValue(bool isPositiveChange)
    {
        if(isPositiveChange == true)
        {
            gameplayVariablesManager.GetComponent<GameplayVariablesManager>().franklinValue++;
        }
        else if(isPositiveChange == false)
        {
            gameplayVariablesManager.GetComponent<GameplayVariablesManager>().franklinValue--;
        }

        Debug.Log("Franklin Value: " + gameplayVariablesManager.GetComponent<GameplayVariablesManager>().franklinValue.ToString());

        //If Franklin's value is high/low enough, change the yarn variable, perhaps based on an exposed unity variable we can play with.
        if(gameplayVariablesManager.GetComponent<GameplayVariablesManager>().franklinValue >= gameplayVariablesManager.GetComponent<GameplayVariablesManager>().franklinValueThreshhold)
        {
            variableManager.SetValue("$franklin_will_share_info", new Yarn.Value(true));
            franklinValueIcon.GetComponent<Image>().color = Color.green;
        }
    }

    //Ruby Values
    /// <summary>
    /// Reveals to the player what Ruby's value is via an addition to the UI.
    /// </summary>
    [YarnCommand("revealRubyValue")]
    public void RevealRubyValue()
    {
        rubyValueDisplay.gameObject.SetActive(true);
    }

    /// <summary>
    /// Changes Ruby's value standing in a positive direction.
    /// </summary>
    [YarnCommand("increaseRubyValue")]
    public void IncreaseRubyValue()
    {
        ChangeRubyValue(true);
    }

    /// <summary>
    /// Changes Ruby's value standing in a negative direction.
    /// </summary>
    [YarnCommand("decreaseRubyValue")]
    public void DecreaseRubyValue()
    {
        ChangeRubyValue(false);
    }

    /// <summary>
    /// Handles all the changes for Ruby's value, positive or negative.
    /// </summary>
    void ChangeRubyValue(bool isPositiveChange)
    {
        if(isPositiveChange == true)
        {
            gameplayVariablesManager.GetComponent<GameplayVariablesManager>().rubyValue++;
        }
        else if(isPositiveChange == false)
        {
            gameplayVariablesManager.GetComponent<GameplayVariablesManager>().rubyValue--;
        }

        Debug.Log("Ruby Value: " + gameplayVariablesManager.GetComponent<GameplayVariablesManager>().rubyValue.ToString());

        //If Ruby's value is high/low enough, change the yarn variable.
        if(gameplayVariablesManager.GetComponent<GameplayVariablesManager>().rubyValue >= gameplayVariablesManager.GetComponent<GameplayVariablesManager>().rubyValueThreshhold)
        {
            variableManager.SetValue("$ruby_will_share_info", new Yarn.Value(true));
            rubyValueIcon.GetComponent<Image>().color = Color.green;
        }
    }

    /*
    public void WhoWillShareInfo()
    {
        int[] valueRatings;
        valueRatings.Add(gameplayVariablesManager.GetComponent<GameplayVariablesManager>().rubyValue);
    }
    */
}
