using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour {

    public bool flagFirst;
    public bool flagOn;

	// Use this for initialization
	void Start ()
    {
        flagFirst = false;
        flagOn = false;	
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setFlags(bool first, bool on = true)
    {
        flagFirst = first;
        flagOn = on;
    }

}
