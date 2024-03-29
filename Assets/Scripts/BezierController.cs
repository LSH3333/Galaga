﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 하나의 오브젝트에 대하여 베지어 곡선을 그려서 이동하도록함 
public class BezierController : MonoBehaviour
{
    // 증가되는 t 값 
    // 증가 값, 클수록 적의 움직임이 빠름  
    public float t_increase;
    public float t = 0;

    // 움직이는 대상 
    public GameObject obj;
    // 조절점들 
    public List<Vector3> controlPoints;

    // 도착 지점 오브젝트 
    public GameObject arrivePoint;


    // 곡선 이동 완료후 자리로 돌아가는 속도 
    private float speed = 4f;

    public GameObject ArrivePoint { get => arrivePoint; set => arrivePoint = value; }
    public float T_increase { get => t_increase; set => t_increase = value; }

    // 1: 베지어 곡선 따라 이동 중 
    // 2: 마지막 컨트롤 포인트에서 도착지점으로 이동중 
    // 3: 도착지점 도착 (Hovering) 
    // 4: Attacking 
    public int status = 1;
    private BezierObjManager bezierObjManager;

    public GameObject beamHitPlayerObj;



    private void Awake()
    {
        controlPoints = new List<Vector3>();
        bezierObjManager = obj.GetComponent<BezierObjManager>();
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
    private Vector3 MovePosition(Vector3 p1, Vector3 p2, float t)
    {
        return GetPos(p1, p2, t);
    }

    // n개의 점을 이어 n-1개의 선분 만들고
    // 각 선분의 t값에 비례하는 곳에 점을 찍는다
    // 즉 재귀함수 한번 호출마다 점의 갯수는 1씩 줄어들고 점의 갯수가 1이되면
    // 그 점의 위치가 다음 위치가 된다 
    Vector3 dfs(ref List<Vector3> points, float t)
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
            newPoints.Add(MovePosition(points[i], points[i + 1], t));
        }
        return dfs(ref newPoints, t);
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
    private void MoveBezierCurve(float t)
    {
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < controlPoints.Count; i++) points.Add(controlPoints[i]);
        // 움직이는 도착지점 갱신 

        // newPoint 에는 점의 다음 위치정보 
        Vector3 newPoint = dfs(ref points, t);

