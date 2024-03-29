﻿using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public float speed = 3f;
	private Rigidbody2D rb;
	private Vector3 movement;
	public GameObject bullet;

	private float bulletCoolTime; // 값 작을수록 shoot 쿨타임 빨리 돌아옴 
	public float bulletCoolSpeed = 3f; 
	public int bulletTotalCnt = 2; // bulletCoolTime 동안 쏠수 있는 탄환의 수 
	private int bulletCnt;

	// shoot sound 
	public AudioSource shootSound;

	public GameObject childPlayer;

	private void Awake()
    {
		bulletCnt = bulletTotalCnt;
		rb = GetComponent<Rigidbody2D>();
    }

	// child player active 
	public void SetChildPlayer()
    {
		childPlayer.SetActive(true);
    }


	bool startTime = false;
    void Update()
	{		
		if(LevelManager.singleton.levelStatus == 2)
        {
			return;
        }
		// move input
		movement = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
		// 화면 밖으로 못 나가도록 
		if (movement.x > Camera.main.orthographicSize / 2) return;

		// bullet
		// 일정 시간 동안 bulletTotalCnt 개의 탄만 쏠수 있음 
		if (Input.GetKeyDown("space"))
        {
			if (bulletCnt > 0)
			{
				// 첫 한 발 쏜 상황 
				if (bulletCnt == bulletTotalCnt) startTime = true;

				Instantiate(bullet, gameObject.transform.position, Quaternion.identity);
				// child player에도 bullet 나가도록 
				if(childPlayer.activeInHierarchy)
                {
					Instantiate(bullet, childPlayer.transform.position, Quaternion.identity);
				}					

				// sound
				shootSound.Play();
				bulletCnt--;
			}
		}

		if(startTime) bulletCoolTime += Time.deltaTime * bulletCoolSpeed;
		// 쿨 다됨 
		if(bulletCoolTime > .5f)
        {
			bulletCoolTime = 0;
			startTime = false;
			bulletCnt = bulletTotalCnt; // 탄 충전 
        }

	}

	void FixedUpdate()
	{
		if (LevelManager.singleton.levelStatus == 2)
		{
			return;
		}

		Vector2 nextPos = transform.position + (movement * speed * Time.deltaTime);
		if (nextPos.x > Camera.main.orthographicSize / 2 || 
			nextPos.x < Camera.main.orthographicSize / 2 * -1) return;

		// 좌우 이동 
		rb.MovePosition(transform.position + (movement * speed * Time.deltaTime));
	}

	// 공격 받음 
    private void OnTriggerEnter2D(Collider2D collision)
    {
		if(collision.gameObject.tag == "enemy" || collision.gameObject.tag == "enemyBullet")
        {
			if(!childPlayer.activeInHierarchy)
            {
				LevelManager.singleton.PlayerDead(gameObject, false);
				childPlayer.SetActive(false);
			}
			else // child player active 상태 
            {
				LevelManager.singleton.DeadEffect(gameObject);
				gameObject.transform.position = childPlayer.transform.position;
				childPlayer.SetActive(false);				
            }			
		}

		// beam captured
		if (collision.gameObject.tag == "beam")
        {
			LevelManager.singleton.PlayerDead(gameObject, true);
			collision.gameObject.GetComponentInParent<BezierController>().SpawnBeamHitPlayer();
		}
    }


}
