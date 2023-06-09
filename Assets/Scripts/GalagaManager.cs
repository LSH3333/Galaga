using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GalagaManager : MonoBehaviour
{
    // singleton 
    public static GalagaManager singleton;

    private void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
        }
    }

    private void Start()
    {
        AudioListener.volume = 0.3f;
    }

    public void RestartScene()
    {
        // Get the current active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Reload the current active scene
        SceneManager.LoadScene(currentScene.name);
    }

}
