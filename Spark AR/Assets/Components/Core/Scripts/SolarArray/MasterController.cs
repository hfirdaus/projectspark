using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MasterController : MonoBehaviour
{

    //int counter = 0;
    //AudioClip clip;
    //float startRecordingTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TouchTrigger();
    }

    public void Test()
    {
        print("button");
    }

    //template of the method to be used in the future for selecting planets
    public void TouchTrigger()
    {
        //if (selected != null)
        //{
        //    return;
        //}
        //if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        if (Input.GetMouseButtonDown(0))
        {
            //Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                string targ = null;
                if(raycastHit.collider.transform.parent != null)
                {
                    targ = raycastHit.collider.transform.parent.name;
                }
                if (targ != null)
                {

                }

                if (raycastHit.collider.GetComponent("Recorder") != null)
                {
                }
            }
            else
            {
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
        }
    }
}
