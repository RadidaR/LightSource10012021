using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeSparkleScript : MonoBehaviour
{
    public PlayerStaminaData playerStaminaData;
    public float sparkleSize;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sparkleSize = playerStaminaData.currentStamina / playerStaminaData.maxStamina;
        Vector2 newSize = gameObject.transform.localScale;
        newSize.y = sparkleSize;
        gameObject.transform.localScale = newSize;
        //gameObject.transform.localScale.y = sparkleSize;
    }
}
