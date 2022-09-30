﻿using System.Collections;
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

    // hit sound 
    public AudioSource hitSound;

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
    }

    private void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            bee_cools[i] = SetRandomCool();
            //bee_attacking[i] = Instantiate(Resources.Load("BezierController") as GameObject).GetComponent<BezierController>();

            butterfly_cools[i] = SetRandomCool();
            boss_cools[i] = SetRandomBossCool();
        }
        
    }

    private float startAttack = 0f; 
    private void Update()
    {
        t += Time.deltaTime * timeSpeed;

        // 소환중  
        if (t >= 1 && patternIdx < patterns.Length)
        {
            t = 0;
            OneWave(patterns[patternIdx++]);
        }

        if (patternIdx >= patterns.Length)
        {
            startAttack += Time.deltaTime;
        }

        // arrivePos 꽉참 (모두 소환 완료) 
        if (patternIdx >= patterns.Length && startAttack > 2f)
        {
            OrderBeeAttack();
            OrderButterflyAttack();
            OrderBossAttack(); 
        }
    }


    ////////////////////////////////////////////////
    
    
    float cool_min = 6f, cool_max = 10f;
    float[] bee_times = { 0f, 0f };
    float[] bee_cools = { 5f, 5f };
    public BezierController[] bee_attacking = { null, null };
    private void OrderBeeAttack()
    {
        // 공격 중인 개체가 파괴되었다면 시간을 쿨타임시간 지나도록 설정해 다시 공격 개체 선정하도록함 
        if(bee_attacking[0] != null && bee_attacking[1] != null)
        {
            for (int i = 0; i < bee_attacking.Length; i++)
            {
                if (bee_attacking[i].obj.GetComponent<BezierObjManager>().destroyed)
                {
                    bee_times[i] = bee_cools[i];
                }
            }
        }
        

        // 시간 흐름 
        for (int i = 0; i < 2; i++)
        {
            bee_times[i] += Time.deltaTime;
        }

        for(int i = 0; i < 2; i++)
        {
            if(bee_times[i] > bee_cools[i])
            {
                bee_times[i] = 0f;
                bee_cools[i] = SetRandomCool();

                List<BezierController> bees = new List<BezierController>();
                foreach (var x in enemiesList)
                {
                    if (x == null || x.status == 4) continue; // destroyed || attacking 
                    if (x.obj.GetComponent<BezierObjManager>().type == Type.Bee)
                    {
                        bees.Add(x);
                    }
                }

                BezierController attacking = FindOrderTarget(bees);
                bee_attacking[i] = attacking;
                attacking.StartAttack();
            }
        }
    }


    float[] butterfly_times = { 0f, 0f };
    float[] butterfly_cools = { 5f, 5f };
    BezierController[] butterfly_attacking = { null, null };
    private void OrderButterflyAttack()
    {
        // 공격 중인 개체가 파괴되었다면 시간을 쿨타임시간 지나도록 설정해 다시 공격 개체 선정하도록함 
        if (butterfly_attacking[0] != null && butterfly_attacking[1] != null)
        {
            for (int i = 0; i < butterfly_attacking.Length; i++)
            {
                if (butterfly_attacking[i].obj.GetComponent<BezierObjManager>().destroyed)
                {
                    butterfly_times[i] = butterfly_cools[i];
                }
            }
        }

        // 시간 흐름 
        for (int i = 0; i < 2; i++)
        {
            butterfly_times[i] += Time.deltaTime;
        }

        for (int i = 0; i < 2; i++)
        {
            if (butterfly_times[i] > butterfly_cools[i])
            {
                butterfly_times[i] = 0f;
                butterfly_cools[i] = SetRandomCool();

                List<BezierController> butterflies = new List<BezierController>();
                foreach (var x in enemiesList)
                {
                    if (x == null || x.status == 4) continue; // destroyed || attacking 
                    if (x.obj.GetComponent<BezierObjManager>().type == Type.Butterfly)
                    {
                        butterflies.Add(x);
                    }
                }

                BezierController attacking = FindOrderTarget(butterflies);
                butterfly_attacking[i] = attacking;
                attacking.StartAttack();
            }
        }
    }


    float boss_cool_min = 8f, boss_cool_max = 12f;
    float[] boss_times = { 0f, 0f };
    float[] boss_cools = { 5f, 5f };
    BezierController[] boss_attacking = { null, null };
    // Boss는 두가지 행동 패턴이 있다 (아직 구현 안함) 
    // 1. Butterfly를 끌고가서 공격하는 행동
    // 2. 빔을 쏴서 player를 끌고가는 행동  
    private void OrderBossAttack()
    {
        // 공격 중인 개체가 파괴되었다면 시간을 쿨타임시간 지나도록 설정해 다시 공격 개체 선정하도록함 
        if (boss_attacking[0] != null && boss_attacking[1] != null)
        {
            for (int i = 0; i < boss_attacking.Length; i++)
            {
                if (boss_attacking[i].obj.GetComponent<BezierObjManager>().destroyed)
                {
                    boss_times[i] = boss_cools[i];
                }
            }
        }

        // 시간 흐름 
        for (int i = 0; i < 2; i++)
        {
            boss_times[i] += Time.deltaTime;
        }

        for (int i = 0; i < 2; i++)
        {
            if (boss_times[i] > boss_cools[i])
            {
                boss_times[i] = 0f;
                boss_cools[i] = SetRandomBossCool();

                List<BezierController> bosses = new List<BezierController>();
                foreach (var x in enemiesList)
                {
                    if (x == null || x.status == 4) continue; // destroyed || attacking 
                    if (x.obj.GetComponent<BezierObjManager>().type == Type.Boss)
                    {
                        bosses.Add(x);
                    }
                }

                BezierController attacking = FindOrderTarget(bosses);
                boss_attacking[i] = attacking;
                attacking.StartAttack();
            }
        }
    }

    private float SetRandomBossCool()
    {
        return Random.Range(boss_cool_min, boss_cool_max);
    }

    private float SetRandomCool()
    {
        return Random.Range(cool_min, cool_max);
    }

    // lists 적들 중 공격 명령 내릴 객체 리턴함 (제일 좌측 or 우측에서 선별) 
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

    ////////////////////////////////////////////////

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
