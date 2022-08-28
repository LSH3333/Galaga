using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{    
    public GameObject[] arrivePos;
    public GameObject BC; // BezierController Prefab
    public SpawnEnemy spawnEnemy;

    // 도착 위치 저장된 적 오브젝트 만들어서 리턴 
    private GameObject Make(int arrivePosIdx)
    {
        GameObject bc = BC;
        bc.GetComponent<BezierController>().Arrival_xpos = arrivePos[arrivePosIdx].gameObject.transform.position.x;
        bc.GetComponent<BezierController>().Arrival_ypos = arrivePos[arrivePosIdx].gameObject.transform.position.y;      
        return bc;
    }

    private void Start()
    {
        List<GameObject> objs = new List<GameObject>();
        objs.Add(Make(0));
        spawnEnemy.SetSpawnObjs(objs);
        spawnEnemy.StartSpawn(true);
    }
}
