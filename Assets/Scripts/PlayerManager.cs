using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public float speed = 20f;
	private Rigidbody2D rb;
	private Vector3 movement;
	public GameObject bullet;

	private float bulletCoolTime;
	public float bulletCoolSpeed = .5f;
	private int bulletTotalCnt = 3; // bulletCoolTime 동안 쏠수 있는 탄환의 수 
	private int bulletCnt; 
	

	private void Awake()
    {
		bulletCnt = bulletTotalCnt;
		rb = GetComponent<Rigidbody2D>();
    }


	bool startTime = false;
    void Update()
	{		
		// move input
		movement = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);

		
		// bullet
        // 일정 시간 동안 bulletTotalCnt 개의 탄만 쏠수 있음 
		if (Input.GetKeyDown("space"))
        {
			if (bulletCnt > 0)
			{
				// 첫 한 발 쏜 상황 
				if (bulletCnt == bulletTotalCnt) startTime = true;

				Instantiate(bullet, gameObject.transform.position, Quaternion.identity);
				bulletCnt--;
			}
		}

		if(startTime) bulletCoolTime += Time.deltaTime * bulletCoolSpeed;
		// 쿨 다됨 
		if(bulletCoolTime > 1)
        {
			bulletCoolTime = 0;
			startTime = false;
			bulletCnt = bulletTotalCnt; // 탄 충전 
        }

	}

	void FixedUpdate()
	{		

		// 좌우 이동 
		rb.MovePosition(transform.position + (movement * speed * Time.deltaTime));
	}



}
