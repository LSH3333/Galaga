﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClass : MonoBehaviour
{
    public GameObject target;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, -1 * Time.deltaTime * 4f);
    }
}
