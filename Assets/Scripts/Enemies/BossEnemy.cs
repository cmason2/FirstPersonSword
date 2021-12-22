using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossEnemy : Enemy
{
    public static int bossHealth = 1;
    BossHealthBar healthBar;
    GameObject fadeOverlay;

    protected override void Start()
    {
        base.Start();
        healthBar = FindObjectOfType<BossHealthBar>();
        healthBar.SetMaxHealth(hp);
        fadeOverlay = GameObject.Find("FadeOverlay");
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
        yield return StartCoroutine(fadeOverlay.GetComponent<SceneFade>().Fade(false, 3f));
        yield return new WaitForSeconds(1f);
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("GameOver");
    }
}
