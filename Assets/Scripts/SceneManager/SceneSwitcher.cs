using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher instance;

    public static int currentScene;
    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void A_ExitButton()
    {
        Application.Quit();
    }

    public void A_LoadScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && currentScene != 3)
            A_ExitButton();
    }
}
