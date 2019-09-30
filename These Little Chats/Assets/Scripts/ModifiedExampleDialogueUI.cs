/*

The MIT License (MIT)

Copyright (c) 2015-2017 Secret Lab Pty. Ltd. and Yarn Spinner contributors.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;

/*
"DON'T SPLIT THE PARTY," NICK AND BEN NOTE: 
This script is a MODIFIED version of the example script supplied by the fine people who wrote the 
above license. It includes new lines of code by us, some of which are marked with our names in
a comment, and some of which are not.
*/

namespace Yarn.Unity.Example
{
    /// Displays dialogue lines to the player, and sends
    /// user choices back to the dialogue system.

    /** Note that this is just one way of presenting the
     * dialogue to the user. The only hard requirement
     * is that you provide the RunLine, RunOptions, RunCommand
     * and DialogueComplete coroutines; what they do is up to you.
     */
    public class ModifiedExampleDialogueUI : Yarn.Unity.DialogueUIBehaviour
    {

        /// The object that contains the dialogue and the options.
        /** This object will be enabled when conversation starts, and 
         * disabled when it ends.
         */
        public GameObject dialogueContainer;

        /// The UI element that displays lines
        public Text lineText;

        //The UI image that acts as a background for the line text above
        public GameObject lineTextBackground;

        /// A UI element that appears after lines have finished appearing
        public GameObject continuePrompt;

        /// A delegate (ie a function-stored-in-a-variable) that
        /// we call to tell the dialogue system about what option
        /// the user selected
        private Yarn.OptionChooser SetSelectedOption;

        /// How quickly to show the text, in seconds per character
        [Tooltip("How quickly to show the text, in seconds per character")]
        public float textSpeed = 0.025f;

        /// The buttons that let the user choose an option
        public List<Button> optionButtons;

        /// Make it possible to temporarily disable the controls when
        /// dialogue is active and to restore them when dialogue ends
        public RectTransform gameControlsContainer;

        public GameObject conversationLog;
        public GameObject conversationLogButton;
        public GameObject environmentSoundFXGameObject;
        public bool conversationLogButtonIsBeingHoveredOver;

        public bool lineIsInterrupted;
        public bool soundEffectIsPlaying;

        public int timesContinuePromptWasShown;

        void Awake()
        {
            // Start by hiding the container, line and option buttons
            if(dialogueContainer != null)
                dialogueContainer.SetActive(false);

            lineText.gameObject.SetActive(false);
            lineTextBackground.gameObject.SetActive(false);

            foreach(var button in optionButtons)
            {
                button.gameObject.SetActive(false);
            }

            // Hide the continue prompt if it exists
            if(continuePrompt != null)
                continuePrompt.SetActive(false);

            conversationLogButtonIsBeingHoveredOver = false;

            timesContinuePromptWasShown = 0;
        }

