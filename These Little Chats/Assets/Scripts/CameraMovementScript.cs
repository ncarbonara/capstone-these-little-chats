using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Yarn.Unity;   //Lets us talk to Yarn stuff
using UnityEngine.UI;   //Lets us talk to UI stuff

public class CameraMovementScript : MonoBehaviour {

    bool gameStarted;

    bool startLerp;
    bool lerpInProcess;
    float lerpStartTime;
    float moveLerpJourneyLength;
    Vector3 moveLerpStartingPos;
    public Vector3 moveLerpDestinationPos;
    public float lerpSpeed;
    float moveLerpDistanceCovered;
    float lerpFracJourney;

    float startingSize;
    public float endingSize;
    float sizeLerpDifference;
    float sizeLerpDifferenceCovered;

    bool fadeBackground;
    public float backgroundFadeRate;

    public GameObject startScreenGameObjectsContainer;
    public GameObject gameplayUIButtonsContainer;
    public GameObject primaryDialogueRunner;
    public GameObject environmentSoundFXGameObject;
    public GameObject startScreenOpacityBackground;

    // Use this for initialization
    void Start () {

        gameStarted = false;

        startLerp = false;
        moveLerpStartingPos = this.GetComponent<Transform>().position;
        startingSize = this.GetComponent<Camera>().orthographicSize;
        fadeBackground = false;
	}

    // Update is called once per frame
    void Update()
    {

        if(Input.anyKeyDown
            && gameStarted == false)
        {
            if (Input.GetMouseButtonDown(0)
                || Input.GetMouseButtonDown(1)
                || Input.GetMouseButtonDown(2))
            {
                return;
            }
            else
            {
                GameStartCameraMovement();
            }
        }

        if(lerpInProcess == true)
        {
            if(startLerp == true)
            {
                lerpStartTime = Time.time;
                startScreenGameObjectsContainer.SetActive(false);
                startLerp = false;
            }

            moveLerpJourneyLength = Vector3.Distance(moveLerpStartingPos, moveLerpDestinationPos);
            sizeLerpDifference = Mathf.Abs(endingSize - startingSize);

            moveLerpDistanceCovered = (Time.time - lerpStartTime) * lerpSpeed;
            sizeLerpDifferenceCovered = (Time.time - lerpStartTime) * lerpSpeed;

            lerpFracJourney = moveLerpDistanceCovered / moveLerpJourneyLength;

            this.GetComponent<Transform>().position = Vector3.Lerp(moveLerpStartingPos, moveLerpDestinationPos, lerpFracJourney);
            this.GetComponent<Camera>().orthographicSize = Mathf.Abs((endingSize-startingSize) * lerpFracJourney) + startingSize;

            //Stops the lerp
            if(this.GetComponent<Camera>().orthographicSize >= endingSize)
            {
                this.GetComponent<Camera>().orthographicSize = endingSize;
                lerpInProcess = false;
                gameplayUIButtonsContainer.SetActive(true);
                primaryDialogueRunner.GetComponent<DialogueRunner>().StartDialogue();
            }

            if(fadeBackground == true)
            {
                float backgroundAlpha = startScreenOpacityBackground.GetComponent<Image>().color.a;
                backgroundAlpha -= backgroundFadeRate;
                startScreenOpacityBackground.GetComponent<Image>().color = new Color(0, 0, 0, backgroundAlpha);

                if (startScreenOpacityBackground.GetComponent<Image>().color.a <= 0)
                {
                    startScreenOpacityBackground.gameObject.SetActive(false);
                }

            }
        }
    }

        public void GameStartCameraMovement()
        {
            gameStarted = true;

            startLerp = true;
            lerpInProcess = true;

            fadeBackground = true;

            environmentSoundFXGameObject.GetComponent<SoundFXYarnCommands>().TriggerSound("doorKnock");
        }
}
