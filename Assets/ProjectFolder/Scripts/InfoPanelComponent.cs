using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPanelComponent : MonoBehaviour {

    public int number;

    private GameObject Scripts;

    private void Start()
    {
        Scripts = GameObject.Find("System");
        int.TryParse(gameObject.name, out number);
    }

    public void OnEnter()
    {
        Debug.Log("OnEnter");
        //Scripts.GetComponent<PanoController>().SetUiInfo($"РАБОТНИК {number}");
    }

    public void OnExit()
    {
        Debug.Log("OnExit");
        //Scripts.GetComponent<PanoController>().DisableUiInfo();
    }

    public void OnDrag()
    {

    }

    public void OnButtonClick()
    {

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
            if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
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
