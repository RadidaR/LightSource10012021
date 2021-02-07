using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    //Take data from Scriptable Object
    public PlayerHealthData playerHealthData;

    //UI elements
    public Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {
        //Assigning slider values
        healthBar.maxValue = playerHealthData.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //Update to display UI accurately
        healthBar.value = playerHealthData.currentHealth;
    }
}
