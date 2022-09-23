using System.Collections;
using UnityEngine;

public class BezierCurve : MonoBehaviour {
    public Transform[] p;
    public float speed = 0.5f;
    private float t = 0;
    private Vector3 bezierPosition;

    // Update is called once per frame
    void Start() {
        StartCoroutine (BezierCurveStart());
    }

    IEnumerator BezierCurveStart() {
        t = 0f;
        while (t < 1) {
            t += Time.deltaTime * speed;
            bezierPosition = Mathf.Pow (1 - t, 3) * p[0].position
                             + 3 * t * Mathf.Pow (1 - t, 2) * p[1].position
                             + 3 * t * Mathf.Pow (1 - t, 1) * p[2].position
                             + Mathf.Pow (t, 3) * p[3].position;

            transform.position = bezierPosition;
            yield return null;
        }
        StartCoroutine (BezierCurveStart());
    }
}