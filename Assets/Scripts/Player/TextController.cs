using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextController : MonoBehaviour {

    Text textBox;
    Player player;

    public float textDelay;

    Coroutine co;

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
        textObj.color = new Color(textObj.color.r, textObj.color.g, textObj.color.b, 1);
        textObj.text = text;

        yield return new WaitForSeconds(waitTime);
        co = StartCoroutine(FadeOutText(textObj, 1f));
    }

    IEnumerator FadeOutText(Text textObj, float fadeRate)
    {
        while (textObj.color.a > 0.0f)
        {
            textObj.color = new Color(textObj.color.r, textObj.color.g, textObj.color.b, textObj.color.a - (Time.deltaTime / fadeRate));
            yield return null;
        }
    }
}
