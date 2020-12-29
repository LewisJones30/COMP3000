using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
