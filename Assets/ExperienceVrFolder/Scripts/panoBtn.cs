using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class panoBtn : MonoBehaviour {

    public GameObject dopMenu;

    [Header("Активные кнопки")]
    public Sprite spriteEndActive;

    [Header("Неактивные кнопки")]
    public Sprite spriteEndUnactive;

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
        Debug.Log("OnClick");
        dopMenu.SetActive(true);
    }

    void Start()
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
