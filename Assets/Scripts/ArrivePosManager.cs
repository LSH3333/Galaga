using UnityEngine;

public class ArrivePosManager : MonoBehaviour
{
    private Vector3 originPos;
    private float val = 0.01f; // 1 프레임에 val 만큼 이동 
    private float moveAmount = 0.3f; // 원점 기준 한쪽 방향으로 움직이는 거리 
    private bool moveLeft = true;

    private void Start()
    {
        originPos = gameObject.transform.position;
    }

    // 한 프레임 이동 
    void MoveOneFrame(bool left)
    {
        if(left)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - val,
                gameObject.transform.position.y, gameObject.transform.position.z);
        }
        else
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + val,
                gameObject.transform.position.y, gameObject.transform.position.z);
        }
        
    }

    private void Update()
    {
        if(gameObject.transform.position.x <= originPos.x - moveAmount)
        {
            gameObject.transform.position = new Vector3(originPos.x - moveAmount , gameObject.transform.position.y, gameObject.transform.position.z);
            moveLeft = !moveLeft;
        }
        else if(gameObject.transform.position.x >= originPos.x + moveAmount)
        {
            gameObject.transform.position = new Vector3(originPos.x + moveAmount , gameObject.transform.position.y, gameObject.transform.position.z);
            moveLeft = !moveLeft;
        }

        MoveOneFrame(moveLeft);

        
    }

}
