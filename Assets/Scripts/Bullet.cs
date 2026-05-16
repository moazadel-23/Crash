using UnityEngine;

/// <summary>
/// سكريبت الرصاصة - اسحبه على Bullet Prefab في Unity
/// تأكد إن الـ Prefab عليه Rigidbody2D و Collider2D (IsTrigger = true)
/// </summary>
public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 15f;          // سرعة الرصاصة
    public float lifetime = 3f;        // مدة الحياة قبل الاختفاء
    public int damage = 1;             // مقدار الضرر

    private void Start()
    {
        // تدمير الرصاصة تلقائياً بعد مدة معينة
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // تتحرك الرصاصة في الاتجاه المحدد (يمين أو شمال حسب الـ localScale)
        float direction = transform.localScale.x > 0 ? 1f : -1f;
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // تجاهل اللاعب نفسه
        if (collision.gameObject.CompareTag("Player")) return;

        // تجاهل الـ Triggers الأخرى
        if (collision.isTrigger) return;

        // لو ضربت عدو
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // لو العدو عنده سكريبت صحة، اطلب منه يستقبل الضرر
            // EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
            // if (enemyHealth != null) enemyHealth.TakeDamage(damage);

            Debug.Log("Bullet hit enemy: " + collision.gameObject.name);
            Destroy(gameObject);
            return;
        }

        // لو ضربت أي حاجة تانية (أرض، جدار...)
        Destroy(gameObject);
    }
}
