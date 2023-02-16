using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceVrButtonComponent : MonoBehaviour {

    public enum PrepearedScenes
    {
        Set1,
        Set2,
        Set3,
        Set4,
        Additional
    }

    public Material[] mats;
    public PrepearedScenes prepearedScene;
    private GameObject Scripts;

    private void Start()
    {
        Scripts = GameObject.Find("Scripts");
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
        Scripts.GetComponent<ExperienceVrController>().SetPrepearedScene(prepearedScene);
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
