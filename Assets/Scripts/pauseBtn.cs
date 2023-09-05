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


}
