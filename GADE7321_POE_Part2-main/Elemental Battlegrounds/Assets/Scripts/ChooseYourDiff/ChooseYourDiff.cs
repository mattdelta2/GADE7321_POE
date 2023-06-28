using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseYourDiff : MonoBehaviour
{


    public void EasyMode()
    {
        SceneManager.LoadScene("GameSceneAIEasy");

    }


    public void MeduimMode()
    {
        SceneManager.LoadScene("GameSceneAIMeduim");

    }



    public void HardMode()
    {

        SceneManager.LoadScene("GameSceneAIHard");

    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
