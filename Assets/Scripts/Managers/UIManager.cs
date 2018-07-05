using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    Image pauseUI;
    Image winUI;
    Image loseUI;

    TextController tc;
    Color currentTextColor;

    // Use this for initialization
    void Start () {
        tc = GameObject.FindObjectOfType<TextController>();

		pauseUI = GameObject.Find("PauseUI").GetComponent<Image>();
        winUI = GameObject.Find("WinUI").GetComponent<Image>();
        loseUI = GameObject.Find("LoseUI").GetComponent<Image>();
        pauseUI.GetComponent<CanvasGroup>().alpha = 0f;
        winUI.GetComponent<CanvasGroup>().alpha = 0f;
        loseUI.GetComponent<CanvasGroup>().alpha = 0f;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PauseMenu()
    {
        Color tempColor = tc.textBox.color;

        if (GameManager.gameState == GameManager.GameState.Paused)
        {
            pauseUI.GetComponent<CanvasGroup>().alpha = 1f;
            currentTextColor = tempColor;
            tc.textBox.color = new Color(tempColor.r, tempColor.g, tempColor.b, 0f);
        }
        else
        {
            pauseUI.GetComponent<CanvasGroup>().alpha = 0f;
            tc.textBox.color = currentTextColor;
            tc.StartCoroutine(tc.FadeOutText(tc.textBox, tc.textDelay));
        }
    }

    public void WinMenu()
    {
        Color tempColor = tc.textBox.color;

        winUI.GetComponent<CanvasGroup>().alpha = 1f;
        tc.textBox.color = new Color(tempColor.r, tempColor.g, tempColor.b, 0f);
    }

    public void LoseMenu()
    {
        Color tempColor = tc.textBox.color;

        loseUI.GetComponent<CanvasGroup>().alpha = 1f;
        tc.textBox.color = new Color(tempColor.r, tempColor.g, tempColor.b, 0f);
    }
}
