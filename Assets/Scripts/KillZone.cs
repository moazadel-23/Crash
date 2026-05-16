using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class KillZone : MonoBehaviour
{
    private bool isDying = false; // عشان نمنع الكود يتكرر مرتين في نفس اللحظة

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isDying)
        {
            isDying = true;
            StartCoroutine(WaitAndDie());
        }
    }

    IEnumerator WaitAndDie()
    {
        // 1. نقص القلب في الذاكرة أولاً
        if (HealthManager.instance != null)
        {
            HealthManager.instance.LoseHeart();
            Debug.Log("القلب نقص فعلياً! المتبقي: " + HealthManager.instance.currentHealth);
        }

        // 2. انتظر ثانية واحدة (مهمة جداً عشان التزامن)
        yield return new WaitForSeconds(0.1f);

        // 3. لو لسه فيه قلوب، عيد المرحلة
        if (HealthManager.instance != null && HealthManager.instance.currentHealth > 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        isDying = false;
    }
}