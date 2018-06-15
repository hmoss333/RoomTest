using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextController : MonoBehaviour {

    Text textBox;
    Player player;

    public float textDelay;
    public static string textToDisplay;

    public void DisplayText()
    {
        if (player == null)
        {
            player = GameObject.FindObjectOfType<Player>();
            textBox = player.GetComponentInChildren<Text>();
        }

        Coroutine co = StartCoroutine(DisplayText(textBox, textToDisplay, textDelay));
    }
    IEnumerator DisplayText(Text textObj, string text, float waitTime)
    {
        textObj.color = new Color(textObj.color.r, textObj.color.g, textObj.color.b, 1);
        textObj.text = textToDisplay;

        yield return new WaitForSeconds(waitTime);
        StartCoroutine(FadeOutText(textObj, 1f));
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
