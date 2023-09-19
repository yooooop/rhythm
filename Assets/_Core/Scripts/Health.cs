using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour {

    public int maxHealth;
    public int currentHealth;

    public HealthBar healthBar;
    
    private void Start () {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }
	
    public void TakeDamage(int dmg, string target)
    {
        if (currentHealth - dmg < 0)
        {
            currentHealth = 0;
            if (target == "boss")
            {
                SceneManager.LoadScene("WinScene");
            } 
            else
            {
                SceneManager.LoadScene("LoseScene");
            }
        }
        else currentHealth -= dmg;
        if (currentHealth == 0) 
        {
            currentHealth = 0;
            if (target == "boss")
            {
                SceneManager.LoadScene("WinScene");
            }
            else 
            {
                SceneManager.LoadScene("LoseScene");
            }
        }
        healthBar.SetHealth(currentHealth); 
    }
	
    public void AddHealth(int health)
    {
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        else currentHealth += health;
        healthBar.SetHealth(currentHealth); 
    }
}