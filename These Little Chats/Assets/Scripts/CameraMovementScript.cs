using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour {



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

    // Use this for initialization
    void Start () {
        startLerp = false;
        moveLerpStartingPos = this.GetComponent<Transform>().position;
        startingSize = this.GetComponent<Camera>().orthographicSize;
	}

    // Update is called once per frame
    void Update()
    {
        if(lerpInProcess == true)
        {
            if(startLerp == true)
            {
                lerpStartTime = Time.time;
                startLerp = false;
            }


            moveLerpJourneyLength = Vector3.Distance(moveLerpStartingPos, moveLerpDestinationPos);
            sizeLerpDifference = Mathf.Abs(endingSize - startingSize);

            moveLerpDistanceCovered = (Time.time - lerpStartTime) * lerpSpeed;
            sizeLerpDifference = (Time.time - lerpStartTime) * lerpSpeed;

            lerpFracJourney = moveLerpDistanceCovered / moveLerpJourneyLength;

            this.GetComponent<Transform>().position = Vector3.Lerp(moveLerpStartingPos, moveLerpDestinationPos, lerpFracJourney);
            this.GetComponent<Camera>().orthographicSize = Mathf.Abs((endingSize-startingSize)/lerpFracJourney);

            //Stops the lerp
            if(this.GetComponent<Transform>().position == moveLerpDestinationPos)
            {
                lerpInProcess = false;
            }
        }
    }

        public void GameStartCameraMovement()
        {
            startLerp = true;
            lerpInProcess = true;
        }
}
