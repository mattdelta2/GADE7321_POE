using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void Options()
    {
        SceneManager.LoadScene("HowToPlay");

    }

    public void StartGame()
    {
        SceneManager.LoadScene("SingleOrMulti");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
