using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private float t;
    public GameObject obj;

    public Transform[] pointsPos;



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

    // points에 담겨있는 점들 사이의 내분점 위치를 t값에 따라 구함
    // 구한 내분점들은 newPoints에 담김, 결과적으로 points-1개의 새로운 내분점들이 생김 
    private void MakeBezier(int curPoint, ref List<Vector3> points, ref List<Vector3> newPoints)
    {
        if (curPoint >= points.Count)
        {

            return;
        }

        newPoints.Add(MovePosition(points[curPoint], points[curPoint]));
        MakeBezier(curPoint + 1, ref points, ref newPoints);
    }

    private Vector3 dfs()
    {
        // 선분 위의 점들 
        List<Vector3> points = new List<Vector3>();
        foreach (var x in pointsPos) points.Add(x.position);

        int cnt = 0;
        // 선분 위의 점들의 갯수가 1이 될때까지 반복함
        while (true)
        {
            List<Vector3> newPoints = new List<Vector3>();
            MakeBezier(0, ref points, ref newPoints);
            // 새롭게 구한 내분점들이 이제 기준점이됨 
            points = newPoints;
            if (newPoints.Count <= 1)
                return newPoints[0];

            if (cnt++ > 10) return newPoints[0];
        }

    }

    private void Update()
    {
        t += 0.005f;

        Vector3 ret = dfs();
        print(ret);
        obj.transform.position = ret;

    }

}
