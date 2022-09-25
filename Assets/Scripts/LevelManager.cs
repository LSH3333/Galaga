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
    // 남은 도착 위치 갯수 0이면 남은 자리가 없다는것 
    private int arrivePosLeft;


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
            enemiesList[0].StartAttack();
        }
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
