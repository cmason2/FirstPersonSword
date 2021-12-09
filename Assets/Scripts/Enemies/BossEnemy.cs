using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossEnemy : Enemy
{
    public static int bossHealth = 1;
    BossHealthBar healthBar;
    Image fadeImage;

    protected override void Start()
    {
        base.Start();
        healthBar = FindObjectOfType<BossHealthBar>();
        healthBar.SetMaxHealth(hp);
        fadeImage = GameObject.Find("FadeOverlay").GetComponent<Image>();
    }

    public override void TakeDamage(int damage)
    {
        Debug.Log(gameObject.name + " took " + damage + " damage!");
        hp -= damage;
        if(healthBar != null)
            healthBar.SetHealth(hp);
        CheckHealth();
        StartCoroutine(ApplyDamageEffect());
    }

    protected override void Death()
    {
        bossHealth = 0;
        Destroy(GameObject.Find("BossHealthBar"));
        StartCoroutine(DeathAnimation());
    }

    IEnumerator DeathAnimation()
    {
        gameObject.GetComponentInChildren<Animation>().Stop();
        Destroy(gameObject.GetComponent<BossMovement>());
        gameObject.AddComponent<Rigidbody>().AddTorque(transform.right * 100f);
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(FadeScene(3f));
        //Move to Victory scene here
    }

    IEnumerator FadeScene(float fadeTime)
    {
        for (float i = 0; i <= fadeTime; i += Time.deltaTime)
        {
            fadeImage.color = new Color(0, 0, 0, i / fadeTime);
            yield return null;
        }
    }
}
