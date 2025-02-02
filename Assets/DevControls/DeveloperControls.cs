using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeveloperControls : MonoBehaviour
{

    void Awake()
    {
        if (!Application.isEditor)
        {
            Destroy(this);
        }
    }

    void Update()
    {
        LoadScenesFromUserInput();
    }

    private static void LoadScenesFromUserInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            int nextSceneIndex = (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1) % UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneIndex);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int prevSceneIndex = (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 1 + UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings) % UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
            UnityEngine.SceneManagement.SceneManager.LoadScene(prevSceneIndex);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}
