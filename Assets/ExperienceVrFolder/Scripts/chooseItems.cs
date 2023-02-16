using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chooseItems : MonoBehaviour, IChooseItem {

    public int number;
    public string name;
    public GameObject plashka;
    public GameObject commonData;
    public GameObject inObj;
    public GameObject inObjUse;

    public Color colorActive;
    public Color colorUnactive;
    public Color colorUnuse;
    public Color colorUse;

    public int type = 0;

    public bool isUse { get; set; } = false;
    public bool isChoose{ get; set; } = false;
    public bool isCursor { get; set; } = false;

    public void OnEnter()
    {
        Debug.Log("OnEnter");
        plashka.SetActive(true);
        this.gameObject.GetComponentInChildren<Text>().text = name;
        isCursor = true;
    }

    public void OnExit()
    {
        Debug.Log("OnExit");
        plashka.SetActive(false);
        isCursor = false;
    }

    public void OnDrag()
    {
        Vector3 startUgol = inObj.GetComponent<Transform>().rotation.eulerAngles;
        inObj.GetComponent<Transform>().rotation = Quaternion.Euler(startUgol.x, startUgol.y + 1, startUgol.z);
    }

    public void OnButtonClick()
    {
        Debug.Log("OnClick");
        fillPlaska();
    }

    void fillPlaska()
    {
        if(!isUse)
        {
            GameObject[] gObjs = GameObject.FindGameObjectsWithTag("dopObj");
            foreach (GameObject gObj in gObjs)
            {
                gObj.GetComponent<Transform>().parent.GetComponent<SpriteRenderer>().color = gObj.GetComponent<Transform>().parent.gameObject.GetComponent<chooseItems>().colorUnactive;
                gObj.GetComponent<Transform>().parent.GetComponent<IChooseItem>().isChoose = false;
            }
            this.gameObject.GetComponent<SpriteRenderer>().color = colorActive;
            this.gameObject.GetComponent<IChooseItem>().isChoose = true;
            commonData.GetComponent<commonData>().selectObj = inObjUse;
        }
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
