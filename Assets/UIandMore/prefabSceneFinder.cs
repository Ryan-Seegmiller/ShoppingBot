using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prefabSceneFinder : MonoBehaviour
{
    //for SceneChange Access
    public static void GoMain()
    {
        UIChanger.instance.SetSceneMain();
    }
    public static void GoMenu()
    {
        UIChanger.instance.SetSceneMenu();
    }
    public static void GoScore()
    {
        UIChanger.instance.SetSceneScoring();
    }

    //TODO get GameManagerPrefabWorking
    public static void PauseGame()
    {
        //GameManager.instance.gameActive = false;
        TimeCalc.instance.PauseTimer();
    }

    public static void UnPauseGame()
    {
        //GameManager.instance.gameActive = true;
        TimeCalc.instance.StartTimer();
    }
}