        // 점 이동 
        obj.transform.position = newPoint;
    }

    
    /// ////////////////////////////////////// Boss Beam Attack 
    
    bool beaming = false;
    private void BossBeamAttack()
    {
        beaming = true;
        obj.transform.rotation = Quaternion.Euler(0, 0, -90f);
        // beam 소환  
        GameObject beam = LevelManager.singleton.bossBeam;
        Vector2 spawnPos = new Vector2(obj.transform.position.x, obj.transform.position.y - 0.5f);
        GameObject instaniated = Instantiate(beam, spawnPos, Quaternion.identity);
        instaniated.transform.SetParent(gameObject.transform);

        StartCoroutine("BeamShooting");
    }
    IEnumerator BeamShooting()
    {
        // 빔 쏘면서 머무는 시간 
        yield return new WaitForSeconds(8f);
        beaming = false;
        RotateDir(arrivePoint.transform.position);
        status = 2;
        LevelManager.singleton.bossBeamAttacking = false; // beam attack 종료 
    }
    
    public void SpawnBeamHitPlayer()
    {
        GameObject beamHitPlayer = LevelManager.singleton.beamHitPlayer;
        beamHitPlayer.GetComponent<BeamHitPlayer>().bc = this;
        beamHitPlayerObj = Instantiate(beamHitPlayer,
            LevelManager.singleton.player.transform.position, Quaternion.identity);
        
    }

    /// //////////////////////////////////////

    // int status 
    // 1: 베지어 곡선 따라 이동 중 
    // 2: 마지막 컨트롤 포인트에서 도착지점으로 이동중 
    // 3: 도착지점 도착 (Hovering) 
    // 4: Attacking 
    private void StartHovering()
    {
        Type type = bezierObjManager.type;        

        if(status == 1)
        {
            status = 2;
        }
        // 아직 도착지점 도달하지 않았다면 도착 지점으로 이동하도록 함
        else if (status == 2)
        {
            MoveToArrivePos();            
        }
        else if(status == 3)
        {
            MoveToArrivePos();
        }
        else if(status == 4)
        {
            if(type == Type.Bee)
            {
                status = 2;
            }
            else if (type == Type.Butterfly)
            {
                obj.transform.position = new Vector2(0f, 5f);
                status = 2;
            }
            else if (type == Type.Boss)
            {
                // beam attack 하는 pattern1 보스 
                if(thisBossBeamAttacking)
                {
                    // beam
                    if (!beaming)
                    {
                        if (LevelManager.singleton.player.GetComponent<PlayerManager>().childPlayer.activeInHierarchy)
                        {
                            status = 2;
                        }
                        else if (beamHitPlayerObj == null)
                        {
                            BossBeamAttack();
                        }
                    }

                    return;
                }
                // beam attack 하지 않는 pattern2 보스  
                else
                {
                    status = 2;
                    return;
                }
                
            }
        }

        // obj가 최종 도착지점에 도착했음
        if (obj.transform.position == ArrivePoint.transform.position)
        {
            obj.transform.rotation = Quaternion.Euler(0, 0, 90f);
            status = 3;
        }
    }

    private bool thisBossBeamAttacking = false;
    // 공격 패턴 지정 
    private void SetAttackControlPoints()
    {
        Transform cps;
        if(bezierObjManager.type == Type.Boss)
        {
            // boss beam attack (pattern1) 
            if(beamHitPlayerObj == null && !LevelManager.singleton.bossBeamAttacking)
            {
                cps = arrivePoint.GetComponent<ArrivePosManager>().attackPattern.transform.Find("ControlPoints");
                thisBossBeamAttacking = true;
                LevelManager.singleton.bossBeamAttacking = true;
            }
            else
            {
                cps = arrivePoint.GetComponent<ArrivePosManager>().attackPattern2.transform.Find("ControlPoints");
                thisBossBeamAttacking = false;
            }
        }
        else
        {
            cps = arrivePoint.GetComponent<ArrivePosManager>().attackPattern.transform.Find("ControlPoints");
        }


        controlPoints = new List<Vector3>();
        foreach(Transform x in cps)
        {
            // 도착지점 arrviePoint의 벡터에 패턴지점 x.position 더해서 패턴의 시작지점이 도착지점이 되도록함 
            controlPoints.Add(arrivePoint.transform.position + x.position);
        }

        // bee라면 도착지점에 도달하도록  
        if (obj.GetComponent<BezierObjManager>().type == Type.Bee)
        {            
            controlPoints.Add(arrivePoint.transform.position);
        }        
    }

    public int bulletPercentageMax = 3;
    public void StartAttack()
    {
        SetAttackControlPoints(); 
        status = 4;
        Type type = obj.GetComponent<BezierObjManager>().type;

        if (type == Type.Bee)
        {
            t_increase = 0.15f; // move speed 
        }
        else if(type == Type.Butterfly)
        {
            t_increase = 0.3f;
        }
        else if(type == Type.Boss)
        {            
            if(thisBossBeamAttacking)
            {
                t_increase = 0.3f;
            }
            else
            {
                t_increase = 0.15f;
            }

        }

        
        t = 0;

        // 1 / bulletPercentageMax 의 확률로 enemy가 bullet shoot 하도록 함 
        int range = Random.Range(0, bulletPercentageMax);
        if(range == 0)
        {
            obj.GetComponent<BezierObjManager>().OrderShoot();
        }
    }

    ////////////////////////////////////////////

    
    private void Update()
    {
        if(LevelManager.singleton.levelStatus == 2)
        {
            return;
        }

        // 곡선 이동 완료
        if (t >= 1)
        {
            StartHovering();
        }
        else // 베지어 곡선 따라 이동 중  
        {
            t += Time.deltaTime * t_increase;
            MoveBezierCurve(t);
        }
    }

}