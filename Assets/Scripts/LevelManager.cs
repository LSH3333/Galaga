using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    Bee,
    Butterfly,
    Boss
} 

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
            OneWave(patterns[patternIdx++]);
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

    private List<GameObject> GetRandomIdxList2(PatternInfo pattern)
    {
        List<GameObject> ret = new List<GameObject>();
        foreach(Transform x in pattern.transform.Find("Objs"))
        {
            int idx = x.GetComponent<BezierObjManager>().arrivePos;
            ret.Add(arrivePos[idx]);
        }

        return ret;
    }



    // spawnTimeRate : 작을수록 적들 빨리 소환됨 
    // enemySpeed : 클수록 적들 이동 속도 빨라짐
    // pattern의 PatternInfo.cs에 담긴 정보 따라 소환됨 
    private void SetWave(GameObject pattern, float spawnTimeRate, float enemySpeed)
    {
        // SpawnEnemy 인스턴스 만들어서 wave 소환하도록 함 
        GameObject seResource = Resources.Load("SpawnEnemy") as GameObject;
        GameObject instanitated = Instantiate(seResource);
        SpawnEnemy se = instanitated.GetComponent<SpawnEnemy>();

        se.SetSpawnTimeRate(spawnTimeRate);
        se.SetEnemySpeed(enemySpeed);
        se.SetObjs(pattern, false);
        se.StartSpawn(true);
    }

    private void OneWave(GameObject pattern)
    {
        SetWave(pattern, spawnRate, speed);
    }

    // pattern의 조절점의 x,y 좌표 List에 저장 후 리턴  
    private List<KeyValuePair<float, float>> GetPatternControlPoints(GameObject pattern, bool mirror)
    {
        List<KeyValuePair<float, float>> controlPoints = new List<KeyValuePair<float, float>>();
        
        foreach(Transform child in pattern.GetComponent<PatternInfo>().ControlPoint.transform)
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
