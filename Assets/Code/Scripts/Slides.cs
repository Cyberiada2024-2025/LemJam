using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Slides : MonoBehaviour
{

    [SerializeField]
    CanvasGroup canvasGroup;

    [SerializeField]
    TMP_Text text;

    int index = 0;

    string[] texts =
    {
        "Trurl needs parts for a machine he is currently building.",
        "In desperation, he sends his Cybermoth for them.",
        "Unfortunately, Klapaucius wants to ruin his plans and sends a ",
        "Swarm of lanterns to his friend's envoy to distract the Cybermoth from her goal."
    };

    private void Start()
    {
        text.outlineWidth = 0.2f;
        text.outlineColor = new Color32(0, 0, 0, 255);
        StartCoroutine(FadeIn());
    }


    private IEnumerator FadeIn()
    {
        text.text = texts[index++];
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / 3;
            yield return null;
        }
       
        canvasGroup.interactable = false;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {        
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / 3;
            yield return null;
        }
        
        canvasGroup.interactable = false;
        if(index < texts.Length)
        {
            StartCoroutine(FadeIn());
        }
        else
        {
            GameManager.Restart();
            Debug.Log("Koniec");
        }
    }
}
