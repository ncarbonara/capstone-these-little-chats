using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;   //Let's us talk to UI stuff.
using Yarn.Unity;   //Lets us talk to Yarn stuff.

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
    //public GameObject gameplayVariablesManager;
    public GameObject characterPortraitGameObject;
    public GameObject nameText;
    //public GameObject portraitBackground;
    public GameObject dialogueTextContainer;
    public GameObject dialogueText;

    //The sprites that the pose image cycles through based on the character's mood
    public GameObject characterPoseGameObject;
    public Sprite positive2PoseImage;
    public Sprite positive1PoseImage;
    public Sprite neutralPoseImage;
    public Sprite negative1PoseImage;
    public Sprite negative2PoseImage;

    //The sprite for the character's current pose, based on their current mood. This is stored so
    //that a character's pose can be overridden from its current mood-based pose and then changed 
    //back later to what it was before.
    Sprite defaultPoseImage;

    //These are used to store the positions of each of the above poses, so they can be
    //lerped towards each other, creating the illusion of bodily movement
    public Vector3 positive2PosePosition;
    public Vector3 positive1PosePosition;
    public Vector3 neutralPosePosition;
    public Vector3 negative1PosePosition;
    public Vector3 negative2PosePosition;

    //The position of the character's pose sprite, based on their current mood. This is stored so
    //that a character's pose can be overridden from its current mood-based pose and then changed 
    //back later to what it was before.
    Vector3 defaultPosePosition;

    //Variables used to allow the lerp to function
    Vector3 currentStartingPosePosition;
    Vector3 currentDestinationPosePosition;
    public float poseLerpSpeed;
    float poseLerpStartTime;
    bool startNewPoseLerp;
    bool poseLerpInProcess;
    float poseLerpJourneyLength;
    float poseLerpDistanceCovered;
    float poseLerpFracJourney;

    //GameObjects for an alternative system of switching through pose sprites that uses multiple
    //gameObjects instead of just one
    public GameObject plusTwoPoseGameObject;
    public GameObject plusOnePoseGameObject;
    public GameObject zeroPoseGameObject;
    public GameObject minusOnePoseGameObject;
    public GameObject minusTwoPoseGameObject;

    GameObject defaultPoseGameObject;

    //Public variables used to handle the location and sprite used by the textbox for each character.
    public Vector3 characterTextBoxPosition;
    public GameObject speechBubble;
    public Sprite characterSpeechBubbleSprite;
    public Sprite characterOffScreenSpeechBubbleSprite;
    public Color characterNameColor;

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
    //said. No longer used, (but left commented out in case it's ever needed again)
    //public GameObject feedbackObject;

    //The "fonts" (actually typefaces) that characters can switch to and from based on if they're 
    //speaking as their D&D characters or not
    public Font inCharacterFont;
    public Font outOfCharacterFont;

    //The size of the fonts for in-character and out-of-character dialogue text, respectively
    public int inCharacterFontSize;
    public int outOfCharacterFontSize;

    //An object that will contain the last line of dialogue that a character said before the player
    //got to a choice selection point
    GameObject frozenDialogueText;

    //Variables and gameObjects needed for moving D20s onscreen
    public GameObject d20RollLerpStartMarker;
    public GameObject d20RollLerpEndMarker;
    public GameObject d20;
    public float d20LerpSpeed;
    float d20LerpStartTime;
    bool startNewD20Lerp;
    bool d20LerpInProcess;
    float d20LerpJourneyLength;
    float d20LerpDistanceCovered;
    float d20LerpFracJourney;

    //The game object that handles sound effects, for functions that also need to trigger some FX
    GameObject soundFXGameObject;

    // Use this for initialization
    void Start() {
        nameText.GetComponent<Text>().text = null;
        //portraitBackground.gameObject.SetActive(false);

        variableManager.SetValue(neutralYarnBool, new Yarn.Value(true));

        /*
        positivePosePosition = positivePose.GetComponent<Transform>().position;
        neutralPosePosition = neutralPose.GetComponent<Transform>().position;
        negativePosePosition = negativePose.GetComponent<Transform>().position;
        */

        startNewPoseLerp = false;

        currentStartingPosePosition = neutralPosePosition;

        defaultPoseImage = neutralPoseImage;
        defaultPosePosition = neutralPosePosition;

        defaultPoseGameObject = zeroPoseGameObject;

        soundFXGameObject = GameObject.FindGameObjectWithTag("SoundFXGameObject");
    }

    void Update()
    {
        //Causes the pose sprite to Lerp to the needed position and change to the needed sprite
        if(poseLerpInProcess == true)
        {
            if(startNewPoseLerp == true)
            {
                poseLerpStartTime = Time.time;
                startNewPoseLerp = false;
            }

            poseLerpJourneyLength = Vector3.Distance(currentStartingPosePosition, currentDestinationPosePosition);
            Debug.Log(this.gameObject.name.ToString() + "'s Journey Length: " + poseLerpJourneyLength.ToString());

            poseLerpDistanceCovered = (Time.time - poseLerpStartTime) * poseLerpSpeed;
            Debug.Log(this.gameObject.name.ToString() + "'s Distance Covered: " + poseLerpDistanceCovered.ToString());

            poseLerpFracJourney = poseLerpDistanceCovered / poseLerpJourneyLength;
            Debug.Log(this.gameObject.name.ToString() + "'s Frac Journey: " + poseLerpFracJourney.ToString());

            characterPoseGameObject.GetComponent<Transform>().position = Vector3.Lerp(currentStartingPosePosition, currentDestinationPosePosition, poseLerpFracJourney);

            //Stops the lerp
            if(characterPoseGameObject.GetComponent<Transform>().position == currentDestinationPosePosition)
            {
                poseLerpInProcess = false;
                currentStartingPosePosition = currentDestinationPosePosition;
            }
        }

        //Moves the character's D20 die if they've just rolled it
        if(d20LerpInProcess == true)
        {
            if(startNewD20Lerp == true)
            {
                d20LerpStartTime = Time.time;
                startNewD20Lerp = false;
            }

            d20LerpJourneyLength = Vector3.Distance(d20RollLerpStartMarker.GetComponent<Transform>().position, d20RollLerpEndMarker.GetComponent<Transform>().position);
            //Debug.Log(this.gameObject.name.ToString() + "'s Journey Length: " + journeyLength.ToString());

            d20LerpDistanceCovered = (Time.time - d20LerpStartTime) * d20LerpSpeed;
            //Debug.Log(this.gameObject.name.ToString() + "'s Distance Covered: " + distanceCovered.ToString());

            d20LerpFracJourney = d20LerpDistanceCovered / d20LerpJourneyLength;
            //Debug.Log(this.gameObject.name.ToString() + "'s Frac Journey: " + fracJourney.ToString());

            d20.GetComponent<Transform>().position = Vector3.Lerp(d20RollLerpStartMarker.GetComponent<Transform>().position, d20RollLerpEndMarker.GetComponent<Transform>().position, d20LerpFracJourney);

            //Stops the lerp
            if(d20.GetComponent<Transform>().position == d20RollLerpEndMarker.GetComponent<Transform>().position)
            {
                d20LerpInProcess = false;
                StartCoroutine(DelayBeforeD20GoesAway());
            }
        }
    }

    /// <summary>
    /// Moves the speech bubble over the head of the appropriate character.
    /// </summary>
    [YarnCommand("activateSpeechBubble")]
    public void ActivateSpeechBubble()
    {
        dialogueTextContainer.GetComponent<Transform>().position = new Vector3(characterTextBoxPosition.x, characterTextBoxPosition.y, characterTextBoxPosition.z);
        speechBubble.GetComponent<Image>().sprite = characterSpeechBubbleSprite;
        nameText.GetComponent<Text>().color = characterNameColor;
        nameText.GetComponent<Text>().text = this.gameObject.name;
    }

    /// <summary>
    /// Does the same thing as the ActivateSpeechBubble() function, except with a speech bubble
    /// that's pointing off screen
    /// </summary>
    [YarnCommand("activateOffScreenSpeechBubble")]
    public void ActivateOffScreenSpeechBubble()
    {
        dialogueTextContainer.GetComponent<Transform>().position = new Vector3(characterTextBoxPosition.x, characterTextBoxPosition.y, characterTextBoxPosition.z);
        speechBubble.GetComponent<Image>().sprite = characterOffScreenSpeechBubbleSprite;
        nameText.GetComponent<Text>().color = characterNameColor;
        nameText.GetComponent<Text>().text = this.gameObject.name;
    }

    /// <summary>
    /// Causes the character to appear or disappear from the screen on command. Currently, the
    /// function doesn't cause any fancy lerp or visual effect when the character appears, which
    /// maybe it should at some point.
    /// </summary>
    /// <param name="characterIsOnScreen"></param>
    [YarnCommand("showCharacter")]
    public void showCharacter(string characterIsOnScreen)
    {
        if(characterIsOnScreen == "true")
        {
            characterPoseGameObject.SetActive(true);
        } else if(characterIsOnScreen == "false")
        {
            characterPoseGameObject.SetActive(false);
        }
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
        if(newPose == "plusTwo")
        {
            currentDestinationPosePosition = positive2PosePosition;
            characterPoseGameObject.GetComponent<Image>().sprite = positive2PoseImage;
            startNewPoseLerp = true;
            poseLerpInProcess = true;
        } else if(newPose == "plusOne")
        {
            currentDestinationPosePosition = positive2PosePosition;
            characterPoseGameObject.GetComponent<Image>().sprite = positive1PoseImage;
            startNewPoseLerp = true;
            poseLerpInProcess = true;
        } else if(newPose == "zero")
        {
            currentDestinationPosePosition = neutralPosePosition;
            characterPoseGameObject.GetComponent<Image>().sprite = neutralPoseImage;
            startNewPoseLerp = true;
            poseLerpInProcess = true;
        } else if(newPose == "minusOne")
        {
            currentDestinationPosePosition = negative2PosePosition;
            characterPoseGameObject.GetComponent<Image>().sprite = negative1PoseImage;
            startNewPoseLerp = true;
            poseLerpInProcess = true;
        } else if(newPose == "minusTwo")
        {
            currentDestinationPosePosition = negative2PosePosition;
            characterPoseGameObject.GetComponent<Image>().sprite = negative2PoseImage;
            startNewPoseLerp = true;
            poseLerpInProcess = true;
        }
    }

    /// <summary>
    /// Returns a character's current pose from a non-mood-based pose (set by the OverridePose 
    /// function) to their default mood-based pose
    /// </summary>
    [YarnCommand("returnToDefaultPose")]
    public void ReturnToDefaultPose()
    {
        currentDestinationPosePosition = defaultPosePosition;
        characterPoseGameObject.GetComponent<Image>().sprite = defaultPoseImage;
        startNewPoseLerp = true;
        poseLerpInProcess = true;
    }

    //Triggers the animation and sound that plays when the character rolls their D20 die
    [YarnCommand("rollD20")]
    public void RollD20()
    {
        soundFXGameObject.GetComponent<SoundFXYarnCommands>().TriggerSound("d20Roll");
        d20.gameObject.SetActive(true);
        startNewD20Lerp = true;
        d20LerpInProcess = true;
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
            dialogueText.GetComponent<Text>().fontSize = inCharacterFontSize;
            dialogueText.GetComponent<Text>().fontStyle = FontStyle.Italic;
        } else if (font == "outOfCharacter")
        {
            dialogueText.GetComponent<Text>().font = outOfCharacterFont;
            dialogueText.GetComponent<Text>().fontSize = outOfCharacterFontSize;
            dialogueText.GetComponent<Text>().fontStyle = FontStyle.Normal;
        } else if(font == "internalMonologue")
        {
            dialogueText.GetComponent<Text>().font = outOfCharacterFont;
            dialogueText.GetComponent<Text>().fontSize = outOfCharacterFontSize;
            dialogueText.GetComponent<Text>().fontStyle = FontStyle.Italic;
        }
    }

    /// <summary>
    /// NOTE: This function doesn't currently work as desired. As such, all its code has been
    /// commented out for now, and an extra line added so that it now just clears the character name
    /// text offscreen.
    /// 
    /// Takes the previous line of dialogue text and keeps it onscreen by copying the object itself
    /// even though YarnSpinner clears the original. Should be called before every choice that
    /// appears onscreen.
    /// </summary>
    [YarnCommand("freezeDialogueText")]
    public void FreezeDialogueText()
    {
        nameText.GetComponent<Text>().text = " ";
        frozenDialogueText = Instantiate(dialogueTextContainer);
        GameObject background = GameObject.Find("Background");
        frozenDialogueText.transform.SetParent(background.GetComponent<Transform>());
        frozenDialogueText.GetComponent<Transform>().position = dialogueTextContainer.GetComponent<Transform>().position;
    }

    /// <summary>
    /// NOTE: This function doesn't currently work as desired. As such, all its code has been
    /// commented out for now.
    /// 
    /// Clears the previous line of dialogue from the screen once the player has chosen an option
    /// and the story continues. Should be called after every choice the player makes.
    /// </summary>
    [YarnCommand("unfreezeDialogueText")]
    public void UnfreezeDialogueText()
    {
        /*
        GameObject.Destroy(frozenDialogueText);
        */
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

            //Code for handling sprite transitions via activating gameObjects rather
            //than lerping a single gameObject
            float oldPoseImageAlpha = defaultPoseGameObject.GetComponent<Image>().color.a;
            oldPoseImageAlpha -= .01f;
            if (oldPoseImageAlpha < 0f)
            {
                oldPoseImageAlpha = 0f;
            }
            defaultPoseGameObject.GetComponent<Image>().color = new Color(1, 1, 1, oldPoseImageAlpha);
            if (defaultPoseGameObject.GetComponent<Image>().color == new Color(1, 1, 1, 0))
            {
                defaultPoseGameObject.gameObject.SetActive(false);
                defaultPoseGameObject = plusTwoPoseGameObject;
            }

            plusTwoPoseGameObject.gameObject.SetActive(true);
            float newPoseImageAlpha = plusTwoPoseGameObject.GetComponent<Image>().color.a;
            newPoseImageAlpha += .01f;
            if(newPoseImageAlpha > 1f)
            {
                newPoseImageAlpha = 1f;
            }
            plusTwoPoseGameObject.GetComponent<Image>().color = new Color(1, 1, 1, newPoseImageAlpha);

            //OUTDATED LERP CODE
            /*
            currentDestinationPosePosition = positive2PosePosition;
            characterPoseGameObject.GetComponent<Image>().sprite = positive2PoseImage;
            defaultPoseImage = positive2PoseImage;   //Sets the new default pose image to this mood, in case we need to override this default image and then go back to it at any point
            defaultPosePosition = positive2PosePosition; //Sets the new default pose image position to that of this mood, again, if we need to temporarily override it
            startNewPoseLerp = true;
            poseLerpInProcess = true;
            */

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

            //Code for handling sprite transitions via activating gameObjects rather
            //than lerping a single gameObject
            float oldPoseImageAlpha = defaultPoseGameObject.GetComponent<Image>().color.a;
            oldPoseImageAlpha -= .01f;
            if(oldPoseImageAlpha < 0f)
            {
                oldPoseImageAlpha = 0f;
            }
            defaultPoseGameObject.GetComponent<Image>().color = new Color(1, 1, 1, oldPoseImageAlpha);
            if(defaultPoseGameObject.GetComponent<Image>().color == new Color(1, 1, 1, 0))
            {
                defaultPoseGameObject.gameObject.SetActive(false);
                defaultPoseGameObject = plusOnePoseGameObject;
            }

            plusOnePoseGameObject.gameObject.SetActive(true);
            float newPoseImageAlpha = plusOnePoseGameObject.GetComponent<Image>().color.a;
            newPoseImageAlpha += .01f;
            if(newPoseImageAlpha > 1f)
            {
                newPoseImageAlpha = 1f;
            }
            plusOnePoseGameObject.GetComponent<Image>().color = new Color(1, 1, 1, newPoseImageAlpha);

            /*
            currentDestinationPosePosition = positive1PosePosition;
            characterPoseGameObject.GetComponent<Image>().sprite = positive1PoseImage;
            defaultPoseImage = positive1PoseImage;   //Sets the new default pose image to this mood, in case we need to override this default image and then go back to it at any point
            defaultPosePosition = positive1PosePosition; //Sets the new default pose image position to that of this mood, again, if we need to temporarily override it
            startNewPoseLerp = true;
            poseLerpInProcess = true;
            */

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

            //Code for handling sprite transitions via activating gameObjects rather
            //than lerping a single gameObject
            defaultPoseGameObject.gameObject.SetActive(false);
            zeroPoseGameObject.gameObject.SetActive(true);
            defaultPoseGameObject = zeroPoseGameObject;

            /*
            currentDestinationPosePosition = neutralPosePosition;
            characterPoseGameObject.GetComponent<Image>().sprite = neutralPoseImage;
            defaultPoseImage = neutralPoseImage;   //Sets the new default pose image to this mood, in case we need to override it and then go back to it at any point
            defaultPosePosition = neutralPosePosition; //Sets the new default pose image position to that of this mood, again, if we need to temporarily override it
            startNewPoseLerp = true;
            poseLerpInProcess = true;
            */

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

            //Code for handling sprite transitions via activating gameObjects rather
            //than lerping a single gameObject
            defaultPoseGameObject.gameObject.SetActive(false);
            minusOnePoseGameObject.gameObject.SetActive(true);
            defaultPoseGameObject = minusOnePoseGameObject;

            /*
            currentDestinationPosePosition = negative1PosePosition;
            characterPoseGameObject.GetComponent<Image>().sprite = negative1PoseImage;
            defaultPoseImage = negative1PoseImage;   //Sets the new default pose image to this mood, in case we need to override it and then go back to it at any point
            defaultPosePosition = negative1PosePosition; //Sets the new default pose image position to that of this mood, again, if we need to temporarily override it
            startNewPoseLerp = true;
            poseLerpInProcess = true;
            */

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

            //Code for handling sprite transitions via activating gameObjects rather
            //than lerping a single gameObject
            defaultPoseGameObject.gameObject.SetActive(false);
            minusOnePoseGameObject.gameObject.SetActive(true);
            defaultPoseGameObject = minusOnePoseGameObject;

            /*
            currentDestinationPosePosition = negative2PosePosition;
            characterPoseGameObject.GetComponent<Image>().sprite = negative2PoseImage;
            defaultPoseImage = negative2PoseImage;   //Sets the new default pose image to this mood, in case we need to override it and then go back to it at any point
            defaultPosePosition = negative2PosePosition; //Sets the new default pose image position to that of this mood, again, if we need to temporarily override it
            startNewPoseLerp = true;
            poseLerpInProcess = true;
            */

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
    /// Used to put a delay on the amount of time it takes for the D20 to disappear from the screen
    /// after a character has rolled it.
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayBeforeD20GoesAway()
    {
        yield return new WaitForSeconds(1f);
        d20.gameObject.SetActive(false);
        d20.GetComponent<Transform>().position = d20RollLerpStartMarker.GetComponent<Transform>().position;
    }

}
