using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains variables for each of the character's values, the thresholds these varaibles need to
/// hit for things to change in the branching narrative, as well as the variable containing how
/// many conversations are remaining before the game ends.
/// Variables from this script are accessed and changed during gameplay by YarnCommands.cs.
/// </summary>
public class GameplayVariablesManager : MonoBehaviour {

    //The number of conversations remaining. 
    //Set the initial value in the inspector!
    public int conversationsRemaining;
}
