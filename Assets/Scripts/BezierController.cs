using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 하나의 오브젝트에 대하여 베지어 곡선을 그려서 이동하도록함 
public class BezierController : MonoBehaviour
{
    // 증가되는 t 값 
    private float t;
    // 증가 값, 클수록 적의 움직임이 빠름  
    private float t_increase;
    // 움직이는 대상 
    public GameObject obj;
    // 조절점들 
    public Transform[] controlPoints;
    //public List<Vector3> controlPoints = new List<Vector3>();
    // 베지어 곡선에 영향 미치는 조절점 갯수
    public int cpCnt;

    // 도착 지점 오브젝트 
    private GameObject arrivePoint;

    
   

    // 곡선 이동 완료후 자리로 돌아가는 속도 
    private float speed = 4f;
    // 최종 도착지점에 도착했음
    private bool arrived = false;


    public bool Arrived { get => arrived; set => arrived = value; }
    public GameObject ArrivePoint { get => arrivePoint; set => arrivePoint = value; }
    public float T_increase { get => t_increase; set => t_increase = value; }

    private void Awake()
    {

    }

    private void Start()
    {
        if (t_increase == 0) t_increase = 0.2f;
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
        return GetPos(p1, p2, t);
    }

    // n개의 점을 이어 n-1개의 선분 만들고
    // 각 선분의 t값에 비례하는 곳에 점을 찍는다
    // 즉 재귀함수 한번 호출마다 점의 갯수는 1씩 줄어들고 점의 갯수가 1이되면
    // 그 점의 위치가 다음 위치가 된다 
    Vector3 dfs(ref List<Vector3> points)
    {
        // 점이 하나 남으면 종료 
        if (points.Count == 1)
        {
            RotateDir(points[0]); // 다음 지점의 방향으로 obj의 방향 돌림 
            return points[0];
        }

        List<Vector3> newPoints = new List<Vector3>(); 
        for (int i = 0; i < points.Count - 1; i++)
        {
            newPoints.Add(MovePosition(points[i], points[i + 1]));
        }
        return dfs(ref newPoints);
    }

    // obj가 베지어 곡선의 방향으로 바라보도록 함 
    private void RotateDir(Vector3 nextPos)
    {
        Vector3 dir = nextPos - obj.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        obj.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // 개체의 도착지점으로 이동  
    private void MoveToArrivePos()
    {
        obj.transform.position = Vector3.MoveTowards(obj.transform.position, new Vector3(arrivePoint.transform.position.x, arrivePoint.transform.position.y, 0f), Time.deltaTime * speed);
    }

    // 베지어 곡선 따라 이동 
    private void MoveBezierCurve()
    {
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < cpCnt; i++) points.Add(controlPoints[i].position);

        // newPoint 에는 점의 다음 위치정보 
        Vector3 newPoint = dfs(ref points);
        
        // 점 이동 
        obj.transform.position = newPoint;
    }

    private void Update()
    {
        t += Time.deltaTime * t_increase;
        // 곡선 이동 완료
        // 해당 개체의 자리로 이동 
        if(t >= 1)
        {
            if(!arrived)
            {
                obj.transform.rotation = Quaternion.Euler(0, 0, 90f);
                MoveToArrivePos();
            }
            
                        
            // obj가 최종 도착지점에 도착했음
            if (obj.transform.position == ArrivePoint.transform.position)
            {
                arrived = true;
            }
            return;
        }

        MoveBezierCurve();
    }

}
