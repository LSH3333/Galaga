using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public LineRenderer line;
    public Transform p1, p2, p3;
    private float t;
    public GameObject obj;

    // (x1,y1) (x2,y2) 내분점 위치 계산
    private Vector3 GetPos(Vector3 p1, Vector3 p2, float m)
    {
        float n = 1 - m;

        float x = (m * p2.x + n * p1.x) / (m + n);
        float y = (m * p2.y + n * p1.y) / (m + n);
        return new Vector3(x, y, 0);
    }

    private void Start()
    {
        t = 0;
    }


    // 두 점을 잇는 선분 위의 점 이동시킴 
    private void MovePosition(Transform p1, Transform p2, int lineIdx)
    {
        Vector3 newPos = GetPos(p1.position, p2.position, t);
        if (newPos.x > p2.position.x && newPos.y > p2.position.y)
        {
            t = 0;
            return;
        }

        line.SetPosition(lineIdx, GetPos(p1.position, p2.position, t));
    }

    private void Update()
    {
        t += 0.005f;

        MovePosition(p1, p2, 0);
        MovePosition(p2, p3, 1);

        obj.transform.position = GetPos(line.GetPosition(0), line.GetPosition(1), t);
        
    }

}
