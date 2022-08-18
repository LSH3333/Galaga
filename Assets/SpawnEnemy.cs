using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject[] objs;
    private float time;
    private int idx;

    private void Start()
    {
        idx = 0;
        time = 120;
    }

    void SetControlPoint(ref GameObject obj)
    {
    }

    void SetSpawnPos(ref GameObject obj)
    {
    }

    // objs의 idx번째 소환 
    void StartSpawn(int idx)
    {
        Instantiate(objs[idx]);
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
