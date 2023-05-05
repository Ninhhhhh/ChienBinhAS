using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public TMP_Text healthText;

    Damageable damageable;
    // Start is called before the first frame update
    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player == null)
        {
            Debug.Log("Khong tim thay player");
        }
        damageable = player.GetComponent<Damageable>();
    }
    void Start()
    {
       
        healthBar.value = CalculateSliderPercentage(damageable.Health, damageable.MaxHealth);
        healthText.text = "Mau" + damageable.Health + "/" + damageable.MaxHealth;
    }

    private void OnEnable()
    {
        damageable.healthChanged.AddListener(OnPlayHealthChanged);
    }
    private void OnDisable()
    {
        damageable.healthChanged.RemoveListener(OnPlayHealthChanged);
    }

    public void OnPlayHealthChanged(int newHealth, int maxHealth)
    {
        healthBar.value = CalculateSliderPercentage(newHealth, maxHealth);
        healthText.text = "Mau: " + newHealth + " / " + maxHealth;
    }
    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
       return  currentHealth /  maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
