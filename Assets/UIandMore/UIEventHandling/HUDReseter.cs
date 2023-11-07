using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Device;

public class HUDReseter : MonoBehaviour
{
    [SerializeField] GameObject mScreen;
    [SerializeField] GameObject pMenu;
    private void OnEnable()
    {
        mScreen.SetActive(true);
        pMenu.SetActive(false);
    }

    private void OnDisable()
    {
        mScreen.SetActive(true);
        pMenu.SetActive(false);
    }
}
