using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public enum Character { testing, Count }; //add more character types later
    public Character currentCharacter;
    public static Character selectedCharacter;
    public enum Level { testing, Count }; //add more level options later; this should be used to determine tilesets/killer types
    public Level currentLevel;
    public static Level selectedLevel;

    public GameObject mainMenu;
    public GameObject characterSelectMenu;
    public GameObject levelSelectMenu;

    public Button playButton;
    public Button confirmCharacterButton;
    public Button confirmLevelButton;
    
    // Use this for initialization
	void Start () {
        mainMenu.SetActive(true);
        characterSelectMenu.SetActive(false);
        levelSelectMenu.SetActive(false);

        playButton.Select();

        GameManager.step = 0;
        GameManager.objectiveCount = 0;
        GameManager.weaponCount = 0;
        GameManager.foundKey = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGame()
    {
        //
        //TO DO: Load character select screen; Load level select screen; 
        //

        mainMenu.SetActive(false);
        characterSelectMenu.SetActive(true);

        confirmCharacterButton.Select();
    }


    //Character Select Menu Functions
    public void NextCharacter()
    {
        if (currentCharacter > Character.Count)
            currentCharacter = 0;
        else
            currentCharacter++;
    }

    public void PreviousCharacter()
    {
        if (currentCharacter < Character.testing)
            currentCharacter = Character.Count;
        else
            currentCharacter--;
    }

    public void ConfirmCharacterSelect()
    {
        selectedCharacter = currentCharacter;

        characterSelectMenu.SetActive(false);
        levelSelectMenu.SetActive(true);

        confirmLevelButton.Select();
    }

    public void CharacterSelectBackButton()
    {
        mainMenu.SetActive(true);
        characterSelectMenu.SetActive(false);

        playButton.Select();
    }


    //Level Select Menu Functions
    public void NextLevel()
    {
        if (currentLevel > Level.Count)
            currentLevel = 0;
        else
            currentLevel++;
    }

    public void PreviousLevel()
    {
        if (currentLevel < Level.testing)
            currentLevel = Level.Count;
        else
            currentLevel--;
    }

    public void ConfirmLevelSelect()
    {
        selectedLevel = currentLevel;

        PlayGame();
    }

    public void LevelSelectBackButton()
    {
        characterSelectMenu.SetActive(true);
        levelSelectMenu.SetActive(false);

        confirmCharacterButton.Select();
    }


    //Load Game Scene
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("House");
    }


    //Exit Game
    public void ExitGame()
    {
        Application.Quit();
    }
}
