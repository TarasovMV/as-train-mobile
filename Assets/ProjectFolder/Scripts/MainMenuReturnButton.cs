using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuReturnButton : MonoBehaviour {

    public Material[] mats;

    private void Start()
    {
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
        SceneManager.LoadScene("MainMenu");
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
                //if (OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad))
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
