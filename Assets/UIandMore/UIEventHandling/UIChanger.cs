using audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChanger : MonoBehaviour
{
    public static UIChanger instance;

    public GameObject menu;
    public GameObject main;
    public GameObject scoring;

    [SerializeField] bool Started = false;

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
    }
    public void SetSceneMenuStop()
    {
        ClickedSound(0);

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
            ClickedSound(0);
            UIEvents.instance.StartGame();
            Started = true;
        }
        else
        {
            ClickedSound(1);
            ResetPauseMainOogaBooga();
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
        //UIEvents.instance.PauseGame();
    }

    public void QuitTheGame()
    {
        ClickedSound(1);

        UIEvents.instance.QuitGame();
    }

    public void SetPause()
    {
        ClickedSound(0);

        pMenu.SetActive(true);
        mScreen.SetActive(false);

        UIEvents.instance.PauseGame();
    }

    //Simplifying method call to audio
    public void ClickedSound(int index)
    {
        AudioManager.instance.PlaySound2D(index);
    }

    public void VolumeSet(float f)
    {
        AudioManager.instance.AdjustMusicVolume(f);
    }
    public void VolumeSetSFX(float f)
    {
        AudioManager.instance.AdjustSoundFXVolume(f);
    }
}
