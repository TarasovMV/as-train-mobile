using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dopMenu : MonoBehaviour {

    public GameObject mainSceneWindow;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (OVRInput.Get(OVRInput.Button.Back) || Input.GetKey(KeyCode.B))
        {
            Debug.Log("Back");
            mainSceneWindow.SetActive(true);
        }
    }
}
