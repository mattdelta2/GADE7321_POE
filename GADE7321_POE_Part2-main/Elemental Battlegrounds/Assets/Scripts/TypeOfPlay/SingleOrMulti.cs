using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleOrMulti : MonoBehaviour
{


    public void SinglePlayer()
    {
        SceneManager.LoadScene("ChooseAiDif");
    }


    public void Multiplayer()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

