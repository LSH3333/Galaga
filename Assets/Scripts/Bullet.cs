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
		
    }

    void FixedUpdate()
	{

		// 좌우 이동 
		rb.MovePosition(transform.position + (movement * bulletSpeed * Time.deltaTime));		
	}


}
