using UnityEngine;

// 적의 본체 obj 
public class BezierObjManager : MonoBehaviour
{
    public BezierController bc;

    // ArrivePos를 따라 좌우로 움직임 
    private void FollowArrivePos()
    {
        if (bc.ArrivePoint != null && bc.Arrived) // bc.Arrived = true시 obj가 도착지점 도달한것 
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
