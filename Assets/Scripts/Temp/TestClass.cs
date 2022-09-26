using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClass : MonoBehaviour
{
    private void Start()
    {
        Vector3 pos1 = new Vector3(1f, 1f, 1f);
        Vector3 pos2 = new Vector3(2f, 2f, 2f);
        Vector3 pos3 = pos1 + pos2;
        print(pos3);
    }
}