        /// Show a line of dialogue, gradually
        public override IEnumerator RunLine(Yarn.Line line)
        {

                // Show the text
                lineText.gameObject.SetActive(true);
                lineTextBackground.gameObject.SetActive(true);


                if(textSpeed > 0.0f)
                {
                    // Display the line one character at a time
                    var stringBuilder = new StringBuilder();

                    foreach(char c in line.text)
                    {
                        stringBuilder.Append(c);
                        lineText.text = stringBuilder.ToString();
                        yield return new WaitForSeconds(textSpeed);
                    }

                    //Once the line is finished displaying, it is recorded in the conversation log
                    conversationLog.GetComponent<ConversationLogScript>().AddToConversationLog(this.gameObject);

                }
                else
                {
                    // Display the line immediately if textSpeed == 0
                    lineText.text = line.text;

                    //Once the line is finished displaying, it is recorded in the conversation log
                    conversationLog.GetComponent<ConversationLogScript>().AddToConversationLog(this.gameObject);
                }

                // Show the 'press any key' prompt when done, if we have one.
                //Modified so that it only appears for the first three lines of dialogue the player
                //sees.
                if(continuePrompt != null
                    && timesContinuePromptWasShown < 1)
                    continuePrompt.SetActive(true);
                    timesContinuePromptWasShown++;

            // Wait for any user input
            while(Input.anyKeyDown == false)
            {

                yield return null;
            }

            //Prevents the dialogue from moving forward while the conversation log is open
            while(conversationLog.activeInHierarchy == true)
            {
                yield return null;
            }

            //DOESN'T WORK
            //Prevents the dialogue from moving forward while the mouse is hovering over the
            //conversation log button
            /*
            while(conversationLogButton.GetComponent<ButtonHoverChecker>().mouseHoveringOverButton == true)
            {
              yield return null;
            }
            */

            //Prevents the dialogue from moving forward when there's an interrupting line of
            //dialogue that comes after it
            while(lineIsInterrupted == true)
            {
                yield return null;
            }

            //Prevents dialogue from continuing until a sound effect has stopped playing
            while (soundEffectIsPlaying == true)
            {
                yield return null;
            }

            //COMMENTED OUT UNTIL FULLY FUNCTIONAL
            /*
            //While a line of text still hasn't been changed entirely to white, don't let the
            //player move to the next line of dialogue
            while(lineText.GetComponent<TextGradualVisibilityScript>().allTextShown == false)
            {
                yield return null;
            }
            */

            //DELETE WHEN ABOVE IS FIXED
            //For some reason, this code keeps the player from accidentally skipping forward too
            //many lines of dialogue
            if(lineText.GetComponent<TextGradualVisibilityScript>().allTextShown == false)
            {
                yield return null;
            }


            //SOME NICK CODE
            if(Input.anyKeyDown == true
                && lineText.GetComponent<TextGradualVisibilityScript>().allTextShown == true)
            {
                lineText.GetComponent<TextGradualVisibilityScript>().ResetTextColor();
                //lineText.GetComponent<TextGradualVisibilityScript>().resetTextColor = true;
            }

            //COMMENTED OUT UNTIL FUNCTIONAL
            //Updates the conversation log when the player presses to continue through text
            //(in order to make added lines appear in the correct order in the log)
            /*
            if(Input.anyKeyDown == true)
            {
                //conversationLog.GetComponent<ConversationLogScript>().LoadToConversationLog();
            }
            */


            // Hide the text and prompt
            lineText.gameObject.SetActive(false);
            lineTextBackground.gameObject.SetActive(false);

            if(continuePrompt != null)
                continuePrompt.SetActive(false);
        }

        /// Show a list of options, and wait for the player to make a selection.
        public override IEnumerator RunOptions(Yarn.Options optionsCollection,
                                                Yarn.OptionChooser optionChooser)
        {
            // Do a little bit of safety checking
            if(optionsCollection.options.Count > optionButtons.Count)
            {
                Debug.LogWarning("There are more options to present than there are" +
                                 "buttons to present them in. This will cause problems.");
            }

            // Display each option in a button, and make it visible
            int i = 0;
            foreach(var optionString in optionsCollection.options)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].GetComponentInChildren<Text>().text = optionString;
                i++;
            }

            // Record that we're using it
            SetSelectedOption = optionChooser;

            // Wait until the chooser has been used and then removed (see SetOption below)
            while(SetSelectedOption != null)
            {
                yield return null;
            }

            // Hide all the buttons
            foreach(var button in optionButtons)
            {
                button.gameObject.SetActive(false);
            }
        }

        /// Called by buttons to make a selection.
        public void SetOption(int selectedOption)
        {

            // Call the delegate to tell the dialogue system that we've
            // selected an option.
            SetSelectedOption(selectedOption);

            // Now remove the delegate so that the loop in RunOptions will exit
            SetSelectedOption = null;
        }

        /// Run an internal command.
        public override IEnumerator RunCommand(Yarn.Command command)
        {
            // "Perform" the command
            Debug.Log("Command: " + command.text);

            yield break;
        }

        /// Called when the dialogue system has started running.
        public override IEnumerator DialogueStarted()
        {
            Debug.Log("Dialogue starting!");

            // Enable the dialogue controls.
            if(dialogueContainer != null)
                dialogueContainer.SetActive(true);

            // Hide the game controls.
            if(gameControlsContainer != null)
            {
                gameControlsContainer.gameObject.SetActive(false);
            }

            yield break;
        }

        /// Called when the dialogue system has finished running.
        public override IEnumerator DialogueComplete()
        {
            //Modified from example to show which dialogue runner is claiming to be complete
            Debug.Log(this.gameObject.name + ": Complete!");

            // Hide the dialogue interface.
            if(dialogueContainer != null)
                dialogueContainer.SetActive(false);

            // Show the game controls.
            if(gameControlsContainer != null)
            {
                gameControlsContainer.gameObject.SetActive(true);
            }

            yield break;
        }

    }

}