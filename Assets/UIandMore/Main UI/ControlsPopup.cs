using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlsPopup : MonoBehaviour
{
    [SerializeField] Sprite[] sprites = new Sprite[5];
    [SerializeField] Image img;

    [SerializeField] string[] displayText = new string[5];
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] string[] descString = new string[5];
    [SerializeField] TextMeshProUGUI desc;
    
    [SerializeField] GameObject panel;
    [SerializeField] GameObject theObject;
    [SerializeField] GameObject HotKeySymbol;

    void Start()
    {
        theObject.SetActive(false);
        HotKeySymbol.SetActive(true);
        //StartCoroutine(ShowControls());
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.H))
        {
            HotKeySymbol.SetActive(false);
            StartCoroutine(ShowControls());
        }
    }

    public IEnumerator ShowControls()
    {
        for(int i = 0; i < sprites.Length; i++)
        {
            yield return new WaitForSeconds(2);
            img.sprite = sprites[i];
            text.text = displayText[i];
            desc.text = descString[i];
            if (!theObject.activeSelf)
            {
                theObject.SetActive(true);
            }
            if (i == sprites.Length - 1)
            {
                panel.SetActive(false);
            }
        }
        theObject.SetActive(false);
        HotKeySymbol.SetActive(true);
    }
}
