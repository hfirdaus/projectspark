using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoRotate : MonoBehaviour
{
    void Update()
    {
		transform.Rotate(transform.parent.up, Time.smoothDeltaTime * 5f);    
    }
}
