using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UIElements;


public class StamiaBar : MonoBehaviour
{
    public Slider staminaBar;

    private int maxStamina = 100;
    private int currentStamina; 

    private WaitForSeconds regenTime = new WaitForSeconds(1f);
    private Coroutine regen;

    public static StamiaBar instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        currentStamina = maxStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;
    }

    public void UseStamaina(int amaount ) 
    {
        if(currentStamina - amaount >= 0)
        {
            currentStamina -= amaount;
            staminaBar.value = currentStamina;
            
            if (regen != null)  
                StopCoroutine(regen);


            regen = StartCoroutine(RegenStamaina());

        }
        else
        {
            Debug.Log("No Stamina");
        }



    }
     private IEnumerable RegenStamina()
    {
        yield return new WaitForSeconds(2);

        while(currentStamina < maxStamina) 
        {
            currentStamina += maxStamina / 100;
            staminaBar.value = currentStamina;
            yield return regenTime;
        
        }

        regen = null;

    }
    







}
