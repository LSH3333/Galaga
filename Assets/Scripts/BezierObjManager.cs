using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 적의 본체 obj 
public class BezierObjManager : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public BezierController bc;
    public Animator animator;

    public RuntimeAnimatorController enemy1_controller;
    public RuntimeAnimatorController enemy2_controller;
    public RuntimeAnimatorController enemy3_controller;
    public RuntimeAnimatorController enemy4_controller;

    public Type type;
    private int hp;
    // 도착 지점
    public int arrivePos;
    public bool destroyed = false;

    // bullet
    public GameObject enemyBullet;

    //private Color yellow = new Color(255f/255f, 200f/255f, 0/255f);
    //private Color red = new Color(255f / 255f, 0f / 255f, 0f / 255f);
    //private Color green = new Color(50f / 255f, 255f / 255f, 0f / 255f);
    //private Color blue = new Color(0f / 255f, 125f / 255f, 255f / 255f);


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        bc = GetComponentInParent<BezierController>();
    }



    public void SetType(Type type)
    {
        if(type == Type.Bee)
        {
            hp = 1;
            animator.runtimeAnimatorController = enemy1_controller;
        }
        else if(type == Type.Butterfly)
        {
            hp = 1;
            animator.runtimeAnimatorController = enemy2_controller;
        }
        else if(type == Type.Boss)
        {
            hp = 2;
            animator.runtimeAnimatorController = enemy3_controller;
        }
    }


    // enemy shoot bullet 
    public void OrderShoot()
    {
        if(gameObject.activeInHierarchy && LevelManager.singleton.player.activeInHierarchy)
        {
            StartCoroutine("DelayShoot");
        }        
    }

    IEnumerator DelayShoot()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(enemyBullet, gameObject.transform.position, Quaternion.identity);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        // bullet에 맞음
        if (collision.gameObject.tag == "bullet")
        {
            hp--;
            if(hp <= 0)
            {
                // beamHitPlayer 갖고 있는 boss가 죽을때 
                if(type == Type.Boss && bc.beamHitPlayerObj != null)
                {
                    bc.beamHitPlayerObj.GetComponent<BeamHitPlayer>().JoinPlayer();
                }
                LevelManager.singleton.EnemyHit(gameObject.transform.position);
                destroyed = true;             
                bc.gameObject.SetActive(false);
                LevelManager.singleton.AddScore(type);
            }
            else // boss hit 
            {
                animator.runtimeAnimatorController = enemy4_controller;
            }

            Destroy(collision.gameObject);
        }
    }
}
