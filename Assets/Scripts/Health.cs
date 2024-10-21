using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100.0f;

    [SerializeField] private float currentHealth = 100.0f;

    [SerializeField] private Slider healthBarPrefab;

    [SerializeField] private ParticleSystem damageEffect;

    [SerializeField] private float timeDisappear = 2.0f;

    [SerializeField] private float valueNotDisappear = 50.0f;

    [SerializeField] private bool shouldHide = true;

    float timeSinceDamage = 0.0f;

    private Slider healthBar;

    public void Start()
    {
        Transform canvas = GameObject.Find("MainUI").transform;
        if (canvas != null)
        {
            healthBar = Instantiate(healthBarPrefab);

            if (healthBar != null)
            {
                healthBar.gameObject.transform.SetParent(canvas.transform, false);

                healthBar.maxValue = maxHealth;
                healthBar.value = currentHealth;

                healthBar.GetComponent<HealthBar>().SetParent(gameObject.transform);

                if (shouldHide) healthBar.gameObject.SetActive(false);
                else healthBar.gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {
        UpdateHealthBar();
    }

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0);

        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(true);
            healthBar.value = currentHealth;
        }
        //if (damageEffect != null)
        //    Instantiate(damageEffect, transform.position, damageEffect.transform.rotation);
    }

    public void AddHealth(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

        healthBar.value = currentHealth;
    }

    private void UpdateHealthBar()
    {
        if (!shouldHide || healthBar == null) return;

        if (healthBar.gameObject.activeSelf)
        {
            timeSinceDamage += Time.deltaTime;
        }

        if (timeSinceDamage > timeDisappear)
        {
            timeSinceDamage = 0;
            healthBar.gameObject.SetActive(false);
        }
    }
}
