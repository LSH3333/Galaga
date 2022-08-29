using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnEnemy : MonoBehaviour
{
    // 소환되는 개체들 
    private List<GameObject> objs = new List<GameObject>();
    List<KeyValuePair<float, float>> arrivePos_List;

    private float time;
    private int idx;
    // 소환주기, 짧을 수록 적들이 연달아 소환됨
    private float spawnTimeRate = 30;

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

    // 개체의 최종 도착 지점 설정  
    void SetArrivePos()
    {
        
    }

    void SetControlPoint(ref GameObject obj)
    {
    }

    void SetSpawnPos(ref GameObject obj)
    {
    }
    
    // 소환 진행전 개체 설정 
    void SetObj()
    {

    }

    // objs의 idx번째 소환 
    void StartSpawn(int idx)
    {
        GameObject instantiated = Instantiate(objs[idx]);
        instantiated.GetComponent<BezierController>().Arrival_xpos = arrivePos_List[idx].Key;
        instantiated.GetComponent<BezierController>().Arrival_ypos = arrivePos_List[idx].Value;
    }


    private void Start()
    {
        idx = 0;
        time = spawnTimeRate;
    }


    private void Update()
    {
        if (!startSpawn) return;

        if(time == spawnTimeRate && idx < objs.Count)
        {
            StartSpawn(idx);
            idx++;
        }

        time -= 1;
        if (time <= 0) time = spawnTimeRate;
    }
}
