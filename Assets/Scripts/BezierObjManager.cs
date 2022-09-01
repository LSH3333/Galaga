using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 적의 본체 obj 
public class BezierObjManager : MonoBehaviour
{
    public BezierController bc;

    // ArrivePos를 따라 좌우로 움직임 
    private void FollowArrivePos()
    {
        if (bc.ArrivePoint != null && bc.Arrived)
        {
            Vector3 arrviePos = bc.ArrivePoint.transform.position;
            gameObject.transform.position = arrviePos;
        }
    }

    private void Update()
    {
        FollowArrivePos();


    }
}
