using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildPlayerManager : MonoBehaviour
{
    private void OnEnable()
    {

    }

    // 공격 받음 
    private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "enemy" || collision.gameObject.tag == "enemyBullet")
		{
			LevelManager.singleton.DeadEffect(gameObject);
			gameObject.SetActive(false);
        }
	}
}
