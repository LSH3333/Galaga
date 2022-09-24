using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnEnemy : MonoBehaviour
{
    // 소환되는 개체들 
    private List<GameObject> objs = new List<GameObject>();
    List<GameObject> arrivePos_List;
    List<KeyValuePair<float, float>> controlPoints;

    private float time;
    private int idx;
    // 소환주기, 짧을 수록 적들이 연달아 소환됨
    private float spawnTimeRate = 30;
    // 적 움직임 속도 
    private float enemySpeed;

    private bool startSpawn = false;

    public void StartSpawn(bool trig)
    {
        startSpawn = trig;
    }

    public void SetSpawnTimeRate(float _spawnTimeRate)
    {
        spawnTimeRate = _spawnTimeRate;
    }

    // arrivePoints : 도착 지점 오브젝트들 담긴 리스트 
    public void SetSpawnObjs(List<GameObject> arrivePoints)
    {
        arrivePos_List = arrivePoints;

        for(int i = 0; i < arrivePoints.Count; i++)
        {
            objs.Add(Resources.Load("BezierController") as GameObject);
        }
    }


    // 조절점 설정, LevelManager에서 조절점 리스트 받아옴  
    public void SetBezierControlPoint(List<KeyValuePair<float, float>> lists)
    {
        controlPoints = lists;
    }

    public void SetEnemySpeed(float _speed)
    {
        enemySpeed = _speed;
    }
    

    // objs의 idx번째 소환 
    void StartSpawn(int idx)
    {
        GameObject instantiated = Instantiate(objs[idx]);
        BezierController bc = instantiated.GetComponent<BezierController>();
        LevelManager.singleton.enemiesList.Add(bc); // 소환한 적 레퍼런스 저장 

        bc.ArrivePoint = arrivePos_List[idx];

        bc.T_increase = enemySpeed;
        bc.cpCnt = controlPoints.Count;
        for (int i = 0; i < controlPoints.Count; i++)
        {            
            bc.controlPoints[i].transform.position = new Vector3(controlPoints[i].Key, controlPoints[i].Value, 0f);                       
        }
    }


    private void Start()
    {
        idx = 0;
        time = spawnTimeRate;
        
    }


    private void Update()
    {
        if (!startSpawn) return;
        
        if (time == spawnTimeRate && idx < objs.Count)
        {
            StartSpawn(idx);
            idx++;
        }

        time -= Time.deltaTime;
        if (time <= 0) time = spawnTimeRate;
    }
}
