using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pauseBtn : MonoBehaviour
{
    private Image image;
    private int prevStatus;
    public Sprite pauseSprite;
    public Sprite playSprite;
    public GameObject restartBtn;

    // blink effect 
    private float blinkInterval = 1f; 
    private float timer;
    private bool isVisible = true;

    private void Awake()
    {
        image = gameObject.GetComponent<Image>();
    }

    public void PauseGame()
    {        
        // 이미 일시정지 상태라면 기존 상태로 되돌림 
        if (LevelManager.singleton.levelStatus == 2)
        {
            LevelManager.singleton.levelStatus = prevStatus;
            image.sprite = pauseSprite;
            restartBtn.SetActive(false);
        }
        // 기존 상태 저장하고 일시정지 처리 
        else
        {
            prevStatus = LevelManager.singleton.levelStatus;
            LevelManager.singleton.levelStatus = 2;
            image.sprite = playSprite;
            restartBtn.SetActive(true);
        }
    }

    private void Update()
    {
        if(LevelManager.singleton.levelStatus == 2)
        {
            // Update the timer
            timer += Time.deltaTime;

            // Check if it's time to toggle visibility
            if (timer >= blinkInterval)
            {
                // Toggle visibility
                isVisible = !isVisible;

                if(isVisible)
                {
                    image.color = Color.white;
                } 
                else
                {
                    image.color = Color.gray;
                }

                // Reset the timer
                timer = 0f;
            }
        }
    }


}
