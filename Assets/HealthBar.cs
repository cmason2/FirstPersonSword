using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Gradient healthGradient;
    public Image fillImage;
    public void SetHealth(int hp)
    {
        healthSlider.value = hp;
        fillImage.color = healthGradient.Evaluate(healthSlider.normalizedValue);
    }

    public void SetMaxHealth(int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;

        fillImage.color = healthGradient.Evaluate(1f);
    }
}
