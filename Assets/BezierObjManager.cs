using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierObjManager : MonoBehaviour
{
    public BezierController bc;


    private void Update()
    {
        if(bc.ArrivePoint != null && bc.Arrived)
        {
            Vector3 arrviePos = bc.ArrivePoint.transform.position;
            gameObject.transform.position = arrviePos;
        }
    }
}
