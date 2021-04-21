﻿using System.Collections;
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
    //Power1 is true if it is the LEFT power chosen. Otherwise, it is false (RIGHT power).
    public void LosePower(bool Power1) 
    {
        UIController ui = GameObject.Find("UIHandler").GetComponent<UIController>();
        PowerBehaviour powers = GameObject.Find("PowerHandler").GetComponent<PowerBehaviour>();
        Progression progressionCheck = GameObject.Find("ProgressionHandler").GetComponent<Progression>();
        GameObject audioController;
        audioController = GameObject.FindGameObjectWithTag("MusicHandler");
        audioController.GetComponent<AudioController>().PlayMusic(0);
        TutorialHandler TutStatus = GameObject.FindWithTag("TutorialHandler").GetComponent<TutorialHandler>();
        if (TutStatus.GetTutorialStage() > 0)
        {
            TutStatus.PowerDrained();
        }
        if (Power1)
        {
            powers.LoseSpecificPower(ui.losePower1);
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
        }
        else
        {
            powers.LoseSpecificPower(ui.losePower2);
            switch (powers.powerHandler[ui.losePower2].PowerStrength)
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
        }
        ui.setLockPauseMenu(false);
        ui.returnToMainGame();
        Cursor.lockState = CursorLockMode.None;
        ui.WeaponSelection();
        progressionCheck.SetWaveComplete(false);
    }
   
    public void MainMenu()
    {
        Time.timeScale = 1.0f;
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
    public void CallLeaderboardEasy()
    {
        UIController ui = GameObject.Find("UIHandler").GetComponent<UIController>();
        ui.CallLeaderboards(1);
    }
    public void CallLeaderboardNormal()
    {
        UIController ui = GameObject.Find("UIHandler").GetComponent<UIController>();
        ui.CallLeaderboards(2);
    }
    public void CallLeaderboardHard()
    {
        UIController ui = GameObject.Find("UIHandler").GetComponent<UIController>();
        ui.CallLeaderboards(3);
    }
    public void CallLeaderboardExpert()
    {
        UIController ui = GameObject.Find("UIHandler").GetComponent<UIController>();
        ui.CallLeaderboards(4);
    }
    public void GoToLeaderboards()
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

    public void MovetoLeaderboards()
    {
        Animation leaderboardObj = GameObject.FindWithTag("LeaderboardsMovementObj").GetComponent<Animation>();
        Animation difficultyObj = GameObject.FindWithTag("DifficultyMovementObj").GetComponent<Animation>();
        GameObject.FindWithTag("UIHandler").GetComponent<UIController>().CallLeaderboards(1);
        leaderboardObj["LeaderboardMoveAnim"].wrapMode = WrapMode.Once;
        leaderboardObj.Play("LeaderboardMoveAnim");
        difficultyObj["ChooseDifficultyMovement"].wrapMode = WrapMode.Once;
        difficultyObj.Play("ChooseDifficultyMovement");
        
    }
    public void MoveBackToMainMenu()
    {
        Animation leaderboardObj = GameObject.FindWithTag("LeaderboardsMovementObj").GetComponent<Animation>();
        Animation difficultyObj = GameObject.FindWithTag("DifficultyMovementObj").GetComponent<Animation>();
        leaderboardObj["MainMenuMovementLeaderboards"].wrapMode = WrapMode.Once;
        leaderboardObj.Play("MainMenuMovementLeaderboards");
        difficultyObj["MainMenuMovementChooseDifficulty"].wrapMode = WrapMode.Once;
        difficultyObj.Play("MainMenuMovementChooseDifficulty");
    }

    public void MoveToMainMenu() //This is the animation called when moving from splash screen to front screen.
    {
        transform.GetComponentInChildren<Button>().enabled = false;
        Animation splashObj = GameObject.FindWithTag("SplashScreenObj").GetComponent<Animation>();
        Animation difficultyObj = GameObject.FindWithTag("DifficultyMovementObj").GetComponent<Animation>();
        splashObj["MovementFrontSplashScreenUp"].wrapMode = WrapMode.Once;
        splashObj.Play("MovementFrontSplashScreenUp");
        difficultyObj["MainMenuMovementChooseDifficulty"].wrapMode = WrapMode.Once;
        difficultyObj.Play("MainMenuMovementChooseDifficulty");
        Invoke("DisableSplash", 0.5f);
    }
    public void DisableSplash()
    {
        GameObject.FindWithTag("SplashScreenObj").SetActive(false);
    }
}
