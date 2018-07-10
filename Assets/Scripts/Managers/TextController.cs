using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextController : MonoBehaviour {

    [HideInInspector]
    public Image textBoxBackground;
    Text textBox;
    Player player;

    public float textDelay;

    Coroutine co;

    private void Start()
    {
        
    }

    public void DisplayText(string text)
    {
        if (player == null)
        {
            player = GameObject.FindObjectOfType<Player>();
            textBoxBackground = player.GetComponentInChildren<Image>();
            textBox = textBoxBackground.GetComponentInChildren<Text>();
        }

        if (co != null)
            StopCoroutine(co);

        co = StartCoroutine(DisplayText(textBoxBackground, textBox, text, textDelay));
    }
    IEnumerator DisplayText(Image background, Text textObj, string text, float waitTime)
    {
        //textObj.color = new Color(textObj.color.r, textObj.color.g, textObj.color.b, 1);
        background.GetComponent<CanvasGroup>().alpha = 1f;
        textObj.text = text;

        yield return new WaitForSeconds(waitTime);
        co = StartCoroutine(FadeOutText(background, 1f));
    }

    public IEnumerator FadeOutText(Image background, float fadeRate)
    {
        CanvasGroup textCanv = background.GetComponent<CanvasGroup>();

        while (textCanv.alpha > 0.0f)//textObj.color.a > 0.0f)
        {
            //textObj.color = new Color(textObj.color.r, textObj.color.g, textObj.color.b, textObj.color.a - (Time.deltaTime / fadeRate));
            textCanv.alpha = textCanv.alpha - (Time.deltaTime / fadeRate);
            yield return null;
        }
    }
}
