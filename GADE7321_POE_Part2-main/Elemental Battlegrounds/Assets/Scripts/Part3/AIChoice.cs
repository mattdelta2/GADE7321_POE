using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AIChoice : MonoBehaviour
{

    public void Part2()
    {
        SceneManager.LoadScene("ChooseAiDif");
    }


    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Part3()
    {
        SceneManager.LoadScene("Part3");
    }

}
