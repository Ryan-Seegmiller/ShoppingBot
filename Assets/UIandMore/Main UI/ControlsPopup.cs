using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlsPopup : MonoBehaviour
{
    [SerializeField] Sprite[] sprites = new Sprite[4];
    [SerializeField] string[] displayText = new string[4]; 
    [SerializeField] Image img;

    [SerializeField] TextMeshProUGUI text;

     void Start()
    {
        //gameObject.transform.position = new Vector3(-180, -50, 0);
        StartCoroutine(ShowControls());
    }

    public IEnumerator ShowControls()
    {
        for(int i = 1; i < sprites.Length; i++)
        {
            yield return new WaitForSeconds(5);
            img.sprite = sprites[i];
            text.text = displayText[i];
        }
        gameObject.SetActive(false);
    }
}
