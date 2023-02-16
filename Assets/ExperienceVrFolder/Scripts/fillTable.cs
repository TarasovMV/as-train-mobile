using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fillTable : MonoBehaviour {

    public GameObject main;
    public GameObject plashka;
    public GameObject papkaObject;
    public GameObject papkaObjectUse;
    public Material[] mats;
    public GameObject[] els;
    public GameObject empty;

    public GameObject testGo;
    public Transform center;
    //public Transform axis;

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyDown(KeyCode.T))
        {
            test();
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            //Vladok();
            fillTableScheme();
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            List<GameObject> objects = new List<GameObject>();
            List<GameObject> objectsUse = new List<GameObject>();

            for (int i = 0; i < papkaObject.transform.childCount; i++)
            {
                objects.Add(papkaObject.transform.GetChild(i).gameObject);
            }

            for (int i = 0; i < papkaObject.transform.childCount; i++)
            {
                objectsUse.Add(papkaObjectUse.transform.GetChild(i).gameObject);
            }

            int count = 0;
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    if(count < 14)
                    {
                        GameObject newPlashka = Instantiate(plashka) as GameObject;
                        newPlashka.transform.parent = main.transform;
                        newPlashka.transform.localPosition = new Vector3(j * 1.1f, 2.2f - i * 1.1f, 0);
                        newPlashka.transform.localScale = new Vector3(1, 1, 1);
                        //newPlashka.transform.Rotate(0, 81, 0);
                        newPlashka.GetComponent<chooseItems>().number = count;

                        GameObject newObject = Instantiate(objects[count]) as GameObject;
                        newObject.transform.parent = newPlashka.transform;
                        newObject.transform.localScale = new Vector3(2, 2, 2);
                        newObject.transform.localPosition = new Vector3(0, 0, 0);
                        newPlashka.GetComponent<chooseItems>().inObj = newObject;

                        newObject = Instantiate(objectsUse[count]) as GameObject;
                        newObject.transform.parent = newPlashka.transform;
                        newObject.transform.localScale = new Vector3(1, 1, 1);
                        newObject.transform.localPosition = new Vector3(0, 0, 0);
                        newObject.SetActive(false);
                        newPlashka.GetComponent<chooseItems>().inObjUse = newObject;

                        count++;
                    }
                    
                }
            }
        }
		
	}

    void Vladok()
    {
        List<GameObject> objects = new List<GameObject>();
        List<GameObject> objectsUse = new List<GameObject>();

        for (int i = 0; i < papkaObject.transform.childCount; i++)
        {
            objects.Add(papkaObject.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < papkaObject.transform.childCount; i++)
        {
            objectsUse.Add(papkaObjectUse.transform.GetChild(i).gameObject);
        }

        foreach (var go in objects)
        {
            GameObject newEmpty = Instantiate(empty,go.transform);
            newEmpty.transform.localPosition = new Vector3(0, 0, 0);
            newEmpty.transform.parent = papkaObject.transform;
            newEmpty.transform.localScale = new Vector3(1, 1, 1);
            newEmpty.transform.localEulerAngles = new Vector3(0, 0, 0);
            newEmpty.name = go.name;
            go.transform.parent = newEmpty.transform;
        }

        foreach (var go in objectsUse)
        {
            GameObject newEmpty = Instantiate(empty, go.transform);
            newEmpty.transform.localPosition = new Vector3(0, 0, 0);
            newEmpty.transform.parent = papkaObjectUse.transform;
            newEmpty.transform.localScale = new Vector3(1, 1, 1);
            newEmpty.transform.localEulerAngles = new Vector3(0, 0, 0);
            newEmpty.name = go.name;
            go.transform.parent = newEmpty.transform;
        }
    }

    void test()
    {
        Vector3 axis = new Vector3(center.transform.position.x, center.transform.position.y+1, center.transform.position.z);
        testGo.GetComponent<Transform>().RotateAround(center.transform.position, axis, 30);
        //testGo.transform.LookAt(center);
    }

    void fillTableScheme()
    {
        int count = 0;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (count < 20)
                {
                    GameObject newPlashka = Instantiate(plashka) as GameObject;
                    newPlashka.transform.parent = main.transform;
                    newPlashka.transform.localPosition = new Vector3(j * 1.1f, 2.2f - i * 1.1f, 0);
                    newPlashka.transform.localScale = new Vector3(1, 1, 1);
                    newPlashka.GetComponent<chooseItems_schema>().number = count;
                    newPlashka.GetComponent<chooseItems_schema>().name = els[count].name;

                    GameObject newObject = Instantiate(els[count]) as GameObject;
                    newObject.transform.parent = newPlashka.transform;
                    newObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    newObject.transform.localPosition = new Vector3(0, 0, -0.01f);

                    newPlashka.GetComponent<chooseItems_schema>().schemeMat[1] = mats[count];

                    count++;
                }

            }
        }
    }
}
