using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    // 증가되는 t 값 
    private float t;
    // 움직이는 대상 
    public GameObject obj;
    // 조절점들 
    public Transform[] pointsPos;



    private void Start()
    {
        t = 0;
    }

    // (x1,y1) (x2,y2) 내분점 위치 계산
    private Vector3 GetPos(Vector3 p1, Vector3 p2, float m)
    {
        float n = 1 - m;

        float x = (m * p2.x + n * p1.x) / (m + n);
        float y = (m * p2.y + n * p1.y) / (m + n);
        return new Vector3(x, y, 0);
    }

    // 두 점 p1, p2을 잇는 선분 위의 점 t값에 따라 이동시킨 지점 리턴 
    private Vector3 MovePosition(Vector3 p1, Vector3 p2)
    {
        Vector3 newPos = GetPos(p1, p2, t);
        if (newPos.x > p2.x && newPos.y > p2.y)
        {
            t = 0;
        }

        return GetPos(p1, p2, t);
    }

    Vector3 dfs(ref List<Vector3> points)
    {
        // 점이 하나 남으면 종료 
        if (points.Count == 1) return points[0];

        List<Vector3> newPoints = new List<Vector3>(); 
        for (int i = 0; i < points.Count - 1; i++)
        {
            newPoints.Add(MovePosition(points[i], points[i + 1]));
        }
        return dfs(ref newPoints);
    }

    private void Update()
    {
        t += 0.0015f;

        List<Vector3> points = new List<Vector3>();
        foreach (var x in pointsPos) points.Add(x.position);
        Vector3 newPoint = dfs(ref points);

        obj.transform.position = newPoint;
    }

}
