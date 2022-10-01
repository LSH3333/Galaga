using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClass : MonoBehaviour
{
    public GameObject enemyBullet;

    private void Start()
    {
        Instantiate(enemyBullet);
    }
}
