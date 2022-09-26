using System.Collections.Generic;
using UnityEngine;

public class DrawGizmos : MonoBehaviour
{
    // 조절점들 
    public List<Transform> controlPoints = new List<Transform>();

    private void Awake()
    {
    }

    // 두 점 p1, p2을 잇는 선분 위의 점 t값에 따라 이동시킨 지점 리턴 
    private Vector3 MovePosition(Vector3 p1, Vector3 p2, float t)
    {
        return GetPos(p1, p2, t);
    }

    // (x1,y1) (x2,y2) 내분점 위치 계산
    private Vector3 GetPos(Vector3 p1, Vector3 p2, float m)
    {
        float n = 1 - m;

        float x = (m * p2.x + n * p1.x) / (m + n);
        float y = (m * p2.y + n * p1.y) / (m + n);
        return new Vector3(x, y, 0);
    }

    /////////////////////// Render Gizmo 
    Vector3 dfsRenderGizmo(ref List<Vector3> points, float t)
    {
        // 점이 하나 남으면 종료 
        if (points.Count == 1)
        {
            Gizmos.DrawSphere(points[0], 0.1f);
            return points[0];
        }

        List<Vector3> newPoints = new List<Vector3>();
        if (points.Count - 1 >= 0)
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                newPoints.Add(MovePosition(points[i], points[i + 1], t));
            }            
        }
        return dfsRenderGizmo(ref newPoints, t);

    }


    private void OnDrawGizmos()
    {
        if (controlPoints == null || controlPoints.Count <= 0) return;
        List<Vector3> points = new List<Vector3>();
        foreach (var x in controlPoints) points.Add(x.position);

        for (float t = 0; t <= 1; t += 0.05f)
        {
            Vector3 newPoint = dfsRenderGizmo(ref points, t);
        }
    }

    ///////////////////////
}
