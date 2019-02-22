﻿using System.Collections;
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
    public GameObject dialogueTextContainer;
    public GameObject dialogueText;

    //public GameObject positivePose;
    public GameObject characterPoseGameObject;
    //public GameObject negativePose;

    //The sprites that the pose image cycles through based on the character's mood
    public Sprite positivePoseImage;
    public Sprite neutralPoseImage;
    public Sprite negativePoseImage;

    //The sprite for the character's current pose, based on their current mood. This is stored so
    //that a character's pose can be overridden from its current mood-based pose and then changed 
    //back later to what it was before.
    Sprite defaultPoseImage;

    //These are used to store the locations of each of the above pose sprites, so they can be
    //lerped towards each other, creating the illusion of bodily movement
    public Vector3 positivePosePosition;
    public Vector3 neutralPosePosition;
    public Vector3 negativePosePosition;

    //The location of the character's pose sprite, based on their current mood. This is stored so
    //that a character's pose can be overridden from its current mood-based pose and then changed 
    //back later to what it was before.
    Vector3 defaultPosePosition;

    //Variables used to allow the lerp to function
    Vector3 currentStartingPosePosition;
    Vector3 currentDestinationPosePosition;
    public float poseLerpSpeed;
    float startTime;
    bool startNewLerp;
    bool lerpInProcess;
    float journeyLength;
    float distanceCovered;
    float fracJourney;

    public Vector3 characterTextBoxPosition;

    //Character variables handled directly in this script. Set initial values in the inspector!
    public int value;
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
    //public GameObject feedbackObject;

    //The fonts that characters can switch to and from based on if they're speaking as their D&D
    //characters or not.
    public Font inCharacterFont;
    public Font outOfCharacterFont;
    
    // Use this for initialization
    void Start () {
        nameText.GetComponent<Text>().text = null;
        portraitBackground.gameObject.SetActive(false);

        variableManager.SetValue(neutralYarnBool, new Yarn.Value(true));

        /*
        positivePosePosition = positivePose.GetComponent<Transform>().position;
        neutralPosePosition = neutralPose.GetComponent<Transform>().position;
        negativePosePosition = negativePose.GetComponent<Transform>().position;
        */

        startNewLerp = false;

        currentStartingPosePosition = neutralPosePosition;

        defaultPoseImage = neutralPoseImage;
        defaultPosePosition = neutralPosePosition;
    }

    void Update()
    {
        //Causes the pose sprite to Lerp to the needed position and change to the needed sprite
        if(lerpInProcess == true)
        {
            if(startNewLerp == true)
            {
                startTime = Time.time;
                startNewLerp = false;
            }

            journeyLength = Vector3.Distance(currentStartingPosePosition, currentDestinationPosePosition);
            Debug.Log(this.gameObject.name.ToString() + "'s Journey Length: " + journeyLength.ToString());

            distanceCovered = (Time.time - startTime) * poseLerpSpeed;
            Debug.Log(this.gameObject.name.ToString() + "'s Distance Covered: " + distanceCovered.ToString());

            fracJourney = distanceCovered / journeyLength;
            Debug.Log(this.gameObject.name.ToString() + "'s Frac Journey: " + fracJourney.ToString());

            characterPoseGameObject.GetComponent<Transform>().position = Vector3.Lerp(currentStartingPosePosition, currentDestinationPosePosition, fracJourney);

            //Stops the lerp
            if(characterPoseGameObject.GetComponent<Transform>().position == currentDestinationPosePosition)
            {
                lerpInProcess = false;
                currentStartingPosePosition = currentDestinationPosePosition;
            }
        }
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
    /// Changes a character's current pose without changing their mood.
    /// </summary>
    [YarnCommand("overridePose")]
    public void OverridePose(string newPose)
    {
        //NOTE: Currently these lines also cause the pose image to lerp to the appropriate
        //position. We may not need these in prototypes that have more finalized art,
        //possibly, maybe, IDK.
        if (newPose == "plusTwo")
        {
            currentDestinationPosePosition = positivePosePosition;
            characterPoseGameObject.GetComponent<Image>().sprite = positivePoseImage;
            startNewLerp = true;
            lerpInProcess = true;
        } else if (newPose == "plusOne")
        {
            currentDestinationPosePosition = positivePosePosition;
            characterPoseGameObject.GetComponent<Image>().sprite = positivePoseImage;
            startNewLerp = true;
            lerpInProcess = true;
        } else if(newPose == "zero")
        {
            currentDestinationPosePosition = neutralPosePosition;
            characterPoseGameObject.GetComponent<Image>().sprite = neutralPoseImage;
            startNewLerp = true;
            lerpInProcess = true;
        } else if(newPose == "minusOne")
        {
            currentDestinationPosePosition = negativePosePosition;
            characterPoseGameObject.GetComponent<Image>().sprite = negativePoseImage;
            startNewLerp = true;
            lerpInProcess = true;
        } else if(newPose == "minusTwo")
        {
            currentDestinationPosePosition = negativePosePosition;
            characterPoseGameObject.GetComponent<Image>().sprite = negativePoseImage;
            startNewLerp = true;
            lerpInProcess = true;
        }
    }

    /// <summary>
    /// Returns a character's current pose from a non-mood-based pose (set by the OverridePose 
    /// function) to their default mood-based pose
    /// </summary>
    [YarnCommand ("returnToDefaultPose")]
    public void ReturnToDefaultPose()
    {
        currentDestinationPosePosition = defaultPosePosition;
        characterPoseGameObject.GetComponent<Image>().sprite = defaultPoseImage;
        startNewLerp = true;
        lerpInProcess = true;
    }

    /// <summary>
    /// Changes the current font of the text based on whether a character is speaking "in-character"
    /// or "out-of-character" (with regards to the D&D game they're playing)
    /// </summary>
    /// <param name="font"></param>
    [YarnCommand ("changeFont")]
    public void ChangeFont(string font)
    {
        if (font == "inCharacter")
        {
            dialogueText.GetComponent<Text>().font = inCharacterFont;
        } else if (font == "outOfCharacter")
        {
            dialogueText.GetComponent<Text>().font = outOfCharacterFont;
        }
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
            //feedbackObject.GetComponent<FeedbackImageManager>().SwitchToPositiveImage();
            //feedbackObject.GetComponent<FeedbackImageManager>().StartReactionMotion();
        }
        else if(isPositiveChange == false)
        {
            value--;
            //feedbackObject.GetComponent<FeedbackImageManager>().SwitchToNegativeImage();
            //The "secondhand reaction motion" function is called so that positive reactions always
            //appear before negative ones, grouping them so as not to overwhelm the player
            //feedbackObject.GetComponent<FeedbackImageManager>().StartSecondhandReactionMotion();
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
            Debug.Log(this.gameObject.name + " is very pleased.");

            /*
            //Changes the character's pose to the "positive" pose
            positivePose.gameObject.SetActive(true);
            neutralPose.gameObject.SetActive(false);
            negativePose.gameObject.SetActive(false);
            */

            currentDestinationPosePosition = positivePosePosition;
            characterPoseGameObject.GetComponent<Image>().sprite = positivePoseImage;
            defaultPoseImage = positivePoseImage;   //Sets the new default pose image to this mood, in case we need to override this default image and then go back to it at any point
            defaultPosePosition = positivePosePosition; //Sets the new default pose image position to that of this mood, again, if we need to temporarily override it
            startNewLerp = true;
            lerpInProcess = true;

            /*
            //Causes the pose sprite to Lerp to the needed position and change to the needed sprite
            if(startNewLerp == true)
            {
                startTime = Time.time;
                startNewLerp = false;
            }
            journeyLength = Vector3.Distance(currentPosePosition, positivePosePosition);
            distanceCovered = (Time.time - startTime) * poseLerpSpeed;
            fracJourney = distanceCovered / journeyLength;
            neutralPose.GetComponent<Transform>().position = Vector2.Lerp(currentPosePosition, positivePosePosition, fracJourney);
            */
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
            Debug.Log(this.gameObject.name + " is somewhat pleased.");

            /*
            //Changes the character's pose to the "positive" pose
            positivePose.gameObject.SetActive(true);
            neutralPose.gameObject.SetActive(false);
            negativePose.gameObject.SetActive(false);
            */

            currentDestinationPosePosition = positivePosePosition;
            characterPoseGameObject.GetComponent<Image>().sprite = positivePoseImage;
            defaultPoseImage = positivePoseImage;   //Sets the new default pose image to this mood, in case we need to override this default image and then go back to it at any point
            defaultPosePosition = positivePosePosition; //Sets the new default pose image position to that of this mood, again, if we need to temporarily override it
            startNewLerp = true;
            lerpInProcess = true;

            /*
            //Causes the pose sprite to Lerp to the needed position and change to the needed sprite
            if(startNewLerp == true)
            {
                startTime = Time.time;
                startNewLerp = false;
            }
            journeyLength = Vector3.Distance(currentPosePosition, positivePosePosition);
            distanceCovered = (Time.time - startTime) * poseLerpSpeed;
            fracJourney = distanceCovered / journeyLength;
            neutralPose.GetComponent<Transform>().position = Vector2.Lerp(currentPosePosition, positivePosePosition, fracJourney);
            */
        }

        //Changes a variable in the Yarn sheet that activates "neutral" text for this character
        if(value == neutralValueThreshold)
        {
            variableManager.SetValue(veryPleasedYarnBool, new Yarn.Value(false));
            variableManager.SetValue(somewhatPleasedYarnBool, new Yarn.Value(false));
            variableManager.SetValue(neutralYarnBool, new Yarn.Value(true));
            variableManager.SetValue(somewhatAngryYarnBool, new Yarn.Value(false));
            variableManager.SetValue(veryAngryYarnBool, new Yarn.Value(false));
            Debug.Log(this.gameObject.name + " is feeling neutral.");

            /*
            //Changes the character's pose to the "neutral" pose
            positivePose.gameObject.SetActive(false);
            neutralPose.gameObject.SetActive(true);
            negativePose.gameObject.SetActive(false);
            */

            currentDestinationPosePosition = neutralPosePosition;
            characterPoseGameObject.GetComponent<Image>().sprite = neutralPoseImage;
            defaultPoseImage = neutralPoseImage;   //Sets the new default pose image to this mood, in case we need to override it and then go back to it at any point
            defaultPosePosition = neutralPosePosition; //Sets the new default pose image position to that of this mood, again, if we need to temporarily override it
            startNewLerp = true;
            lerpInProcess = true;

            /*
            //Causes the pose sprite to Lerp to the needed position and change to the needed sprite
            if(startNewLerp == true)
            {
                startTime = Time.time;
                startNewLerp = false;
            }
            journeyLength = Vector3.Distance(currentPosePosition, neutralPosePosition);
            distanceCovered = (Time.time - startTime) * poseLerpSpeed;
            fracJourney = distanceCovered / journeyLength;
            neutralPose.GetComponent<Transform>().position = Vector2.Lerp(currentPosePosition, neutralPosePosition, fracJourney);
            */
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
            Debug.Log(this.gameObject.name + " is somewhat angry.");

            /*
            //Changes the character's pose to the "negative" pose
            positivePose.gameObject.SetActive(false);
            neutralPose.gameObject.SetActive(false);
            negativePose.gameObject.SetActive(true);
            */

            currentDestinationPosePosition = negativePosePosition;
            characterPoseGameObject.GetComponent<Image>().sprite = negativePoseImage;
            defaultPoseImage = negativePoseImage;   //Sets the new default pose image to this mood, in case we need to override it and then go back to it at any point
            defaultPosePosition = negativePosePosition; //Sets the new default pose image position to that of this mood, again, if we need to temporarily override it
            startNewLerp = true;
            lerpInProcess = true;

            /*
            //Causes the pose sprite to Lerp to the needed position and change to the needed sprite
            if(startNewLerp == true)
            {
                startTime = Time.time;
                startNewLerp = false;
            }
            journeyLength = Vector3.Distance(currentPosePosition, negativePosePosition);
            distanceCovered = (Time.time - startTime) * poseLerpSpeed;
            fracJourney = distanceCovered / journeyLength;
            neutralPose.GetComponent<Transform>().position = Vector2.Lerp(currentPosePosition, negativePosePosition, fracJourney);
            */
        }

        //Changes a variable in the Yarn sheet that activates "very angry" text for this character
        if(value == veryAngryValueThreshold)
        {
            variableManager.SetValue(veryPleasedYarnBool, new Yarn.Value(false));
            variableManager.SetValue(somewhatPleasedYarnBool, new Yarn.Value(false));
            variableManager.SetValue(neutralYarnBool, new Yarn.Value(false));
            variableManager.SetValue(somewhatAngryYarnBool, new Yarn.Value(false));
            variableManager.SetValue(veryAngryYarnBool, new Yarn.Value(true));
            Debug.Log(this.gameObject.name + " is very angry.");

            /*
            //Changes the character's pose to the "negative" pose
            positivePose.gameObject.SetActive(false);
            neutralPose.gameObject.SetActive(false);
            negativePose.gameObject.SetActive(true);
            */

            currentDestinationPosePosition = negativePosePosition;
            characterPoseGameObject.GetComponent<Image>().sprite = negativePoseImage;
            defaultPoseImage = negativePoseImage;   //Sets the new default pose image to this mood, in case we need to override it and then go back to it at any point
            defaultPosePosition = negativePosePosition; //Sets the new default pose image position to that of this mood, again, if we need to temporarily override it
            startNewLerp = true;
            lerpInProcess = true;

            /*
            //Causes the pose sprite to Lerp to the needed position and change to the needed sprite
            if(startNewLerp == true)
            {
                startTime = Time.time;
                startNewLerp = false;
            }

            journeyLength = Vector3.Distance(currentPosePosition, negativePosePosition);
            distanceCovered = (Time.time - startTime) * poseLerpSpeed;
            fracJourney = distanceCovered / journeyLength;
            neutralPose.GetComponent<Transform>().position = Vector2.Lerp(currentPosePosition, negativePosePosition, fracJourney);
            */
        }
    }

    /// <summary>
    /// Moves the speech bubble over the head of the appropriate character.
    /// </summary>
    void MoveTextboxPosition()
    {
        dialogueTextContainer.GetComponent<Transform>().position = new Vector3(characterTextBoxPosition.x, characterTextBoxPosition.y, characterTextBoxPosition.z);
    }
}
