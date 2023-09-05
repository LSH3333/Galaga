using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float bulletSpeed = 5f;
	private Rigidbody2D rb;
	private Vector3 movement;
	

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		movement = new Vector2(0f, 1f);
	}

    private void Update()
    {
		// 화면 위로 나가면 탄 파괴 
		if (transform.position.y > Camera.main.transform.position.y +
			Camera.main.orthographicSize)
			Destroy(gameObject);
    }

    void FixedUpdate()
	{
		if (LevelManager.singleton.levelStatus == 2)
		{
			return;
		}
		rb.MovePosition(transform.position + (movement * bulletSpeed * Time.deltaTime));		
	}


}
