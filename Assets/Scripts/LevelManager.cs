using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{    
    public GameObject[] arrivePos;
    public GameObject BC; // BezierController Prefab
    public SpawnEnemy spawnEnemy;


    // idxs : arrivePos[]의 인덱스 값
    // 도착지점들을 리스트로 전달하면 해당 도착지점의들의 x,y 값들을 SpawnEnemy에 전달함
    // 리스트 idxs의 크기만큼 적들 소환됨  
    private void SetWave(List<int> idxs)
    {
        List<KeyValuePair<float, float>> arrive_list = new List<KeyValuePair<float, float>>();
        foreach (var x in idxs)
        {
            KeyValuePair<float, float> p = new KeyValuePair<float, float>(arrivePos[x].transform.position.x, arrivePos[x].transform.position.y);
            arrive_list.Add(p);
        }

        spawnEnemy.SetSpawnObjs(arrive_list);
        spawnEnemy.StartSpawn(true);
    }

    private void Start()
    {
        SetWave(new List<int> { 0, 1, 2, 3, 4 });

    }
}
