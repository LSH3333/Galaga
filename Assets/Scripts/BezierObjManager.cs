using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 적의 본체 obj 
public class BezierObjManager : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public BezierController bc;

    public Type type;
    private int hp;
    // 도착 지점
    public int arrivePos;
    public bool destroyed = false;

    // bullet
    public GameObject enemyBullet;

    private Color yellow = new Color(255f/255f, 200f/255f, 0/255f);
    private Color red = new Color(255f / 255f, 0f / 255f, 0f / 255f);
    private Color green = new Color(50f / 255f, 255f / 255f, 0f / 255f);
    private Color blue = new Color(0f / 255f, 125f / 255f, 255f / 255f);



    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }



    public void SetType(Type type)
    {
        if(type == Type.Bee)
        {
            hp = 1;
            spriteRenderer.color = yellow;
        }
        else if(type == Type.Butterfly)
        {
            hp = 1;
            spriteRenderer.color = red;
        }
        else if(type == Type.Boss)
        {
            hp = 2;
            spriteRenderer.color = green;
        }
    }

    // ArrivePos를 따라 좌우로 움직임 
    private void FollowArrivePos()
    {
        Vector3 arrviePos = bc.ArrivePoint.transform.position;
        gameObject.transform.position = arrviePos;
    }

    // enemy shoot bullet 
    public void OrderShoot()
    {
        if(gameObject.activeInHierarchy)
        {
            StartCoroutine("DelayShoot");
        }        
    }

    IEnumerator DelayShoot()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(enemyBullet, gameObject.transform.position, Quaternion.identity);
    }


    

    private void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // bullet에 맞음
        if (collision.gameObject.tag == "bullet")
        {
            hp--;
            if(hp <= 0)
            {
                LevelManager.singleton.EnemyHit(gameObject.transform.position);
                destroyed = true;                
                gameObject.SetActive(false);
            }
            else
            {
                spriteRenderer.color = blue;
            }

            Destroy(collision.gameObject);
        }
    }
}
