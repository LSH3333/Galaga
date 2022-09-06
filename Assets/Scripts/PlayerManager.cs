using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public float speed = 20f;
	private Rigidbody2D rb;
	private Vector3 movement;
	public GameObject bullet;

	private float bulletCoolTime;
	private float bulletCoolSpeed = 4f;
	private int bulletCnt;

	private void Awake()
    {
		rb = GetComponent<Rigidbody2D>();
    }

	bool startTime = false;
	bool canFire = true;
    void Update()
	{		
		// move input
		movement = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);

		
		// bullet 
		if (Input.GetKeyDown("space"))
        {
			if (canFire)
			{
				Instantiate(bullet, gameObject.transform.position, Quaternion.identity);
				bulletCnt++;
			}
		}
		print(bulletCoolTime);
		if (bulletCnt == 1) startTime = true;
		if (startTime) bulletCoolTime += Time.deltaTime * bulletCoolSpeed;
		if (bulletCoolTime < 1 && bulletCnt >= 2) canFire = false;
		if (bulletCoolTime >= 1) { bulletCoolTime = 0; canFire = true; startTime = false; }
	}

	void FixedUpdate()
	{		

		// 좌우 이동 
		rb.MovePosition(transform.position + (movement * speed * Time.deltaTime));
	}



}
