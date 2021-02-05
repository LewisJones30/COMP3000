using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonCaller : MonoBehaviour
{
    //This script is designed to call from other scripts on specific game objects.
    //For example, Resume will call Resume from UIController to start the flow of the game again.



    public void Resume()
    {
       UIController ui =  GameObject.Find("UIHandler").GetComponent<UIController>();
       ui.ResumeButton();
    }
    public void ExitGameConfirmScreen()
    {
        Debug.Log("ExitGameConfirmScreen button pushed!");
        UIController ui = GameObject.Find("UIHandler").GetComponent<UIController>();
        ui.ExitGameConfirmScreen();
    }
    public void ExitGame()
    {
        UIController ui = GameObject.Find("UIHandler").GetComponent<UIController>();
        ui.ExitGame();
    }
    public void Test()
    {
        Debug.Log("CLick!");
    }
    public void ReturnToPauseMenu()
    {
        UIController ui = GameObject.Find("UIHandler").GetComponent<UIController>();
        ui.ReturnToPauseScreen();
    }
    //Button callers for player - Starting weaponry
    public void StartMace()
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.SpawnSpecificWeapon(0); //Mace is ID 0.
        UIController ui = GameObject.Find("UIHandler").GetComponent<UIController>();
        ui.ResumeButton();
    }
    public void StartStaff()
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.SpawnSpecificWeapon(1); //Staff is ID 1.
        UIController ui = GameObject.Find("UIHandler").GetComponent<UIController>();
        ui.ResumeButton();
    }
    public void LosePower1()
    {
        UIController ui = GameObject.Find("UIHandler").GetComponent<UIController>();
        PowerBehaviour powers = GameObject.Find("PowerHandler").GetComponent<PowerBehaviour>();
        powers.loseSpecificPower(ui.losePower1);
        ui.setLockPauseMenu(false);
        if (ui.getTutorialStage() > 0)
        {
            ui.TutorialEnemyKilled();
        }
        ui.returnToMainGame();
        Cursor.lockState = CursorLockMode.None;
        ui.WeaponSelection();
    }
    public void LosePower2()
    {
        UIController ui = GameObject.Find("UIHandler").GetComponent<UIController>();
        PowerBehaviour powers = GameObject.Find("PowerHandler").GetComponent<PowerBehaviour>();
        powers.loseSpecificPower(ui.losePower2);
        ui.setLockPauseMenu(false);
        if (ui.getTutorialStage() > 0)
        {
            ui.TutorialEnemyKilled();
        }
        ui.returnToMainGame();
        Cursor.lockState = CursorLockMode.None;
        ui.WeaponSelection();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("UI Scale Testing");
    }
    public void ResetTutorial()
    {
        PlayerPrefs.SetInt("TutorialCompleteStatus", 0);
    }
    public void startGameEasy()
    {
        PlayerPrefs.SetInt("DifficultyChosen", 1);
        SceneManager.LoadScene("Game");
    }
    public void startGameNormal()
    {
        PlayerPrefs.SetInt("DifficultyChosen", 2);
        SceneManager.LoadScene("Game");
    }
    public void startGameHard()
    {
        PlayerPrefs.SetInt("DifficultyChosen", 3);
        SceneManager.LoadScene("Game");
    }
    public void startGameExpert()
    {
        PlayerPrefs.SetInt("DifficultyChosen", 4);
        SceneManager.LoadScene("Game");
    }
    public void startGameSatanic()
    {
        PlayerPrefs.SetInt("DifficultyChosen", 5);
        SceneManager.LoadScene("Game");
    }
}
