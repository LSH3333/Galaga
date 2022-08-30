using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{    
    public GameObject[] arrivePos;
    public GameObject BC; // BezierController Prefab
    public SpawnEnemy spawnEnemy;

    private float speed = 0.7f, spawnRate = 0.3f; 

    // true면 해당 칸 이미 적이 자리 차지함 
    private bool[] markArrivePos = new bool[55];

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

    // 중복되지 않는 랜덤 숫자가 담긴 리스트 리턴 
    private List<int> GetRandomIdxList()
    {
        List<int> ret = new List<int>();
        ret.Add(GetRandomPosIdx());
        ret.Add(GetRandomPosIdx());
        ret.Add(GetRandomPosIdx());
        ret.Add(GetRandomPosIdx());
        return ret;
    }


    // idxs : arrivePos[]의 인덱스 값
    // controlPoints : 조절점들 위치
    // spawnTimeRate : 작을수록 적들 빨리 소환됨 
    // 도착지점들을 리스트로 전달하면 해당 도착지점의들의 x,y 값들을 SpawnEnemy에 전달함
    // 리스트 idxs의 크기만큼 적들 소환됨  
    private void SetWave(List<int> idxs, List<KeyValuePair<float, float>> controlPoints, float enemySpeed, float spawnTimeRate)
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



    // 조절점 p1, p2, p3, p4
    // 시작점인 p1은 네곳중 랜덤으로 도착지점인 p4는 고정
    // p2, p3 는 랜덤 
    private void TestCase()
    {
        List<int> enemies = new List<int>();
        List<KeyValuePair<float, float>> controlPoints = new List<KeyValuePair<float, float>>();

        enemies = GetRandomIdxList();
        controlPoints = GetRandomControlPoints();

        SetWave(enemies, controlPoints, spawnRate, speed);
    }

    private void Case1()
    {
        List<int> enemies = new List<int>(); 
        List<KeyValuePair<float, float>> controlPoints = new List<KeyValuePair<float, float>>();

        enemies = new List<int> { GetRandomPosIdx(), GetRandomPosIdx(), GetRandomPosIdx(), GetRandomPosIdx() };
        controlPoints = GetPairs(pattern1, false);
        SetWave(enemies, controlPoints, spawnRate, speed);

        enemies = new List<int> { GetRandomPosIdx(), GetRandomPosIdx(), GetRandomPosIdx(), GetRandomPosIdx() };
        controlPoints = GetPairs(pattern1, true);
        SetWave(enemies, controlPoints, spawnRate, speed);
    }

    private void Start()
    {
        TestCase();

        //Case1();


    }
}
