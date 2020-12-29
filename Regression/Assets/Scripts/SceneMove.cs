using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public PowerBehaviour power;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //For testing purposes, load the game scene, powermodifiers NOT updated.
    public void startGameEasy()
    {
        PlayerPrefs.SetInt("DifficultyChosen", 1);
        SceneManager.LoadScene("Game");

    }

}
