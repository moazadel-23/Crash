using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance;

    [Header("Health Settings")]
    public int maxHealth = 3;
    public int currentHealth;

    private bool canTakeDamage = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            currentHealth = maxHealth;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    private void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        canTakeDamage = true;
        UpdateUI();
    }

    public void LoseHeart()
    {
        if (!canTakeDamage) return;

        currentHealth--;
        UpdateUI();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            canTakeDamage = false;

            if (GameManager.instance != null)
            {
                GameManager.instance.GameOver();
            }
        }
        else
        {
            canTakeDamage = false;
        }
    }

    public void AddHeart()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth++;
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        if (UIManager.instance != null) UIManager.instance.UpdateHearts(currentHealth);
        if (GameManager.instance != null) GameManager.instance.UpdateHeartsVisual(currentHealth);
    }
}