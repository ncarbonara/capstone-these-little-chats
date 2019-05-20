using System.Collections;
using UnityEngine;

using UnityEngine.UI;   //Let's us talk to UI stuff.
using Yarn.Unity;   //Lets us talk to Yarn stuff.
using Yarn.Unity.Example;   //Lets us talk to the ModifiedExampleDialogueUI.cs script

/// <summary>
/// Contains all of the functions that can be called from a Yarn sheet to alter character values,
/// change portraits, etc.
/// Attach this script to each character gameObject in the scene (The gameObjects called "Lance,"
/// "Allison," etc).
/// </summary>
public class YarnCommands : MonoBehaviour {

    public VariableStorageBehaviour variableManager;
    
    //The main gameObjects for dialogue text
    public GameObject primaryDialogueTextContainer;
    public GameObject primaryDialogueText;
    public GameObject primaryDialogue;
    public GameObject primarySpeechBubble;
    public GameObject primaryNameText;

    //Secondary objects for dialogue text, used when TWO characters are speaking at once
    public GameObject secondaryDialogueTextContainer;
    public GameObject secondaryDialogueText;
    public GameObject secondaryDialogue;
    public GameObject secondarySpeechBubble;
    public GameObject secondaryNameText;

    //Tertiary objects for dialogue text, used when THREE characters are speaking at once
    public GameObject tertiaryDialogueTextContainer;
    public GameObject tertiaryDialogueText;
    public GameObject tertiaryDialogue;
    public GameObject tertiarySpeechBubble;
    public GameObject tertiaryNameText;

    //Tertiary objects for dialogue text, used when FOUR characters are speaking at once
    public GameObject quaternaryDialogueTextContainer;
    public GameObject quaternaryDialogueText;
    public GameObject quaternaryDialogue;
    public GameObject quaternarySpeechBubble;
    public GameObject quaternaryNameText;

    //Contains the fail screen
    public GameObject failScreenObjects;

    //The sprites that the pose image cycles through based on the character's mood
    /*
    public GameObject characterPoseGameObject;
    public Sprite veryHappyPoseImage;
    public Sprite happyPoseImage;
    public Sprite neutralPoseImage;
    public Sprite irritatedPoseImage;
    public Sprite veryAngryPoseImage;
    public Sprite rollingD20PoseImage;
    public Sprite shockedPoseImage;
    public Sprite dungeonMasterNarrationPoseImage;
    public Sprite rantingPoseImage;
    public Sprite sadPoseImage;
    */

    //The sprite for the character's current pose, based on their current mood. This is stored so
    //that a character's pose can be overridden from its current mood-based pose and then changed 
    //back later to what it was before.
    Sprite defaultPoseImage;

    //These are used to store the positions of each of the above poses, so they can be
    //lerped towards each other, creating the illusion of bodily movement
    /*
    public Vector3 veryHappyPosePosition;
    public Vector3 happyPosePosition;
    public Vector3 neutralPosePosition;
    public Vector3 irritatedPosePosition;
    public Vector3 veryAngryPosePosition;
    public Vector3 rollingD20PosePosition;
    public Vector3 shockedPosePosition;
    public Vector3 dungeonMasterNarrationPosePosition;
    public Vector3 rantingPosePosition;
    public Vector3 sadPosePosition;
    */

    //The position of the character's pose sprite, based on their current mood. This is stored so
    //that a character's pose can be overridden from its current mood-based pose and then changed 
    //back later to what it was before.
    Vector3 defaultPosePosition;

    //Variables used to allow the pose/expression lerp to function
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
    public GameObject veryHappyPoseGameObject;
    public GameObject happyPoseGameObject;
    public GameObject neutralPoseGameObject;
    public GameObject irritatedPoseGameObject;
    public GameObject veryAngryPoseGameObject;
    public GameObject rollingD20PoseGameObject;
    public GameObject uncomfortablePoseGameObject;
    public GameObject dungeonMasterNarrationPoseGameObject;
    public GameObject rantingPoseGameObject;
    public GameObject sadPoseGameObject;

    GameObject defaultPoseGameObject;
    GameObject newPoseGameObject;
    GameObject preD20RollPoseGameObject;

    //A bool used to determine when this script should fade-in/fade-out a character's different 
    //poses
    bool poseIsChanging;

    public float poseRateOfTransition;

    //Public variables used to handle the location and sprite used by the textbox for each character.
    public Vector3 characterTextBoxPosition;
    public Sprite characterSpeechBubbleSprite;
    public Sprite characterOffScreenSpeechBubbleSprite;
    public Sprite characterInterruptingSpeechBubbleSprite;
    public Color characterSpeechBubbleColor;


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

