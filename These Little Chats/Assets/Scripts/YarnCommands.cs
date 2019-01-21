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
    public GameObject dialogueText;

    public Sprite neutral;
    public Sprite happy;
    public Sprite sad;
    public Sprite angry;

    //public GameObject positivePose;
    public GameObject neutralPose;
    //public GameObject negativePose;

    //These are used to store the locations of each of the above pose sprites, so they can be
    //lerped towards each other, creating the illusion of bodily movement
    Vector3 currentPosePosition;
    public Vector3 positivePosePosition;
    public Vector3 neutralPosePosition;
    public Vector3 negativePosePosition;

    //The speed at which the pose sprite moves between its different positions
    public float poseLerpSpeed;

    public Vector3 characterTextBoxPosition;

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

    //The gameObject that moves whenever a character responds to something that the player has 
    //said.
    public GameObject feedbackObject;

    // Use this for initialization
    void Start () {
        nameText.GetComponent<Text>().text = null;
        portraitBackground.gameObject.SetActive(false);

        convosRemainingText.GetComponent<Text>().text = gameplayVariablesManager.GetComponent<GameplayVariablesManager>().conversationsRemaining.ToString() + " TALKS LEFT";

        valueDisplay.gameObject.SetActive(false);

        variableManager.SetValue(neutralYarnBool, new Yarn.Value(true));

        /*
        positivePosePosition = positivePose.GetComponent<Transform>().position;
        neutralPosePosition = neutralPose.GetComponent<Transform>().position;
        negativePosePosition = negativePose.GetComponent<Transform>().position;
        */
    }


    /// <summary>
    /// Activates this character's portrait display, and switches in their neutral portrait.
    /// </summary>
    [YarnCommand("activateNeutralPortrait")]
    public void ActivateNeutralPortrait()
    {
        MoveTextboxPosition();

        /*
        characterPortraitGameObject.GetComponent<Image>().sprite = neutral;
        characterPortraitGameObject.GetComponent<Image>().color = Color.white;

        portraitBackground.gameObject.SetActive(true);
        */
        nameText.GetComponent<Text>().text = this.gameObject.name;
    }

    /// <summary>
    /// Activates this character's portrait display, and switches in their happy portrait.
    /// </summary>
    [YarnCommand("activateHappyPortrait")]
    public void ActivateHappyPortrait()
    {
        MoveTextboxPosition();

        /*
        characterPortraitGameObject.GetComponent<Image>().sprite = happy;
        characterPortraitGameObject.GetComponent<Image>().color = Color.white;

        portraitBackground.gameObject.SetActive(true);
        */
        nameText.GetComponent<Text>().text = this.gameObject.name;
    }

    /// <summary>
    /// Activates this character's portrait display, and switches in their sad portrait.
    /// </summary>
    [YarnCommand("activateSadPortrait")]
    public void ActivateSadPortrait()
    {
        MoveTextboxPosition();

        /*
        characterPortraitGameObject.GetComponent<Image>().sprite = sad;
        characterPortraitGameObject.GetComponent<Image>().color = Color.white;

        portraitBackground.gameObject.SetActive(true);
        */
        nameText.GetComponent<Text>().text = this.gameObject.name;
    }

    /// <summary>
    /// Activates this character's portrait display, and switches in their angry portrait.
    /// </summary>
    [YarnCommand("activateAngryPortrait")]
    public void ActivateAngryPortrait()
    {
        MoveTextboxPosition();

        /*
        characterPortraitGameObject.GetComponent<Image>().sprite = angry;
        characterPortraitGameObject.GetComponent<Image>().color = Color.white;

        portraitBackground.gameObject.SetActive(true);
        */
        nameText.GetComponent<Text>().text = this.gameObject.name;
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

            //Causes the feedback image to change to the currect image and rise/fall to indicate a
            //change in the character's feelings
            feedbackObject.GetComponent<FeedbackImageManager>().SwitchToPositiveImage();
            feedbackObject.GetComponent<FeedbackImageManager>().StartReactionMotion();
        }
        else if(isPositiveChange == false)
        {
            value--;
            feedbackObject.GetComponent<FeedbackImageManager>().SwitchToNegativeImage();
            //The "secondhand reaction motion" function is called so that positive reactions always
            //appear before negative ones, grouping them so as not to overwhelm the player
            feedbackObject.GetComponent<FeedbackImageManager>().StartSecondhandReactionMotion();
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

            /*
            //Changes the character's pose to the "positive" pose
            positivePose.gameObject.SetActive(true);
            neutralPose.gameObject.SetActive(false);
            negativePose.gameObject.SetActive(false);
            */

            //Causes the pose sprite to Lerp to the needed position and change to the needed sprite
            float startTime = Time.time;
            float journeyLength = Vector3.Distance(currentPosePosition, positivePosePosition);
            float distanceCovered = (Time.time - startTime) * poseLerpSpeed;
            float fracJourney = distanceCovered / journeyLength;
            neutralPose.GetComponent<Transform>().position = Vector3.Lerp(currentPosePosition, positivePosePosition, fracJourney);
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

            /*
            //Changes the character's pose to the "positive" pose
            positivePose.gameObject.SetActive(true);
            neutralPose.gameObject.SetActive(false);
            negativePose.gameObject.SetActive(false);
            */

            //Causes the pose sprite to Lerp to the needed position and change to the needed sprite
            float startTime = Time.time;
            float journeyLength = Vector3.Distance(currentPosePosition, positivePosePosition);
            float distanceCovered = (Time.time - startTime) * poseLerpSpeed;
            float fracJourney = distanceCovered / journeyLength;
            neutralPose.GetComponent<Transform>().position = Vector3.Lerp(currentPosePosition, positivePosePosition, fracJourney);
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

            /*
            //Changes the character's pose to the "neutral" pose
            positivePose.gameObject.SetActive(false);
            neutralPose.gameObject.SetActive(true);
            negativePose.gameObject.SetActive(false);
            */

            //Causes the pose sprite to Lerp to the needed position and change to the needed sprite
            float startTime = Time.time;
            float journeyLength = Vector3.Distance(currentPosePosition, neutralPosePosition);
            float distanceCovered = (Time.time - startTime) * poseLerpSpeed;
            float fracJourney = distanceCovered / journeyLength;
            neutralPose.GetComponent<Transform>().position = Vector3.Lerp(currentPosePosition, neutralPosePosition, fracJourney);
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

            /*
            //Changes the character's pose to the "negative" pose
            positivePose.gameObject.SetActive(false);
            neutralPose.gameObject.SetActive(false);
            negativePose.gameObject.SetActive(true);
            */

            //Causes the pose sprite to Lerp to the needed position and change to the needed sprite
            float startTime = Time.time;
            float journeyLength = Vector3.Distance(currentPosePosition, negativePosePosition);
            float distanceCovered = (Time.time - startTime) * poseLerpSpeed;
            float fracJourney = distanceCovered / journeyLength;
            neutralPose.GetComponent<Transform>().position = Vector3.Lerp(currentPosePosition, negativePosePosition, fracJourney);
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

            /*
            //Changes the character's pose to the "negative" pose
            positivePose.gameObject.SetActive(false);
            neutralPose.gameObject.SetActive(false);
            negativePose.gameObject.SetActive(true);
            */

            //Causes the pose sprite to Lerp to the needed position and change to the needed sprite
            float startTime = Time.time;
            float journeyLength = Vector3.Distance(currentPosePosition, negativePosePosition);
            float distanceCovered = (Time.time - startTime) * poseLerpSpeed;
            float fracJourney = distanceCovered / journeyLength;
            neutralPose.GetComponent<Transform>().position = Vector3.Lerp(currentPosePosition, negativePosePosition, fracJourney);
        }
    }

    void MoveTextboxPosition()
    {
        dialogueText.GetComponent<Transform>().position = new Vector3(characterTextBoxPosition.x, characterTextBoxPosition.y, characterTextBoxPosition.z);
    }
}
