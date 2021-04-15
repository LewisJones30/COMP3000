using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverOverScript : MonoBehaviour
{
    //Public variables

    //SerializeField variables
    [SerializeField]
    GameObject powerDescription, powerStateObj, powerTitleObj;
    //Non-SerializeField variables
    PowerBehaviour powers;
    Text description, powerState, powerTitle; //Description of the power, active/inactive state.
    Color cActive, cInactive; //Color pallettes for active/inactive.
    void Start()
    {
        powers = GameObject.Find("PowerHandler").GetComponent<PowerBehaviour>();
        description = powerDescription.GetComponent<Text>(); //0,255,20
        cActive.r = 0f; //0
        cActive.g = 1f; //255
        cActive.b = 0.078f; //20 //255,19,0
        cActive.a = 1f;
        cInactive.r = 1f; //255
        cInactive.g = 0.0745f; //19
        cInactive.b = 0f; //0
        cInactive.a = 1f;
        powerState = powerStateObj.GetComponent<Text>();
        powerTitle = powerTitleObj.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //This method does not obtain the description from the power so that it can be formatted separately to the power description.
    public void FindPowerDescription() 
    {
        switch (transform.name) 
        {
            case "Power1":
                var p1 = powers.powerHandler[0];
                description.text = "Unavailable outside of the tutorial.";
                powerTitle.text = p1.GetPowerName();
                if (p1.GetPowerActive())
                {
                    powerState.text = "Active";
                    powerState.color = cActive;
                }
                else
                {
                    powerState.text = "Inactive";
                    powerState.color = cInactive;
                }
                return;
            case "Power2":
                var p2 = powers.powerHandler[1];
                powerTitle.text = p2.GetPowerName();
                description.text = "All damage dealt is \n doubled against enemies.";
                if (p2.GetPowerActive())
                {
                    powerState.text = "Active";
                    powerState.color = cActive;
                }
                else
                {
                    powerState.text = "Inactive";
                    powerState.color = cInactive;
                }
                return;
            case "Power3":
                var p3 = powers.powerHandler[2];
                powerTitle.text = p3.GetPowerName();
                description.text = "Your health is doubled. \n Start with maximum health.";
                if (p3.GetPowerActive())
                {
                    powerState.text = "Active";
                    powerState.color = cActive;
                }
                else
                {
                    powerState.text = "Inactive";
                    powerState.color = cInactive;
                }
                return;
            case "Power4":
                var p4 = powers.powerHandler[3];
                powerTitle.text = p4.GetPowerName();
                description.text = "Your health regenerates at \n 2.5% per second.";
                if (p4.GetPowerActive())
                {
                    powerState.text = "Active";
                    powerState.color = cActive;
                }
                else
                {
                    powerState.text = "Inactive";
                    powerState.color = cInactive;
                }
                return;
            case "Power5":
                var p5 = powers.powerHandler[4];
                powerTitle.text = p5.GetPowerName();
                description.text = "As you lose health \n you deal more damage.";
                if (p5.GetPowerActive())
                {
                    powerState.text = "Active";
                    powerState.color = cActive;
                }
                else
                {
                    powerState.text = "Inactive";
                    powerState.color = cInactive;
                }
                return;
            case "Power6":
                var p6 = powers.powerHandler[5];
                powerTitle.text = p6.GetPowerName();
                description.text = "You will not be \n damaged by fire.";
                if (p6.GetPowerActive())
                {
                    powerState.text = "Active";
                    powerState.color = cActive;
                }
                else
                {
                    powerState.text = "Inactive";
                    powerState.color = cInactive;
                }
                return;
            case "Power7":
                var p7 = powers.powerHandler[6];
                powerTitle.text = p7.GetPowerName();
                description.text = "Magical weapons recharge faster.";
                if (p7.GetPowerActive())
                {
                    powerState.text = "Active";
                    powerState.color = cActive;
                }
                else
                {
                    powerState.text = "Inactive";
                    powerState.color = cInactive;
                }
                return;
            case "Power8":
                var p8 = powers.powerHandler[7];
                powerTitle.text = p8.GetPowerName();
                description.text = "Enemies are 50% larger.";
                if (p8.GetPowerActive())
                {
                    powerState.text = "Active";
                    powerState.color = cActive;
                }
                else
                {
                    powerState.text = "Inactive";
                    powerState.color = cInactive;
                }
                return;
            case "Power9":
                var p9 = powers.powerHandler[8];
                powerTitle.text = p9.GetPowerName();
                description.text = "Swords deal increased damage.";
                if (p9.GetPowerActive())
                {
                    powerState.text = "Active";
                    powerState.color = cActive;
                }
                else
                {
                    powerState.text = "Inactive";
                    powerState.color = cInactive;
                }
                return;
            case "Power10":
                var p10 = powers.powerHandler[9];
                powerTitle.text = p10.GetPowerName();
                description.text = "You take 50% less damage.";
                if (p10.GetPowerActive())
                {
                    powerState.text = "Active";
                    powerState.color = cActive;
                }
                else
                {
                    powerState.text = "Inactive";
                    powerState.color = cInactive;
                }
                return;
            case "Power11":
                var p11 = powers.powerHandler[10];
                powerTitle.text = p11.GetPowerName();
                description.text = "A certain weapon \n will extinguish fires.";
                if (p11.GetPowerActive())
                {
                    powerState.text = "Active";
                    powerState.color = cActive;
                }
                else
                {
                    powerState.text = "Inactive";
                    powerState.color = cInactive;
                }
                return;
            case "Power12":
                var p12 = powers.powerHandler[11];
                powerTitle.text = p12.GetPowerName();
                description.text = "???";
                if (p12.GetPowerActive())
                {
                    powerState.text = "Active";
                    powerState.color = cActive;
                }
                else
                {
                    powerState.text = "Inactive";
                    powerState.color = cInactive;
                }
                return;
            case "Power13":
                var p13 = powers.powerHandler[12];
                powerTitle.text = p13.GetPowerName();
                description.text = "You have a small chance of all fires \n being extinguished temporarily.";
                if (p13.GetPowerActive())
                {
                    powerState.text = "Active";
                    powerState.color = cActive;
                }
                else
                {
                    powerState.text = "Inactive";
                    powerState.color = cInactive;
                }
                return;
            case "Power14":
                var p14 = powers.powerHandler[13];
                powerTitle.text = p14.GetPowerName();
                description.text = "occasionally, projectiles fall from the sky\n damaging all enemies.";
                if (p14.GetPowerActive())
                {
                    powerState.text = "Active";
                    powerState.color = cActive;
                }
                else
                {
                    powerState.text = "Inactive";
                    powerState.color = cInactive;
                }
                return;
            case "Power15":
                var p15 = powers.powerHandler[14];
                powerTitle.text = p15.GetPowerName();
                description.text = "null";
                if (p15.GetPowerActive())
                {
                    powerState.text = "Active";
                    powerState.color = cActive;
                }
                else
                {
                    powerState.text = "Inactive";
                    powerState.color = cInactive;
                }
                return;

        }
    }
    public void ClearDescription()
    {
        description.text = "";
        powerState.text = "";
        powerTitle.text = "";
    }
}