    //Variables and gameObjects needed for moving speech bubbles for visual juice
    public GameObject speechBubbleLerpStartMarker;
    public GameObject speechBubbleLerpEndMarker;
    public float speechBubbleLerpSpeed;
    float primarySpeechBubbleLerpStartTime;
    float primarySpeechBubbleLerpJourneyLength;
    float primarySpeechBubbleLerpDistanceCovered;
    float primarySpeechBubbleLerpFracJourney;
    bool startNewPrimarySpeechBubbleLerp;
    bool primarySpeechBubbleLerpInProcess;
    float secondarySpeechBubbleLerpStartTime;
    float secondarySpeechBubbleLerpJourneyLength;
    float secondarySpeechBubbleLerpDistanceCovered;
    float secondarySpeechBubbleLerpFracJourney;
    bool startNewSecondarySpeechBubbleLerp;
    bool secondarySpeechBubbleLerpInProcess;
    float tertiarySpeechBubbleLerpStartTime;
    float tertiarySpeechBubbleLerpJourneyLength;
    float tertiarySpeechBubbleLerpDistanceCovered;
    float tertiarySpeechBubbleLerpFracJourney;
    bool startNewTertiarySpeechBubbleLerp;
    bool tertiarySpeechBubbleLerpInProcess;
    float quaternarySpeechBubbleLerpStartTime;
    float quaternarySpeechBubbleLerpJourneyLength;
    float quaternarySpeechBubbleLerpDistanceCovered;
    float quaternarySpeechBubbleLerpFracJourney;
    bool startNewQuaternarySpeechBubbleLerp;
    bool quaternarySpeechBubbleLerpInProcess;

    //The game objects that handle sound effects and music, for functions that also need to trigger
    //FX or music in addition to their other operations
    public GameObject soundFXGameObject;
    public GameObject musicGameObject;
    GameObject d20RollSoundFXGameObject;

    //Variables used to help manage when people interrupt each other
    bool someoneWillInterrupt;
    float waitTimeBeforeInterrupting;
    string interruptionTextNodeName;
    string interruptionNumber;

    // Use this for initialization
    void Start() {
        primaryNameText.GetComponent<Text>().text = null;
        //portraitBackground.gameObject.SetActive(false);

        variableManager.SetValue(neutralYarnBool, new Yarn.Value(true));

        startNewPoseLerp = false;

        //currentStartingPosePosition = neutralPosePosition;

        //defaultPoseImage = neutralPoseImage;
        //defaultPosePosition = neutralPosePosition;

        defaultPoseGameObject = neutralPoseGameObject;
        preD20RollPoseGameObject = neutralPoseGameObject;

        poseIsChanging = false;
    }

    void Update()
    {
        //Causes the pose sprite to Lerp to the needed position and change to the needed sprite
        /*
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
        */

        //Moves the character's D20 die if they've just rolled it
        if(d20LerpInProcess == true)
        {
            if(startNewD20Lerp == true)
            {
                d20LerpStartTime = Time.time;
                startNewD20Lerp = false;
            }

            d20LerpJourneyLength = Vector3.Distance(d20RollLerpStartMarker.GetComponent<Transform>().position, d20RollLerpEndMarker.GetComponent<Transform>().position);

            d20LerpDistanceCovered = (Time.time - d20LerpStartTime) * d20LerpSpeed;

            d20LerpFracJourney = d20LerpDistanceCovered / d20LerpJourneyLength;

            d20.GetComponent<Transform>().position = Vector3.Lerp(d20RollLerpStartMarker.GetComponent<Transform>().position, d20RollLerpEndMarker.GetComponent<Transform>().position, d20LerpFracJourney);

            //Stops the lerp
            if(d20.GetComponent<Transform>().position == d20RollLerpEndMarker.GetComponent<Transform>().position)
            {
                d20LerpInProcess = false;
                StartCoroutine(DelayBeforeD20GoesAway());
            }
        }

        //Causes the PRIMARY speech bubble sprite to Lerp to the needed position when a character
        //begins speaking
        if(primarySpeechBubbleLerpInProcess == true)
        {
            if(startNewPrimarySpeechBubbleLerp == true)
            {
                primarySpeechBubbleLerpStartTime = Time.time;
                startNewPrimarySpeechBubbleLerp = false;
            }

            primarySpeechBubbleLerpJourneyLength = Vector3.Distance(speechBubbleLerpStartMarker.GetComponent<Transform>().position, speechBubbleLerpEndMarker.GetComponent<Transform>().position);

            primarySpeechBubbleLerpDistanceCovered = (Time.time - primarySpeechBubbleLerpStartTime) * speechBubbleLerpSpeed;

            primarySpeechBubbleLerpFracJourney = primarySpeechBubbleLerpDistanceCovered / primarySpeechBubbleLerpJourneyLength;

            primarySpeechBubble.GetComponent<Transform>().position = Vector3.Lerp(speechBubbleLerpStartMarker.GetComponent<Transform>().position, speechBubbleLerpEndMarker.GetComponent<Transform>().position, primarySpeechBubbleLerpFracJourney);
            primarySpeechBubble.GetComponent<Transform>().localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), primarySpeechBubbleLerpFracJourney);

