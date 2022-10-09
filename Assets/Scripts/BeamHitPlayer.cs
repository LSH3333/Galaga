using UnityEngine;

// boss beam 맞은 player 
public class BeamHitPlayer : MonoBehaviour
{
    public BezierController bc;
    private float rot = 0f;
    private float moveSpeed = 3f;
    private Vector3 goalPos;

    // 0: 보스 빔 맞고 보스에 붙어 있는 상태
    // 1: 플레이어에게 구출된 상태 
    private int status = 0;

    private void Update()
    {
        if(status == 0)
        {
            goalPos = bc.obj.transform.position;
            goalPos.y += .3f;
            if (Vector3.Distance(transform.position, goalPos) > .3f)
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
       else if (status == 1)
        {
            goalPos = LevelManager.singleton.player.transform.position;
            goalPos.x += .25f;
            if (Vector3.Distance(transform.position, goalPos) > .1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, goalPos, Time.deltaTime * moveSpeed);
                transform.rotation = Quaternion.Euler(0f, 0f, rot);
                rot += 10f;
            }
            else
            {
                status = 2;
                Destroy(gameObject);
                LevelManager.singleton.player.GetComponent<PlayerManager>().SetChildPlayer();
            }
        }
    }

    public void JoinPlayer()
    {
        
        status = 1;
    }
}
