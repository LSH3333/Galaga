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
        bee_cool = SetRandomCool();
    }
    
    private void Update()
    {
        t += Time.deltaTime * timeSpeed;

        // 소환중  
        if (t >= 1 && patternIdx < patterns.Length)
        {
            t = 0;
            OneWave(patterns[patternIdx++]);
        }

        // arrivePos 꽉참 (모두 소환 완료) 
        if (patternIdx >= patterns.Length)
        {
            OrderAttackBee();
            
        }
    }

    private float SetRandomCool()
    {
        return Random.Range(cool_min, cool_max);
    }

    float cool_min = 5f, cool_max = 7f;
    float bee_time = 0f;
    float bee_cool = 5f;
    List<BezierController> attackingBees;
    private void OrderAttackBee()
    {
        bee_time += Time.deltaTime;
        if (bee_time <= bee_cool) return;
        bee_time = 0f;
        bee_cool = SetRandomCool(); 

        List<BezierController> bees = new List<BezierController>();
        foreach(var x in enemiesList)
        {
            if (x == null) continue; // destroyed
            if(x.obj.GetComponent<BezierObjManager>().type == Type.Bee)
            {
                bees.Add(x);
            }
        }

        FindOrderTarget(bees).StartAttack();
    }



    // lists 적들 중 공격 명령 내릴 객체 리턴함 
    private BezierController FindOrderTarget(List<BezierController> lists)
    {
        BezierController leftObj = lists[0];
        BezierController rightObj = lists[0];

        foreach(var x in lists)
        {
            if(x.arrivePoint.transform.position.x < leftObj.arrivePoint.transform.position.x)
            {
                leftObj = x;
            }
            if(x.arrivePoint.transform.position.x > rightObj.arrivePoint.transform.position.x)
            {
                rightObj = x;
            }
        }

        int leftOrRight = Random.Range(0, 2);
        if (leftOrRight == 0) return leftObj;
        else return rightObj;
    }


    // spawnTimeRate : 작을수록 적들 빨리 소환됨 
    // enemySpeed : 클수록 적들 이동 속도 빨라짐
    // pattern의 PatternInfo.cs에 담긴 정보 따라 소환됨 
    private void SetWave(GameObject pattern, float spawnTimeRate, float enemySpeed, bool mirror)
    {
        // SpawnEnemy 인스턴스 만들어서 wave 소환하도록 함 
        GameObject seResource = Resources.Load("SpawnEnemy") as GameObject;
        GameObject instanitated = Instantiate(seResource);
        SpawnEnemy se = instanitated.GetComponent<SpawnEnemy>();

        se.SetSpawnTimeRate(spawnTimeRate);
        se.SetEnemySpeed(enemySpeed);
        se.SetObjs(pattern, mirror);
        se.StartSpawn(true);
    }

    private void OneWave(GameObject pattern)
    {
        SetWave(pattern, spawnRate, speed, false);

        if (pattern.GetComponent<PatternInfo>().mirror)
        {
            SetWave(pattern, spawnRate, speed, true);
        }
    }

   
}
