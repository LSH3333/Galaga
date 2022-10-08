using UnityEngine;

public class BeamHitPlayer : MonoBehaviour
{
    public BezierController bc;
    private float rot = 0f;
    private float moveSpeed = 3f;
    private Vector3 goalPos;

    private void Start()
    {
        
    }

    private void Update()
    {
        

        goalPos = bc.obj.transform.position;
        goalPos.y += .5f;
        if (Vector3.Distance(transform.position, goalPos) > .1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, goalPos, Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.Euler(0f, 0f, rot);
            rot += 10f;
        }
        else
        {
            transform.position = goalPos;
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }

        
    }
}
