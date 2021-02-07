using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarScript : MonoBehaviour
{
    //Take data from Scriptable Object
    public PlayerStaminaData playerStaminaData;

    //UI elements
    public Slider staminaBar;

    // Start is called before the first frame update
    void Start()
    {
        //Assigning slider values
        staminaBar.maxValue = playerStaminaData.maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        //Update to display UI accurately
        staminaBar.value = playerStaminaData.currentStamina;
    }
}
