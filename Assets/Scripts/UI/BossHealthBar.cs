using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : BossEnemy
{
   

    public static int maxhealth;
    public static int currentHealth;
    private Image healthBar;



    void Start()
    {
        healthBar = GetComponent<Image>();
        currentHealth = hp;
        maxhealth = hp;

    }

    void Update()
    {
        healthBar.fillAmount = (float)currentHealth / (float)maxhealth;

    }




}

