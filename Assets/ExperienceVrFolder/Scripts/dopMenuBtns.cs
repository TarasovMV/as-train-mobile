using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class dopMenuBtns : MonoBehaviour {

    public enum ExperienceVrPopupPanelButtonType
    {
        Exit,
        Return
    }

    public ExperienceVrPopupPanelButtonType ButtonType;
    public System.Action action;

    [Header("Активные кнопки")]
    public Sprite spriteBackActive;
    public Sprite spriteEndActive;

    [Header("Неактивные кнопки")]
    public Sprite spriteBackUnactive;
    public Sprite spriteEndUnactive;

    public void OnEnter()
    {
        Debug.Log("OnEnter");
        switch (ButtonType)
        {
            case ExperienceVrPopupPanelButtonType.Exit:
                this.gameObject.GetComponent<Image>().sprite = spriteEndActive;
                break;
            case ExperienceVrPopupPanelButtonType.Return:
                this.gameObject.GetComponent<Image>().sprite = spriteBackActive;
                break;
        }
    }

    public void OnExit()
    {
        Debug.Log("OnExit");
        switch (ButtonType)
        {
            case ExperienceVrPopupPanelButtonType.Exit:
                this.gameObject.GetComponent<Image>().sprite = spriteEndUnactive;
                break;
            case ExperienceVrPopupPanelButtonType.Return:
                this.gameObject.GetComponent<Image>().sprite = spriteBackUnactive;
                break;
        }
    }

    public void OnDrag()
    {

    }

    public void OnButtonClick()
    {
        Debug.Log("OnClick");
        action?.Invoke();
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
