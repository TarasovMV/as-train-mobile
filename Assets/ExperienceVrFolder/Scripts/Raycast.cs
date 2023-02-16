using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour {

    public GameObject controller;
    public LineRenderer line;

    bool flagFirst = true;
    //string nameHit;
    GameObject nameHit;

	// Use this for initialization
	void Start ()
    {
        flagFirst = true;
        //nameHit = "MenuBg";
        nameHit = null;
	}

    // Update is called once per frame
    void Update()
    {
        GameObject gObj = raycastHitObj();
        //if (nameHit == gObj.name)
        if (nameHit == gObj)
        {
            flagFirst = false;
        }
        else
        {
            flagFirst = true;
            //GameObject findGobj = GameObject.Find(nameHit);
            //if(findGobj.tag == "interactive")
            if (nameHit)
            {
                if (nameHit.tag == "interactive")
                {
                    //findGobj.GetComponent<InteractiveObject>().setFlags(true, false);
                    nameHit.GetComponent<InteractiveObject>().setFlags(true, false);
                }
            }

        }
        //nameHit = raycastHitObj().name;
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
                //Vector3 posStart = line.transform.position;
                //Vector3 posEnd = hit.point;
                //float length = Mathf.Sqrt(Mathf.Pow((posStart.x - posEnd.x), 2) + Mathf.Pow((posStart.y - posEnd.y), 2) + Mathf.Pow((posStart.y - posEnd.y), 2));
                //Debug.Log(posStart);
                //line.transform.localScale = new Vector3(0.2f,0.2f,length/1.95f);

                line.SetPosition(0, pos);
                line.SetPosition(1, hit.point);
            }

            return hit.collider.gameObject;
        }
        else
        {
            if (line)
            {
                //line.transform.localScale = new Vector3(0.2f, 0.2f, 1);

                Ray ray = new Ray(pos, fw * 5);
                line.SetPosition(0, pos);
                line.SetPosition(1, ray.GetPoint(3));

            }

            return null;
        }
        

    }

    
}
