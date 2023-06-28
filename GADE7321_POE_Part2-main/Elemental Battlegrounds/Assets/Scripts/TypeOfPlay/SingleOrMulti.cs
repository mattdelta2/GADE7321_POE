using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleOrMulti : MonoBehaviour
{


    public void SinglePlayer()
    {
        SceneManager.LoadScene("ChooseAI");
    }


    public void Multiplayer()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Back()
    {
        SceneManager.LoadScene("SingleOrMulti");
    }
}