            //Stops the lerp
            if(primarySpeechBubble.GetComponent<Transform>().position == speechBubbleLerpEndMarker.GetComponent<Transform>().position)
            {
                primarySpeechBubbleLerpInProcess = false;
            }
        }

        //Causes the SECONDARY speech bubble sprite to Lerp to the needed position when a character
        //begins speaking
        if(secondarySpeechBubbleLerpInProcess == true)
        {
            if(startNewSecondarySpeechBubbleLerp == true)
            {
                secondarySpeechBubbleLerpStartTime = Time.time;
                startNewSecondarySpeechBubbleLerp = false;
            }

            secondarySpeechBubbleLerpJourneyLength = Vector3.Distance(speechBubbleLerpStartMarker.GetComponent<Transform>().position, speechBubbleLerpEndMarker.GetComponent<Transform>().position);

            secondarySpeechBubbleLerpDistanceCovered = (Time.time - secondarySpeechBubbleLerpStartTime) * speechBubbleLerpSpeed;

            secondarySpeechBubbleLerpFracJourney = secondarySpeechBubbleLerpDistanceCovered / secondarySpeechBubbleLerpJourneyLength;

            secondarySpeechBubble.GetComponent<Transform>().position = Vector3.Lerp(speechBubbleLerpStartMarker.GetComponent<Transform>().position, speechBubbleLerpEndMarker.GetComponent<Transform>().position, secondarySpeechBubbleLerpFracJourney);
            secondarySpeechBubble.GetComponent<Transform>().localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), secondarySpeechBubbleLerpFracJourney);

            //Stops the lerp
            if(secondarySpeechBubble.GetComponent<Transform>().position == speechBubbleLerpEndMarker.GetComponent<Transform>().position)
            {
                secondarySpeechBubbleLerpInProcess = false;
            }
        }

        //Causes the TERTIARY speech bubble sprite to Lerp to the needed position when a character
        //begins speaking
        if(tertiarySpeechBubbleLerpInProcess == true)
        {
            if(startNewTertiarySpeechBubbleLerp == true)
            {
                tertiarySpeechBubbleLerpStartTime = Time.time;
                startNewTertiarySpeechBubbleLerp = false;
            }

            tertiarySpeechBubbleLerpJourneyLength = Vector3.Distance(speechBubbleLerpStartMarker.GetComponent<Transform>().position, speechBubbleLerpEndMarker.GetComponent<Transform>().position);

            tertiarySpeechBubbleLerpDistanceCovered = (Time.time - secondarySpeechBubbleLerpStartTime) * speechBubbleLerpSpeed;

            tertiarySpeechBubbleLerpFracJourney = tertiarySpeechBubbleLerpDistanceCovered / tertiarySpeechBubbleLerpJourneyLength;

            tertiarySpeechBubble.GetComponent<Transform>().position = Vector3.Lerp(speechBubbleLerpStartMarker.GetComponent<Transform>().position, speechBubbleLerpEndMarker.GetComponent<Transform>().position, tertiarySpeechBubbleLerpFracJourney);
            tertiarySpeechBubble.GetComponent<Transform>().localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), tertiarySpeechBubbleLerpFracJourney);

            //Stops the lerp
            if(tertiarySpeechBubble.GetComponent<Transform>().position == speechBubbleLerpEndMarker.GetComponent<Transform>().position)
            {
                tertiarySpeechBubbleLerpInProcess = false;
            }
        }

        //Causes the QUATERNARY speech bubble sprite to Lerp to the needed position when a character
        //begins speaking
        if(quaternarySpeechBubbleLerpInProcess == true)
        {
            if(startNewQuaternarySpeechBubbleLerp == true)
            {
                quaternarySpeechBubbleLerpStartTime = Time.time;
                startNewQuaternarySpeechBubbleLerp = false;
            }

            quaternarySpeechBubbleLerpJourneyLength = Vector3.Distance(speechBubbleLerpStartMarker.GetComponent<Transform>().position, speechBubbleLerpEndMarker.GetComponent<Transform>().position);

            quaternarySpeechBubbleLerpDistanceCovered = (Time.time - quaternarySpeechBubbleLerpStartTime) * speechBubbleLerpSpeed;

            quaternarySpeechBubbleLerpFracJourney = quaternarySpeechBubbleLerpDistanceCovered / quaternarySpeechBubbleLerpJourneyLength;

            quaternarySpeechBubble.GetComponent<Transform>().position = Vector3.Lerp(speechBubbleLerpStartMarker.GetComponent<Transform>().position, speechBubbleLerpEndMarker.GetComponent<Transform>().position, quaternarySpeechBubbleLerpFracJourney);
            quaternarySpeechBubble.GetComponent<Transform>().localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), quaternarySpeechBubbleLerpFracJourney);

            //Stops the lerp
            if(quaternarySpeechBubble.GetComponent<Transform>().position == speechBubbleLerpEndMarker.GetComponent<Transform>().position)
            {
                quaternarySpeechBubbleLerpInProcess = false;
            }
        }

        //Creates a delay until the point at which a character interrupts another
        if(someoneWillInterrupt == true)
        {
            StartCoroutine(WaitBeforeSomeoneInterrupts(waitTimeBeforeInterrupting));
            someoneWillInterrupt = false;
        }

        //Swaps out a character's sprites when needed
        if(poseIsChanging == true)
        {
            float oldPoseImageAlpha = defaultPoseGameObject.GetComponent<Image>().color.a;
            oldPoseImageAlpha -= poseRateOfTransition;
            if(oldPoseImageAlpha < 0f)
            {
                oldPoseImageAlpha = 0f;
            }
            defaultPoseGameObject.GetComponent<Image>().color = new Color(1, 1, 1, oldPoseImageAlpha);
            if(defaultPoseGameObject.GetComponent<Image>().color == new Color(1, 1, 1, 0))
            {
                defaultPoseGameObject.gameObject.SetActive(false);
                defaultPoseGameObject = newPoseGameObject;
            }

            newPoseGameObject.gameObject.SetActive(true);
            float newPoseImageAlpha = newPoseGameObject.GetComponent<Image>().color.a;
            newPoseImageAlpha += poseRateOfTransition;
            if(newPoseImageAlpha > 1f)
            {
                newPoseImageAlpha = 1f;
            }
            newPoseGameObject.GetComponent<Image>().color = new Color(1, 1, 1, newPoseImageAlpha);
        }
    }

    /// <summary>
    /// Moves the speech bubble over the head of the appropriate character.
    /// </summary>
    [YarnCommand("activateSpeechBubble")]
    public void ActivateSpeechBubble()
    {
        //Only starts the lerp if the speech bubble is not already where it needs to be
        if(primarySpeechBubble.GetComponent<Transform>().position != speechBubbleLerpEndMarker.GetComponent<Transform>().position)
        {
            //dialogueTextContainer.GetComponent<Transform>().position = new Vector3(characterTextBoxPosition.x, characterTextBoxPosition.y, characterTextBoxPosition.z);
            primarySpeechBubble.GetComponent<Transform>().position =
                new Vector3(speechBubbleLerpStartMarker.GetComponent<Transform>().position.x,
                speechBubbleLerpStartMarker.GetComponent<Transform>().position.y,
                speechBubbleLerpStartMarker.GetComponent<Transform>().position.z);
            primarySpeechBubble.GetComponent<Transform>().localScale = new Vector3(0, 0, 0);

            startNewPrimarySpeechBubbleLerp = true;
            primarySpeechBubbleLerpInProcess = true;
        }

        primarySpeechBubble.GetComponent<Image>().sprite = characterSpeechBubbleSprite;
        primarySpeechBubble.GetComponent<Image>().color = characterSpeechBubbleColor;
        primaryDialogueText.GetComponent<Text>().color = characterSpeechBubbleColor;
        primaryNameText.GetComponent<Text>().text = this.gameObject.name;
    }

    /// <summary>
    /// Tells Unity to pull up a second text bubble to show text from one person speaking at the 
    /// same time as another
    /// </summary>
    /// <param name="secondaryNodeName"></param>
    [YarnCommand("cueSimultaneousDialogue")]
    public void CueSimultaneousDialogue(string secondaryNodeName, string tertiaryNodeName, string quaternaryNodeName)
    {
        secondaryDialogue.GetComponent<DialogueRunner>().startNode = secondaryNodeName;
        secondaryDialogue.GetComponent<DialogueRunner>().StartDialogue();

        tertiaryDialogue.GetComponent<DialogueRunner>().startNode = tertiaryNodeName;
        tertiaryDialogue.GetComponent<DialogueRunner>().StartDialogue();

        quaternaryDialogue.GetComponent<DialogueRunner>().startNode = quaternaryNodeName;
        quaternaryDialogue.GetComponent<DialogueRunner>().StartDialogue();
    }

    /// <summary>
    /// Moves the speech bubble over the head of the appropriate SECOND character when that
    /// character and another character are speaking at the same time
    /// </summary>
    [YarnCommand("activateSimultaneousSpeechBubble")]
    public void ActivateSimultaneousSpeechBubble(string speakerNumber)
    {
        GameObject speechBubbleToMove = null;
        GameObject dialogueTextToUse = null;
        GameObject nameTextToUse = null;

        if(speakerNumber == "speakerTwo")
        {
            speechBubbleToMove = secondarySpeechBubble;
            dialogueTextToUse = secondaryDialogueText;
            nameTextToUse = secondaryNameText;
        }
        else if(speakerNumber == "speakerThree")
        {
            speechBubbleToMove = tertiarySpeechBubble;
            dialogueTextToUse = tertiaryDialogueText;
            nameTextToUse = tertiaryNameText;
        }
        else if(speakerNumber == "speakerFour")
        {
            speechBubbleToMove = quaternarySpeechBubble;
            dialogueTextToUse = quaternaryDialogueText;
            nameTextToUse = quaternaryNameText;
        }

        //Only starts the lerp if the speech bubble is not already where it needs to be
        if(speechBubbleToMove.GetComponent<Transform>().position != speechBubbleLerpEndMarker.GetComponent<Transform>().position)
        {
            //dialogueTextContainer.GetComponent<Transform>().position = new Vector3(characterTextBoxPosition.x, characterTextBoxPosition.y, characterTextBoxPosition.z);
            speechBubbleToMove.GetComponent<Transform>().position =
                new Vector3(speechBubbleLerpStartMarker.GetComponent<Transform>().position.x,
                speechBubbleLerpStartMarker.GetComponent<Transform>().position.y,
                speechBubbleLerpStartMarker.GetComponent<Transform>().position.z);
            speechBubbleToMove.GetComponent<Transform>().localScale = new Vector3(0, 0, 0);

            if(speakerNumber == "speakerTwo")
            {
                startNewSecondarySpeechBubbleLerp = true;
                secondarySpeechBubbleLerpInProcess = true;
            }
            else if(speakerNumber == "speakerThree")
            {
                startNewTertiarySpeechBubbleLerp = true;
                tertiarySpeechBubbleLerpInProcess = true;
            }
            else if(speakerNumber == "speakerFour")
            {
                startNewQuaternarySpeechBubbleLerp = true;
                quaternarySpeechBubbleLerpInProcess = true;
            }

            speechBubbleToMove.GetComponent<Image>().sprite = characterSpeechBubbleSprite;
            speechBubbleToMove.GetComponent<Image>().color = characterSpeechBubbleColor;
            dialogueTextToUse.GetComponent<Text>().color = characterSpeechBubbleColor;
            nameTextToUse.GetComponent<Text>().text = this.gameObject.name;
        }
    }

    /// <summary>
    /// Does the same thing as ActivateSimultaneousSpeechBubble() except with an offscreen speech
    /// bubble.
    /// NOTE: This code could probably stand to be more efficient with it's line use.
    /// </summary>
    [YarnCommand("activateOffScreenSimultaneousSpeechBubble")]
    public void ActivateOffScreenSimultaneousSpeechBubble(string speakerNumber)
    {
        GameObject speechBubbleToMove = null;
        GameObject dialogueTextToUse = null;
        GameObject nameTextToUse = null;

        if(speakerNumber == "speakerTwo")
        {
            speechBubbleToMove = secondarySpeechBubble;
            dialogueTextToUse = secondaryDialogueText;
            nameTextToUse = secondaryNameText;
        }
        else if(speakerNumber == "speakerThree")
        {
            speechBubbleToMove = tertiarySpeechBubble;
            dialogueTextToUse = tertiaryDialogueText;
            nameTextToUse = tertiaryNameText;
        }
        else if(speakerNumber == "speakerFour")
        {
            speechBubbleToMove = quaternarySpeechBubble;
            dialogueTextToUse = quaternaryDialogueText;
            nameTextToUse = quaternaryNameText;
        }

        //Only starts the lerp if the speech bubble is not already where it needs to be
        if(speechBubbleToMove.GetComponent<Transform>().position != speechBubbleLerpEndMarker.GetComponent<Transform>().position)
        {
            //dialogueTextContainer.GetComponent<Transform>().position = new Vector3(characterTextBoxPosition.x, characterTextBoxPosition.y, characterTextBoxPosition.z);
            speechBubbleToMove.GetComponent<Transform>().position =
                new Vector3(speechBubbleLerpStartMarker.GetComponent<Transform>().position.x,
                speechBubbleLerpStartMarker.GetComponent<Transform>().position.y,
                speechBubbleLerpStartMarker.GetComponent<Transform>().position.z);
            speechBubbleToMove.GetComponent<Transform>().localScale = new Vector3(0, 0, 0);

            if(speakerNumber == "speakerTwo")
            {
                startNewSecondarySpeechBubbleLerp = true;
                secondarySpeechBubbleLerpInProcess = true;
            }
            else if(speakerNumber == "speakerThree")
            {
                startNewTertiarySpeechBubbleLerp = true;
                tertiarySpeechBubbleLerpInProcess = true;
            }
            else if(speakerNumber == "speakerFour")
            {
                startNewQuaternarySpeechBubbleLerp = true;
                quaternarySpeechBubbleLerpInProcess = true;
            }

            speechBubbleToMove.GetComponent<Image>().sprite = characterOffScreenSpeechBubbleSprite;
            speechBubbleToMove.GetComponent<Image>().color = characterSpeechBubbleColor;
            dialogueTextToUse.GetComponent<Text>().color = characterSpeechBubbleColor;
            nameTextToUse.GetComponent<Text>().text = this.gameObject.name;
        }
    }

    /// <summary>
    /// Used to make one character interrupt another partway through a sentence
    /// </summary>
    /// <param name="nodeName"></param>
    /// <param name="waitForSecondsAmount"></param>
    [YarnCommand("someoneInterrupting")]
    public void SomeoneInterrupting(string nodeName, string waitForSecondsAmount, string number)
    {

        waitTimeBeforeInterrupting = float.Parse(waitForSecondsAmount);
        someoneWillInterrupt = true;
        interruptionTextNodeName = nodeName;
        interruptionNumber = number;

        /*
        if(interruptionNumber == "interruptionOne")
        {
            Vector3 tempSecondaryDialogueContainerPos = new Vector3(0, 0, 0);
            tempSecondaryDialogueContainerPos.x = characterTextBoxPosition.x + 1f;
            tempSecondaryDialogueContainerPos.x = characterTextBoxPosition.y - 0.25f;
            secondaryDialogueTextContainer.GetComponent<Transform>().position = tempSecondaryDialogueContainerPos;
        }
        else if(interruptionNumber == "interruptionTwo")
        {
            Vector3 tempTertiaryDialogueContainerPos = new Vector3(0, 0, 0);
            tempTertiaryDialogueContainerPos.x = characterTextBoxPosition.x + 1f;
            tempTertiaryDialogueContainerPos.x = characterTextBoxPosition.y - 0.25f;
            tertiaryDialogueTextContainer.GetComponent<Transform>().position = tempTertiaryDialogueContainerPos;
        }
        */
    }

    /// <summary>
    /// Cues the secondary speech bubble to aggressively cover up the primary speech bubble, so as
    /// to simulate the experience of one character interrupting another
    /// </summary>
    [YarnCommand("activateInterruptingSpeechBubble")]
    public void ActivateInterruptingSpeechBubble(string interruptionNumber)
    {
        if(interruptionNumber == "interruptionOne")
        {
            //secondarySpeechBubble.GetComponent<Transform>().position = new Vector3(primarySpeechBubble.GetComponent<Transform>().position.x, primarySpeechBubble.GetComponent<Transform>().position.y - 1.5f, primarySpeechBubble.GetComponent<Transform>().position.z);
           
            //Revised approach, where speech bubbles no longer overlap
            secondarySpeechBubble.GetComponent<Transform>().position =
                new Vector3(speechBubbleLerpStartMarker.GetComponent<Transform>().position.x,
                speechBubbleLerpStartMarker.GetComponent<Transform>().position.y,
                speechBubbleLerpStartMarker.GetComponent<Transform>().position.z);
            startNewSecondarySpeechBubbleLerp = true;
            secondarySpeechBubbleLerpInProcess = true;
            secondarySpeechBubble.GetComponent<Image>().sprite = characterSpeechBubbleSprite;

            //secondarySpeechBubble.GetComponent<Image>().sprite = characterInterruptingSpeechBubbleSprite;
            secondarySpeechBubble.GetComponent<Image>().color = characterSpeechBubbleColor;
            secondaryDialogueText.GetComponent<Text>().color = characterSpeechBubbleColor;
            secondaryNameText.GetComponent<Text>().text = this.gameObject.name;
        } else if (interruptionNumber == "interruptionTwo")
        {
            //tertiarySpeechBubble.GetComponent<Transform>().position = new Vector3(secondarySpeechBubble.GetComponent<Transform>().position.x, secondarySpeechBubble.GetComponent<Transform>().position.y - 1.5f, secondarySpeechBubble.GetComponent<Transform>().position.z);
           
            //Revised approach, where speech bubbles no longer overlap
            tertiarySpeechBubble.GetComponent<Transform>().position =
                new Vector3(speechBubbleLerpStartMarker.GetComponent<Transform>().position.x,
                speechBubbleLerpStartMarker.GetComponent<Transform>().position.y,
                speechBubbleLerpStartMarker.GetComponent<Transform>().position.z);
            startNewTertiarySpeechBubbleLerp = true;
            tertiarySpeechBubbleLerpInProcess = true;
            tertiarySpeechBubble.GetComponent<Image>().sprite = characterInterruptingSpeechBubbleSprite;

            //tertiarySpeechBubble.GetComponent<Image>().sprite = characterInterruptingSpeechBubbleSprite;
            tertiarySpeechBubble.GetComponent<Image>().color = characterSpeechBubbleColor;
            tertiaryDialogueText.GetComponent<Text>().color = characterSpeechBubbleColor;
            tertiaryNameText.GetComponent<Text>().text = this.gameObject.name;
        }
    }

    /// <summary>
    /// Does the same thing as the ActivateSpeechBubble() function, except with a speech bubble
    /// that's pointing off screen
    /// </summary>
    [YarnCommand("activateOffScreenSpeechBubble")]
    public void ActivateOffScreenSpeechBubble()
    {
        //Only starts the lerp if the speech bubble is not already where it needs to be
        if(primarySpeechBubble.GetComponent<Transform>().position != speechBubbleLerpEndMarker.GetComponent<Transform>().position)
        {
            //dialogueTextContainer.GetComponent<Transform>().position = new Vector3(characterTextBoxPosition.x, characterTextBoxPosition.y, characterTextBoxPosition.z);
            primarySpeechBubble.GetComponent<Transform>().position =
                new Vector3(speechBubbleLerpStartMarker.GetComponent<Transform>().position.x,
                speechBubbleLerpStartMarker.GetComponent<Transform>().position.y,
                speechBubbleLerpStartMarker.GetComponent<Transform>().position.z);
            primarySpeechBubble.GetComponent<Transform>().localScale = new Vector3(0, 0, 0);

            startNewPrimarySpeechBubbleLerp = true;
            primarySpeechBubbleLerpInProcess = true;
        }

        primarySpeechBubble.GetComponent<Image>().sprite = characterOffScreenSpeechBubbleSprite;
        primarySpeechBubble.GetComponent<Image>().color = characterSpeechBubbleColor;
        primaryDialogueText.GetComponent<Text>().color = characterSpeechBubbleColor;
        primaryNameText.GetComponent<Text>().text = this.gameObject.name;

        /*
        dialogueTextContainer.GetComponent<Transform>().position = new Vector3(characterTextBoxPosition.x, characterTextBoxPosition.y, characterTextBoxPosition.z);
        speechBubble.GetComponent<Image>().sprite = characterOffScreenSpeechBubbleSprite;
        speechBubble.GetComponent<Image>().color = characterSpeechBubbleColor;
        //nameText.GetComponent<Text>().color = characterSpeechBubbleColor;
        nameText.GetComponent<Text>().text = this.gameObject.name;
        */
    }

    /// <summary>
    /// Causes the character to appear or disappear from the screen on command. Currently, the
    /// function doesn't cause any fancy lerp or visual effect when the character appears, which
    /// maybe it should at some point.
    /// </summary>
    /// <param name="characterIsOnScreen"></param>
    [YarnCommand("showCharacter")]
    public void ShowCharacter(string characterIsOnScreen)
    {
        if(characterIsOnScreen == "true")
        {
            neutralPoseGameObject.SetActive(true);
        } else if(characterIsOnScreen == "false")
        {
            //neutralPoseGameObject.SetActive(false);
            poseIsChanging = false;
            defaultPoseGameObject.SetActive(false);
            newPoseGameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Changes a character's current pose without changing their mood.
    /// </summary>
    [YarnCommand("overridePose")]
    public void OverridePose(string newPose)
    {
        if(newPose == "veryHappy")
        {
            newPoseGameObject = veryHappyPoseGameObject;
            preD20RollPoseGameObject = veryHappyPoseGameObject;
            poseIsChanging = true;

            //OUTDATED LERP CODE
            /*
            currentDestinationPosePosition = positive2PosePosition;
            characterPoseGameObject.GetComponent<Image>().sprite = positive2PoseImage;
            startNewPoseLerp = true;
            poseLerpInProcess = true;
            */

        } else if(newPose == "happy")
        {
            newPoseGameObject = happyPoseGameObject;
            preD20RollPoseGameObject = happyPoseGameObject;
            poseIsChanging = true;

            //OUTDATED LERP CODE
            /*
            currentDestinationPosePosition = positive2PosePosition;
            characterPoseGameObject.GetComponent<Image>().sprite = positive1PoseImage;
            startNewPoseLerp = true;
            poseLerpInProcess = true;
            */
        } else if(newPose == "neutral")
        {
            newPoseGameObject = neutralPoseGameObject;
            preD20RollPoseGameObject = neutralPoseGameObject;
            poseIsChanging = true;

            //OUTDATED LERP CODE
            /*
            currentDestinationPosePosition = neutralPosePosition;
            characterPoseGameObject.GetComponent<Image>().sprite = neutralPoseImage;
            startNewPoseLerp = true;
            poseLerpInProcess = true;
            */
        }
        else if(newPose == "irritated")
        {
            newPoseGameObject = irritatedPoseGameObject;
            preD20RollPoseGameObject = irritatedPoseGameObject;
            poseIsChanging = true;

            //OUTDATED LERP CODE
            /*
            currentDestinationPosePosition = negative2PosePosition;
            characterPoseGameObject.GetComponent<Image>().sprite = negative1PoseImage;
            startNewPoseLerp = true;
            poseLerpInProcess = true;
            */
        }
        else if(newPose == "veryAngry")
        {
            newPoseGameObject = veryAngryPoseGameObject;
            preD20RollPoseGameObject = veryAngryPoseGameObject;
            poseIsChanging = true;

            //OUTDATED LERP CODE
            /*
            currentDestinationPosePosition = negative2PosePosition;
            characterPoseGameObject.GetComponent<Image>().sprite = negative2PoseImage;
            startNewPoseLerp = true;
            poseLerpInProcess = true;
            */
        } else if (newPose == "uncomfortable")
        {
            newPoseGameObject = uncomfortablePoseGameObject;
            preD20RollPoseGameObject = uncomfortablePoseGameObject;
            poseIsChanging = true;
        } else if(newPose == "dungeonMasterNarration")
        {
            newPoseGameObject = dungeonMasterNarrationPoseGameObject;
            preD20RollPoseGameObject = dungeonMasterNarrationPoseGameObject;
            poseIsChanging = true;
        } else if(newPose == "ranting")
        {
            newPoseGameObject = rantingPoseGameObject;
            preD20RollPoseGameObject = rantingPoseGameObject;
            poseIsChanging = true;
        } else if(newPose == "sad")
        {
            newPoseGameObject = sadPoseGameObject;
            preD20RollPoseGameObject = sadPoseGameObject;
            poseIsChanging = true;
        }
    }

    /// <summary>
    /// Returns a character's current pose from a non-mood-based pose (set by the OverridePose 
    /// function) to their default mood-based pose
    /// </summary>
    [YarnCommand("returnToDefaultPose")]
    public void ReturnToDefaultPose()
    {
        newPoseGameObject = defaultPoseGameObject;
        preD20RollPoseGameObject = defaultPoseGameObject;
        poseIsChanging = true;

        //OUTDATED LERP CODE
        /*
        currentDestinationPosePosition = defaultPosePosition;
        characterPoseGameObject.GetComponent<Image>().sprite = defaultPoseImage;
        startNewPoseLerp = true;
        poseLerpInProcess = true;
        */
    }

    /// <summary>
    /// Triggers the animation and sound that plays when the character rolls their D20 die
    /// </summary>
    [YarnCommand("rollD20")]
    public void RollD20()
    {
        soundFXGameObject.GetComponent<SoundFXYarnCommands>().TriggerSound("d20Roll");
        //d20RollSoundFXGameObject.GetComponent<SoundFXYarnCommands>().TriggerSound("d20Roll");
        newPoseGameObject = rollingD20PoseGameObject;
        poseIsChanging = true;
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
            primaryDialogueText.GetComponent<Text>().font = inCharacterFont;
            primaryDialogueText.GetComponent<Text>().fontSize = inCharacterFontSize;
        } else if (font == "outOfCharacter")
        {
            primaryDialogueText.GetComponent<Text>().font = outOfCharacterFont;
            primaryDialogueText.GetComponent<Text>().fontSize = outOfCharacterFontSize;
        }
    }

    [YarnCommand("changeFontSimultaneousDialogue")]
    public void ChangeFontSimultaneousDialogue(string font)
    {
        if(font == "inCharacter")
        {
            secondaryDialogueText.GetComponent<Text>().font = inCharacterFont;
            secondaryDialogueText.GetComponent<Text>().fontSize = inCharacterFontSize;
        }
        else if(font == "outOfCharacter")
        {
            secondaryDialogueText.GetComponent<Text>().font = outOfCharacterFont;
            secondaryDialogueText.GetComponent<Text>().fontSize = outOfCharacterFontSize;
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
    /// Sends the player to the fail screen
    /// </summary>
    [YarnCommand("sendToFailScreen")]
    public void SendToFailScreen()
    {
        musicGameObject.GetComponent<MusicYarnCommands>().FadeOutSound();
        failScreenObjects.SetActive(true);
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

            //Cutting pose change stuff, as pose changes are no longer tied to values
            //newPoseGameObject = veryHappyPoseGameObject;
            //preD20RollPoseGameObject = veryHappyPoseGameObject;

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

            //Cutting pose change stuff, as pose changes are no longer tied to values
            //newPoseGameObject = happyPoseGameObject;
            //preD20RollPoseGameObject = happyPoseGameObject;

            //OUTDATED LERP CODE
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

            //Cutting pose change stuff, as pose changes are no longer tied to values
            //newPoseGameObject = neutralPoseGameObject;
            //preD20RollPoseGameObject = neutralPoseGameObject;

            //OUTDATED LERP CODE
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

            //Cutting pose change stuff, as pose changes are no longer tied to values
            //newPoseGameObject = irritatedPoseGameObject;
            //preD20RollPoseGameObject = irritatedPoseGameObject;

            //OUTDATED LERP CODE
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

            //Cutting pose change stuff, as pose changes are no longer tied to values
            //newPoseGameObject = veryAngryPoseGameObject;
            //preD20RollPoseGameObject = veryAngryPoseGameObject;

            //OUTDATED LERP CODE
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

        poseIsChanging = true;
    }

    /// <summary>
    /// Used to put a delay on the amount of time it takes for the D20 to disappear from the screen
    /// after a character has rolled it
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayBeforeD20GoesAway()
    {
        yield return new WaitForSeconds(1f);
        d20.gameObject.SetActive(false);
        newPoseGameObject = preD20RollPoseGameObject;
        poseIsChanging = true;
        d20.GetComponent<Transform>().position = d20RollLerpStartMarker.GetComponent<Transform>().position;
    }

    /// <summary>
    /// Used to put a delay on the amount of time before someone interrupts someone else's line
    /// </summary>
    /// <param name="waitForSecondsAmount"></param>
    /// <returns></returns>
    IEnumerator WaitBeforeSomeoneInterrupts(float waitForSecondsAmount)
    {
        if(interruptionNumber == "interruptionOne")
        {
            //Blocks click-to-continue
            primaryDialogue.GetComponent<ModifiedExampleDialogueUI>().lineIsInterrupted = true;

            //Waits awhile until it's the person's time to interrupt
            yield return new WaitForSeconds(waitForSecondsAmount);

            //Activates the secondary dialogue bubble
            secondaryDialogue.GetComponent<DialogueRunner>().startNode = interruptionTextNodeName;
            secondaryDialogue.GetComponent<DialogueRunner>().StartDialogue();

            //Unblocks click-to-continue
            primaryDialogue.GetComponent<ModifiedExampleDialogueUI>().lineIsInterrupted = false;
        }
        else if(interruptionNumber == "interruptionTwo")
        {
            //Blocks click-to-continue
            secondaryDialogue.GetComponent<ModifiedExampleDialogueUI>().lineIsInterrupted = true;

            //Waits awhile until it's the person's time to interrupt
            yield return new WaitForSeconds(waitForSecondsAmount);

            //Activates the secondary dialogue bubble
            tertiaryDialogue.GetComponent<DialogueRunner>().startNode = interruptionTextNodeName;
            tertiaryDialogue.GetComponent<DialogueRunner>().StartDialogue();

            //Unblocks click-to-continue
            secondaryDialogue.GetComponent<ModifiedExampleDialogueUI>().lineIsInterrupted = false;
        }
    }
}
