using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour {

    public GameObject controller;
    public LineRenderer line;

    bool flagFirst = true;
    GameObject nameHit;

	void Start ()
    {
        flagFirst = true;
        nameHit = null;
	}

    void Update()
    {
        GameObject gObj = raycastHitObj();
        if (nameHit == gObj)
        {
            flagFirst = false;
        }
        else
        {
            flagFirst = true;
            if (nameHit)
            {
                if (nameHit.tag == "interactive")
                {
                    nameHit.GetComponent<InteractiveObject>().setFlags(true, false);
                }
            }
        }

        nameHit = raycastHitObj();

        if(gObj != null)
        {
            if (gObj.tag == "interactive")
            {
                gObj.GetComponent<InteractiveObject>().setFlags(flagFirst);
            }
        }
        
    }

    GameObject raycastHitObj()
    {
        Vector3 pos = controller.GetComponent<Transform>().position;
        Vector3 fw = controller.GetComponent<Transform>().forward;
        Debug.DrawRay(pos, fw * 10);
        RaycastHit hit;
        
        
        if(Physics.Raycast(pos, fw, out hit, Mathf.Infinity))
        {
            if (line)
            {
                line.SetPosition(0, pos);
                line.SetPosition(1, hit.point);
            }

            return hit.collider.gameObject;
        }
        else
        {
            if (line)
            {
                Ray ray = new Ray(pos, fw * 5);
                line.SetPosition(0, pos);
                line.SetPosition(1, ray.GetPoint(3));

            }
            return null;
        }
    }
}
