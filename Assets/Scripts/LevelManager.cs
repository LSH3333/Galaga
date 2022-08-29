using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{    
    public GameObject[] arrivePos;
    public GameObject BC; // BezierController Prefab
    public SpawnEnemy spawnEnemy;


    // idxs : arrivePos[]의 인덱스 값
    // controlPoints : 조절점들 위치 
    // 도착지점들을 리스트로 전달하면 해당 도착지점의들의 x,y 값들을 SpawnEnemy에 전달함
    // 리스트 idxs의 크기만큼 적들 소환됨  
    private void SetWave(List<int> idxs, List<KeyValuePair<float, float>> controlPoints, float spawnTimeRate, float enemySpeed)
    {
        List<KeyValuePair<float, float>> arrive_list = new List<KeyValuePair<float, float>>();
        foreach (var x in idxs)
        {
            KeyValuePair<float, float> p = new KeyValuePair<float, float>(arrivePos[x].transform.position.x, arrivePos[x].transform.position.y);
            arrive_list.Add(p);
        }

        spawnEnemy.SetSpawnTimeRate(spawnTimeRate);
        spawnEnemy.SetBezierControlPoint(controlPoints);
        spawnEnemy.SetEnemySpeed(enemySpeed);
        spawnEnemy.SetSpawnObjs(arrive_list);
        spawnEnemy.StartSpawn(true);
    }

    private void TestCase()
    {
        List<int> idxs = new List<int>();
        List<KeyValuePair<float, float>> controlPoints = new List<KeyValuePair<float, float>>();

        for (int i = 0; i < 4; i++)
        {
            idxs.Add(i);
        }

        controlPoints.Add(new KeyValuePair<float, float>(2.37f, 5.58f));
        controlPoints.Add(new KeyValuePair<float, float>(-5.18f, 0.75f));
        controlPoints.Add(new KeyValuePair<float, float>(0.1f, -3.92f));
        controlPoints.Add(new KeyValuePair<float, float>(0.23f, 1.99f));


        SetWave(idxs, controlPoints, 200f, 0.001f);
    }

    private void Case1()
    {
        List<int> idxs = new List<int>();
        List<KeyValuePair<float, float>> controlPoints = new List<KeyValuePair<float, float>>();

        idxs.Add(37);
        idxs.Add(38);
        idxs.Add(48);
        idxs.Add(49);

        controlPoints.Add(new KeyValuePair<float, float>(2.37f, 5.58f));
        controlPoints.Add(new KeyValuePair<float, float>(-5.18f, 0.75f));
        controlPoints.Add(new KeyValuePair<float, float>(0.1f, -3.92f));
        controlPoints.Add(new KeyValuePair<float, float>(0.23f, 1.99f));

        SetWave(idxs, controlPoints, 200f, 0.001f);

    }

    private void Start()
    {
        //TestCase();

        Case1();


    }
}
