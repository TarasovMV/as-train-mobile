using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testTarget : MonoBehaviour {
    public Transform main;

	// Use this for initialization
	void Start () {
        this.gameObject.transform.LookAt(main);
        this.gameObject.transform.localScale = new Vector3(-this.gameObject.transform.localScale.x, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
        //this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1f, this.gameObject.transform.position.z);
    }
	
	// Update is called once per frame
	void Update () {
        
	}
}
