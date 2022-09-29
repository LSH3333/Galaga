using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClass : MonoBehaviour
{
    public GameObject[] objs = { null, null };

    private void Start()
    {
        if(objs[0].transform.Find("child") == null)
        {
            print("null");
        }
            
    }
}
