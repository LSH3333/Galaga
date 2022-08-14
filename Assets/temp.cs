using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class temp : MonoBehaviour
{
    private void Start()
    {
        System.Numerics.Vector2 v1 = System.Numerics.Vector2.Zero;
        System.Numerics.Vector2 v2 = new System.Numerics.Vector2(3, 4);
        float distance = (v1 - v2).Length();
        System.Numerics.Vector2 res = (v1 - v2);

        print(distance);
        print(res);
    }

    void func()
    {
        
        
    }
}
