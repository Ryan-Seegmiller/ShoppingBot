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
    [SerializeField] GameObject MotKeySymbol;

    [SerializeField] Sprite blank;
    [SerializeField] string[] motivation = new string[5];

    bool going = false;

    void Start()
    {
        theObject.SetActive(false);
        HotKeySymbol.SetActive(true);
        //StartCoroutine(ShowControls());
    }

    void Update()
    {
        if (!going)
        {
            if (Input.GetKeyUp(KeyCode.H))
            {
                HotKeySymbol.SetActive(false);
                MotKeySymbol.SetActive(false);
                StartCoroutine(ShowControls());
            }
            if (Input.GetKeyUp(KeyCode.M))
            {
                HotKeySymbol.SetActive(false);
                MotKeySymbol.SetActive(false);
                StartCoroutine(ShowMotivation());
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIChanger.instance.SetPause();
        }
    }

    public IEnumerator ShowControls()
    {
        going = true;
        for(int i = 0; i < sprites.Length; i++)
        {
            img.sprite = sprites[i];
            text.text = displayText[i];
            desc.text = descString[i];
            if (!theObject.activeSelf)
            {
                theObject.SetActive(true);
            }
            yield return new WaitForSeconds(2);
        }
        theObject.SetActive(false);
        HotKeySymbol.SetActive(true);
        MotKeySymbol.SetActive(true);
        going = false;
    }

    public IEnumerator ShowMotivation()
    {
        going = true;
        for (int i = 0; i < motivation.Length; i++)
        {
            img.sprite = blank;
            text.text = "";
            desc.text = motivation[i];
            if (!theObject.activeSelf)
            {
                theObject.SetActive(true);
            }
            yield return new WaitForSeconds(2);
        }
        theObject.SetActive(false);
        HotKeySymbol.SetActive(true);
        MotKeySymbol.SetActive(true);
        going = false;
    }
}
