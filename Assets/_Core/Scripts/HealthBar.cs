using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private AudioClip hurtSound;

    private SoundManager soundManager;
    public Slider slider;

    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void SetHealth(int health)
    {
        if (health < slider.value && hurtSound != null)
        {
            soundManager.AddSound(hurtSound);
        }
        slider.value = health;
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

}
