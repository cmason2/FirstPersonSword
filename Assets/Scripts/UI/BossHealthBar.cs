using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossHealthBar : BossEnemy //A UI element should not inherit from an Enemy class, but it works so ok
{
    public static int maxhealth;
    public static int currentHealth;
    private Image healthBar;

    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if(sceneName != "Boss")
        {
            Destroy(gameObject);
        }
        healthBar = GetComponent<Image>();
        currentHealth = hp;
        maxhealth = hp;

    }

    void Update()
    {
        healthBar.fillAmount = (float)currentHealth / (float)maxhealth;
    }




}

