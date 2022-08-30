using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalagaManager : MonoBehaviour
{
    // singleton 
    public static GalagaManager singleton;

    private void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
        }
    }



}
