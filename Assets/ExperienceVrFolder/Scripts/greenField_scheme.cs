using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class greenField_scheme : MonoBehaviour, IGreenField {

    public int chooseNum { get; set; } = -1;
    public bool isCursor { get; set; } = false;
    public GameObject commonData;
    public int[] pointToScore;
    public Material empty;
    public int side;

    private float kf = 1.57f;
    
    [HideInInspector]
    public GameObject activeTable;
    [HideInInspector]
    public bool isUse = false;
    bool isClick;

    private void Start()
    {
        chooseNum = -1;
        empty = this.gameObject.GetComponent<MeshRenderer>().materials[0];
    }

    public void OnEnter()
    {
        Debug.Log("OnEnter");
        Color col = this.gameObject.GetComponent<MeshRenderer>().materials[1].color;
        this.gameObject.GetComponent<MeshRenderer>().materials[1].color = new Color(col.r, col.g, col.b, 0.4583f);
        isCursor = true;
    }

    public void OnExit()
    {
        Debug.Log("OnExit");
        Color col = this.gameObject.GetComponent<MeshRenderer>().materials[1].color;
        this.gameObject.GetComponent<MeshRenderer>().materials[1].color = new Color(col.r, col.g, col.b, 0);
        isCursor = false;
    }

    public void OnDrag()
    {

    }

    public void OnButtonClick()
    {
        Debug.Log("OnClick");
        if (chooseNum == -1)
        {
            if (commonData.GetComponent<commonData>().selectObj != null)
            {
                activeTable = commonData.GetComponent<commonData>().selectObj;
                activeTable.GetComponent<chooseItems_schema>().colorUnactive = activeTable.GetComponent<chooseItems_schema>().colorUnuse;
                activeTable.GetComponent<chooseItems_schema>().isUse = true;
                chooseNum = activeTable.GetComponent<chooseItems_schema>().number;
                activeTable.GetComponent<SpriteRenderer>().color = activeTable.GetComponent<chooseItems_schema>().colorUnactive;

                switch (side)
                {
                    case 0:
                        this.gameObject.GetComponent<MeshRenderer>().material = activeTable.GetComponent<chooseItems_schema>().schemeMat[0];
                        break;
                    case 2:
                        this.gameObject.GetComponent<MeshRenderer>().material = activeTable.GetComponent<chooseItems_schema>().schemeMat[1];
                        break;
                    case 1:
                        this.gameObject.GetComponent<MeshRenderer>().material = activeTable.GetComponent<chooseItems_schema>().schemeMat[2];
                        break;
                    case (-1):
                        this.gameObject.GetComponent<MeshRenderer>().material = activeTable.GetComponent<chooseItems_schema>().schemeMat[3];
                        break;
                }
                this.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Angle", side * kf);

                commonData.GetComponent<commonData>().selectObj = null;

                isUse = true;

                //включение и выключение полей схемы
                foreach (GameObject field in commonData.GetComponent<commonData>().massPoints)
                {
                    if (field.GetComponent<greenField_scheme>().isUse)
                    {
                        field.SetActive(true);
                    }
                    else
                    {
                        field.SetActive(false);
                    }
                }
            }
        }
        else
        {
            activeTable.GetComponent<chooseItems_schema>().colorUnactive = activeTable.GetComponent<chooseItems_schema>().colorUse;
            activeTable.GetComponent<SpriteRenderer>().color = activeTable.GetComponent<chooseItems_schema>().colorUse;
            activeTable.GetComponent<chooseItems_schema>().isUse = false;
      
            this.gameObject.GetComponent<MeshRenderer>().material = empty;
            activeTable = null;
            chooseNum = -1;

            isUse = false;

            //включение и выключение полей схемы
            bool isAllow = false;
            if(commonData.GetComponent<commonData>().selectObj != null)
            {
                foreach (GameObject allowField in commonData.GetComponent<commonData>().selectObj.GetComponent<chooseItems_schema>().allowFields)
                {
                    if (this.gameObject == allowField)
                    {
                        isAllow = true;
                        break;
                    }
                }
                if (isAllow)
                {
                    this.gameObject.SetActive(true);
                }
                else
                {
                    this.gameObject.SetActive(false);
                    OnExit();
                }
            }
            else
            {
                this.gameObject.SetActive(false);
                OnExit();
            }
        }
    }

    void rotate()
    {
        this.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Angle", side * kf);
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
