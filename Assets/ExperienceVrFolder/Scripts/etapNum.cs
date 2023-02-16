using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class etapNum : MonoBehaviour {

    public Material[] mats;

	// Use this for initialization
	void Start ()
    {
        if (PlayerPrefs.GetInt("mainMenu") == 0)
        {
            this.gameObject.GetComponent<MeshRenderer>().material = mats[0];
        }
        else
        {
            this.gameObject.GetComponent<MeshRenderer>().material = mats[1];
        }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
