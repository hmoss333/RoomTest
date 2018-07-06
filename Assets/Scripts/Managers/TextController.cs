using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextController : MonoBehaviour {

    [HideInInspector]
    public Text textBox;
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
            textBox = player.GetComponentInChildren<Text>();
        }

        if (co != null)
            StopCoroutine(co);

        co = StartCoroutine(DisplayText(textBox, text, textDelay));
    }
    IEnumerator DisplayText(Text textObj, string text, float waitTime)
    {
        //textObj.color = new Color(textObj.color.r, textObj.color.g, textObj.color.b, 1);
        textObj.GetComponent<CanvasGroup>().alpha = 1f;
        textObj.text = text;

        yield return new WaitForSeconds(waitTime);
        co = StartCoroutine(FadeOutText(textObj, 1f));
    }

    public IEnumerator FadeOutText(Text textObj, float fadeRate)
    {
        CanvasGroup textCanv = textObj.GetComponent<CanvasGroup>();

        while (textCanv.alpha > 0.0f)//textObj.color.a > 0.0f)
        {
            //textObj.color = new Color(textObj.color.r, textObj.color.g, textObj.color.b, textObj.color.a - (Time.deltaTime / fadeRate));
            textCanv.alpha = textCanv.alpha - (Time.deltaTime / fadeRate);
            yield return null;
        }
    }
}
