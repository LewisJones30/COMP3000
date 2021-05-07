using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialHandler : MonoBehaviour
{

    [SerializeField]
    GameObject WelcomeImage, WelcomeText, JumpingArrow, Enemy1, Enemy2; //Used for the UI in the tutorial.
    int TutorialStage = 0; //Track state of the tutorial
    PowerBehaviour powerController;
    UIController UIControl; //Reference to the main UIController - used for calling pause/unpause functionality.
    // Start is called before the first frame update
    void Start()
    {
        powerController = GameObject.FindGameObjectWithTag("PowerHandler").GetComponent<PowerBehaviour>();
        UIControl = GameObject.FindGameObjectWithTag("UIHandler").GetComponent<UIController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game") //Ensure player is in main game when checking for pause
        {
            if (PlayerPrefs.GetInt("TutorialCompleteStatus") != 1 && TutorialStage == 0)
            {
                TutorialStage = 1;
            }
        }
            if (TutorialStage > 0)
        {
            if (TutorialStage == 1)
            {
                DisableUIForTutorial();
                Tutorial();
            }
            if (TutorialStage == 2 && Input.GetMouseButton(0) && !UIControl.GetInPauseMenu())
            {
                TutorialStage = 3;
                Tutorial();
            }
            if (TutorialStage == 4 && Input.GetMouseButton(0) && !UIControl.GetInPauseMenu())
            {
                TutorialStage = 5;
                Tutorial();
            }
            if (TutorialStage == 6 && Input.GetMouseButton(0) && !UIControl.GetInPauseMenu())
            {
                TutorialStage = 7;
                Tutorial();
            }
            if (TutorialStage == 8 && Input.GetMouseButton(0) && !UIControl.GetInPauseMenu())
            {
                TutorialStage = 9;
                Tutorial();
            }
            if (TutorialStage == 10 && Input.GetMouseButton(0) && !UIControl.GetInPauseMenu())
            {
                TutorialStage = 11;
                Tutorial();
                UIControl.SetIsPaused(false);
            }
        }

    }


    /// <summary>
    /// This is the code where the tutorial is programmed. TutorialStage is used to track the current position, and run specific code involved in the flow of the tutorial.
    /// Depending on the current TutorialStage depends on how far through the user is. 
    /// The stages are listed below:
    /// 
    /// 
    /// 0 - Player not in tutorial. A check is determined as to whether the player has been in the tutorial before.
    /// 1 - Player has not done tutorial before. Tutorial code is therefore run when the player loads into the game. This greets the player with the welcome screen.
    /// 2 - Awaiting user to press left click to continue. Advanced in update loop.
    /// 3 - Playing animation for next section.
    /// 4 - Awaiting user to press left click to continue.
    /// 5 - Health bar introduction.
    /// 6 - Awaiting user to press left click to continue.
    /// 7 - Powers information.
    /// 8 - Awaiting user to press left click to continue.
    /// 9 - Points information.
    /// 10 - Awaiting user to press left click to continue.
    /// 11 - Unlock movement, show the controls in the top left.
    /// 12 - Await user killing enemy.
    /// 13 - Enemy killed, showing the drain power screen.
    /// 14 - Awaiting user draining power.
    /// 15 - Enemy(s) spawned. Final part of tutorial.
    /// 16 - Enemy killed. Show final message, and then the tutorial is completed. Pause game, transfer user to main menu in 5-10 seconds.
    /// </summary>


    public int GetTutorialStage() //Get the current tutorial stage.
    {
        return TutorialStage;
    }
    private void DisableUIForTutorial()
    {
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (obj.tag == "In-GameUI")
            {
                obj.SetActive(false);
                return;
            }
        }
    }
    private void DisableTutorialUI()
    {
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (obj.tag == "In-GameUI")
            {
                obj.SetActive(false);
                return;
            }
        }
    }
    private void Tutorial()
    {
        if (TutorialStage == 1)
        {
            powerController.powerHandler[0].SetPowerAvailable(true);
            powerController.powerHandler[0].SetPowerActive(true);
            UIControl.SetIsPaused(true);
            TutorialStage = 2;
            WelcomeImage.SetActive(true);
            WelcomeText.SetActive(true);
            WelcomeText.GetComponent<Text>().text = "Welcome to Regression!\nPlease press left click to continue.";
            WelcomeText.transform.localPosition = new Vector3(-244, 108, 0);

        }
        if (TutorialStage == 3)
        {
            StartCoroutine("TutState2");
        }
        if (TutorialStage == 5)
        {

            StartCoroutine("TutState5");
        }
        if (TutorialStage == 7)
        {
            StartCoroutine("TutState7");
        }
        if (TutorialStage == 9)
        {
            StartCoroutine("TutState9");
        }
        if (TutorialStage == 11)
        {
            StartCoroutine("TutState11");
            UIControl.ResumeButton();
        }
        if (TutorialStage == 13)
        {
            StartCoroutine("TutState13");
        }
        if (TutorialStage == 15)
        {
            StartCoroutine("TutState15");
        }
        if (TutorialStage == 17)
        {
            StartCoroutine("TutState17");
        }
    }
    IEnumerator TutState2()
    {
        Animation anim = WelcomeImage.GetComponent<Animation>();
        anim["ExpandTB1"].wrapMode = WrapMode.Once;
        anim.Play("ExpandTB1");
        Animation animText = WelcomeText.GetComponent<Animation>();
        animText["TextDisappear"].wrapMode = WrapMode.Once;
        animText.Play("TextDisappear");
        yield return new WaitForSeconds(0.5f);
        WelcomeText.GetComponent<Text>().text = "You have been cursed by the Devil himself.\n" +
            "He plans to invade through the various portals here.\n" +
            "As you progress through the hordes of enemies, the curse's corruption increases\n" +
            "forcing you to channel this corruption into your powers, disabling their ability.\n" +
            "In this tutorial, you will be able to understand this unique mechanic." +
            "\nPlease press left click to continue!";
        WelcomeText.transform.localPosition = new Vector3(-700, 300, 0);
        animText["TextAppear"].wrapMode = WrapMode.Once;
        animText.Play("TextAppear");
        TutorialStage = 4;
    }
    IEnumerator TutState5()
    {

        Animation anim = WelcomeImage.GetComponent<Animation>();
        Animation animText = WelcomeText.GetComponent<Animation>();
        anim["FadeTB1"].wrapMode = WrapMode.Once;
        anim.Play("FadeTB1");
        animText.Play("TextDisappear");
        WelcomeImage.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.SpawnSpecificWeapon(1); //Staff is ID 1.
        UIControl.ResumeButton();
        WelcomeImage.transform.localScale = new Vector3(17.72895f, 1.921767f, 2.03252f);
        WelcomeImage.transform.localPosition = new Vector3(-18, -210, 0);
        WelcomeText.transform.localPosition = new Vector3(-350, -162, 0);
        WelcomeText.GetComponent<Text>().fontSize = 50;
        WelcomeText.GetComponent<Text>().text = "This is your health bar. Make sure it doesn't reach 0!";
        anim["ShowTB1"].wrapMode = WrapMode.Once;
        anim.Play("ShowTB1");
        animText.Play("TextAppear");
        JumpingArrow.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        TutorialStage = 6;
    }
    IEnumerator TutState7()
    {
        Animation anim = WelcomeImage.GetComponent<Animation>();
        Animation animText = WelcomeText.GetComponent<Animation>();
        Animation animArrow = JumpingArrow.GetComponent<Animation>();
        animText.Play("TextDisappear");
        animArrow.Stop();
        animArrow.Play("jumping arrow disappear");
        yield return new WaitForSeconds(0.5f);
        JumpingArrow.transform.localPosition = new Vector3(-481, -8, 0);
        JumpingArrow.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
        WelcomeText.transform.localPosition = new Vector3(-318, 25, 0);
        animText.GetComponent<Text>().text = "These are your powers. You can see the effects in the pause menu!";
        animText.Play("TextAppear");
        animArrow["jumping arrow appear"].wrapMode = WrapMode.Once;
        animArrow.Play("jumping arrow appear");
        yield return new WaitForSeconds(0.5f);
        animArrow.Play("Jumping arrow left");
        TutorialStage = 8;

    }
    IEnumerator TutState9()
    {
        Animation anim = WelcomeImage.GetComponent<Animation>();
        Animation animText = WelcomeText.GetComponent<Animation>();
        Animation animArrow = JumpingArrow.GetComponent<Animation>();
        animText.Play("TextDisappear");
        animArrow.Stop();
        animArrow.Play("jumping arrow disappear");
        yield return new WaitForSeconds(0.5f);
        JumpingArrow.transform.localPosition = new Vector3(267, 459, 0);
        WelcomeText.transform.localPosition = new Vector3(460, 575, 0);
        WelcomeText.GetComponent<Text>().text = "This is your point counter.\nThe higher the difficulty,\n the more points you'll get!";
        animText.Play("TextAppear");
        animArrow["jumping arrow appear"].wrapMode = WrapMode.Once;
        animArrow.Play("jumping arrow appear");
        yield return new WaitForSeconds(0.5f);
        animArrow.Play("jumping arrow left points");
        TutorialStage = 10;
    }
    IEnumerator TutState11()
    {
        Animation anim = WelcomeImage.GetComponent<Animation>();
        Animation animText = WelcomeText.GetComponent<Animation>();
        Animation animArrow = JumpingArrow.GetComponent<Animation>();
        animText.Play("TextDisappear");
        animArrow.Stop();
        animArrow.Play("jumping arrow disappear");
        yield return new WaitForSeconds(0.5f);
        Enemy1.transform.position = new Vector3(19.90631f, 1.021423f, -7.240521f);
        WelcomeText.GetComponent<Text>().text = "To attack, press left click. You will fire a magical bullet at the enemy.\nKill this enemy to continue.\n" +
            "You can move by using WASD.\n" +
            "To pause the game, simply press escape during the game.";
        WelcomeText.transform.localPosition = new Vector3(-483, -20, 0);
        Enemy1.SetActive(true);
        animText.Play("TextAppear");
        Cursor.lockState = CursorLockMode.Locked;
        TutorialStage = 12;
    }
    IEnumerator TutState13()
    {
        Animation anim = WelcomeImage.GetComponent<Animation>();
        Animation animText = WelcomeText.GetComponent<Animation>();
        Animation animArrow = JumpingArrow.GetComponent<Animation>();
        UIControl.setLockPauseMenu(true);
        animText.Play("TextDisappear");
        yield return new WaitForSeconds(0.5f);
        WelcomeText.GetComponent<Text>().text = "Well done! Wave complete!\n The corruption inside you starts to grow...";
        animText.Play("TextAppear");
        yield return new WaitForSeconds(2f);
        animText.Play("TextDisappear");
        yield return new WaitForSeconds(2f);
        animText.Play("TextAppear");
        WelcomeText.transform.localPosition = new Vector3(-233, 238, 0);
        WelcomeText.GetComponent<Text>().fontSize = 45;
        animText.GetComponent<Text>().text = "Choose a power to drain...";
        UIControl.SetIsPaused(true);
        UIControl.PowerDrainScreen(0, 0);
        TutorialStage = 14;

    }
    IEnumerator TutState15()
    {
        Animation anim = WelcomeImage.GetComponent<Animation>();
        Animation animText = WelcomeText.GetComponent<Animation>();
        Animation animArrow = JumpingArrow.GetComponent<Animation>();
        animText.Play("TextDisappear");
        yield return new WaitForSeconds(0.5f);
        WelcomeText.GetComponent<Text>().text = "You are no longer immune to death!" +
            "\nPlease kill the final enemy to complete the tutorial!";
        WelcomeText.transform.localPosition = new Vector3(-547, 271, 0);
        WelcomeText.GetComponent<Text>().fontSize = 60;
        animText.Play("TextAppear");
        Enemy2.SetActive(true);
        TutorialStage = 16;

    }
    IEnumerator TutState17()
    {
        Animation anim = WelcomeImage.GetComponent<Animation>();
        Animation animText = WelcomeText.GetComponent<Animation>();
        Animation animArrow = JumpingArrow.GetComponent<Animation>();
        animText.Play("TextDisappear");
        UIControl.setLockPauseMenu(true);
        yield return new WaitForSeconds(0.5f);
        WelcomeText.GetComponent<Text>().text = "Congratulations! Tutorial complete.\nNow you understand the basic mechanics of Regression!\n" +
            "Good luck in slaying the devil's forces!\n" +
            "Returning to the main menu in 10 seconds.";
        WelcomeText.transform.localPosition = new Vector3(-547, 271, 0);
        WelcomeText.GetComponent<Text>().fontSize = 60;
        animText.Play("TextAppear");
        UIControl.SetIsPaused(true);
        Cursor.lockState = CursorLockMode.None;
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("UI Scale Testing");
        PlayerPrefs.SetInt("TutorialCompleteStatus", 1);
    }
    public void TutorialEnemyKilled()
    {
        if (TutorialStage == 12)
        {
            TutorialStage = 13;
            Tutorial();
        }
        if (TutorialStage == 14)
        {
            TutorialStage = 15;
            Tutorial();
        }
        if (TutorialStage == 16)
        {
            TutorialStage = 17;
            Tutorial();
        }
    }
    public void PowerDrained()
    {
        TutorialStage = 15;
        UIControl.SetIsPaused(false);
        Tutorial();

    }

    //Getters

}
