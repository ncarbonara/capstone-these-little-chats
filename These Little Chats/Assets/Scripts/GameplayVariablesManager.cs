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

    //General variables
    public int conversationsRemaining;

    //Lance's variables
    public int lanceValue;
    public int lanceValueThreshhold;

    //Allison's variables
    public int allisonValue;
    public int allisonValueThreshhold;

    //Franklin's variables
    public int franklinValue;
    public int franklinValueThreshhold;

    //Ruby's variables
    public int rubyValue;
    public int rubyValueThreshhold;

    // Use this for initialization
    void Start () {
        lanceValue = 0;
        allisonValue = 0;
        franklinValue = 0;
        rubyValue = 0;
	}

}
