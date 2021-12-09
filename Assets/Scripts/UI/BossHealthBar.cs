using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossHealthBar : MonoBehaviour //A UI element should not inherit from an Enemy class, but it works so ok
{
    public Slider healthSlider;
    public Image fillImage;

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName != "Boss")
        {
            Destroy(gameObject);
        }
    }
    public void SetHealth(int hp)
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
        healthSlider.value = hp;
    }

    public void SetMaxHealth(int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

}
