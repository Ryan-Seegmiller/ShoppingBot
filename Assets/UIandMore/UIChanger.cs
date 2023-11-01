using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChanger : MonoBehaviour
{
    public static UIChanger instance;

    public GameObject menu;
    public GameObject main;
    public GameObject scoring;

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
    }

    public void SetSceneMenu()
    {
        menu.SetActive(true);
        ResetPauseMainOogaBooga();
        main.SetActive(false);
        scoring.SetActive(false);
    }
    public void SetSceneMain()
    {
        main.SetActive(true);
        menu.SetActive(false);
        scoring.SetActive(false);

        //Tell game Manager to start
    }
    public void SetSceneScoring()
    {
        scoring.SetActive(true);
        menu.SetActive(false);
        ResetPauseMainOogaBooga();
        main.SetActive(false);
    }
    public void ResetPauseMainOogaBooga()
    {
        mScreen.SetActive(true);
        pMenu.SetActive(false);
    }
}
