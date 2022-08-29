using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnEnemy : MonoBehaviour
{
    // 소환되는 개체들 
    private List<GameObject> objs = new List<GameObject>();
    List<KeyValuePair<float, float>> arrivePos_List;
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

    public void SetSpawnObjs(List<KeyValuePair<float, float>> lists)
    {
        arrivePos_List = lists;

        for(int i = 0; i < lists.Count; i++)
        {
            objs.Add(Resources.Load("BezierController") as GameObject);
        }
    }

    // 조절점 설정 
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
        bc.Arrival_xpos = arrivePos_List[idx].Key;
        bc.Arrival_ypos = arrivePos_List[idx].Value;
        bc.T_increase = enemySpeed;
        bc.p1.transform.position = new Vector3(controlPoints[0].Key, controlPoints[0].Value, 0f);
        bc.p2.transform.position = new Vector3(controlPoints[1].Key, controlPoints[1].Value, 0f);
        bc.p3.transform.position = new Vector3(controlPoints[2].Key, controlPoints[2].Value, 0f);
        bc.p4.transform.position = new Vector3(controlPoints[3].Key, controlPoints[3].Value, 0f);
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

        time -= 1;
        if (time <= 0) time = spawnTimeRate;
    }
}
