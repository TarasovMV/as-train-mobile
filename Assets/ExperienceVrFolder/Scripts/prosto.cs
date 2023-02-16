using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prosto : MonoBehaviour {

    public GameObject[] spisokNew;
    public GameObject[] spisokOld;
    public GameObject[] greenFields;
    public GameObject empty;
    public GameObject osnova;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyDown(KeyCode.I))
        {
            //foreach(var go in spisok)
            //{
            //    GameObject newEmpty = Instantiate(empty, go.transform);
            //    newEmpty.transform.localPosition = Vector3.zero;
            //    newEmpty.transform.parent = go.transform.parent;
            //    newEmpty.transform.localScale = new Vector3(1, 1, 1);
            //    newEmpty.transform.rotation = Quaternion.Euler(0, 0, 0);
            //    go.transform.parent = newEmpty.transform;
            //    newEmpty.name = go.name;
            //}
            setPos();
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            setObj();
        }
		
	}

    void childNew()
    {
        for(int i = 0; i < spisokNew.Length; i++)
        {
            GameObject oldChild = spisokOld[i].transform.GetChild(0).gameObject;
            GameObject newChild = spisokNew[i].transform.GetChild(0).gameObject;
            oldChild.transform.localPosition = newChild.transform.localPosition;
            oldChild.transform.localRotation = newChild.transform.localRotation;
            oldChild.gameObject.transform.localScale = newChild.transform.localScale;
            Debug.Log(i + " " + oldChild.name + " " + newChild.name);
            Debug.Log(newChild.transform.localRotation);
            Debug.Log(oldChild.transform.localRotation);
        }
    }

    void proverka()
    {
        foreach(var objOld in spisokOld)
        {
            GameObject newObj = Instantiate(objOld, osnova.transform.parent);
            newObj.transform.localPosition = osnova.transform.localPosition;
            newObj.transform.localRotation = osnova.transform.localRotation;
            newObj.transform.localScale = osnova.transform.localScale;
            newObj.SetActive(true);
        }
    }

    void setPos()
    {
        foreach (var field in greenFields)
        {
            GameObject obj = field.transform.GetChild(1).gameObject;
            GameObject newField = field.transform.GetChild(0).gameObject;

            newField.GetComponent<greenField_nasos>().pos = obj.transform.localPosition;
            //Vector3 ugol3 = obj.transform.localRotation.eulerAngles;
            //newField.GetComponent<greenField_nasos>().ugol = ugol3;
            newField.GetComponent<greenField_nasos>().scaleSize = obj.transform.localScale[0];
            //Debug.Log(ugol3);
        }
    }

    void setObj()
    {
        foreach (var field in greenFields)
        {
            GameObject obj = Instantiate(osnova, field.transform);
            GameObject newField = field.transform.GetChild(0).gameObject;
            obj.SetActive(true);

            obj.transform.localPosition = newField.GetComponent<greenField_nasos>().pos;
            obj.transform.localRotation = Quaternion.Euler(newField.GetComponent<greenField_nasos>().ugol);
            obj.transform.localScale = new Vector3(newField.GetComponent<greenField_nasos>().scaleSize,newField.GetComponent<greenField_nasos>().scaleSize,newField.GetComponent<greenField_nasos>().scaleSize);
            //Debug.Log(ugol3);
        }
    }
}
