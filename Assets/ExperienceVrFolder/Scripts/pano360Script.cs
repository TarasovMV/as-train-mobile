using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pano360Script : MonoBehaviour {

    public Material[] mats;
    public GameObject sphere;
    public GameObject head;
    public int count = 0;
    public int kat = 0;
    public GameObject arrows;
    bool isClick = false;
    public Text panoNum;
    public GameObject mainSceneWindow;

    void Start ()
    {
        kat = PlayerPrefs.GetInt("kat");
        sphere.GetComponent<MeshRenderer>().material = mats[kat*3];
	}
	
	void FixedUpdate ()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            count++;

            if (count > 2)
            {
                count = 0;
            }
            sphere.GetComponent<MeshRenderer>().material = mats[kat * 3 + count];
            Debug.Log(count + kat * 3);
            panoNum.text = (count + 1).ToString();
        }
        //if (Input.GetMouseButtonUp(0) || OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        //if ((OVRInput.Get(OVRInput.Button.PrimaryTouchpad)))
        //{
        //    if (!this.gameObject.GetComponent<dopMenu>().mainSceneWindow.activeSelf)
        //    {
        //        if (!isClick)
        //        {
        //            count++;

        //            if (count > 2)
        //            {
        //                count = 0;
        //            }
        //            sphere.GetComponent<MeshRenderer>().material = mats[kat * 3 + count];
        //            Debug.Log(count + kat * 3);
        //            panoNum.text = (count + 1).ToString();

        //            isClick = true;
        //        }
                
        //    }
            
        //}
        //else
        //{
        //    isClick = false;
        //}


        //if (OVRInput.Get(OVRInput.Button.Back))
        //{
        //    Debug.Log("Back");
        //    mainSceneWindow.SetActive(true);
        //}
    }

    IEnumerator startEndAnim()
    {
        yield return new WaitForSeconds(1.5f);
        arrows.SetActive(false);
    }

    public void nextPano()
    {
        count++;

        if (count > 2)
        {
            count = 0;
        }
        sphere.GetComponent<MeshRenderer>().material = mats[kat * 3 + count];
        Debug.Log(count + kat * 3);
        panoNum.text = (count + 1).ToString();
    }
}
