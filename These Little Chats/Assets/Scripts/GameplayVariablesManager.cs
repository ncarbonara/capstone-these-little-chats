using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayVariablesManager : MonoBehaviour {

    //Old prototype variables
    public int steveTrust;
    public int danaTrust;
    public int xanderTrust;
    public int keithTrustLevel;
    public int steveComfortWithBeingHere;

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
        steveTrust = 0;
        danaTrust = 0;
        xanderTrust = 0;
        keithTrustLevel = 0;

        lanceValue = 0;
        allisonValue = 0;
        franklinValue = 0;
        rubyValue = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
