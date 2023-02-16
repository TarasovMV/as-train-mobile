using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nextPano : MonoBehaviour {
    public GameObject main;
    [Header("Активные кнопки")]
    public Sprite spriteEndActive;

    [Header("Неактивные кнопки")]
    public Sprite spriteEndUnactive;

    bool isClick = false;

    public void OnEnter()
    {
        Debug.Log("OnEnter");
        this.gameObject.GetComponent<Image>().sprite = spriteEndActive;
    }

    public void OnExit()
    {
        Debug.Log("OnExit");
        this.gameObject.GetComponent<Image>().sprite = spriteEndUnactive;
    }

    public void OnDrag()
    {

    }

    public void OnButtonClick()
    {
        main.GetComponent<pano360Script>().nextPano();
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
            //if ((OVRInput.Get(OVRInput.Button.PrimaryTouchpad)))
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
