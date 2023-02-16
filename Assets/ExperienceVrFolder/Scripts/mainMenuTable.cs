using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenuTable : MonoBehaviour
{
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
        
        if(this.gameObject.name == "pano360")
        {
            PlayerPrefs.SetInt("mainMenu", 0);
        }
        else
        {
            PlayerPrefs.SetInt("mainMenu", 1);
        }
        StartCoroutine(sceneLoad());
    }

    IEnumerator sceneLoad()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("kategoryChoose");
    }


	void Start () {
		
	}

	void Update ()
    {
		if(this.gameObject.GetComponent<InteractiveObject>().flagOn)
        {
            if(this.gameObject.GetComponent<InteractiveObject>().flagFirst)
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
            if(this.gameObject.GetComponent<InteractiveObject>().flagFirst)
            {
                OnExit();
                this.gameObject.GetComponent<InteractiveObject>().flagFirst = false;                
            }
        }
	}
}
