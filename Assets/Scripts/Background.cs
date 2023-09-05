using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private Rigidbody2D _rb2d;

    private void Start()
    {
        setRigidbody(2f);
    }

    public void setRigidbody(float speed)
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _rb2d.bodyType = RigidbodyType2D.Kinematic;
        _rb2d.velocity = new Vector2(0f, -speed);
    }

    public void setStop()
    {
        _rb2d.velocity = Vector2.zero;
    }
}
