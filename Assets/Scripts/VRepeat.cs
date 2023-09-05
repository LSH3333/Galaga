using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class VRepeat : MonoBehaviour
{
    private BoxCollider2D _box;
    public float _verticalLength;

    private void Awake()
    {
        setBoxCollider();
    }

    private void Update()
    {
        updateObject();
    }

    public void setBoxCollider()
    {
        _box = GetComponent<BoxCollider2D>();
        _verticalLength = _box.size.y;
        
    }

    public void updateObject()
    {
        if (-transform.position.y > _verticalLength)
        {
            ResetPosition();
        }            
    }

    private void ResetPosition()
    {
        Vector2 addPos = new Vector2(0f, 2 * _verticalLength);
        transform.position = (Vector2)transform.position + addPos;
    }
}
