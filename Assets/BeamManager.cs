using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamManager : MonoBehaviour
{
    public GameObject child1, child2;
    private float t;
    private float parent_t = 1f;
    private float child1_t = 2f;
    private float child2_t = 3f;
    private float end_t = 8f;

    private void Awake()
    {
        child2.GetComponent<BoxCollider2D>().enabled = false;
    }

    private void Update()
    {
        t += Time.deltaTime;

        if(t >= parent_t)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        if(t >= child1_t)
        {
            child1.GetComponent<SpriteRenderer>().enabled = true;
        }
        if(t >= child2_t)
        {
            child2.GetComponent<SpriteRenderer>().enabled = true;
            child2.GetComponent<BoxCollider2D>().enabled = true;
        }
        if(t >= end_t)
        {
            Destroy(gameObject);
        }
    }
}
