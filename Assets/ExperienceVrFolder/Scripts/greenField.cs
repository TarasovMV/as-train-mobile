using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class greenField : MonoBehaviour, IGreenField {

    public int chooseNum { get; set; } = -1;
    public bool isCursor { get; set; } = false;
    public GameObject commonData;
    GameObject newSelectObj;
    GameObject newText;
    bool isClick;
    public GameObject activeTable;
    public int[] pointToScore;

    [Header("Стандартная позиция")]
    public float scaleSize = 1;
    public Vector3 pos;
    public Vector3 ugol;
    public Vector3 scaleVector = Vector3.zero;

    public void OnEnter()
    {
        Debug.Log("OnEnter");
        Color col = this.gameObject.GetComponent<MeshRenderer>().material.color;
        this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(col.r, col.g, col.b, 0.4583f);
        isCursor = true;
    }

    public void OnExit()
    {
        Debug.Log("OnExit");
        Color col = this.gameObject.GetComponent<MeshRenderer>().material.color;
        this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(col.r, col.g, col.b, 0);
        isCursor = false;
    }

    public void OnDrag()
    {

    }

    public void OnButtonClick()
    {
        Debug.Log("OnClick");

        if(chooseNum == -1)
        {
            if (commonData.GetComponent<commonData>().selectObj != null)
            {
                activeTable = commonData.GetComponent<commonData>().selectObj.GetComponent<Transform>().parent.gameObject;
                activeTable.GetComponent<chooseItems>().colorUnactive = activeTable.GetComponent<chooseItems>().colorUnuse;
                activeTable.GetComponent<chooseItems>().isUse = true;
                chooseNum = activeTable.GetComponent<chooseItems>().number;
                newSelectObj = Instantiate(commonData.GetComponent<commonData>().selectObj) as GameObject;
                newSelectObj.SetActive(true);
                newSelectObj.transform.parent = this.gameObject.transform;
                newSelectObj.transform.localRotation = Quaternion.Euler(ugol);
                newSelectObj.transform.localScale = scaleVector != Vector3.zero ? scaleVector : new Vector3(scaleSize, scaleSize, scaleSize);
                
                newSelectObj.transform.localPosition = pos;
                newSelectObj.tag = "Untagged";

                commonData.GetComponent<commonData>().selectObj = null;
                activeTable.GetComponent<SpriteRenderer>().color = activeTable.GetComponent<chooseItems>().colorUnactive;
            }
        }
        else
        {
            activeTable.GetComponent<chooseItems>().colorUnactive = activeTable.GetComponent<chooseItems>().colorUse;
            activeTable.GetComponent<SpriteRenderer>().color = activeTable.GetComponent<chooseItems>().colorUse;
            activeTable.GetComponent<chooseItems>().isUse = false;

            if (newSelectObj)
            {
                Destroy(newSelectObj);
            }

            activeTable = null;
            chooseNum = -1;
        }
    }

    void Start()
    {
        chooseNum = -1;
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
                if(!isClick)
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
