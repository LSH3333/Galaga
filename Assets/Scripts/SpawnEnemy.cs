using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnEnemy : MonoBehaviour
{
    // 소환되는 개체들 
    public List<GameObject> objs;
    private float time;
    private int idx;
    private const float spawnTimeRate = 120;

    private bool startSpawn = false;

    public void StartSpawn(bool trig)
    {
        startSpawn = trig;
    }

    public void SetSpawnObjs(List<GameObject> objList)
    {
        foreach(var x in objList)
        {
            //print("A: " + x.GetComponent<BezierController>().Arrival_xpos);
            objs.Add(x);
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
        instantiated.GetComponent<BezierController>().Arrival_xpos = objs[idx].GetComponent<BezierController>().Arrival_xpos;
        instantiated.GetComponent<BezierController>().Arrival_ypos = objs[idx].GetComponent<BezierController>().Arrival_ypos;
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
