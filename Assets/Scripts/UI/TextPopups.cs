using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPopups : MonoBehaviour
{
    public TextMeshProUGUI WhateverTextThingy;  //Add reference to UI Text here via the inspector
    private float timeToAppear = 2f;
    private float timeWhenDisappear;

    //Call to enable the text, which also sets the timer
    public void EnableText()
    {
        WhateverTextThingy.enabled = true;
        timeWhenDisappear = Time.time + timeToAppear;
    }

    //We check every frame if the timer has expired and the text should disappear
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            EnableText();
        }

        if (WhateverTextThingy.enabled && (Time.time >= timeWhenDisappear))
        {
            WhateverTextThingy.enabled = false;
        }
    }
}
