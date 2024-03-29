﻿using UnityEngine;

// 이 클래스에 의해 ArrivePos 들이 움직이게됨 
public class ArrivePosManager : MonoBehaviour
{
    private Vector3 originPos;
    private float val = 0.4f; // 1 프레임에 val 만큼 이동, 클수록 빠르게 움직임  
    private float moveAmount = 0.3f; // 원점 기준 한쪽 방향으로 움직이는 거리, 클수록 많이 움직임 
    private Vector3 moveVector; // Vector 연산위해 
    private bool moveLeft = true; // true시 obj 왼쪽 이동 

    // 이 ArrivePos 위치의 Obj의 attack pattern 
    public GameObject attackPattern;
    public GameObject attackPattern2; // boss용 attack pattern 

    private void Start()
    {
        moveVector = new Vector3(val, 0f, 0f);
        originPos = gameObject.transform.position;
    }

    // 좌 or 우 한 프레임 이동 
    void MoveOneFrame(bool left)
    {
        if(left)
        {
            gameObject.transform.position -= moveVector * Time.deltaTime;
        }
        else
        {
            gameObject.transform.position += moveVector * Time.deltaTime;
        }
        
    }

    // 좌우 이동 
    private void MoveLeftAndRight()
    {
        if (gameObject.transform.position.x <= originPos.x - moveAmount)
        {
            gameObject.transform.position = new Vector3(originPos.x - moveAmount, gameObject.transform.position.y, gameObject.transform.position.z);
            moveLeft = !moveLeft;
        }
        else if (gameObject.transform.position.x >= originPos.x + moveAmount)
        {
            gameObject.transform.position = new Vector3(originPos.x + moveAmount, gameObject.transform.position.y, gameObject.transform.position.z);
            moveLeft = !moveLeft;
        }

        MoveOneFrame(moveLeft);
    }


    private void Update()
    {
        MoveLeftAndRight();

        
    }

}
