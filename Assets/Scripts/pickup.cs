using UnityEngine;

public class pickup : MonoBehaviour
{
    public enum pickupType { coin, gem, health }
    public pickupType pt;
    [SerializeField] GameObject PickupEffect;

    private bool isCollected = false; // قفل لمنع التكرار

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. التحقق من التاغ ومن إننا مجمعناش العنصر ده قبل كدة في نفس اللحظة
        if (collision.gameObject.tag != "Player" || isCollected) return;

        switch (pt)
        {
            case pickupType.coin:
                isCollected = true; // اقفل العنصر فوراً
                GameManager.instance.IncrementCoinCount();
                SpawnEffect();
                Destroy(gameObject); // يفضل المسح الفوري أو إغلاق الـ Collider
                break;

            case pickupType.gem:
                isCollected = true;
                GameManager.instance.IncrementGemCount();
                SpawnEffect();
                Destroy(gameObject);
                break;

            case pickupType.health:
                // التأكد من أن الصحة أقل من الحد الأقصى
                if (HealthManager.instance.currentHealth < HealthManager.instance.maxHealth)
                {
                    isCollected = true; // اقفل العنصر عشان ميتحسبش مرتين
                    HealthManager.instance.AddHeart();
                    SpawnEffect();
                    Destroy(gameObject);
                }
                break;
        }
    }

    private void SpawnEffect()
    {
        if (PickupEffect != null)
        {
            Instantiate(PickupEffect, transform.position, Quaternion.identity);
        }
    }
}