using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject specialZombie;

    void Awake()
    {
        // نستخدم Awake لضمان الاختفاء قبل بداية اللعبة تماماً
        if (specialZombie != null)
        {
            specialZombie.SetActive(false);
        }
    }

    void Start()
    {
        if (specialZombie != null)
        {
            StartCoroutine(ShowZombieAfterDelay(20f));
        }
    }

    IEnumerator ShowZombieAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (specialZombie != null)
        {
            specialZombie.SetActive(true);
            Debug.Log("الزومبي رقم 3 ظهر الآن!");
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (HealthManager.instance != null)
            {
                HealthManager.instance.LoseHeart(); // دي اللي هتنقص الرقم في الـ Inspector
                Debug.Log("الزومبي خبط اللاعب!");
            }
        }
    }
}