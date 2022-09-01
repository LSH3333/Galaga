﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // 적들 도착 위치 
    public GameObject[] arrivePos;
    // 남은 도착 위치 갯수 0이면 남은 자리가 없다는것 
    private int arrivePosLeft; 

    public GameObject BC; // BezierController Prefab
    public SpawnEnemy spawnEnemy;

    private float speed = 0.5f, spawnRate = 0.3f; 

    // true면 해당 칸 이미 적이 자리 차지함 
    private bool[] markArrivePos = new bool[55];

    private float t, timeSpeed = 0.2f;



    // 적들의 시작점인 p1의 위치들 
    KeyValueList<float, float> p1Pos = new KeyValueList<float, float>
    {
        {2.37f, 5.58f }, {-2.37f, 5.58f }, {4f, -2.78f}, {-4f, -2.78f}
    };


    // 우측 상단에서 좌측 하단으로 곡선 이동하면서 한바퀴 도는 패턴 
    KeyValueList<float, float> pattern1 = new KeyValueList<float, float>
    {
        {2.37f, 5.58f }, {-5.18f, 0.75f}, {0.1f, -3.92f}, {0.23f, 1.99f}
    };



    public class KeyValueList<TKey, TValue> : List<KeyValuePair<TKey, TValue>>
    {
        public void Add(TKey key, TValue value)
        {
            Add(new KeyValuePair<TKey, TValue>(key, value));
        }
    }


    // keyValList : pattern 전달하면 해당되는 List<KeyValuePair<>> 리턴함
    // reverse : pattern 의 반전버전 리턴 
    private List<KeyValuePair<float, float>> GetPairs(KeyValueList<float, float> keyValList, bool reverse)
    {
        List<KeyValuePair<float, float>> ret = new List<KeyValuePair<float, float>>();
        foreach (var x in keyValList)
        {
            if (reverse) ret.Add(new KeyValuePair<float, float>(-1 * x.Key, x.Value));
            else ret.Add(x);
        }
        return ret;
    }

    // 랜덤한 4개의 컨트롤 포인트 리턴 
    private List<KeyValuePair<float, float>> GetRandomControlPoints()
    {
        List<KeyValuePair<float, float>> ret = new List<KeyValuePair<float, float>>();

        int idx = Random.Range(0, p1Pos.Count);
        ret.Add(new KeyValuePair<float, float>(p1Pos[idx].Key, p1Pos[idx].Value));
        ret.Add(new KeyValuePair<float, float>(Random.Range(-2.3f, 2.3f), Random.Range(2f, -4f)));
        ret.Add(new KeyValuePair<float, float>(Random.Range(-2.3f, 2.3f), Random.Range(2f, -4f)));
        ret.Add(new KeyValuePair<float, float>(0.23f, 1.99f));
        return ret;
    }

    // 남은 도착자리 탐색해서 인덱스 리턴 
    private int GetRandomPosIdx()
    {               
        while(arrivePosLeft > 0)
        {
            int res = Random.Range(0, 55);
            if(!markArrivePos[res])
            {
                markArrivePos[res] = true;
                arrivePosLeft--;
                return res;
            }
        }
        return -1;
    }

    // 중복되지 않는 랜덤 (도착 지점)가 담긴 리스트 리턴   
    private List<GameObject> GetRandomIdxList()
    {
        //List<int> ret = new List<int>();
        List<GameObject> ret = new List<GameObject>();
        for(int i = 0; i < 4; i++)
        {
            int idx = GetRandomPosIdx();
            if (idx == -1) break;
            ret.Add(arrivePos[idx]);
        }

        return ret;
    }


    // arrivePoints : 도착 지점 오브젝트 레퍼런스  
    // controlPoints : 조절점들 위치
    // spawnTimeRate : 작을수록 적들 빨리 소환됨 
    // 도착지점들을 리스트로 전달하면 해당 도착지점의들의 x,y 값들을 SpawnEnemy에 전달함
    // 리스트 idxs의 크기만큼 적들 소환됨  
    private void SetWave(List<GameObject> arrivePoints, List<KeyValuePair<float, float>> controlPoints, float spawnTimeRate, float enemySpeed)
    {
        List<KeyValuePair<float, float>> arrive_list = new List<KeyValuePair<float, float>>();

        // SpawnEnemy 인스턴스 만들어서 wave 소환하도록 함 
        GameObject seResource = Resources.Load("SpawnEnemy") as GameObject;
        GameObject instanitated = Instantiate(seResource);
        SpawnEnemy se = instanitated.GetComponent<SpawnEnemy>();
        se.SetSpawnTimeRate(spawnTimeRate);
        se.SetBezierControlPoint(controlPoints);
        se.SetEnemySpeed(enemySpeed);
        se.SetSpawnObjs(arrivePoints);
        se.StartSpawn(true);
    }


    private void OneWave()
    {
        List<GameObject> enemies;
        List<KeyValuePair<float, float>> controlPoints;
        // 한번에 1 or 2의 wave  
        int waveCnt = Random.Range(1, 3);
        for(int i = 0; i < waveCnt; i++)
        {
            enemies = GetRandomIdxList();
            controlPoints = GetRandomControlPoints();
            SetWave(enemies, controlPoints, spawnRate, speed);
        }
    }


    private void Start()
    {
        arrivePosLeft = arrivePos.Length;


    }

    private void Update()
    {
        if (arrivePosLeft <= 0) return;

        t += Time.deltaTime * timeSpeed;

        if (t >= 1)
        {
            t = 0;
            OneWave();
        }
    }
}
