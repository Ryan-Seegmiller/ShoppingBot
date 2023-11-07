using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChanger : MonoBehaviour
{
    public static UIChanger instance;

    public GameObject menu;
    public GameObject main;
    public GameObject scoring;

    bool Started = false;

    //lazy workaround. fix after MVP
    [SerializeField] GameObject mScreen;
    [SerializeField] GameObject pMenu;

    // Start is called before the first frame update
    void Start()
    {
        //Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        menu.SetActive(true);
        main.SetActive(false);
        scoring.SetActive(false);
        Started = false;
    }

    public void SetSceneMenu()
    {
        menu.SetActive(true);
        //ResetPauseMainOogaBooga();
        main.SetActive(false);
        scoring.SetActive(false);
        Started = false;

        UIEvents.instance.StopGame();
    }
    public void SetSceneMain()
    {
        main.SetActive(true);
        menu.SetActive(false);
        scoring.SetActive(false);

        if (!Started)
        {
            UIEvents.instance.StartGame();
            Started = true;
        }
        else
        {
            UIEvents.instance.ContinueGame();
        }
    }
    public void SetSceneScoring()
    {
        scoring.SetActive(true);
        menu.SetActive(false);
        //ResetPauseMainOogaBooga();
        main.SetActive(false);
    }
    public void ResetPauseMainOogaBooga()
    {
        mScreen.SetActive(true);
        pMenu.SetActive(false);
        UIEvents.instance.PauseGame();
    }

    public void QuitTheGame()
    {
        UIEvents.instance.QuitGame();
    }

    public void SetPause()
    {
        pMenu.SetActive(true);
        mScreen.SetActive(false);

        UIEvents.instance.PauseGame();
    }

    //Add call to things and stuff
}
