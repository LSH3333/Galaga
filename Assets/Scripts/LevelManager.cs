using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{    
    public GameObject[] arrivePos;
    public GameObject BC; // BezierController Prefab
    public SpawnEnemy spawnEnemy;

    private float speed = 20f, spawnRate = 0.011f;
    // true면 해당 칸 이미 적이 자리 차지함 
    private bool[] markArrivePos = new bool[55];


    public class KeyValueList<TKey, TValue> : List<KeyValuePair<TKey, TValue>>
    {
        public void Add(TKey key, TValue value)
        {
            Add(new KeyValuePair<TKey, TValue>(key, value));
        }
    }

    // 우측 상단에서 좌측 하단으로 곡선 이동하면서 한바퀴 도는 패턴 
    KeyValueList<float, float> pattern1 = new KeyValueList<float, float>
    {
        {2.37f, 5.58f }, {-5.18f, 0.75f}, {0.1f, -3.92f}, {0.23f, 1.99f}
    };

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

    // 남은 도착자리 탐색해서 인덱스 리턴 
    private int GetRandomPosIdx()
    {               
        while(true)
        {
            int res = Random.Range(0, 55);
            if(!markArrivePos[res])
            {
                markArrivePos[res] = true;
                return res;
            }
        }
    }


    // idxs : arrivePos[]의 인덱스 값
    // controlPoints : 조절점들 위치
    // spawnTimeRate : 작을수록 적들 빨리 소환됨 
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
        // SpawnEnemy 인스턴스 만들어서 wave 소환하도록 함 
        GameObject seResource = Resources.Load("SpawnEnemy") as GameObject;
        GameObject instanitated = Instantiate(seResource);
        SpawnEnemy se = instanitated.GetComponent<SpawnEnemy>();
        se.SetSpawnTimeRate(spawnTimeRate);
        se.SetBezierControlPoint(controlPoints);
        se.SetEnemySpeed(enemySpeed);
        se.SetSpawnObjs(arrive_list);
        se.StartSpawn(true);
    }




    private void TestCase()
    {
        List<int> enemies = new List<int>();
        List<KeyValuePair<float, float>> controlPoints = new List<KeyValuePair<float, float>>();

        
    }

    private void Case1()
    {
        List<int> enemies = new List<int>(); 
        List<KeyValuePair<float, float>> controlPoints = new List<KeyValuePair<float, float>>();

        enemies = new List<int> { GetRandomPosIdx(), GetRandomPosIdx(), GetRandomPosIdx(), GetRandomPosIdx() };
        controlPoints = GetPairs(pattern1, false);
        SetWave(enemies, controlPoints, speed, spawnRate);

        enemies = new List<int> { GetRandomPosIdx(), GetRandomPosIdx(), GetRandomPosIdx(), GetRandomPosIdx() };
        controlPoints = GetPairs(pattern1, true);
        SetWave(enemies, controlPoints, speed, spawnRate);
    }

    private void Start()
    {
        //TestCase();

        Case1();


    }
}
