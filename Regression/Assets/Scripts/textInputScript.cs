using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textInputScript : MonoBehaviour
{

    public void leaderboardUpdateName()
    {
        string nameToAdd = this.gameObject.GetComponent<InputField>().text;
        int leaderBoardPosition = PlayerPrefs.GetInt("LastLeaderboardPosition");
        int difficultyChosen = PlayerPrefs.GetInt("DifficultyChosen");
        GameObject confirmedText = GameObject.Find("txtLeaderboardMessage");
        switch (leaderBoardPosition)
        {
            case 1:
                {
                    switch (difficultyChosen)
                    {
                        case 1:
                            {
                                PlayerPrefs.SetString("EasyFirstPlaceScoreName", nameToAdd);
                                confirmedText.GetComponent<Text>().text = "Successfully updated your name to: " + nameToAdd;
                                return;
                            }
                        case 2:
                            {
                                PlayerPrefs.SetString("NormalFirstPlaceScoreName", nameToAdd);
                                confirmedText.GetComponent<Text>().text = "Successfully updated your name to: " + nameToAdd;
                                return;
                            }
                        case 3:
                            {
                                PlayerPrefs.SetString("HardFirstPlaceScoreName", nameToAdd);
                                confirmedText.GetComponent<Text>().text = "Successfully updated your name to: " + nameToAdd;
                                return;
                            }
                        case 4:
                            {
                                PlayerPrefs.SetString("ExpertFirstPlaceScoreName", nameToAdd);
                                confirmedText.GetComponent<Text>().text = "Successfully updated your name to: " + nameToAdd;
                                return;
                            }
                        default:
                            {
                                return;
                            }
                    }
                }
            case 2:
                {
                    switch (difficultyChosen)
                    {
                        case 1:
                            {
                                PlayerPrefs.SetString("EasySecondPlaceScoreName", nameToAdd);
                                confirmedText.GetComponent<Text>().text = "Successfully updated your name to: " + nameToAdd;
                                return;
                            }
                        case 2:
                            {
                                PlayerPrefs.SetString("NormalSecondPlaceScoreName", nameToAdd);
                                confirmedText.GetComponent<Text>().text = "Successfully updated your name to: " + nameToAdd;
                                return;
                            }
                        case 3:
                            {
                                PlayerPrefs.SetString("HardSecondPlaceScoreName", nameToAdd);
                                confirmedText.GetComponent<Text>().text = "Successfully updated your name to: " + nameToAdd;
                                return;
                            }
                        case 4:
                            {
                                PlayerPrefs.SetString("ExpertSecondPlaceScoreName", nameToAdd);
                                confirmedText.GetComponent<Text>().text = "Successfully updated your name to: " + nameToAdd;
                                return;
                            }
                        default:
                            {
                                return;
                            }
                    }
                }
            case 3:
                {
                    switch (difficultyChosen)
                    {
                        case 1:
                            {
                                PlayerPrefs.SetString("EasyThirdPlaceScoreName", nameToAdd);
                                confirmedText.GetComponent<Text>().text = "Successfully updated your name to: " + nameToAdd;
                                return;
                            }
                        case 2:
                            {
                                PlayerPrefs.SetString("NormalThirdPlaceScoreName", nameToAdd);
                                confirmedText.GetComponent<Text>().text = "Successfully updated your name to: " + nameToAdd;
                                return;
                            }
                        case 3:
                            {
                                PlayerPrefs.SetString("HardThirdPlaceScoreName", nameToAdd);
                                confirmedText.GetComponent<Text>().text = "Successfully updated your name to: " + nameToAdd;
                                return;
                            }
                        case 4:
                            {
                                PlayerPrefs.SetString("ExpertThirdPlaceScoreName", nameToAdd);
                                confirmedText.GetComponent<Text>().text = "Successfully updated your name to: " + nameToAdd;
                                return;
                            }
                        default:
                            {
                                return;
                            }
                    }
                }
            case 4:
                {
                    switch (difficultyChosen)
                    {
                        case 1:
                            {
                                PlayerPrefs.SetString("EasyFourthPlaceScoreName", nameToAdd);
                                confirmedText.GetComponent<Text>().text = "Successfully updated your name to: " + nameToAdd;
                                return;
                            }
                        case 2:
                            {
                                PlayerPrefs.SetString("NormalFourthPlaceScoreName", nameToAdd);
                                confirmedText.GetComponent<Text>().text = "Successfully updated your name to: " + nameToAdd;
                                return;
                            }
                        case 3:
                            {
                                PlayerPrefs.SetString("HardFourthPlaceScoreName", nameToAdd);
                                confirmedText.GetComponent<Text>().text = "Successfully updated your name to: " + nameToAdd;
                                return;
                            }
                        case 4:
                            {
                                PlayerPrefs.SetString("ExpertFourthPlaceScoreName", nameToAdd);
                                confirmedText.GetComponent<Text>().text = "Successfully updated your name to: " + nameToAdd;
                                return;
                            }
                        default:
                            {
                                return;
                            }
                    }
                }
        }
    }



}
