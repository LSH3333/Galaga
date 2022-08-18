using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    // 소환되는 개체들 
    public GameObject[] objs;
    private float time;
    private int idx;

    // 개체의 최종 도착 지점 설정  
    void SetArrivePos(ref GameObject obj)
    {
        foreach(var x in objs)
        {
            
        }
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
        Instantiate(objs[idx]);
    }




    private void Start()
    {
        idx = 0;
        time = 120;
    }


    private void Update()
    {
        if(time == 120 && idx < objs.Length)
        {
            StartSpawn(idx);
            idx++;
        }

        time -= 1;
        if (time <= 0) time = 120;
    }
}
