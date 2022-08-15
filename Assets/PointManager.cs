using UnityEngine;

public class PointManager : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public GameObject prevPoint;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.SetPosition(0, gameObject.transform.position);
        lineRenderer.SetPosition(1, prevPoint.transform.position);
    }


}
