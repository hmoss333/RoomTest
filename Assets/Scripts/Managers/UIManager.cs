using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField] Image pauseUI;
    [SerializeField] Image winUI;
    [SerializeField] Image loseUI;

    // Use this for initialization
    void Start () {
        pauseUI.GetComponent<CanvasGroup>().alpha = 0f;
        winUI.GetComponent<CanvasGroup>().alpha = 0f;
        loseUI.GetComponent<CanvasGroup>().alpha = 0f;
    }

    public void PauseMenu()
    {
        pauseUI.GetComponent<CanvasGroup>().alpha = GameManager.gameState == GameManager.GameState.Paused ? 1f : 0f;
    }

    public void WinMenu()
    {
        winUI.GetComponent<CanvasGroup>().alpha = 1f;
    }

    public void LoseMenu()
    {
        loseUI.GetComponent<CanvasGroup>().alpha = 1f;
    }
}
