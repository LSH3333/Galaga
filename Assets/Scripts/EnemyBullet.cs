using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float bulletSpeed = 20f;
    private Rigidbody2D rb;
    private Vector3 targetPosition;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // bullet 소환시 player가 있던 위치를 향하도록함  
        targetPosition = GameObject.Find("Player").transform.position;
        rb.velocity = (targetPosition - transform.position) * bulletSpeed;

    }

    private void Update()
    {
        // 화면 아래로 bullet 나가면 파괴 
        if (transform.position.y < Camera.main.transform.position.y -
            Camera.main.orthographicSize)
            Destroy(gameObject);

    }

    void FixedUpdate()
    {
    }

}
