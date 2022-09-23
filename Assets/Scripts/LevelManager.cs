using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager singleton;

    // 적들 도착 위치 
    public GameObject[] arrivePos;
    // true면 해당 칸 이미 적이 자리 차지함 
    private bool[] markArrivePos;
    // 남은 도착 위치 갯수 0이면 남은 자리가 없다는것 
    private int arrivePosLeft;

    
    // true시 이동 할당된 enemy 
    private bool[] movingEnemy = new bool[55];
    private int movingEnemyLeft;

    public GameObject BC; // BezierController Prefab
    public SpawnEnemy spawnEnemy;

    private float speed = 0.4f, spawnRate = 0.1f; 
   
    private float t, timeSpeed = 0.2f;

    // 소환된 적들 레퍼런스 
    public List<BezierController> enemiesList = new List<BezierController>();

    // 패턴의 조절점들 식별용 오브젝트 
    public GameObject[] patterns;
    private int patternIdx = 0;


    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
    }

    private void Start()
    {
        arrivePosLeft = arrivePos.Length;
        markArrivePos = new bool[arrivePos.Length];
        movingEnemyLeft = movingEnemy.Length;
    }
    
    private void Update()
    {
        t += Time.deltaTime * timeSpeed;

        // 소환중  
        if (arrivePosLeft > 0 && t >= 1 && patternIdx < patterns.Length)
        {
            t = 0;
            OneWave(patterns[patternIdx].GetComponent<PatternInfo>().enemyCnt, patterns[patternIdx++]);
        }

        // arrivePos 꽉참 (모두 소환 완료) 
        if (arrivePosLeft <= 0)
        {

        }

    }

    // 남은 도착자리 탐색해서 인덱스 리턴 
    private int GetRandomPosIdx()
    {               
        while(arrivePosLeft > 0)
        {
            int res = Random.Range(0, arrivePos.Length);
            if(!markArrivePos[res])
            {
                markArrivePos[res] = true;
                arrivePosLeft--;
                return res;
            }
        }
        return -1;
    }

    private int GetRandomEnemyIdx()
    {
        while (movingEnemyLeft > 0)
        {
            int res = Random.Range(0, 55);
            if (!movingEnemy[res])
            {
                movingEnemy[res] = true;
                movingEnemyLeft--;
                return res;
            }
        }
        return -1;
    }

    // 중복되지 않는 랜덤 (도착 지점)가 담긴 리스트 리턴
    // cnt : 하나의 wave에 적들의 수 
    private List<GameObject> GetRandomIdxList(int cnt)
    {
        List<GameObject> ret = new List<GameObject>();
        for(int i = 0; i < cnt; i++)
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

    // cnt : 하나의 wave에 적들의 수 
    private void OneWave(int cnt, GameObject pattern)
    {        
        List<GameObject> enemies = GetRandomIdxList(cnt);
        List<KeyValuePair<float, float>> controlPoints = GetPatternControlPoints(pattern, false);
        SetWave(enemies, controlPoints, spawnRate, speed);

        // 대칭 소환 
        if (pattern.GetComponent<PatternInfo>().mirror)
        {
            List<GameObject> mirrorEnemies = GetRandomIdxList(cnt);
            List<KeyValuePair<float, float>> mirrorControlPoints = GetPatternControlPoints(pattern, true);
            SetWave(mirrorEnemies, mirrorControlPoints, spawnRate, speed);
        }
    }

    // pattern의 조절점의 x,y 좌표 List에 저장 후 리턴  
    private List<KeyValuePair<float, float>> GetPatternControlPoints(GameObject pattern, bool mirror)
    {
        List<KeyValuePair<float, float>> controlPoints = new List<KeyValuePair<float, float>>();

        foreach(Transform child in pattern.transform)
        {
            if(!mirror)
            {
                controlPoints.Add(new KeyValuePair<float, float>(child.transform.position.x, child.transform.position.y));
            }
            else
            {
                controlPoints.Add(new KeyValuePair<float, float>(child.transform.position.x * -1, child.transform.position.y));
            }
            
        }

        return controlPoints;
    }

   
}
