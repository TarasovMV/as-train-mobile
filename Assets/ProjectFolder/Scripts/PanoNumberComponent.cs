using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanoNumberComponent : MonoBehaviour
{
    public Material[] mats;
    public int number;

    private GameObject Scripts;

    private void Start()
    {
        Scripts = GameObject.Find("System");
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
        if (number == 0)
        {
            return;
        }

        Debug.Log("click" + number + "pano");
        Scripts.GetComponent<PanoController>().SelectPano(number - 1);
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
            //if (Input.GetMouseButtonUp(0) || OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
            if (OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad))
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
