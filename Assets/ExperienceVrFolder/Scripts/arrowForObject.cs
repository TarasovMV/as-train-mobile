using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class arrowForObject : MonoBehaviour {

    public bool arrowRight;
    public GameObject mainObj;

	
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
        if (arrowRight)
        {
            mainObj.GetComponent<Transform>().Rotate(0, 1f, 0);
        }
        else
        {
            mainObj.GetComponent<Transform>().Rotate(0, -1, 0);
        }
    }




    void Start()
    {

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            

        }
        

        if (this.gameObject.GetComponent<InteractiveObject>().flagOn)
        {
            if (this.gameObject.GetComponent<InteractiveObject>().flagFirst)
            {
                OnEnter();
            }
            OnDrag();
            //if (Input.GetMouseButtonUp(0) || OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
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
