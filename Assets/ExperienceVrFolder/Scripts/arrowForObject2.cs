using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowForObject2 : MonoBehaviour {

    public bool arrowUp;
    public GameObject mainObj;
    bool isClick = false;
    public GameObject commonData;


    public void OnEnter()
    {
        Debug.Log("OnEnter");
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.85f, 0.85f, 0.85f);
    }

    public void OnExit()
    {
        Debug.Log("OnExit");
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
    }

    public void OnDrag()
    {

    }

    public void OnButtonClick()
    {
        Debug.Log("OnClick");

        if (arrowUp)
        {
            if (commonData.GetComponent<commonData>().rotCount <= 0)
            {
                commonData.GetComponent<commonData>().rotCount++;
                mainObj.transform.Rotate(-15, 0, 0);
            }    
        }
        else
        {
            if (commonData.GetComponent<commonData>().rotCount >= 0)
            {
                commonData.GetComponent<commonData>().rotCount--;
                mainObj.transform.Rotate(15, 0, 0);
            }
        }

    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        if (this.gameObject.GetComponent<InteractiveObject>().flagOn)
        {
            if (this.gameObject.GetComponent<InteractiveObject>().flagFirst)
            {
                OnEnter();
            }
            OnDrag();
            //if (Input.GetMouseButtonUp(0) || OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            if ((OVRInput.Get(OVRInput.Button.PrimaryTouchpad)))
            {
                if (!isClick)
                {
                    OnButtonClick();
                }

                isClick = true;
            }
            else
            {
                isClick = false;
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
