using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverOverScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject powerDescription, powerStateObj, powerTitleObj;
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

    public void FindPowerDescription()
    {
        switch (transform.name) 
        {
            case "Power1":
                var p1 = powers.powerHandler[0];
                description.text = "Unavailable outside of the tutorial.";
                powerTitle.text = p1.PowerName;
                if (p1.PowerActive == true)
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
                powerTitle.text = p2.PowerName;
                description.text = "All damage dealt is \n doubled against enemies.";
                if (p2.PowerActive == true)
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
                powerTitle.text = p3.PowerName;
                description.text = "Your health is doubled. \n Start with maximum health.";
                if (p3.PowerActive == true)
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
                powerTitle.text = p4.PowerName;
                description.text = "Your health regenerates at \n 2.5% per second.";
                if (p4.PowerActive == true)
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
                powerTitle.text = p5.PowerName;
                description.text = "As you lose health \n you deal more damage.";
                if (p5.PowerActive == true)
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
                powerTitle.text = p6.PowerName;
                description.text = "You will not be \n damaged by fire.";
                if (p6.PowerActive == true)
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
                powerTitle.text = p7.PowerName;
                description.text = "Magical weapons recharge faster.";
                if (p7.PowerActive == true)
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
                powerTitle.text = p8.PowerName;
                description.text = "Arrow based weapons \n reload faster.";
                if (p8.PowerActive == true)
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
                powerTitle.text = p9.PowerName;
                description.text = "Swords deal increased damage.";
                if (p9.PowerActive == true)
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
                powerTitle.text = p10.PowerName;
                description.text = "You take 50% less damage.";
                if (p10.PowerActive == true)
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
                powerTitle.text = p11.PowerName;
                description.text = "A certain weapon \n will extinguish fires.";
                if (p11.PowerActive == true)
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
                powerTitle.text = p12.PowerName;
                description.text = "???";
                if (p12.PowerActive == true)
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
                powerTitle.text = p13.PowerName;
                description.text = "You have a small chance of all fires \n being extinguished temporarily.";
                if (p13.PowerActive == true)
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
                powerTitle.text = p14.PowerName;
                description.text = "occasionally, projectiles fall from the sky\n damaging all enemies.";
                if (p14.PowerActive == true)
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
                powerTitle.text = p15.PowerName;
                description.text = "null";
                if (p15.PowerActive == true)
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
