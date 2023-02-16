using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceVrSceneButton : MonoBehaviour {
    public enum ExperienceVrSceneButtons
    {
        Forward,
        Back,
        Finish,
    }
    public ExperienceVrSceneButtons ButtonType;

    [Header("Активные кнопки")]
    public Sprite spriteForwardActive;
    public Sprite spriteBackActive;
    public Sprite spriteEndActive;

    [Header("Неактивные кнопки")]
    public Sprite spriteForwardUnactive;
    public Sprite spriteBackUnactive;
    public Sprite spriteEndUnactive;

    bool isClick;
    private ExperienceVrController experienceVrController;
    //private AdditionalVrController experienceVrController;

    void Start()
    {
        experienceVrController = GameObject.Find("Scripts").GetComponent<ExperienceVrController>();
        //experienceVrController = GameObject.Find("Scripts").GetComponent<AdditionalVrController>();
    }

    public void OnEnter()
    {
        Debug.Log("OnEnter");
        switch (ButtonType)
        {
            case ExperienceVrSceneButtons.Forward: //далее                
                this.gameObject.GetComponent<Image>().sprite = spriteForwardActive;
                break;
            case ExperienceVrSceneButtons.Back: //вернуться               
                this.gameObject.GetComponent<Image>().sprite = spriteBackActive;
                break;
            case ExperienceVrSceneButtons.Finish: //завершить
                this.gameObject.GetComponent<Image>().sprite = spriteEndActive;
                break;
        }

    }

    public void OnExit()
    {
        Debug.Log("OnExit");
        switch (ButtonType)
        {
            case ExperienceVrSceneButtons.Forward: //далее                
                this.gameObject.GetComponent<Image>().sprite = spriteForwardUnactive;
                break;
            case ExperienceVrSceneButtons.Back: //вернуться               
                this.gameObject.GetComponent<Image>().sprite = spriteBackUnactive;
                break;
            case ExperienceVrSceneButtons.Finish: //завершить
                this.gameObject.GetComponent<Image>().sprite = spriteEndUnactive;
                break;
        }
    }

    public void OnDrag()
    {

    }

    public void OnButtonClick()
    {
        Debug.Log("OnClick");
        StartCoroutine(waiting());
    }

    public IEnumerator waiting()
    {
        switch (ButtonType)
        {
            case ExperienceVrSceneButtons.Forward: //далее
                yield return new WaitForSeconds(0.5f);
                experienceVrController.ActivateSceneNext();
                break;
            case ExperienceVrSceneButtons.Back: //вернуться
                yield return new WaitForSeconds(0.5f);
                experienceVrController.ActivateScenePrevious();
                break;
            case ExperienceVrSceneButtons.Finish: //завершить
                experienceVrController.ActivateSceneFinish();
                break;
        }
        
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
