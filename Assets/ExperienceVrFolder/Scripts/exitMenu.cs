using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exitMenu : MonoBehaviour {
    public TextMesh text;
    public Material[] mats;

	// Use this for initialization
	void Start () {
        text.text = PlayerPrefs.GetString("score");
    }
	

    public void OnEnter()
    {
        Debug.Log("OnEnter");
        this.gameObject.GetComponent<MeshRenderer>().material = mats[1];
    }

    public void OnExit()
    {
        Debug.Log("OnExit");
        this.gameObject.GetComponent<MeshRenderer>().material = mats[0];
    }

    public void OnDrag()
    {

    }

    public void OnButtonClick()
    {
        SceneManager.LoadScene("myMainMenu");
    }

    void Update()
    {
        if (this.gameObject.GetComponent<InteractiveObject>().flagOn)
        {
            if (this.gameObject.GetComponent<InteractiveObject>().flagFirst)
            {
                OnEnter();
            }
            OnDrag();
            if (Input.GetMouseButtonUp(0) || OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad))
            //if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
            {
                OnButtonClick();
            }
        }
        else
        {
            if (this.gameObject.GetComponent<InteractiveObject>().flagFirst)
            {
                OnExit();
                this.gameObject.GetComponent<InteractiveObject>().flagFirst = false;
            }
        }
    }
}
