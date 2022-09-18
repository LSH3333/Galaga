using System.Collections.Generic;
using UnityEngine;

public class GizmoRender : MonoBehaviour
{
    [SerializeField]
    private Transform[] controlPoints;

    //private Vector2 gizmosPosition;

    //private void OnDrawGizmos()
    //{
    //    for (float t = 0; t <= 1; t += 0.05f)
    //    {
    //        gizmosPosition = Mathf.Pow(1 - t, 3) * controlPoints[0].position + 3 *
    //            Mathf.Pow(1 - t, 2) * t * controlPoints[1].position + 3 * (1 - t) *
    //             Mathf.Pow(t, 2) * controlPoints[2].position +
    //             Mathf.Pow(t, 3) * controlPoints[3].position;

    //        Gizmos.DrawSphere(gizmosPosition, 0.25f);
    //    }

    //    Gizmos.DrawLine(new Vector2(controlPoints[0].position.x, controlPoints[0].position.y), new Vector2(controlPoints[1].position.x, controlPoints[1].position.y));
    //    Gizmos.DrawLine(new Vector2(controlPoints[2].position.x, controlPoints[2].position.y), new Vector2(controlPoints[3].position.x, controlPoints[3].position.y));
    //}



    private Vector3 GetPos(Vector3 p1, Vector3 p2, float m)
    {
        float n = 1 - m;

        float x = (m * p2.x + n * p1.x) / (m + n);
        float y = (m * p2.y + n * p1.y) / (m + n);
        return new Vector3(x, y, 0);
    }

    private Vector3 MovePosition(Vector3 p1, Vector3 p2, float t)
    {
        return GetPos(p1, p2, t);
    }

    Vector3 dfs(ref List<Vector3> points, float t)
    {
        // 점이 하나 남으면 종료 
        if (points.Count == 1)
        {
            Gizmos.DrawSphere(points[0], 0.1f);
            return points[0];
        }

        List<Vector3> newPoints = new List<Vector3>();
        for (int i = 0; i < points.Count - 1; i++)
        {
            newPoints.Add(MovePosition(points[i], points[i + 1],t));
        }
        return dfs(ref newPoints, t);
    }


    private void OnDrawGizmos()
    {
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < controlPoints.Length; i++) points.Add(controlPoints[i].position);
 
        for (float t = 0; t <= 1; t += 0.05f)
        {
            Vector3 newPoint = dfs(ref points, t);
        }
    }
}
