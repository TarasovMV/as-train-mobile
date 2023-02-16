using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class kategoryMenu : MonoBehaviour {

    public Material[] mats;

    public void OnEnter()
    {
        Debug.Log("OnEnter");
        this.gameObject.GetComponent<MeshRenderer>().material = mats[1];
    }

    public void OnExit()
    {
        Debug.Log("OnExit");
        this.gameObject.GetComponent<MeshRenderer>().material = mats[0];
    }

    public void OnDrag()
    {

    }

    public void OnButtonClick()
    {
        Debug.Log("OnClick");

        Debug.Log("Choose: " + (Convert.ToInt32(this.gameObject.name)-1).ToString());
        PlayerPrefs.SetInt("kat", Convert.ToInt32(this.gameObject.name)-1);
        Debug.Log(PlayerPrefs.GetInt("mainMenu"));
        if(PlayerPrefs.GetInt("mainMenu") == 0)
        {
            if(PlayerPrefs.GetInt("kat") == 3)
            {
                StartCoroutine(startScene("myMainMenu")); 
            }
            else
            {
                StartCoroutine(startScene("pano360Scene"));
            }
            
        }
        else
        {
            if(PlayerPrefs.GetInt("kat") == 1)
            {
                StartCoroutine(startScene("3dScene OT"));
            }
            else if(PlayerPrefs.GetInt("kat") == 2)
            {
                StartCoroutine(startScene("3dScene KU"));
            }
            else if(PlayerPrefs.GetInt("kat") == 0)
            {
                StartCoroutine(startScene("3dScene OTU"));
            }
            else
            {
                StartCoroutine(startScene("myMainMenu"));
            }
        }

    }

    IEnumerator startScene(string sceneName)
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);

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
