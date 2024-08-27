using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetupImage : MonoBehaviour
{
    //Initialize parameters and arrays
    public RawImage taskimage;
    public TMP_Text tasktext;
    public string[] instructions = {"1.-Turn on the red panel power switch", "2.-Connect Ethernet cable to the PC", "3.-Enter Putty credentials and run Machinekit ", "4.-Clean and position the Hotbed glass plate", "5.-Check that there is enough filament in the filament holder box", "6.-Using machinekit define zero of the coordinate axis", "7.-Heat Hotend and Hotbed until the desired temperature is reached", "8.-Load G-code to Machinekit and execute the program" };
    public Texture[] myinstTextures = new Texture[8];

    //counter
    private int currentItem = 0;

    // Start is called before the first frame update
    void Start()
    {
        updateScreen();
    }

    //Fuction for Next
    public void NextButton()
    {
        currentItem++;

        if (currentItem > instructions.Length -1)
        {
            currentItem = 0;
        }
        updateScreen();
    }

    //Fuction for Previous
    public void BackButton()
    {
        currentItem--;

        if (currentItem<0)
        {
            currentItem = instructions.Length - 1;
        }
        updateScreen();
    }

    //Fuction to UpdateScreen
    public void updateScreen()
    {
        taskimage.texture = myinstTextures[currentItem];
        tasktext.text = instructions[currentItem];
    }
}
