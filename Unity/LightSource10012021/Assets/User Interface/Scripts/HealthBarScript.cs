using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    //Take data from Scriptable Object
    public PlayerHealth playerHealthData;

    //UI elements
    public Slider healthBar;
    private float currentValue;

    // Start is called before the first frame update
    void Start()
    {
        //Assigning slider values
        healthBar.maxValue = playerHealthData.maxHealth;
        currentValue = playerHealthData.currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //Update to display UI accurately
        currentValue = playerHealthData.currentHealth;
        healthBar.value = currentValue;
    }
}
