using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackImageManager : MonoBehaviour {

    public Sprite positiveEmoji;
    public Sprite negativeEmoji;

    Vector2 higherPosition;
    Vector2 lowerPosition;

    Vector2 currentStartingPoint;
    Vector2 currentDestination;

    public float speed;

    float startTime;

    public float movementHeight;

    float distanceCovered;
    float journeyLength;
    float fracJourney;

    bool makeImageRise;
    bool makeImageFall;

    bool startNewLerp;

    public float timeBeforeImageFalls;
    public float timeBeforeSecondhandReactionsAppear;

    // Use this for initialization
    void Start () {
        //higherPosition = this.GetComponent<Transform>().position;
        lowerPosition = this.GetComponent<Transform>().position;

        //NOTE: Any more efficient ways to write these lines?
        //float lowerXPos = higherPosition.x;
        //float lowerYPos = higherPosition.y - movementHeight;
        //lowerPosition = new Vector2(lowerXPos, lowerYPos);
        float higherXPos = lowerPosition.x;
        float higherYPos = lowerPosition.y + movementHeight;
        higherPosition = new Vector2(higherXPos, higherYPos);

        startNewLerp = false;
        makeImageRise = false;
        makeImageFall = false;

        journeyLength = higherPosition.y - lowerPosition.y;

        StartReactionMotion();
    }

    void Update()
    {
        //if(makeImageFall == true)
        if(makeImageRise == true)
        {
            if(startNewLerp == true)
            {
                startTime = Time.time;
                startNewLerp = false;
            }

            //Debug.Log("I'm falling!");
            Debug.Log("I'm rising!");

            //currentStartingPoint = higherPosition;
            currentStartingPoint = lowerPosition;
            //currentDestination = lowerPosition;
            currentDestination = higherPosition;

            distanceCovered = (Time.time - startTime) * speed;
            fracJourney = distanceCovered / journeyLength;
            //this.GetComponent<Transform>().position = Vector2.Lerp(higherPosition, lowerPosition, fracJourney);
            this.GetComponent<Transform>().position = Vector2.Lerp(lowerPosition, higherPosition, fracJourney);

            //if(this.transform.position.y == lowerPosition.y)
            if(this.transform.position.y >= higherPosition.y)
            {
                this.transform.position = higherPosition;
                makeImageRise = false;
                startNewLerp = false;
                StartCoroutine(ImageDelayBeforeFalling());
            }

        }
        else if(makeImageFall == true)
        {
            Debug.Log("I'm Falling!");

            if(startNewLerp == true)
            {
                startTime = Time.time;
                startNewLerp = false;
            }

            currentStartingPoint = higherPosition;
            currentDestination = lowerPosition;

            distanceCovered = (Time.time - startTime) * speed;
            fracJourney = distanceCovered / journeyLength;
            this.GetComponent<Transform>().position = Vector2.Lerp(higherPosition, lowerPosition, fracJourney);

            if(this.transform.position.y <= lowerPosition.y)
            {
                this.transform.position = lowerPosition;
                makeImageFall = false;
                startNewLerp = false;
            }
        }
    }

    /// <summary>
    /// Called by Fungus to switch the reaction image to positive before StartReactionMotion() has been called.
    /// </summary>
    public void SwitchToPositiveImage()
    {
        this.GetComponent<Image>().sprite = positiveEmoji;
    }

    /// <summary>
    /// Called by Fungus to switch the reaction image to negative before StartReactionMotion() has been called.
    /// </summary>
    public void SwitchToNegativeImage()
    {
        this.GetComponent<Image>().sprite = negativeEmoji;
    }

    /// <summary>
    /// Called by Fungus to start the reaction motion, after SwitchToPositiveImage() or SwitchToNegativeImage() have been called.
    /// </summary>
    public void StartReactionMotion()
    {
        Debug.Log("Lerp Started.");
        startNewLerp = true;
        //makeImageFall = true;
        makeImageRise = true;
    }

    /// <summary>
    /// Called to cause a momentary delay before the image falls back down after rising.
    /// </summary>
    /// <returns></returns>
    public IEnumerator ImageDelayBeforeFalling()
    {
        Debug.Log("Waiting to fall...");
        yield return new WaitForSeconds(timeBeforeImageFalls);
        startNewLerp = true;
        //makeImageRise = true;
        makeImageFall = true;
    }

    public void StartSecondhandReactionMotion()
    {
        StartCoroutine(DelayBeforeSecondhandReactionImages());
    }

    public IEnumerator DelayBeforeSecondhandReactionImages()
    {
        yield return new WaitForSeconds(timeBeforeSecondhandReactionsAppear);
        StartReactionMotion();
    }
}