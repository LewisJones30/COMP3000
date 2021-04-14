using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        Progression progressionCheck = GameObject.Find("ProgressionHandler").GetComponent<Progression>();
        GameObject audioController;
        audioController = GameObject.FindGameObjectWithTag("MusicHandler");
        audioController.GetComponent<AudioController>().PlayMusic(0);
        powers.loseSpecificPower(ui.losePower1);
        switch(powers.powerHandler[ui.losePower1].PowerStrength)
        {
            case 3:
                {
                    ui.AddPoints((progressionCheck.GetMaximumWaves() - progressionCheck.GetCurrentWave()) * 500);
                    break;
                }
            case 2:
                {
                    ui.AddPoints((progressionCheck.GetMaximumWaves() - progressionCheck.GetCurrentWave()) * 250);
                    break;
                }
            case 1:
                {
                    break;
                }
            default:
                {
                    break;
                }
        }
        ui.setLockPauseMenu(false);
        if (ui.getTutorialStage() > 0)
        {
            ui.TutorialEnemyKilled();
        }
        ui.returnToMainGame();
        Cursor.lockState = CursorLockMode.None;
        ui.WeaponSelection();
        progressionCheck.SetWaveComplete(false);
    }
    public void LosePower2()
    {
        UIController ui = GameObject.Find("UIHandler").GetComponent<UIController>();
        PowerBehaviour powers = GameObject.Find("PowerHandler").GetComponent<PowerBehaviour>();
        powers.loseSpecificPower(ui.losePower2);
        GameObject audioController;
        audioController = GameObject.FindGameObjectWithTag("MusicHandler");
        audioController.GetComponent<AudioController>().PlayMusic(0);
        Progression progressionCheck = GameObject.Find("ProgressionHandler").GetComponent<Progression>();
        switch (powers.powerHandler[ui.losePower1].PowerStrength)
        {
            case 3:
                {
                    ui.AddPoints((progressionCheck.GetMaximumWaves() - progressionCheck.GetCurrentWave()) * 500);
                    break;
                }
            case 2:
                {
                    ui.AddPoints((progressionCheck.GetMaximumWaves() - progressionCheck.GetCurrentWave()) * 250);
                    break;
                }
            case 1:
                {
                    break;
                }
            default:
                {
                    break;
                }
        }
        ui.setLockPauseMenu(false);
        if (ui.getTutorialStage() > 0)
        {
            ui.TutorialEnemyKilled();
        }
        ui.returnToMainGame();
        Cursor.lockState = CursorLockMode.None;
        ui.WeaponSelection();
        progressionCheck.SetWaveComplete(false);
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
        GetComponentInChildren<Text>().text = "Loading";
        PlayerPrefs.SetInt("DifficultyChosen", 1);
        SceneManager.LoadScene("Game");

    }
    public void startGameNormal()
    {
        GetComponentInChildren<Text>().text = "Loading";
        PlayerPrefs.SetInt("DifficultyChosen", 2);
        SceneManager.LoadScene("Game");

    }
    public void startGameHard()
    {
        GetComponentInChildren<Text>().text = "Loading";
        PlayerPrefs.SetInt("DifficultyChosen", 3);
        SceneManager.LoadScene("Game");
    }
    public void startGameExpert()
    {
        GetComponentInChildren<Text>().text = "Loading";
        PlayerPrefs.SetInt("DifficultyChosen", 4);
        SceneManager.LoadScene("Game");
    }
    public void startGameSatanic()
    {
        GetComponentInChildren<Text>().text = "Loading";
        PlayerPrefs.SetInt("DifficultyChosen", 5);
        SceneManager.LoadScene("Game");
    }
    public void callLeaderboardEasy()
    {
        UIController ui = GameObject.Find("UIHandler").GetComponent<UIController>();
        ui.callLeaderboards(1);
    }
    public void callLeaderboardNormal()
    {
        UIController ui = GameObject.Find("UIHandler").GetComponent<UIController>();
        ui.callLeaderboards(2);
    }
    public void callLeaderboardHard()
    {
        UIController ui = GameObject.Find("UIHandler").GetComponent<UIController>();
        ui.callLeaderboards(3);
    }
    public void callLeaderboardExpert()
    {
        UIController ui = GameObject.Find("UIHandler").GetComponent<UIController>();
        ui.callLeaderboards(4);
    }
    public void goToLeaderboards()
    {
        SceneManager.LoadScene("Leaderboards");
    }
    public void ResetLeaderboards()
    {
        PlayerPrefs.SetInt("EasyFirstPlaceScore", 0);
        PlayerPrefs.SetInt("EasySecondPlaceScore", 0);
        PlayerPrefs.SetInt("EasyThirdPlaceScore", 0);
        PlayerPrefs.SetInt("EasyFourthPlaceScore", 0);
        PlayerPrefs.SetInt("EasyFifthPlaceScore", 0);
        PlayerPrefs.SetInt("NormalFirstPlaceScore", 0);
        PlayerPrefs.SetInt("NormalSecondPlaceScore", 0);
        PlayerPrefs.SetInt("NormalThirdPlaceScore", 0);
        PlayerPrefs.SetInt("NormalFourthPlaceScore", 0);
        PlayerPrefs.SetInt("NormalFifthPlaceScore", 0);
        PlayerPrefs.SetInt("HardFirstPlaceScore", 0);
        PlayerPrefs.SetInt("HardSecondPlaceScore", 0);
        PlayerPrefs.SetInt("HardThirdPlaceScore", 0);
        PlayerPrefs.SetInt("HardFourthPlaceScore", 0);
        PlayerPrefs.SetInt("HardFifthPlaceScore", 0);
        PlayerPrefs.SetInt("ExpertFirstPlaceScore", 0);
        PlayerPrefs.SetInt("ExpertSecondPlaceScore", 0);
        PlayerPrefs.SetInt("ExpertThirdPlaceScore", 0);
        PlayerPrefs.SetInt("ExpertFourthPlaceScore", 0);
        PlayerPrefs.SetInt("ExpertFifthPlaceScore", 0);

        PlayerPrefs.SetString("EasyFirstPlaceScoreName", "");
        PlayerPrefs.SetString("EasySecondPlaceScoreName", "");
        PlayerPrefs.SetString("EasyThirdPlaceScoreName", "");
        PlayerPrefs.SetString("EasyFourthPlaceScoreName", "");
        PlayerPrefs.SetString("EasyFifthPlaceScoreName", "");
        PlayerPrefs.SetString("NormalFirstPlaceScoreName", "");
        PlayerPrefs.SetString("NormalSecondPlaceScoreName", "");
        PlayerPrefs.SetString("NormalThirdPlaceScoreName", "");
        PlayerPrefs.SetString("NormalFourthPlaceScoreName", "");
        PlayerPrefs.SetString("NormalFifthPlaceScoreName", "");
        PlayerPrefs.SetString("HardFirstPlaceScoreName", "");
        PlayerPrefs.SetString("HardSecondPlaceScoreName", "");
        PlayerPrefs.SetString("HardThirdPlaceScoreName", "");
        PlayerPrefs.SetString("HardFourthPlaceScoreName", "");
        PlayerPrefs.SetString("HardFifthPlaceScoreName", "");
        PlayerPrefs.SetString("ExpertFirstPlaceScoreName", "");
        PlayerPrefs.SetString("ExpertSecondPlaceScoreName", "");
        PlayerPrefs.SetString("ExpertThirdPlaceScoreName", "");
        PlayerPrefs.SetString("ExpertFourthPlaceScoreName", "");
        PlayerPrefs.SetString("ExpertFifthPlaceScoreName", "");
    }
}
